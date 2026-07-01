using UnityEngine;
using static UnityEngine.UI.Image;

public class Splinde : InteractableBase
{
    private bool _canBeHighlighted = true;

    public override void Interact()
    {
        SleepManager.Instance.ChangeSleepState();
    }

    public override void Highlight(bool state)
    {
        if (!_canBeHighlighted) return;

        base.Highlight(state);
    }
}
