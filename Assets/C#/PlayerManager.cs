using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    private void OnEnable()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            GameObject.Destroy(this);
        }
    }

    private void Update()
    {
        PlayerLocomotion.Instance.ManageMovement();   
    }

    private void FixedUpdate()
    {
       // ManageRotation();
    }


}
