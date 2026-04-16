using UnityEngine;

public class PegSpawner : MonoBehaviour
{
    //controlled random number of how many pegs to spawn (about 9-20)
    private int spawnNumber;
    //stored prefab for peg
    public GameObject pegPrefab;
    public GameObject area;
    public Transform areaMin;
    public Transform areaMax;

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
            offsetY = Random.Range(-8.4f, 1.59f);
            offsetZ = Random.Range(areaMin.transform.position.z, areaMax.transform.position.z);
            targetPos = new Vector3(area.transform.position.x + offsetX, area.transform.position.y + offsetY, area.transform.position.z + offsetZ);
            GameObject thisPeg = Instantiate(pegPrefab, targetPos, Quaternion.identity);
            thisPeg.transform.SetParent(area.transform, true);
            thisPeg.GetComponent<Renderer>().material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
        }
    }
}
