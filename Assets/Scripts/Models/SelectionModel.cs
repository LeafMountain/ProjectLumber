using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionModel {

    private List<Selectable> selected = new List<Selectable>();
    private List<ICommandListener> commandListeners = new List<ICommandListener>();

	public void AddSelection(Selectable selectable){
        if(!selected.Contains(selectable)){
            selected.Add(selectable);
            commandListeners.AddRange(ExtractCommandListeners(selectable));
        }
	}

	public void RemoveSelection(Selectable selectable){
        if(!selected.Contains(selectable)){
            selected.Remove(selectable);

            ICommandListener[] commandListener = ExtractCommandListeners(selectable);

            for (int i = 0; i < commandListener.Length; i++)
            {
                commandListeners.Remove(commandListener[i]);            
            }
            
        }
    }

	public void ClearSelection(){
        selected.Clear();
        commandListeners.Clear();
	}

	public List<Selectable> GetSelection(){
		return selected;
	}

    public List<ICommandListener> GetCommandListeners(){
        return commandListeners;
    }

    private ICommandListener[] ExtractCommandListeners(Selectable selectable){
		return selectable.GetComponents<ICommandListener>();
	}
}
