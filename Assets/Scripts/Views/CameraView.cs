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

	private Vector3 LookPosition {
		get {
			RaycastHit hit;
			Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity);
			return hit.point;
		}
	}

	void Update () {
		Move(MoveDirection(), speed);
		Zoom(Scroll(), zoomSpeed);
		Rotate(ScrollClick(), rotationSpeed);
	}

	private void Move(Vector3 moveDirection, float speed){
		Vector3 rightMoveDir = new Vector3(moveDirection.x, 0, 0);
		Vector3 forwardMoveDir = moveDirection.z * new Vector3(transform.forward.x, 0, transform.forward.z);

		transform.Translate(rightMoveDir * speed, Space.Self);
		transform.Translate(forwardMoveDir * speed, Space.World);


		// transform.position += new Vector3(transform.forward.x * moveDirection.z, 0, transform.forward.z * moveDirection.x) * speed;

		// Debug.Log(transform.forward  + " " + (Vector3.forward + forwardMoveDir).normalized);
		

	}

	private void Zoom(float amount, float speed){
		transform.position += transform.forward * amount * speed;
	}

	private void Rotate(float amount, float speed){
		transform.RotateAround(LookPosition, Vector3.up, amount * speed);
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
