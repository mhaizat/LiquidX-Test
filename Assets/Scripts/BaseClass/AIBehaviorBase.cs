using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class AIBehaviorBase : MonoBehaviour
{
    public string Key;
    protected bool isActive;
    [SerializeField] private Material agentMaterial;

    protected AIBehaviorManager BehaviorManager;

    public virtual void Start()
    {
        BehaviorManager = GetComponent<AIBehaviorManager>();
    }

    public virtual void Activate()
    {
        isActive = true;
    }

    public virtual void Deactivate()
    {
        isActive = false;
    }

    public abstract void UpdateBehavior(NavMeshAgent agent);
}