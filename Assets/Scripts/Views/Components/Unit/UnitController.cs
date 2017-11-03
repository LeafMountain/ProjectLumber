using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnitController : MonoBehaviour {

	private IUnitState currentState;
	// public List<Command> commandQueue = new List<Command>();
	private CommandController commandController;
	public CommandController CommandController { get { return commandController; }}

	public Inventory inventory;
	public Gatherer gatherer;
	public Mover mover;

	public void ChangeState(IUnitState newState){
		if(currentState != null){
			currentState.ExitState();
		}
		currentState = newState;
		currentState.EnterState();
	}

	private void Awake(){
		commandController = new CommandController(this);
	}

	private void Start(){
		ChangeState(new UnitIdleState());
	}

	private void Update(){
		currentState.Update();
	}

	public void NextCommand(){
		CommandController.CommandDone();
		Command(CommandController.CurrentCommand);
	}

	private void Command(Command command){
		if(command.Interactable){
			Extractable extractable = command.Interactable.GetComponent<Extractable>();

			if(gatherer && extractable){
				ChangeState(new UnitGatherState(this, gatherer, extractable));
			}
		}
		else{
			if(mover != null){
				ChangeState(new UnitMoveState(this, mover, command.Position));
			}	
		}
	}
}
