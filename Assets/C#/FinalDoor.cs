using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalDoor : InteractableBase
{
    
    public override void Interact()
    {
        base.Interact();
        if (KeyStatusManager.Instance.isCorrectPicked)
        {
            SceneManager.LoadScene(2);

        }
        else
        {
            TimedLockPuzzleManager.Instance.StartPuzzle();
        }

        Key.KeyAlreadyTaken = false;
    }
    
}