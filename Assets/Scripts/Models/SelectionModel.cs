using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionModel {

    private List<Selectable> selected = new List<Selectable>();

	public void AddSelection(Selectable selectable){
        if(!selected.Contains(selectable)){
            selected.Add(selectable);
        }
	}

	public void RemoveSelection(Selectable selectable){
        if(!selected.Contains(selectable)){
            selected.Remove(selectable);
        }
    }

	public void ClearSelection(){
        selected.Clear();
	}

	public List<Selectable> GetSelection(){
		return selected;
	}
}
