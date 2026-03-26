using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    //color object glows when player looks at it
    public Color highlightColor = Color.magenta;
    //how strongly the highlight color blends with original
    //0 = no effect; 1 = full replace
    [SerializeField, Range(0, 1f)] public float highlightStrength = 0.4f;

    private Renderer objectRenderer; //render comp on obj
    private Color originalColor; //color before highlight applied
    private bool isHighlighted = false; //check for highlighted

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        objectRenderer = GetComponent<Renderer>(); //cache renderer so not calling get comp each frame
        if(objectRenderer != null)
        {
            //store original color
            //read from material property
            //use sharedmaterial to read base color without instancing
            originalColor = objectRenderer.sharedMaterial.color;
        }
        else
        {
            Debug.Log($"Interactable object on {gameObject.name} has no renderer, can't highlight");
        }
    }

    public void Highlight()
    {
        if(isHighlighted && objectRenderer == null)
        {
            Debug.Log("no obj renderer & isHighlighted is true");
            return;
        }

        //color.lerp blends original and highlighted by highlight strength amt
        //use material not shared material to create unique instance so we don't effect all objects using same material
        objectRenderer.material.color = Color.Lerp(originalColor, highlightColor, highlightStrength);
        isHighlighted = true;
    }

    public void UnHighlight()
    {
        //called by object grabber when player looks away
        //removes highlight
        if (!isHighlighted && objectRenderer == null) return;

        objectRenderer.material.color = originalColor;
        isHighlighted = false;
    }
}
