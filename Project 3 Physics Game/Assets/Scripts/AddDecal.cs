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
        thisText.transform.SetParent(canvas.transform);
        StartCoroutine(FloatUp(thisText));
        Destroy(thisText);
    }

    public IEnumerator FloatUp(GameObject thisObj)
    { 
        for (float alpha = 1f; alpha >= 0; alpha -= 0.1f)
        {
            thisObj.GetComponent<TextMeshPro>().alpha -= alpha;
            thisObj.GetComponent<Transform>().position += new Vector3(0, alpha * 10);
            yield return null;
        }
    }
}
