using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class Extractable : MonoBehaviour {

	[SerializeField]
	private int extractTime;

	private Inventory inventory;
	private Gatherer gatherer;

	public void Start(){
		inventory = GetComponent<Inventory>();
	}

	public void Extract(Gatherer gatherer){
		this.gatherer = gatherer;
		StartCoroutine("_Extract");
	}

	IEnumerable _Extract(){
		yield return new WaitForSeconds(extractTime);
		
		
		gatherer.Gather(inventory.WithdrawItem());
	}

	public void Interact(){
		Extract(null);
	}
}
