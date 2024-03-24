using UnityEngine;
using UnityEngine.AI;

public abstract class AIBehaviorBase : MonoBehaviour
{
    public string behaviorID;
    protected bool isActive;

    [SerializeField] private Color colorState;

    protected AIbehaviorManager behaviorManager;

    public virtual void Start()
    {
        behaviorManager = GetComponent<AIbehaviorManager>();
    }

    public virtual void Activate()
    {
        isActive = true;
        behaviorManager.SetAgentMaterial(colorState);
    }

    public virtual void Deactivate()
    {
        isActive = false;
    }

    public abstract void UpdateBehavior(NavMeshAgent agent);
}