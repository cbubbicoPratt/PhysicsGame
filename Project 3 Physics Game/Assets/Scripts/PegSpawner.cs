using UnityEngine;

public class PegSpawner : MonoBehaviour
{
    //controlled random number of how many pegs to spawn (about 9-20)
    private int spawnNumber;
    //stored prefab for peg
    public GameObject pegPrefab;

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
        Vector3 targetPos;
        //float x;
        float offsetX;
        float offsetY;
        float offsetZ;
        for (int i = spawnNumber; i >= 0; i--)
        {

            //x = Random.Range(0f, 1f);
            offsetX = Random.Range(-10f, 10f);
            offsetY = Random.Range(-5f, 2f);
            offsetZ = Random.Range(-20f, 24f);
            targetPos = new Vector3(offsetX,offsetY,offsetZ);
            GameObject thisPeg = Instantiate(pegPrefab, gameObject.transform.position, Quaternion.identity, gameObject.transform);
            thisPeg.transform.localPosition = targetPos;
        }
    }
}
