using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameApplication : MonoBehaviour {

	public GameController Controller { get; private set; }
	public GameModel Model { get; private set; }

	private void Awake(){
		Controller = GetComponentInChildren<GameController>();
		Model = GetComponentInChildren<GameModel>();
	}
}
