using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
    [Header("Mass and Motion")]
    //heaviness of obj in kg, affects force needed to move
    [Range(0.1f, 100)]
    public float mass = 1f;

    //linear drag: how quickly object slows in air (0 = no draft, 10 = sluggish)
    [Range(0f, 10f)]
    public float drag = 0.5f;

    //angular drag: how quickly spinning obj slows down
    [Range(0, 10f)]
    public float angularDrag = 0.5f;

    [Header("Surface Properties")]
    //bounciness of surface (0 = no bounce, 1 = perfect bounce)
    //requires physics material on collider
    [Range(0, 1f)]
    public float bounciness = 0f;
    [Range(0, 1f)]
    public float friction = 0.5f;

    [Header("Puzzle Properties")]
    //effective weight used by PressurePlate
    //defaults to mass but can be overwritten
    public float puzzleWeight = -1f;

    Rigidbody rb;
    PhysicsMaterial physMat;
    public bool isHeld;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        ApplyRigidBodySettings();
        ApplySurfaceSettings();
    }

    //sets mass and drag directly
    void ApplyRigidBodySettings()
    {
        rb.mass = mass;
        rb.linearDamping = drag;
        rb.angularDamping = angularDrag;
    }

    //physics material in unity control bounce and friction
    //we create a physmat at runtime and assign it
    void ApplySurfaceSettings()
    {
        physMat = new PhysicsMaterial(gameObject.name);
        physMat.bounciness = bounciness;
        physMat.dynamicFriction = friction;
        physMat.staticFriction = friction;

        //combineMode.maximum means the higher friction of the two
        //colliding object wins good default for solid objects
        physMat.frictionCombine = PhysicsMaterialCombine.Average;
        physMat.bounceCombine = PhysicsMaterialCombine.Maximum;

        //assign material to collider
        Collider col = GetComponent<Collider>();
        if(col != null)
        {
            col.material = physMat;
        }
    }

    //preview in editor
    //when you change values in inspector during play mode, makes it apply immediately w/out restarting
    private void OnValidate()
    {
        //OnValidate runs in editor whenever an inspector value changes
        if(rb != null) ApplyRigidBodySettings();
    }
}
