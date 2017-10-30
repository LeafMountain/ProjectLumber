using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animate : MonoBehaviour {

	private Animator animator;
	private Mover mover;

	private void Awake(){
		animator = GetComponent<Animator>();
		mover = GetComponent<Mover>();
	}

	private void Update(){
		if(mover && animator.GetFloat("velocity") != mover.Velocity){
			animator.SetFloat("velocity", mover.Velocity);
		}
	}
}
