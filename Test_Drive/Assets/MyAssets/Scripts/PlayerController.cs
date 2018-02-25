using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Char_Code {
	//bool onGround = true;
	//float runForce = 30f;
	//float jumpVel = 25.0f;
	bool canAirJump = true;
	//Rigidbody rb;
	string horizontal;
	string vertical;
	Char_Code player;
	private Vector2 relativePos;
	private int jmpForce = 7000;

	// Use this for initialization
	void Start () {
		player = GetComponentInParent<Char_Code> ();
		rb = player.GetComponent<Rigidbody>();
		relativePos = new Vector3 (0,0,0);
	}

	public void setUprightAngle(Vector2 pos){
		float angleChar = getAngle(pos);
		transform.eulerAngles = new Vector3(0, 0, angleChar);
		relativePos = pos;
	}

	// Update is called once per frame
	void Update () {
		//Check if the user has applied input on their controller
		if (Input.GetAxis(horizontal) != 0 || Input.GetAxis(vertical) != 0) {
			Move ();
		}

		if (Input.GetKeyDown ("joystick button 2"))
			Debug.Log (horizontal + " pressed x");

		//To get the joystick mapping correct the format needs to be "joystick # button 0"
		if ((Input.GetKeyDown("joystick " + player.playerNumber + " button 0") | Input.GetKey(KeyCode.Space)) && (onGround | canAirJump)) {
            Jump ();
		}

	}

	void Move()
	{
		// This is how our charactor will move with analog sticks
		float angleChar = getAngle(relativePos);

		Vector2 directionRun = new Vector2(Input.GetAxisRaw(horizontal), Input.GetAxisRaw(vertical));
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

			float angleDiff = angleRunDir - angleChar;
			if (angleDiff < 0)
				angleDiff += 360;
			if (angleDiff < 180 && angleDiff > 0)
				rb.AddRelativeForce(Vector3.left * runForce*moveMod);
			else
				rb.AddRelativeForce(Vector3.right * runForce*moveMod);
		}
	}


	void Jump()
	{
		rb.AddForce (relativePos* jmpForce);
		onGround = false;
		// Air jump logic
		if (canAirJump)
			canAirJump = false;
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
	}
}
