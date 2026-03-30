using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectGrabber : MonoBehaviour
{
    [Header("Grab Settings")]
    [Tooltip("How far away player can grab objects from")]
    public float grabRange = 4;

    [Tooltip("How fast held object moves to hold point (higher = snappier)")]
    public float holdSmoothing = 15;

    //point in front of camera where object is held
    public Transform holdPoint;

    //how much force is applied when throwing
    [SerializeField, Range(0, 50)] public float throwForce = 15;

    private Rigidbody heldObject;
    private bool isHolding = false;

    private bool rotating = false;

    private Vector2 lookInput;
    private float lookSensitivity;

    private float objectYaw;
    private float objectPitch;

    private InteractableObject currentHighlight;
    private void Awake()
    {
        lookSensitivity = GameObject.FindGameObjectWithTag("Player").GetComponent<FPSPlayer>().lookSensitivity;
    }
    private void FixedUpdate()
    {
        //fixedUpdate runs on an interval schedule
        //we move held object here; stays smooth, physics accurate
        if (isHolding && heldObject != null)
        {
            MoveHeldObject();
            ObjectRotate();
            //RotateHeldObject();
            if (!rotating)
            {
                heldObject.angularVelocity = Vector3.zero;
                heldObject.freezeRotation = true;
            }
        }
    }

    
    void Update()
    {
        //run detection raycast every frame
        //diff from grab raycast
        //highlight toggles for what interactable player is looking at
        UpdateHighlight();
    }

     void TryGrab()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        //drawing ray
        Debug.DrawRay(transform.position, transform.forward * grabRange, Color.purple, 0.5f);

        if(Physics.Raycast(ray, out hit, grabRange))
        {
            //checking if object his has interactable marker script
            InteractableObject interactable = hit.collider.GetComponent<InteractableObject>();
            if (interactable != null)
            {
                //get rigidbody
                heldObject = hit.collider.GetComponent<Rigidbody>();
                if(heldObject != null)
                {
                    //disable gravity
                    heldObject.useGravity = false;

                    //freeze rotation
                    heldObject.freezeRotation = true;

                    //zero out existing velocity
                    heldObject.linearVelocity = Vector3.zero;
                    heldObject.angularVelocity = Vector3.zero;

                    //unhighlight when grabbed object is now in hand
                    interactable.UnHighlight();
                    currentHighlight = interactable;

                    isHolding = true;
                    Debug.Log($"Grabbed {heldObject.name}");
                }
            }
        }
    }

    //called every fixed update while holding an object
    //smoothly moves rigidbody towards holdpoint
    void MoveHeldObject()
    {
        Vector3 targetPos = holdPoint.position;
        Vector3 currentPos = heldObject.position;
        

        //smoothly interpolate towards held point
        //move position respects physics collision (wont clip)
        Vector3 newPos = Vector3.Lerp(currentPos, targetPos, holdSmoothing * Time.fixedDeltaTime);

        heldObject.MovePosition(newPos);
        
    }

    //only called when we interact with an object so it only rotates once
    //void RotateHeldObject()
    //{
    //    Quaternion targetRotate = holdPoint.rotation;
    //    Quaternion currentRotate = heldObject.rotation;
    //    Quaternion newRotate = Quaternion.Lerp(currentRotate, targetRotate, holdSmoothing * Time.fixedDeltaTime);
    //    heldObject.MoveRotation(newRotate);
    //    objectRotated = true;
    //}

    //drop
    //releases object and resumes physics
    void DropObject()
    {
        if (heldObject != null) return; //stop if we arent holding anything

        //re-enable gravity and rotation
        heldObject.useGravity = true;
        heldObject.freezeRotation = false;

        heldObject = null; //clear it
        isHolding = false;
        Debug.Log("Dropped Object");
    }

    //releases obj with force
    void ThrowObject()
    {
        if (heldObject == null) return;

        //re-enable physics
        heldObject.useGravity = true;
        heldObject.freezeRotation = false;

        //apply force in direction camera is facing
        //forcemode.impulse applies force instantly (like punch)
        //forcemode.force applies gradually over time
        heldObject.AddForce(transform.forward * throwForce, ForceMode.Impulse);

        heldObject = null;
        isHolding = false;
        Debug.Log("Threw object");
    }

    public void OnGrabPerformed(InputAction.CallbackContext context)
    {
        if (isHolding) DropObject();
        else TryGrab();

    }

    public void OnThrowPerformed(InputAction.CallbackContext context)
    {
        if (isHolding) ThrowObject();
    }

    public void OnToggleRotate(InputAction.CallbackContext context)
    {
        if (heldObject != null && context.performed)
        {
            rotating = true;
            heldObject.freezeRotation = false;
        }
        else rotating = false;
    }

    public void OnRotate(InputAction.CallbackContext context)
    {
        if (rotating) lookInput = context.ReadValue<Vector2>();
    }

    public bool BroadcastRotate()
    {
        return rotating;
    }

    void ObjectRotate()
    {
        if(heldObject == null) return;

        float mouseX = lookInput.x * lookSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * lookSensitivity * Time.deltaTime;

        objectYaw += mouseX;

        objectPitch -= mouseY;

        heldObject.rotation = Quaternion.Euler(objectPitch, objectYaw, 0);

        if (!rotating)
        {
            heldObject.freezeRotation = true;
            heldObject.angularVelocity = Vector3.zero;
        }
    }

    void UpdateHighlight()
    {
        //don't change highlight while holding an object
        if (isHolding) return;

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.forward * grabRange, Color.red);

        if (Physics.Raycast(ray, out hit, grabRange))
        {
            InteractableObject interactable = hit.collider.GetComponent<InteractableObject>();
            if (interactable != null)
            {
                //unhighlight old object if looking at new one
                if (currentHighlight != null && currentHighlight != interactable)
                {
                    currentHighlight.UnHighlight();
                    Debug.Log("unhighlighted");
                }

                //highlight the new obj
                interactable.Highlight();
                Debug.Log("highlighted");
                currentHighlight = interactable;
                return;
            }
        }
            //raycast hits nothing interactable clear highlight
            if(currentHighlight != null)
            {
                currentHighlight.UnHighlight();
                Debug.Log("unhighlighted");
                currentHighlight = null;
            }
    }
}
