using UnityEngine;

//requires GameObjects with this script to have our PositionFollower class
[RequireComponent(typeof(PositionFollower))]
public class ViewBobbing : MonoBehaviour
{
    //float for the overall intensity, the intensity along the x axis (should be greater than the y axis) and the speed at which the camera bobs
    public float effectIntensity;
    public float effectIntensityX;
    public float effectSpeed;

    //variable for the attached PositionFollower, the original offset of the pivot and a time for the sine function used in the bobbing motion
    private PositionFollower FollowerInstance;
    private Vector3 OriginalOffset;
    private float sinTime;


    void Start()
    {
        //accessing the attached PositionFollower script and storing the original offset of the pivot
        FollowerInstance = GetComponent<PositionFollower>();
        OriginalOffset = FollowerInstance.offset;
    }

    void Update()
    {
        //checking if player is giving mouse input
        Vector3 inputVector = new Vector3(Input.GetAxis("Vertical"), 0f, Input.GetAxis("Horizontal"));

        //if there is input, the duration of the input is put into a negative sine function for the y and a cosine function for the x
        //if not, the function stops
        if (inputVector.magnitude > 0f)
        {
            sinTime += Time.deltaTime * effectSpeed;

        }
        else
        {
            sinTime = 0f;
        }

        //formula to calculate y movement and x movement
        //for y, the negative value of the absolute value of a sine function using the set intensity and elapsed time is calculated
        //for x, a Vector3 is used where the local right position is multiplied by the effect intensity, a cosine function using the elapsed time and a separate intensity for the x axis to get left and right motion
        float sinAmountY = -Mathf.Abs(effectIntensity * Mathf.Sin(sinTime));
        Vector3 sinAmountX = FollowerInstance.transform.right * effectIntensity * Mathf.Cos(sinTime) * effectIntensityX;

        //setting the offset using the formulas we just used
        FollowerInstance.offset = new Vector3
        {
            //the offset is kept as the original but adds the sine function we used for the y to bob up and down
            x = OriginalOffset.x,
            y = OriginalOffset.y + sinAmountY,
            z = OriginalOffset.z
        };
        //the x amount needs to be added separately because it is a Vector3
        FollowerInstance.offset += sinAmountX;
    }
}
