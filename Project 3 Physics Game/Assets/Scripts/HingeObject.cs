using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class HingeObject : MonoBehaviour
{
    public float minAngle = 0f; //min angle hinge can rotate to; 0 = closed
    public float maxAngle = 90f; //max angle hinge can rotate to; 90 = fully open
    public bool useSpring = true; //if true hinge will spring back towards rest angle when released
    public float springTargetAngle = 0f; //angle spring tries to return to
    public float springForce = 50f; //how strong spring force is
    public float springDamper = 5f; //how much spring dampens (reduces oscillation)

    //EVENTS
    public UnityEvent OnReachMax; //fires when hinge eraches or passes max angle
    public UnityEvent OnReachMin; //fires when hinge returns to or passes to min angle
    public float eventThreshold = 5f; //how close to limit angle before event fires (degrees)

    HingeJoint hinge;
    bool maxEventFired = false;
    bool minEventFired = false;

    void Awake()
    {
        hinge = GetComponent<HingeJoint>();
        ConfigureHinge();
    }

    private void Update()
    {
        //check if we hit the limits and should fire puzzle events
        float currentAngle = hinge.angle;
        
        if(!maxEventFired && currentAngle >= maxAngle - eventThreshold)
        {
            maxEventFired = true;
            minEventFired = false;
            OnReachMax?.Invoke();
            Debug.Log(gameObject.name + "hinge reached max angle");
        } else if(!minEventFired && currentAngle <= minAngle - eventThreshold)
        {
            minEventFired = true;
            maxEventFired = false;
            OnReachMin?.Invoke();
            Debug.Log(gameObject.name + "hinge reached min angle");
        }
    }
    //config hinges
    //sets up joint limits and spring
    void ConfigureHinge()
    {
        //limits
        //jointlimits is a struct we have to set all fields then assign it back
        JointLimits limits = hinge.limits;
        limits.min = minAngle;
        limits.max = maxAngle;
        limits.bounciness = 0f;
        limits.bounceMinVelocity = 0.2f;
        hinge.limits = limits;
        hinge.useLimits = true;

        if(useSpring)
        {
            JointSpring spring = hinge.spring;
            spring.targetPosition = springTargetAngle;
            spring.spring = springForce;
            spring.damper = springDamper;
            hinge.spring = spring;
            hinge.useSpring = true;
        }
        else
        {
            hinge.useSpring = false;
        }
    }

    public void DrivetoMax()
    {
        SetMotorTarget(maxAngle);
    }

    public void DriveToMin()
    {
        SetMotorTarget(minAngle);
    }

    void SetMotorTarget(float targetAngle)
    {
        JointMotor motor = hinge.motor;
        //motor velocity direction determines which way it moves
        motor.targetVelocity = targetAngle > hinge.angle ? 50f : -50f; //shorthand if statement
        motor.force = 100f;
        motor.freeSpin = false;
        hinge.motor  = motor;
        hinge.useMotor = true;
    }
}
