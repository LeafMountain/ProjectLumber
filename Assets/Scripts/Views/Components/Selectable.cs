using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour {

	public void Select(){
		Debug.Log(transform.name + " selected");
	}

	public void DeSelect(){
		Debug.Log(transform.name + " deselected");		
	}
}
