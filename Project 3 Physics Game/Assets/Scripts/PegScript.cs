using UnityEngine;

public class PegScript : MonoBehaviour
{
    //when launched object hits the peg
    //give points equal to stored amount
    //halve stored amount
    //increase size
    //when peg grows big enough it "pops" and disappears
    public int storedScore;
    private int increasedTimes = 0;
    private Material pegMTL;
    private Color currentColor;
    private float hValue;
    private float sValue;
    private float lValue;
    private void Awake()
    {
        pegMTL = GetComponent<Renderer>().material;
        currentColor = pegMTL.color;
        Color.RGBToHSV(currentColor, out hValue, out sValue, out lValue);
    }
    private void Update()
    {
        pegMTL.color = currentColor;
        if(increasedTimes >= 5)
        {
            Destroy(gameObject);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.collider.tag);
        hValue += 0.1f;
        currentColor = Color.HSVToRGB(hValue, sValue, lValue);
        if(collision.collider.tag == "Launched")
        {
            Debug.Log($"Collided with {collision.collider}");
            
            transform.localScale += Vector3.one;
            increasedTimes++;
        }
    }
}
