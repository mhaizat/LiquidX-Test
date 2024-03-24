using UnityEngine;
using UnityEngine.AI;

public class AIPatrolBehavior : AIBehaviorBase
{
    [SerializeField] private Transform[] waypoints;

    private int currentWaypointIndex = 0;
    private float waitUntilNextWaypoint = 0;

    [SerializeField] private LayerMask obstacleLayer;

    public override void Activate()
    {
        base.Activate();
        behaviorManager.SetIsLooking(false);
    }

    public override void Deactivate()
    {
        base.Deactivate();
        behaviorManager.SetIsLooking(true);
    }

    public override void UpdateBehavior(NavMeshAgent agent)
    {
        if (isActive)
        {
            waitUntilNextWaypoint += Time.deltaTime;
            if (waitUntilNextWaypoint >= 1)
            {
                SetNextWaypoint(agent);
                waitUntilNextWaypoint = 0;
            }
        }
    }
    public void SetNextWaypoint(NavMeshAgent agent)
    {
        if (waypoints.Length == 0) return;

        agent.SetDestination(waypoints[currentWaypointIndex].position);

        if (agent.remainingDistance < 0.1f && !agent.pathPending)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }
}