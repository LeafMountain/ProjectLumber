using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class Extractable : MonoBehaviour {

	[SerializeField]
	private int extractTime;

	private Inventory inventory;

	public void Start(){
		inventory = GetComponent<Inventory>();
	}

	public Item Extract(){
		return inventory.WithdrawItem(inventory.Items[0]);
	}
}
