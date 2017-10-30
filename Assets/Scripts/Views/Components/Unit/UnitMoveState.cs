using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMoveState : IUnitState {

	private UnitController unitController;
	private Mover mover;

	public UnitMoveState(UnitController unitController, Mover mover, Vector3 moveTo){
		this.unitController = unitController;
		this.mover = mover;

		mover.MoveTo(moveTo);

		mover.DestinationReached += OnDestinationReached;
	}

	public void EnterState(){
	}

	public void ExitState(){
	}

	public void Update(){

	}

	private void OnDestinationReached(){
		mover.DestinationReached -= OnDestinationReached;
		unitController.NextCommand();
	}
	
}
