using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraView : MonoBehaviour {

	[SerializeField]
	private float speed = .5f;
	[SerializeField]	
	private float zoomSpeed = 3;

	[SerializeField]
	private float rotationSpeed = 1;

	void Update () {
		Move(MoveDirection(), speed);
		Zoom(Scroll(), zoomSpeed);
		Rotate(ScrollClick(), rotationSpeed);
	}

	private void Move(Vector3 moveDirection, float speed){
		transform.Translate(moveDirection * speed, Space.World);
	}

	private void Zoom(float amount, float speed){
		transform.position += transform.forward * amount * speed;
	}

	private void Rotate(float amount, float speed){
		RaycastHit hit;
		Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity);

		transform.RotateAround(hit.point, Vector3.up, amount * speed);
	}

	private Vector3 MoveDirection(){
		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");

		return new Vector3(horizontal, 0, vertical);
	}

	private float Scroll(){
		return Input.GetAxis("Mouse ScrollWheel");
	}

	private float ScrollClick(){
		if(Input.GetMouseButton(1)){
			return Input.GetAxis("Mouse X");
		}
		return 0;
	}
}
