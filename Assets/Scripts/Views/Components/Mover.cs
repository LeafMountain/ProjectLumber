using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Mover : MonoBehaviour {

	private NavMeshAgent agent;

	public float Velocity { get { return agent.velocity.magnitude; } }
	public Vector3 Destination { get { return agent.destination; } }

	public delegate void MoverEvent();
	public MoverEvent DestinationReached;

	private void Awake(){
		agent = GetComponent<NavMeshAgent>();
	}

	public void MoveTo(Vector3 destination){
		agent.SetDestination(destination);
		StartCoroutine("CheckIfDestinationReached");
	}

	IEnumerator CheckIfDestinationReached(){
		while(Vector3.Distance(transform.position, Destination) > agent.stoppingDistance){
			yield return new WaitForSeconds(.1f);
		}

		OnDestinationReached();
	}

	private void OnDestinationReached(){
		if(DestinationReached != null){
			DestinationReached.Invoke();
		}
	}
}
