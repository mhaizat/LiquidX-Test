using UnityEngine.AI;

public class AIChaseBehavior : AIBehaviorBase
{
    public override void UpdateBehavior(NavMeshAgent agent)
    {
        if (isActive)
        {
            if (behaviorManager.GetIsLooking())
            {
                agent.SetDestination(behaviorManager.GetLastKnownPlayerLocation());

                if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
                {
                    behaviorManager.SetBehaviorState("Patrol");
                    return;
                }
            }
        }
    }
}
