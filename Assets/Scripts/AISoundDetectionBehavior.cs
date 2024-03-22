using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class AISoundDetectionBehavior : AIBehaviorBase
{
    public Vector3 soundSource;
    public LayerMask obstacleLayer;

    private float distanceThreshold = 0.1f;

    public override void UpdateBehavior(NavMeshAgent agent)
    {
        if (isActive)
        {
            Vector3 direction = soundSource - agent.transform.position;

            RaycastHit hit;
            if (Physics.Raycast(agent.transform.position, direction, out hit, Mathf.Infinity, obstacleLayer))
            {
                //! if being blocked
                Debug.DrawRay(agent.transform.position, direction, Color.red);
                BehaviorManager.ChangeBehaviorState("Patrol");
                Debug.Log("It is not being seen");
            }
            else
            {
                //! if not being blocked
                Debug.DrawRay(agent.transform.position, direction, Color.green);

                float distance = Vector3.Distance(agent.transform.position, soundSource);

                if (distance < distanceThreshold)
                {
                    //! if player is in FOV
                    BehaviorManager.ChangeBehaviorState("Patrol");
                    return;
                }

                Debug.Log("It is being seen");
                agent.SetDestination(soundSource);
            }
        }
    }

    public void SetSoundSource(Vector3 soundPosition)
    {
        soundSource = soundPosition;
    }
}
