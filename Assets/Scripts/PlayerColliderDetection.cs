using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerMovement;

public class PlayerColliderDetection : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Guard"))
        {
            Debug.Log("Guard is in");
            GlobalVariable._isGuardInProximity = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Guard"))
        { 
            GlobalVariable._isGuardInProximity = false;
            Debug.Log("Guard is out");
        }
    }
}
