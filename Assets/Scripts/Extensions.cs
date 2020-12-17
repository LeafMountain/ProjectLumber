using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public static class Extensions
{
    public static IEnumerator MoveToPosition(this NavMeshAgent agent, Vector3 destination)
    {
        agent.SetDestination(destination);
        yield return null;
        while (agent.remainingDistance > agent.stoppingDistance + 1f)
        {
            yield return null;
        }
    }

    public static IEnumerator MoveToInteractable(this NavMeshAgent agent, Interactable interactable)
    {
        Vector3 targetPosition = interactable.GetComponent<Collider>().bounds.ClosestPoint(agent.transform.position);
        yield return agent.MoveToPosition(targetPosition);
    }
}
