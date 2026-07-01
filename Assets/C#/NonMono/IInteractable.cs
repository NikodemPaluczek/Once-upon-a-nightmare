using UnityEngine;

public interface IInteractable
{
    void Interact();
    void CancelInteraction();
    void Highlight(bool state);
}
