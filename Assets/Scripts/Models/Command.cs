using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Command {

	public Vector3 Position { get; private set; }
	public Interactable Interactable { get; private set; }

	public Command (Vector3 position, Interactable interactable){
		Position = position;
		Interactable = interactable;
	}
}
