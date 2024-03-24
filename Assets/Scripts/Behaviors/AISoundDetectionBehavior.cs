using UnityEngine;
using UnityEngine.AI;

public class AISoundDetectionBehavior : AIBehaviorBase
{
    public override void UpdateBehavior(NavMeshAgent agent)
    {
        if (isActive)
        {
            if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
            {
                behaviorManager.SetBehaviorState("Patrol");
                return;
            }

            agent.SetDestination(behaviorManager.GetSoundSourcePosition());
        }
    }
}
