using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

	[SerializeField]
	private int inventorySlots;
	public int InventorySlots{ get { return inventorySlots; }}

	[SerializeField]
	private Item[] items;
	public Item[] Items { get{ return items; }}

	private void Awake(){
		items = new Item[InventorySlots];
	}

	public bool DepositItem(Item item){
		for (int i = 0; i < InventorySlots; i++){
			if(items[i] == null){
				items[i] = item;
				OnItemDeposited(item);
				return true;
			}
		}

		return false;
	}

	public Item WithdrawItem(Item item){
		for (int i = 0; i < InventorySlots; i++){
			if(items[i] == item){
				items[i] = null;
				OnItemWithdrawn(item);
				return item;
			}
		}
	
		return null;
	}

	public Item FindItemOfType(){
		return null;
	}

#region Events

	public delegate void InventoryEvent(Item item);

	public InventoryEvent ItemDeposited;
	public InventoryEvent ItemWithdrawn;

	private void OnItemDeposited(Item item){
		if(ItemDeposited != null){
			ItemDeposited.Invoke(item);
		}
	}

	private void OnItemWithdrawn(Item item){
		if(ItemWithdrawn != null){
			ItemWithdrawn.Invoke(item);
		}
	}
#endregion
}
