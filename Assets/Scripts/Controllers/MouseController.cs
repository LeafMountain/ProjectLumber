using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : ControllerElement {

	// private static MouseController instance;

	private Ray MouseRay{
		get{
			return Camera.main.ScreenPointToRay(Input.mousePosition);
		}
	}
	private RaycastHit MouseHit{
		get {
			RaycastHit hit;
			Physics.Raycast(MouseRay, out hit, Mathf.Infinity);
			return hit;
		}
	}

	public MouseController(GameApplication app) : base (app) {}

	public void Update () {
		LeftClick(MouseHit);
		RightClick(MouseHit);
	}

	private Selectable LeftClick(RaycastHit mouseHit){
		if(Input.GetMouseButtonDown(0)){
			//Keep selection if shift is pressed
			if(!Input.GetKey(KeyCode.LeftShift)){
				App.Controller.Selection.ClearSelection();
			}
			
			Selectable selectableObject = mouseHit.transform.GetComponent<Selectable>();

			if(selectableObject){
				App.Controller.Selection.AddSelection(selectableObject);
			}
			return selectableObject;
			
		}

		return null;
	}

	private void RightClick(RaycastHit mouseHit){
		if(Input.GetMouseButtonDown(1)){
			App.Controller.Selection.CommandSelection(mouseHit);
		}
	}
}
