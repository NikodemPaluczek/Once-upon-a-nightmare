using UnityEngine;

public class FinalDoor : MonoBehaviour, IInteractable
{
    [SerializeField] private Renderer targetRenderer;

    [SerializeField] private GameObject gameWon;

    private uint _defaultLayerMask;
    private uint _outlineLayerMask = 1u << 8;

    private void Awake()
    {
        _defaultLayerMask = targetRenderer.renderingLayerMask;
    }

    public void Interact()
    {
        if (KeyStatus.Instance.isCorrectPicked)
        {
            if (gameWon != null)
            {
                gameWon.SetActive(true);
            }
        }
        else
        {
            TimedLockPuzzleManager.Instance.StartPuzzle();
        }

        KeyManager.KeyAlreadyTaken = false;
    }

    public void Highlight(bool state)
    {
        if (state)
        {
            targetRenderer.renderingLayerMask =
                _defaultLayerMask | _outlineLayerMask;
        }
        else
        {
            targetRenderer.renderingLayerMask =
                _defaultLayerMask;
        }
    }
}