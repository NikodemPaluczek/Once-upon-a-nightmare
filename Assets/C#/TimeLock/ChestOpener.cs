using UnityEngine;
using DG.Tweening;

public class ChestOpener : InteractableBase
{
    [SerializeField] private Transform lockTransform;
    [SerializeField] private Transform lidTransform;

    [SerializeField] private float lockDropDistance = 0.15f;
    [SerializeField] private float lockDropDuration = 0.3f;
    [SerializeField] private float lidOpenDuration = 0.5f;
    
    public static ChestOpener Instance;

    private void Awake()
    {
        Instance = this;
    }

    public override void Interact()
    {
        base.Interact();
        
        TimedLockPuzzleManager.Instance.ShowLockCanvas();
    }
    
    public void OpenChest()
    {
        lockTransform.DOKill();
        lidTransform.DOKill();

        Sequence sequence = DOTween.Sequence();

        sequence.Append(
            lockTransform.DOLocalMove(
                lockTransform.localPosition + Vector3.down * lockDropDistance,
                lockDropDuration)
        );

        sequence.Append(
            lidTransform.DOLocalRotate(
                lidTransform.localEulerAngles + new Vector3(-90f, 0f, 0f),
                lidOpenDuration)
        );
    }
}