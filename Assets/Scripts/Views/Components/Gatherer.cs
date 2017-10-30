using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class Gatherer : MonoBehaviour {

	private Inventory inventory;

	private void Start(){
		inventory = GetComponent<Inventory>();
	}

	public void Gather(Storeable storeable){
		inventory.DepositItem(storeable);
	}
}
