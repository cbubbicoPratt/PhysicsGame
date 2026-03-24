using UnityEngine;

public class PositionFollower : MonoBehaviour
{
    //variables for the transform of the camera and an offset variable for implementations of the PositionFollower class
    public Transform targetTransform;
    public Vector3 offset;

    void Update()
    {
        //constantly adds the camera's position of the camera to the offset to get the position of the pivot
        transform.position = targetTransform.position + offset;
    }
}
