using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressurePlate : MonoBehaviour
{
    //weight settings
    //how much total weight is needed to activate plate
    public float weightThreshold = 5f;
    
    //if true, plate stays activated even after object is removed
    public bool lockOnActivate = false;

    //event
    //fired when total weight exceeds threshold
    //diff from event action which we were previously doing
    //unity event needs to be wired in inspector; is more like buttons
    //static event action just code, doesn't need reference to sender b/c static (not as designer friendly)
    //fired when total exceeds threshold
    public UnityEvent onActivated;
    //fired when weight drops below threshold
    //ignored on lockOnActivate = true
    public UnityEvent onDeactivated;

    //visual feedback
    //optional: plate mesh that moves down when pressed
    public Transform plate;

    //how far plate depresses when activated (world units)
    public float pressDepth = 0.05f;

    float currentWeight = 0f;
    bool isActivated = false;
    bool isLocked = false;
    Vector3 plateResetPos;
    Vector3 platePressedPos;

    HashSet<PhysicsObject> objectsOnPlate = new HashSet<PhysicsObject>();

    void Start()
    {
        if (plate != null)
        {
            //storing plate location
            plateResetPos = plate.localPosition;
            //moving it down
            platePressedPos = plateResetPos + Vector3.down * pressDepth;
        }
    }

    //called when weight changes; activates if threshold is met
    void CheckActivation()
    {
        if(!isActivated && currentWeight >= weightThreshold)
        {
            isActivated = true;
            if (lockOnActivate) isLocked = true;

            //calls for whatever is listening to it
            onActivated.Invoke();
            Debug.Log("Pressure plate is activated");

            if(plate != null)
            {
                //move plate when activated
                plate.localPosition = platePressedPos;
            }
        }
    }

    //fires when any collider enters trigger zone
    //check for physics obj to get weight
    private void OnTriggerEnter(Collider other)
    {
        PhysicsObject physOb = other.GetComponent<PhysicsObject>();
        if (physOb != null) return;

        if (physOb.isHeld) return; //doesnt go off if we're just holding it in trigger area

        //SIMPLE VERSION
        /*currentWeight = physOb.puzzleWeight;
        Debug.Log($"{other.gameObject.name} entered plate; total weight: {currentWeight}");
        CheckActivation();*/

        //this is instead adding it to a list at first just to make sure nothing gets double activated
        //above works too
        if(objectsOnPlate.Add(physOb))
        {
            currentWeight += physOb.puzzleWeight;
            CheckActivation();
        }
    }

    //call when weight removed; deactivates if below threshold
    void CheckDeactivation()
    {
        if(isActivated && !isLocked && currentWeight < weightThreshold)
        {
            isActivated = false;
            onDeactivated.Invoke();
            Debug.Log("Pressure Plate Deactivated");

            if(plate != null)
            {
                plate.localPosition = plateResetPos;
            }
        }
    }
}
