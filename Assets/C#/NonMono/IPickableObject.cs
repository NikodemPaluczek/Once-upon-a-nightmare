using UnityEngine;

public interface IPickableObject
{
    void OnPick(Transform holdPoint);
    void OnDrop();
    void Rotate(Vector2 input);
}
