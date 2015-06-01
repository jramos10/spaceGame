using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Controller2D))]
public class Player : MonoBehaviour
{
		 
	public float jumpHeight = 4f;
	public float timeToJumpApex = 2f;
	float airborneAccelerationTime = .5f;
	float groundedAccelerationTime = .3f;
	float gravity;
	float jumpVelocity;
	float moveSpeed = 13;
	Vector3 velocity;
	Vector3 input;
	int numberOfUpBoosts = 0;
	int numberOfDownBoosts = 0;
	Controller2D controller;
	float velocityXSmoothing;
	
	void Start () {
		controller = GetComponent<Controller2D> ();

		gravity = -(2 * jumpHeight) / Mathf.Pow (timeToJumpApex, 2);
		jumpVelocity = Mathf.Abs (gravity) * timeToJumpApex;
		print (gravity + " " + jumpVelocity);
		input = new Vector3 (Input.GetAxisRaw ("Horizontal") + .8f, Input.GetAxisRaw ("Vertical"));
	}

	void Update () {

		if (controller.collisions.below) {
			numberOfUpBoosts = 0;
			numberOfDownBoosts = 0;
		}

		if (controller.collisions.above) {
			velocity.y = 0;
		}

		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			if (numberOfUpBoosts <= 1) {
				velocity.y = jumpVelocity;
				numberOfUpBoosts++;
			}
		}

		if (Input.GetKeyDown (KeyCode.DownArrow)) {
			velocity.y = -jumpVelocity;
			numberOfUpBoosts++;
		}
	

		float targetVelocityX = input.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, 
		                               (controller.collisions.below ? groundedAccelerationTime : airborneAccelerationTime));

		velocity.y += gravity * Time.deltaTime;
		controller.Move (velocity * Time.deltaTime);
	}
}
