using UnityEngine;

public class BarrierScript : MonoBehaviour
{
    public GameObject barrier;
    void Start()
    {
        barrier.SetActive(false);
    }

    private void OnEnable()
    {
        PhysicsObject.OnEnd += ResetActive;
    }

    private void OnTriggerEnter(Collider other)
    {
        barrier.SetActive(true);
        gameObject.SetActive(false);
    }

    public void ResetActive()
    {
        barrier.SetActive(false);
        gameObject.SetActive(true);
    }
}
