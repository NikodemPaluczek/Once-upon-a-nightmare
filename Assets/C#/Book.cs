using UnityEngine;

public class Book : MonoBehaviour, IHighlightable, IInteractable
{
    [SerializeField] private Material highlightMaterial;
    [SerializeField] private Renderer targetRenderer;

    private Renderer rend;
    private Material[] original;

    [SerializeField] private GameObject canvasGameObject;
    private bool _shouldShow = true;


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
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (_shouldShow)
            {
                InputManager.Instance.DisableAllControls();
            }
            else
            {
                InputManager.Instance.EnablePlayerControls();
            }
                canvasGameObject.SetActive(_shouldShow);
            _shouldShow = !_shouldShow;
        }
    }

    public void Interact()
    {
        if (_shouldShow)
        {
            InputManager.Instance.DisableAllControls();
        }
        else
        {
            InputManager.Instance.EnablePlayerControls();
        }
        canvasGameObject.SetActive(_shouldShow);
        _shouldShow = !_shouldShow;
    }

    public void Highlight(bool state)
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
