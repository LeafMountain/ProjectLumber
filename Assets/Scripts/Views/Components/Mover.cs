using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Mover : MonoBehaviour, ICommandListener {

	private NavMeshAgent agent;

	public float Velocity { get { return agent.velocity.magnitude; } }

	private void Awake(){
		agent = GetComponent<NavMeshAgent>();
	}

	public void MoveTo(Vector3 destination){
		agent.SetDestination(destination);
	}

	public void RightClicked(RaycastHit hit){
		MoveTo(hit.point);
	}
}
