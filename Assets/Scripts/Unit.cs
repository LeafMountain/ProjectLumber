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
            if(hit.transform.GetComponent<IUnitInteractable>() is IUnitInteractable unitInteractable)
            {
                yield return unitInteractable.DoInteraction(this);
            }
            else
            {
                navAgent.SetDestination(hit.point);
                currentHit = new RaycastHit();
            }
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

    public IEnumerator LookAt(Interactable interactable)
    {
        Vector3 lookRotation = interactable.transform.position - transform.position;
        while (Vector3.Angle(transform.forward, lookRotation) > 5f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(lookRotation), Time.deltaTime * 10f);
            yield return null;
        }
    }
}
