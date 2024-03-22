using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIPatrolBehavior : AIBehaviorBase
{
    [SerializeField] private Transform[] waypoints;

    private int currentWaypointIndex = 0;
    private float waitUntilNextWaypoint = 0;

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
                    BehaviorManager.ChangeBehaviorState("Chase");
                    return;
                }
            }

            SetNextWaypoint(agent);
        }
    }
    public void SetNextWaypoint(NavMeshAgent agent)
    {
        if (waypoints.Length == 0) return;

        if (agent.remainingDistance < 0.1f && !agent.pathPending)
        {
            waitUntilNextWaypoint += Time.deltaTime;
            if (waitUntilNextWaypoint >= 1)
            { 
                Debug.Log(currentWaypointIndex);
                agent.SetDestination(waypoints[currentWaypointIndex].position);
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
                waitUntilNextWaypoint = 0;    
            }
        }
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