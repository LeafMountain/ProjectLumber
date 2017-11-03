using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitGatherState : IUnitState {

	private UnitController unitController;
	private Gatherer gatherer;
	private Extractable extractable;

	public UnitGatherState(UnitController unitController, Gatherer gatherer, Extractable extractable){
		this.unitController = unitController;
		this.gatherer = gatherer;
		this.extractable = extractable;
	}
	public void EnterState(){

		//Check if near extractable
		if(Vector3.Distance(gatherer.transform.position, extractable.transform.position) > 1){
			
		}
	}

	public void ExitState(){

	}

	public void Update(){

	}
	
}
