using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Controller2D))]
public class Player : MonoBehaviour {
		 
	public float jumpHeight = 6;
	public float timeToJumpApex = .4f;
	float airborneAccelerationTime = .3f;
	float groundedAccelerationTime = .1f;

	float gravity;
	float jumpVelocity;

	float moveSpeed = 8;

	Vector3 velocity;
	Controller2D controller;
	float velocityXSmoothing;
	
	void Start() {
		controller = GetComponent<Controller2D> ();

		gravity = -(2 * jumpHeight) / Mathf.Pow (timeToJumpApex, 2);
		jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
		print (gravity + " " + jumpVelocity);
	}

	void Update() {
		if (controller.collisions.above || controller.collisions.below) {
			velocity.y = 0;
		}

		Vector3 input = new Vector3(Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw("Vertical"));

		if (Input.GetKeyDown (KeyCode.Space) && controller.collisions.below) {
			velocity.y = jumpVelocity;
		}

		float targetVelocityX = input.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, 
		                               (controller.collisions.below ? groundedAccelerationTime : airborneAccelerationTime));

		velocity.y += gravity * Time.deltaTime;
		controller.Move (velocity * Time.deltaTime);
	}
}
