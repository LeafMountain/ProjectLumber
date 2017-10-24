using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class ResourceNode : MonoBehaviour {

	private Inventory inventory;

	[SerializeField]
	private Item[] resources;

	private void Start(){
		inventory = GetComponent<Inventory>();

		inventory.ItemWithdrawn += OnItemRemoved;
	}

	private void AddItemsToInventory(){
		if(resources != null && resources.Length > 0){
			for (int i = 0; i < resources.Length; i++) {
				inventory.DepositItem(resources[i]);
			}
		}
	}

	private void OnItemRemoved(Item item){

	}

	public Item Gather(){
		return null;
	}

}
