using UnityEngine;

public class Journal : InteractableBase
{
    [SerializeField] private Renderer targetRenderer;

    [SerializeField] private GameObject canvasGameObject;



    public override void CancelInteraction()
    {
        HideJournal();

        base.CancelInteraction();
    }

    public override void Interact()
    {
        base.Interact();
        ShowJournal();
    }

    private void ShowJournal()
    {
        canvasGameObject.SetActive(true);
        InputManager.Instance.EnableObjectControls();

    }

    private void HideJournal()
    {
        canvasGameObject.SetActive(false);
        InputManager.Instance.EnablePlayerControls();
    }
    


}