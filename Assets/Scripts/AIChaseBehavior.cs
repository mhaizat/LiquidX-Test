using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIChaseBehavior : AIBehaviorBase
{
    public float visionRange = 1f; // Range at which the AI can detect the player
    public LayerMask obstacleLayer;
    public float fieldOfViewAngle = 90f;

    public override void UpdateBehavior(NavMeshAgent agent)
    {
        if (isActive)
        {
            if (Vector3.Distance(agent.transform.position, BehaviorManager.GetPlayerTransform().position) < visionRange)
            {
                // Check if there is a clear line of sight to the player
                if (HasLineOfSight(agent))
                {
                    Debug.Log("Player is in sight!");
                    //BehaviorManager.ChangeBehaviorState("Chase");
                    //agent.SetDestination(BehaviorManager.GetLastKnownPlayerLocation());
                    //return;
                }

                BehaviorManager.ChangeBehaviorState("Patrol");
                return;
            }

            Debug.Log("Chase the player bro");
            agent.SetDestination(BehaviorManager.GetLastKnownPlayerLocation());
            //! get to the location
            //! check to see if there is a player in sight
            //! if yes: chase again
            //! if not: resume patrol
        }
    }

    public void SetPlayerLastKnownLocation(Vector3 lastPosition)
    {
        BehaviorManager.SetLastKnownPlayerLocation(lastPosition);
    }

    private bool HasLineOfSight(NavMeshAgent agent)
    {
        Vector3 directionToPlayer = BehaviorManager.GetPlayerTransform().position - agent.transform.position;
        float angle = Vector3.Angle(agent.transform.forward, directionToPlayer);

        // Check if the player is within the vision range and within the field of view
        if (angle < fieldOfViewAngle && directionToPlayer.magnitude < visionRange)
        {
            // Check if there is a clear line of sight to the player
            RaycastHit hit;
            if (Physics.Raycast(agent.transform.position, directionToPlayer, out hit, visionRange, ~obstacleLayer))
            {
                if (hit.transform == BehaviorManager.GetPlayerTransform().transform)
                {
                    // Player is in line of sight
                    Debug.DrawRay(agent.transform.position, directionToPlayer, Color.green);
                    return true;
                }
            }
        }

        return false;
    }
}
