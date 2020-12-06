using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour, IRightClickInteraction
{
    new public string name = "UNIT (MISSING NAME)";
    public NavMeshAgent navAgent;
    public Animator animator;

    public void OnRightClick(RaycastHit hit)
    {
        navAgent.SetDestination(hit.point);
    }

    private void Update()
    {
        animator.SetFloat("velocity", navAgent.velocity.magnitude);
    }
}
