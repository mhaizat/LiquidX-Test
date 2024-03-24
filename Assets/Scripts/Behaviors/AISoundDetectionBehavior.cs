using UnityEngine;
using UnityEngine.AI;

public class AISoundDetectionBehavior : AIBehaviorBase
{
    public override void UpdateBehavior(NavMeshAgent agent)
    {
        if (isActive)
        {
            if (Vector3.Distance(agent.transform.position, behaviorManager.GetSoundSourcePosition()) <= agent.stoppingDistance && !agent.pathPending)
            {
                behaviorManager.SetBehaviorState("Patrol");
                return;
            }

            agent.SetDestination(behaviorManager.GetSoundSourcePosition());
        }
    }
}
