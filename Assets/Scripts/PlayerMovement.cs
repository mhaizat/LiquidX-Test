using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController characterController;

    [SerializeField] private Transform mainCamera;

    [SerializeField] private AIBehaviorManager AIBehaviorManager;

    private float movementSpeed = 1.0f;
    private float turnSmooothTime = 0.1f;
    float turnSmoothVelocity;

    [SerializeField] private AudioClip footstepSound;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Collider soundTrigger;

    float detectionradius = 10.0f;
    public static class GlobalVariable
    {
       public static bool _isGuardInProximity = false;
    }

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }
    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 1.0f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + mainCamera.eulerAngles.y;

            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmooothTime);
            transform.rotation = Quaternion.Euler(0, angle, 0);
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            characterController.Move(moveDirection.normalized * movementSpeed * Time.deltaTime);
        }

        if (Input.GetKeyUp(KeyCode.F))
        {
            Debug.Log("Make noise");
            if (GlobalVariable._isGuardInProximity)
            {
                AISoundDetectionBehavior sound = AIBehaviorManager.GetBehaviorState("Sound") as AISoundDetectionBehavior;
                sound.SetSoundSource(transform.position);
                AIBehaviorManager.ChangeBehaviorState("Sound");
            }

            //audioSource.PlayOneShot(footstepSound);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw detection radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionradius);
    }
}
