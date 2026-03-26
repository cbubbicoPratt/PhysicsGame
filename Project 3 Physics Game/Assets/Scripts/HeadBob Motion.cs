using System.Runtime.CompilerServices;
using UnityEngine;

public class HeadBobMotion : MonoBehaviour
{
    //declaring variables for amplitude and frequency of the head bob motion
    //amplitude controls size of bob while frequency controls speed
    //forcing the private variables to show in the editor and giving them slider options for convenience
    [SerializeField, Range (0, 0.1f)] private float _amplitude = 0.015f;
    [SerializeField, Range (0, 30)] private float _frequency = 10.0f;

    //reference to the transform value of the camera attached to the player and an empty it is parented to
    public Transform playerCamera;
    public Transform cameraHolder;

    //setting a speed threshold to trigger the bobbing motion
    //storing the intial position of the camera in a Vector3
    //referencing player's Rigidbody to detect movement
    private float _toggleSpeed = 1.0f;
    private Vector3 _startPos;
    private Rigidbody _rb;

    void Start()
    {
        //setting initial values for player's Rigidbody and camera's starting location
        _rb = GetComponent<Rigidbody>();
        _startPos = playerCamera.localPosition;
    }

    void Update()
    {
        CheckMotion();
        ResetPosition();
        playerCamera.LookAt(FocusTarget());
    }

    //function to emulate movement of player's footsteps and bob the camera accordingly
    //always returns the position of the bob's offset
    private Vector3 FootStepMotion()
    {
        //setting an initial local position for the movement at (0,0,0)
        Vector3 pos = Vector3.zero;
        //"nerd math stuff" which represents the arc of the movement which the bob follows from left to right
        //a sine wave using the frequency multiplied by the time and amplitude represents the y movement of the camera
        //a cosine wave using the same formula but halving the frequency and then multiplying the result by 2 represents the x movement of the camera
        pos.y = Mathf.Sin(Time.time * _frequency) * _amplitude;
        pos.x = Mathf.Cos(Time.time * _frequency / 2) * _amplitude * 2;
        return pos;
    }

    //function to actually move the camera when given a Vector3
    private void PlayMotion(Vector3 motion)
    {
        //adds the target location to the current position so the camera moves
        playerCamera.localPosition += motion;
    }
    
    //function to check that the player is moving and isn't grounded
    private void CheckMotion()
    {
        //magnitude of the player's velocity on the x and z axis
        //if it's anything greater than the threshold for player motion, the movement will trigger.
        float speed = new Vector3(_rb.linearVelocity.x, 0, _rb.linearVelocity.z).magnitude;

        if (speed < _toggleSpeed) return;

        PlayMotion(FootStepMotion());
    }

    private void ResetPosition()
    {
        if (playerCamera.localPosition == _startPos) return;
        playerCamera.localPosition = Vector3.Lerp(playerCamera.localPosition, _startPos, 5 * Time.deltaTime);
    }

    private Vector3 FocusTarget()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + cameraHolder.localPosition.y, transform.position.z);
        pos += cameraHolder.forward * 15.0f;
        return pos;
    }
}
