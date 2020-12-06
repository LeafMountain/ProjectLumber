using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour, IRightClickInteraction
{
    new public string name = "UNIT (MISSING NAME)";
    public NavMeshAgent navAgent;
    public Animator animator;
    public Inventory inventory;

    public void OnRightClick(RaycastHit hit)
    {
        DoInteractionSequence(hit);
    }

    public async Task DoInteractionSequence(RaycastHit hit)
    {
        if(hit.transform)
        {
            if(hit.transform.GetComponent<Storable>() is Storable storable)
            {
                navAgent.SetDestination(hit.point);
                while(navAgent.remainingDistance > navAgent.stoppingDistance)
                {
                    await Task.Yield();
                }
                inventory.Deposit(storable);
            }
            else
            {
                navAgent.SetDestination(hit.point);
            }
        }
    }

    private void Update()
    {
        animator.SetFloat("velocity", navAgent.velocity.magnitude);
    }
}
