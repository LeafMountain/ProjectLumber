using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameElement : MonoBehaviour {

	private static GameApplication app;
	public static GameApplication App { 
		get{
			if(app == null){
				app = (GameApplication)FindObjectOfType(typeof(GameApplication));
			}

			return app;
		}
	}
}
