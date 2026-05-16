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
    {
        Color c = thisObj.GetComponent<Renderer>().material.color;
        for (float alpha = 1f; alpha >= 0; alpha -= 0.1f)
        {
            c.a = alpha;
            thisObj.GetComponent<Renderer>().material.color = c;
            thisObj.GetComponent<Transform>().position += new Vector3(0, alpha * 10);
            yield return null;
        }
    }
}
