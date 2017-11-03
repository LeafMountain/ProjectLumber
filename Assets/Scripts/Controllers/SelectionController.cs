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

	public void CommandSelection(RaycastHit hit, bool keepCurrentCommands = true){
		Command command = new Command(hit.point, hit.transform.GetComponent<Interactable>());
		
		for (int j = 0; j < App.Model.Selection.GetSelection().Count; j++)
		{
			if(keepCurrentCommands){
				App.Model.Selection.GetSelection()[j].OnShiftRightClick(command);
			}
			else
			{
				App.Model.Selection.GetSelection()[j].OnRightClick(command);
			}		
		}
	}
}

