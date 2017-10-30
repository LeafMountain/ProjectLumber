using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CapsuleCollider))]
public class Selectable : MonoBehaviour {

	public UnityEvent selected;
	public UnityEvent deselected;

	public void Select(){
		selected.Invoke();
	}

	public void DeSelect(){
		deselected.Invoke();
	}

	public void OnRightClick(CommandModel command){
		ICommandListener[] commandListeners = GetComponents<ICommandListener>();

		GetComponent<UnitController>().NewCommand(command);

		// for (int i = 0; i < commandListeners.Length; i++)
		// {
		// 	commandListeners[i].CommandRequested(command);
		// }
	}

	public void OnShiftRightClick(CommandModel command) {
		GetComponent<UnitController>().AddCommand(command);		
	}
}