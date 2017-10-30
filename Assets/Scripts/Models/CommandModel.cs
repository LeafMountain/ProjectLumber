using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandModel {

	public Vector3 Position { get; private set; }
	public Interactable Interactable { get; private set; }

	public CommandModel (Vector3 position, Interactable interactable){
		Position = position;
		Interactable = interactable;
	}
}
