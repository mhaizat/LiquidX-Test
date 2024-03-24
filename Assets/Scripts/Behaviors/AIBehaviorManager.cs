using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class AIbehaviorManager : MonoBehaviour
{
    [SerializeField] private List<AIBehaviorBase> Behaviors;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Vector3 playerLastKnownLocation;
    [SerializeField] private Vector3 soundSourcePosition;

    private bool _isLooking;

    private AIBehaviorBase currentState;

    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Material agentMaterial;

    [SerializeField] private LayerMask obstacleLayerMask;

    public LayerMask GetObstacleLayerMask() { return obstacleLayerMask; }
    public Material GetAgentMaterial() { return agentMaterial; }
    public bool GetIsLooking() { return _isLooking; }
    public Transform GetPlayerTransform() { return playerTransform; }
    public Vector3 GetSoundSourcePosition() { return soundSourcePosition; }
    public Vector3 GetLastKnownPlayerLocation() { return playerLastKnownLocation; }
    public NavMeshAgent GetAgent() { return agent; }

    public void SetSoundSourcePosition(Vector3 _soundSourcePosition) { soundSourcePosition = _soundSourcePosition; }
    public void SetLastKnownPlayerLocation() { playerLastKnownLocation = playerTransform.position; }
    public void SetIsLooking(bool isLooking) { _isLooking = isLooking; }
    public void SetAgentMaterial(Color materialColor) { agentMaterial.color = materialColor; }

    void Start()
    {
        if (!currentState)
        {
            currentState = Behaviors.OfType<AIPatrolBehavior>().FirstOrDefault();
            currentState.Activate();
        }
    }

    void Update()
    {
        if (currentState)
        {
            currentState.UpdateBehavior(agent);
        }
    }

    public void SetBehaviorState(string behaviorKey)
    {
        currentState.Deactivate();

        foreach (AIBehaviorBase behaviorState in Behaviors)
        {
            if (behaviorState.behaviorID == behaviorKey)
            {
                currentState = behaviorState;
            }
        }

        currentState.Activate();
    }

    public AIBehaviorBase GetBehaviorState(string behaviorKey)
    {
        foreach (AIBehaviorBase behaviorState in Behaviors)
        {
            if (behaviorState.behaviorID == behaviorKey)
            {
                if (currentState == behaviorState)
                return currentState;
            }
        }

        return null;
    }
}
