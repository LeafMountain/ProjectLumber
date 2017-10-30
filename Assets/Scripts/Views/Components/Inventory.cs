using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

	[SerializeField]
	private int inventorySlots;
	public int InventorySlots{ get { return inventorySlots; }}

	[SerializeField]
	private Storeable[] storeables;
	public Storeable[] Storeables { get{ return storeables; }}

	[SerializeField]
	private Transform[] storePositions;

	public bool Full { 
		get{
			for (int i = 0; i < InventorySlots; i++){
				if(storeables[i] == null){
					return false;
				}
			}
			return true;
		}
	}

	private void Awake(){
		storeables = new Storeable[InventorySlots];
	}

	public bool DepositItem(Storeable storeable){
		for (int i = 0; i < InventorySlots; i++){
			if(storeables[i] == null){
				storeables[i] = storeable;

				storeable.transform.SetParent(storePositions[i]);
				storeable.transform.position = storePositions[i].position;

				return true;
			}
		}

		return false;
	}

	public Storeable WithdrawItem(Storeable storeable){
		for (int i = InventorySlots - 1; i >= 0; i--){
			if(storeables[i] == storeable){
				storeables[i] = null;
				storePositions[i] = null;				

				return storeable;
			}
		}
	
		return null;
	}

	public Storeable WithdrawItem(){
		for (int i = 0; i < InventorySlots; i++){
			if(Storeables[i] != null){
				return Storeables[i];
			}
		}
	
		return null;
	}

	public Storeable FindItemOfType(){
		return null;
	}
}
