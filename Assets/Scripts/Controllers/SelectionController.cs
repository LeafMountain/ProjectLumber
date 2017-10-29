using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionController : ControllerElement {

	public SelectionController(GameApplication app) : base(app){}

	public void AddSelection(Selectable selectable){
        App.Model.Selection.AddSelection(selectable);
		selectable.Select();
	}

	public void RemoveSelection(Selectable selectable){
        App.Model.Selection.RemoveSelection(selectable);
		selectable.DeSelect();
    }

	public void ClearSelection(){
		for (int i = 0; i < App.Model.Selection.GetSelection().Count; i++)
		{
			App.Model.Selection.GetSelection()[i].DeSelect();
		}
		
        App.Model.Selection.ClearSelection();
	}

	public List<Selectable> GetSelection(){
		return App.Model.Selection.GetSelection();
	}

	public void CommandSelection(RaycastHit hit){
		for (int i = 0; i < App.Model.Selection.GetCommandListeners().Count; i++)
		{
			App.Model.Selection.GetCommandListeners()[i].RightClicked(hit);
		}
	}
}
