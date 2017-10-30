using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CapsuleCollider))]
public class Selectable : MonoBehaviour {

	public UnityEvent selected;
	public UnityEvent deselected;

	public void Select(){
		selected.Invoke();
	}

	public void DeSelect(){
		deselected.Invoke();
	}
}