using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewElement : MonoBehaviour {

	private static GameApplication app;
	protected static GameApplication App { 
		get{
			if(app == null){
				app = (GameApplication)FindObjectOfType(typeof(GameApplication));
			}

			return app;
		}
	}
}

public class ControllerElement {
	private static GameApplication app;
	public static GameApplication App { 
		get{
			return app;
		}
	}

	public ControllerElement(GameApplication application){
		app = application;
	}
}
