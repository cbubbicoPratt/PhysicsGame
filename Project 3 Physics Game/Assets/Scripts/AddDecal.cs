using System.Collections;
using TMPro;
using UnityEngine;

public class AddDecal : MonoBehaviour
{
    public Canvas canvas;
    public GameObject add;
    private TextMeshProUGUI addText;

    private void Start()
    {
        addText = add.GetComponent<TextMeshProUGUI>();
    }
    private void OnEnable()
    {
        PegScript.onHit += ShowDecal;
    }

    public void ShowDecal(int number)
    {
        GameObject thisText = Instantiate(add, Vector3.zero, Quaternion.identity);
        Debug.Log("Instantiated!");
        thisText.GetComponent<TextMeshProUGUI>().text = "+" + number;
        thisText.transform.parent = canvas.transform;
        StartCoroutine(FloatUp(thisText));
        Destroy(thisText);
    }

    public IEnumerator FloatUp(GameObject thisObj)
<<<<<<< HEAD
<<<<<<< HEAD
    { 
        for (float alpha = 1f; alpha >= 0; alpha -= 0.1f)
        {
            thisObj.GetComponent<TextMeshPro>().alpha -= alpha;
=======
    {
        Color c = thisObj.GetComponent<Renderer>().material.color;
        for (float alpha = 1f; alpha >= 0; alpha -= 0.1f)
        {
            c.a = alpha;
            thisObj.GetComponent<Renderer>().material.color = c;
>>>>>>> parent of 7b513ab (Plahytested, tweaked pegs but they still spawn out of bounds :/)
=======
    {
        Color c = thisObj.GetComponent<TextMeshPro>().color;
        for (float alpha = 1f; alpha >= 0; alpha -= 0.1f)
        {
            c.a = alpha;
            thisObj.GetComponent<TextMeshPro>().color = c;
>>>>>>> parent of e8a8b4a (Fixed Peg issues, added color and made horse ball)
            thisObj.GetComponent<Transform>().position += new Vector3(0, alpha * 10);
            yield return null;
        }
    }
}
