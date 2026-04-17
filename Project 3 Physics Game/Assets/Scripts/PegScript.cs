using JetBrains.Annotations;
using System;
using UnityEngine;

public class PegScript : MonoBehaviour
{
    //when launched object hits the peg
    //give points equal to stored amount
    //halve stored amount
    //increase size
    //when peg grows big enough it "pops" and disappears
    private int storedScore;
    private int increasedTimes = 0;
    private Material pegMTL;
    private Color currentColor;
    private float hValue;
    private float sValue;
    private float lValue;
    private RoundManager roundManager;

    public static event Action<int> onHit;
    private void Awake()
    {
        roundManager = UnityEngine.Object.FindFirstObjectByType<RoundManager>();
        GetComponent<Renderer>().material.color = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), 1f);
        int randScale = UnityEngine.Random.Range(4, 6);
        storedScore = (UnityEngine.Random.Range(roundManager.GetCurrentRound() * 256, roundManager.GetCurrentRound() * 512)) / randScale;
        transform.localScale = new Vector3(randScale, randScale, randScale);
        pegMTL = GetComponent<Renderer>().material;
        currentColor = pegMTL.color;
        //Color.RGBToHSV(currentColor, out hValue, out sValue, out lValue);
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
        currentColor = new Color(currentColor.r + .25f, currentColor.g -.25f, currentColor.b - .25f);
        //hValue += 0.1f;
        //currentColor = Color.HSVToRGB(hValue, sValue, lValue);
        if(collision.collider.tag == "Launched")
        {
            Debug.Log($"Collided with {collision.collider}");

            onHit?.Invoke(storedScore);
            collision.rigidbody.AddForce(collision.rigidbody.linearVelocity * .25f, ForceMode.Impulse);
            transform.localScale += Vector3.one;
            ScoreManager.UpdateScore(storedScore);
            storedScore = storedScore / 2;
            increasedTimes++;
        }
    }
}
