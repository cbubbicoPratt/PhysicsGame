using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    //list for prefabs
    //transform for spawn position
    public GameObject[] prefabs;

    private void OnEnable()
    {
        PhysicsObject.OnEnd += SpawnObject;
    }

    private void Start()
    {
        SpawnObject();
    }

    public void SpawnObject()
    {
        Instantiate(prefabs[Random.Range(0, prefabs.Length)], transform.position, Quaternion.identity);
    }
}
