using UnityEngine;

public class PegSpawner : MonoBehaviour
{
    //controlled random number of how many pegs to spawn (about 9-20)
    private int spawnNumber;
    //stored prefab for peg
    public GameObject pegPrefab;
    private Transform spawnCollider;
    private Bounds spawnArea;
    private void Start()
    {
        spawnNumber = Random.Range(12, 30);
        spawnCollider = GetComponent<Transform>();
        spawnArea = new Bounds(spawnCollider.position, spawnCollider.localScale);
        Vector3 randPos;
        for (int i = spawnNumber; i >= 0; i--)
        {
            randPos = new Vector3(Random.Range(spawnArea.min.x, spawnArea.max.x), Random.Range(spawnArea.min.y, spawnArea.max.y), Random.Range(spawnArea.min.z, spawnArea.max.z));
            GameObject thisPeg = Instantiate(pegPrefab, randPos, Quaternion.identity);
            thisPeg.GetComponent<Renderer>().material.color = Random.ColorHSV();
        }
    }
}
