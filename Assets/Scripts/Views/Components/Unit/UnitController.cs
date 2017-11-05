using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnitController : MonoBehaviour {

	private IUnitState currentState;
	public List<CommandModel> commandQueue = new List<CommandModel>();

	public Inventory inventory;
	public Gatherer gatherer;
	public Mover mover;

	public void ChangeState(IUnitState newState){
		if(currentState != null){
			currentState.ExitState();
		}
		currentState = newState;
		currentState.EnterState();
		// Debug.Log(currentState);
	}

	private void Start(){
		ChangeState(new UnitIdleState());
	}

	private void Update(){
		currentState.Update();
	}

	public void AddCommand(CommandModel command){
		commandQueue.Add(command);
	}

	public void NewCommand(CommandModel command){
		commandQueue.Clear();
		if(currentState.GetType() != typeof(UnitIdleState)){
			ChangeState(new UnitIdleState());
		}
		AddCommand(command);
		NextCommand();
	}

	public void NextCommand(){
		if(commandQueue != null && commandQueue.Count > 0){
			CommandModel command = commandQueue[0];			
			commandQueue.RemoveAt(0);			

			Command(command);			
		}
		else{
			ChangeState(new UnitIdleState());
		}
	}

	private void Command(CommandModel command){
		if(mover != null){
			ChangeState(new UnitMoveState(this, mover, command.Position));
		}		

	}
}
