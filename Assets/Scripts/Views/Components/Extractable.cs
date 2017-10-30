using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class Extractable : MonoBehaviour {

	[SerializeField]
	private int extractTime;

	private Inventory inventory;
	private Inventory extractorInventory;

	public void Start(){
		inventory = GetComponent<Inventory>();
	}

	public void Extract(Inventory extractorInventory){
		this.extractorInventory = extractorInventory;
		StartCoroutine("_Extract");
	}

	IEnumerable _Extract(){
		yield return new WaitForSeconds(extractTime);
		extractorInventory.DepositItem(inventory.WithdrawItem());
	}

	public void Interact(){
		Extract(null);
	}
}
