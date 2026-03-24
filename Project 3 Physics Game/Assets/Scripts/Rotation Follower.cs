using UnityEngine;

public class RotationFollower : MonoBehaviour
{
    //targetting the player camera
    public Transform target;

    void Update()
    {
        //setting the rotation to the camera's rotation so that the weapon moves on the y axis with the camera
        transform.rotation = target.rotation;
    }
}
