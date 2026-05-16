using UnityEngine;

public class PegSpawner : MonoBehaviour
{
    //controlled random number of how many pegs to spawn (about 9-20)
    private int spawnNumber;
    //stored prefab for peg
    public GameObject pegPrefab;
<<<<<<< HEAD
=======
    private Transform spawnCollider;
    private Bounds spawnArea;
>>>>>>> parent of 7b513ab (Plahytested, tweaked pegs but they still spawn out of bounds :/)

    private void OnEnable()
    {
        RoundManager.onUpdate += SpawnPegs;
    }
    private void Start()
    {
        SpawnPegs();
    }

    public void SpawnPegs()
    {
        spawnNumber = Random.Range(12, 30);
        spawnCollider = GetComponent<Transform>();
        spawnArea = new Bounds(spawnCollider.position, spawnCollider.localScale);
        Vector3 randPos;
        for (int i = spawnNumber; i >= 0; i--)
        {
<<<<<<< HEAD

            //x = Random.Range(0f, 1f);
            offsetX = Random.Range(-10f, 10f);
            offsetY = Random.Range(-5f, 2f);
            offsetZ = Random.Range(-20f, 24f);
            targetPos = new Vector3(offsetX,offsetY,offsetZ);
            GameObject thisPeg = Instantiate(pegPrefab, gameObject.transform.position, Quaternion.identity, gameObject.transform);
            thisPeg.transform.localPosition = targetPos;
=======
            randPos = new Vector3(Random.Range(spawnArea.min.x, spawnArea.max.x), Random.Range(spawnArea.min.y, spawnArea.max.y), Random.Range(spawnArea.min.z, spawnArea.max.z));
            GameObject thisPeg = Instantiate(pegPrefab, randPos, Quaternion.identity);
            thisPeg.GetComponent<Renderer>().material.color = Random.ColorHSV();
>>>>>>> parent of 7b513ab (Plahytested, tweaked pegs but they still spawn out of bounds :/)
        }
    }
}
