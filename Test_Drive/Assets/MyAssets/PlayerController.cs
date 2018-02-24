using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	bool onGround = true;
	float runForce = 30f;
	float jumpVel = 25.0f;
	bool canAirJump = true;
	float canSprint = 1f;
	Rigidbody rb;
	GameObject planet;
	string horizontal;
	string vertical;
	string keyboardHorizontal;
	string keyboardVertical;
	Char_Code player;

	// Use this for initialization
	void Start () {
		planet = GameObject.Find ("Simple_Ground");
		player = GetComponent<Char_Code> ();
		rb = player.GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void Update () {

		//This will force the character to stand upright
		Vector2 relativePosition = transform.position - planet.transform.position;
		float angleChar = getAngle(relativePosition);
		transform.eulerAngles = new Vector3 (0, 0, angleChar);

		//Check if the user has applied input on their controller
		if (Input.GetAxis(horizontal) != 0 || Input.GetAxis(vertical) != 0 || Input.GetAxis(keyboardHorizontal) != 0 || Input.GetAxis(keyboardVertical) != 0) {
			Move ();
		}

		if (Input.GetKeyDown ("joystick button 2"))
			Debug.Log (horizontal + " pressed x");

		//To get the joystick mapping correct the format needs to be "joystick # button 0"
		if (Input.GetKey("joystick " + player.playerNumber + " button 0") || Input.GetKeyDown(KeyCode.Space) && (onGround | canAirJump)) {
			Jump ();
		}

		if (Input.GetKeyDown (KeyCode.LeftShift)) {
			canSprint = 2f;
		} else { canSprint = 1f; }

	}

	void Move()
	{
		if (planet == null)
			return;

		Vector2 relativePosition = transform.position - planet.transform.position;

		// This is how our charactor will move with analog sticks
		float angleChar = getAngle(relativePosition);

		Vector2 directionRun = new Vector2(0, 0);

		if(Input.GetAxis(horizontal) != 0 || Input.GetAxis(vertical) != 0)
			directionRun = new Vector2(Input.GetAxisRaw(horizontal), Input.GetAxisRaw(vertical));
		else if(Input.GetAxis(keyboardHorizontal) != 0 || Input.GetAxis(keyboardVertical) != 0)
			directionRun = new Vector2(Input.GetAxisRaw(keyboardHorizontal), Input.GetAxisRaw(keyboardVertical));
		
		float angleRunDir = getAngle(directionRun);

		//check if they desired location is the same as current location
		if ((int)angleChar + 8 >= (int)angleRunDir && (int)angleRunDir >= (int)angleChar - 8)
		{
			if (onGround)
				rb.velocity = new Vector2(0, 0);
		}
		else // Run to given location
		{
			float moveMod = 1f;
			if (!onGround)
				moveMod = 0.5f;


			if (angleChar>angleRunDir)
			{
				if (angleChar - angleRunDir < 180)
					rb.AddRelativeForce(Vector3.right * runForce*moveMod*canSprint);
				else
					rb.AddRelativeForce(Vector3.left * runForce*moveMod*canSprint);
			}
			else
			{
				if(angleRunDir - angleChar < 180)
					rb.AddRelativeForce(Vector3.left * runForce*moveMod*canSprint);
				else
					rb.AddRelativeForce(Vector3.right * runForce*moveMod*canSprint);

			}
		}
		transform.eulerAngles = new Vector3(0, 0, angleChar);
	}

	float getAngle(Vector2 vector)
	{
		float angle;

		if (vector[0] == 0)
		{
			if (vector[1] > 0)
			{
				angle = 0;
			}
			else
			{
				angle = 180;
			}
		}
		else if (vector[0] > 0)       //check if char is to right of planet
		{
			if (vector[1] > 0) //   Quadrant I
			{
				angle = 270 + (Mathf.Atan(vector[1] / vector[0]) * 180) / Mathf.PI;
			}
			else   // Quadrant II
			{
				angle = 270 + (Mathf.Atan(vector[1] / vector[0]) * 180) / Mathf.PI;
			}

		}
		else
		{
			if (vector[1] > 0) //   Quadrant IIV
			{
				angle = 90 + (Mathf.Atan(vector[1] / vector[0]) * 180) / Mathf.PI;
			}
			else  // Quadrand III
			{
				angle = 90 + (Mathf.Atan(vector[1] / vector[0]) * 180) / Mathf.PI;
			}
		}
		return angle;
	}

	Vector2 getUnitVector(float x, float y)
	{
		float magnitude = Mathf.Sqrt(x * x + y * y);

		return new Vector2(x / magnitude, y / magnitude);
	}

	void Jump()
	{
		{
			//determine where on planet the Char is
			Vector3 relative_position = rb.transform.position - planet.transform.position;
			//DELETE AFTER USE// float magnitude = Mathf.Sqrt(relative_position[0] * relative_position[0] + relative_position[1] * relative_position[1]);
			Vector2 jumpUnitVector = getUnitVector(relative_position[0], relative_position[1]);
			// jumpDirection is the direction normal to the surface
			Vector3 jumpDirection = new Vector3(jumpUnitVector[0]*jumpVel, jumpUnitVector[1]*jumpVel, 0);

			// Get current speed of Char
			Vector3 currentSpeed = rb.velocity;

			//if moving we need to combine the current momentum into the jump
			rb.velocity = new Vector3(currentSpeed[0] + jumpDirection[0], currentSpeed[1] + jumpDirection[1], 0);

			onGround = false;

			// Air jump logic
			if (canAirJump)
				canAirJump = false;
		}
	}

	void OnCollisionEnter(Collision collider)
	{
		onGround = true;
	}

	void OnCollisionExit(Collision collider)
	{
		onGround = false;
		canAirJump = true;
	}

	/**
	 * SetController is where the Axes get mapped
	 * The strings need to match EXACTLY what the InputManager says
	 * */
	public void SetController(int number)
	{
		horizontal = "Joystick" + number + "Horizontal";
		vertical = "Joystick" + number + "Vertical";
		keyboardHorizontal = "Keyboard" + number + "Horizontal";
		keyboardVertical = "Keyboard" + number + "Vertical";
	}
}
