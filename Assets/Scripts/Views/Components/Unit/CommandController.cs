using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandController {

	private UnitController unitController;
	private List<Command> commandQueue = new List<Command>();
	public Command NextCommand {
		get {
			return commandQueue[0];
		}
	}

	public Command CurrentCommand { get; private set; }

	public CommandController(UnitController unitController){
		this.unitController = unitController;
	}

	public void AddCommand(Command command){
		commandQueue.Add(command);

		if(commandQueue.Count == 1){
			unitController.NextCommand();
		}
	}

	public void RemoveCommand(Command command){
		commandQueue.Remove(command);
	}

	public void CommandDone(){
		CurrentCommand = NextCommand;
		RemoveCommand(NextCommand);
	}

	public void ClearCommandQueue(){
		commandQueue.Clear();
	}

	public delegate void CommandEvent();
	public CommandEvent NewEventQueue;

	private void OnNewEventQueue(){
		if(NewEventQueue != null){
			NewEventQueue.Invoke();
		}
	}
}
