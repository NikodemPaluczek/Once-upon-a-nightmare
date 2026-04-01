using UnityEngine;

public interface PickableObject
{
    void OnPick(Transform holdPoint);
    void OnDrop();
    void Rotate(Vector2 input);
}
