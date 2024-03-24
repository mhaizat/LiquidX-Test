using UnityEngine;
using static PlayerMovement;

public class PlayerColliderDetection : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Guard"))
        {
            GlobalVariable._isGuardInProximity = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Guard"))
        { 
            GlobalVariable._isGuardInProximity = false;
        }
    }
}
