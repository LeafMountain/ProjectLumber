using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour, ICommandListener {

	private Interactable interactable;

	public void InteractWith(Interactable interactable){
		this.interactable = interactable;		
		StartCoroutine("Interact");
	}

	public void CommandRequested(Command command){		
		if(command.Interactable){
			InteractWith(command.Interactable);
		}
	}

	IEnumerator Interact(){		
		while(Vector3.Distance(transform.position, interactable.transform.position) > 1){			
			yield return new WaitForSeconds(.1f);
		}

		interactable.Interact();
	}
}
