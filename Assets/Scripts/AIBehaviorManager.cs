using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;

public class AIBehaviorManager : MonoBehaviour
{
    [SerializeField] private List<AIBehaviorBase> Behaviors;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Vector3 playerLastKnownLocation;

    private AIBehaviorBase currentState;

    [SerializeField] private NavMeshAgent agent;
    void Start()
    {
        if (!currentState)
        {
            currentState = Behaviors.OfType<AIPatrolBehavior>().FirstOrDefault();
            currentState.Activate();
            currentState.UpdateBehavior(agent);
        }
    }

    void Update()
    {
        if (currentState)
        {
            currentState.UpdateBehavior(agent);
        }
    }

    public void ChangeBehaviorState(string behaviorKey)
    {
        currentState.Deactivate();

        foreach (AIBehaviorBase behaviorState in Behaviors)
        {
            if (behaviorState.Key == behaviorKey)
            {
                currentState = behaviorState;
                Debug.Log("State change to: " + currentState);
            }
        }

        currentState.Activate();
        currentState.UpdateBehavior(agent);
    }

    public AIBehaviorBase GetBehaviorState(string behaviorKey)
    {
        foreach (AIBehaviorBase behaviorState in Behaviors)
        {
            if (behaviorState.Key == behaviorKey)
            {
                Debug.Log("State is: " + currentState);
                return behaviorState;
            }
        }

        return null;
    }

    public void SetLastKnownPlayerLocation(Transform playerLocation)
    {
        playerTransform = playerLocation;
    }

    public Transform GetPlayerTransform() { return playerTransform; }
    public Vector3 GetLastKnownPlayerLocation() { return playerLastKnownLocation; }
    public void SetLastKnownPlayerLocation(Vector3 lastPosition) { playerLastKnownLocation = lastPosition; }

}
