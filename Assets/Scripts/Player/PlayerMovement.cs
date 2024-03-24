using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController characterController;

    [SerializeField] private Transform mainCamera;

    [SerializeField] private AIbehaviorManager behaviorManager;

    [SerializeField] private float movementSpeed = 1.0f;
    [SerializeField] private float turnSmooothTime = 0.1f;
    [SerializeField] private float turnSmoothVelocity;

    [SerializeField] private AudioClip soundEffectClip;
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private LayerMask obstacleMask;

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
            if (GlobalVariable._isGuardInProximity)
            {
                audioSource.PlayOneShot(soundEffectClip);
                
                Vector3 playerDirection = transform.position - behaviorManager.GetAgent().transform.position;
                float distanceToPlayer = playerDirection.magnitude;
                playerDirection.Normalize();

                RaycastHit hit;
                if (Physics.Raycast(behaviorManager.GetAgent().transform.position, playerDirection, out hit, distanceToPlayer, obstacleMask))
                {
                    return;
                }

                if (behaviorManager.GetBehaviorState("Patrol"))
                { 
                    behaviorManager.SetSoundSourcePosition(transform.position);
                    behaviorManager.SetBehaviorState("Sound");
                }
            }

        }
    }
}
