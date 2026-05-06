using UnityEngine;
using static UnityEngine.UI.Image;

public class Wrzeciono : MonoBehaviour, IHighlightable
{
    [SerializeField] private Material highlightMaterial;
    [SerializeField] private Renderer targetRenderer;

    private Renderer rend;
    private Material[] original;

    private void Awake()
    {
        rend = targetRenderer;
        original = rend.sharedMaterials;
    }
    public void SetHighlight(bool state)
    {
        if (state)
        {
            var mats = rend.sharedMaterials;

            Material[] newMats = new Material[mats.Length];

            for (int i = 0; i < newMats.Length; i++)
                newMats[i] = highlightMaterial;

            rend.materials = newMats;
        }
        else
        {
            rend.materials = original;
        }
    }

}
