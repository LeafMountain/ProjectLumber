using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Mover : MonoBehaviour {

	private NavMeshAgent agent;

	private void Awake(){
		agent = GetComponent<NavMeshAgent>();
	}

	public void MoveTo(Vector3 destination){
		agent.SetDestination(destination);
	}
}
