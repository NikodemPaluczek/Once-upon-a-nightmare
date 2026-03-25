using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    public float pickupRange = 3f;
    public Transform holdPoint;
    public LayerMask pickupLayer;

    private PickupObject heldObject;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldObject == null)
                TryPickup();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            DropObject();
        }
    }

    void TryPickup()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickupRange, pickupLayer))
        {
            PickupObject obj = hit.collider.GetComponent<PickupObject>();

            if (obj != null)
            {
                heldObject = obj;
                obj.Pickup(holdPoint);
            }
        }
    }

    void DropObject()
    {
        if (heldObject != null)
        {
            heldObject.Drop();
            heldObject = null;
        }
    }
}