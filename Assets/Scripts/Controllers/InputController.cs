using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : ControllerElement {

	public delegate void InputEvent();
	public InputEvent MoveKeys;

	public InputController (GameApplication app) : base (app) { }

	private void OnMoveKeys(){

	}
}
