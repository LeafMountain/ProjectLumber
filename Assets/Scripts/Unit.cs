using System.Security.Cryptography.X509Certificates;
using System.Threading;
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
    public Interactable interactable;

    public RaycastHit currentHit;

    private IEnumerator ticker;

    private void Start()
    {
        ticker = Tick();
        StartCoroutine(ticker);
    }

    public void OnRightClick(RaycastHit hit)
    {
        StopCoroutine(ticker);
        currentHit = hit;
        StartCoroutine(ticker);
    }

    public IEnumerator DoInteractionSequence(RaycastHit hit)
    {
        if(hit.transform)
        {
            if(hit.transform.GetComponent<Storable>() is Storable storable)
            {
                yield return MoveToPosition(hit.point);
                inventory.Deposit(storable);
                currentHit = new RaycastHit();
            }
            else if(hit.transform.GetComponent<Building>() is Building building)
            {
                yield return MoveToPosition(hit.point);
                animator.SetTrigger("attackMelee");
                yield return new WaitForSeconds(.5f);
                building.GetComponent<Health>().Damage(-1, interactable);
                yield return new WaitForSeconds(.2f);
            }
            // else if(hit.transform.GetComponent<Unit>() is Unit unit)
            // {
            //     yield return MoveToPosition(hit.point);
            //     animator.SetTrigger("attackMelee");
            //     unit.GetComponent<Health>().Damage(-1, interactable);
            //     yield return new WaitForSeconds(1f);
            // }
            else
            {
                navAgent.SetDestination(hit.point);
                currentHit = new RaycastHit();
            }
        }
    }

    private IEnumerator MoveToPosition(Vector3 position)
    {
        navAgent.SetDestination(position);
        yield return null;
        while(navAgent.remainingDistance > navAgent.stoppingDistance)
        {
            yield return null;
        }
    }

    public IEnumerator Tick()
    {
        while(true)
        {
            if(currentHit.transform)
            {
                yield return DoInteractionSequence(currentHit);
            }
            yield return new WaitForSeconds(1f);
        }
    }

    private void LateUpdate()
    {
        animator.SetFloat("velocity", navAgent.velocity.magnitude);
    }
}
