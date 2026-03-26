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

    private InteractableObject currentHighlight;

    private void FixedUpdate()
    {
        //fixedUpdate runs on an interval schedule
        //we move held object here; stays smooth, physics accurate
        if(isHolding && heldObject != null) MoveHeldObject();
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

    void UpdateHighlight()
    {
        //don't change highlight while holding an object
        if (isHolding) return;

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.forward * grabRange, Color.red);

        if(Physics.Raycast(ray, out hit, grabRange)) 
        {
            InteractableObject interactable = hit.collider.GetComponent<InteractableObject>();
            if(interactable != null)
            {
                //unhighlight old object if looking at new one
                if(currentHighlight != null && currentHighlight != interactable)
                {
                    currentHighlight.UnHighlight();
                    Debug.Log("unhighlighted");
                }

                //highlight the new obj
                interactable.Highlight();
                currentHighlight = interactable;
                return;
            }

            //raycast hits nothing interactable clear highlight
            if(currentHighlight != null)
            {
                currentHighlight.UnHighlight();
                currentHighlight = null;
            }
        }
    }
}
