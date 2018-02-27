using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Char_Code {
	//bool onGround = true;
	//float runForce = 30f;
	//float jumpVel = 25.0f;
	bool canAirJump = true;
	//Rigidbody rb;
	//GameObject planet;
	string horizontal;
	string vertical;
	Char_Code player;

	public TetherEmitterController tetherEmitter;


	// Use this for initialization
	void Start () {
		//planet = GameObject.Find ("Simple_Ground");
		player = GetComponentInParent<Char_Code> ();
		rb = player.GetComponent<Rigidbody>();
        
	}

	// Update is called once per frame
	void Update () {

		//This will force the character to stand upright
		Vector2 relativePosition = transform.position - planet.transform.position;
		float angleChar = getAngle(relativePosition);
		transform.eulerAngles = new Vector3 (0, 0, angleChar);

		//Check if the user has applied input on their controller
		if (Input.GetAxis(horizontal) != 0 || Input.GetAxis(vertical) != 0) {
			Move ();
		}

		//Check if the user has applied input to their right analog stick
		if (Input.GetAxis("RightAnalogHorizontal") != 0 || Input.GetAxis("RightAnalogVertical") != 0) {
			Aim ();
		}

		if (Input.GetKeyDown ("joystick button 2"))
			Debug.Log (horizontal + " pressed x");
		
		//tether firing
		if (Input.GetAxisRaw ("RightTrigger") == 1) {
			tetherEmitter.launchTether(); 
		}

		//To get the joystick mapping correct the format needs to be "joystick # button 0"
		if ((Input.GetKeyDown("joystick " + player.playerNumber + " button 0") | Input.GetKey(KeyCode.Space)) && (onGround | canAirJump)) {
            Jump ();
		}

	}

	void Aim(){
		Vector2 relativePosition = this.tetherEmitter.firePoint.position - transform.GetComponent<Renderer> ().bounds.center;
		Vector2 directionAim = new Vector2 (Input.GetAxisRaw ("RightAnalogHorizontal"), Input.GetAxisRaw ("RightAnalogVertical"));
		float angleCrosshair = getAngle (directionAim);
		this.tetherEmitter.transform.eulerAngles = new Vector3(0, 0, angleCrosshair);
	}

	void Move()
	{
		if (planet == null)
			return;

		Vector2 relativePosition = transform.position - planet.transform.position;

		// This is how our charactor will move with analog sticks
		float angleChar = getAngle(relativePosition);

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


			if (angleChar>angleRunDir)
			{
				if (angleChar - angleRunDir < 180)
					rb.AddRelativeForce(Vector3.right * runForce*moveMod);
				else
					rb.AddRelativeForce(Vector3.left * runForce*moveMod);
			}
			else
			{
				if(angleRunDir - angleChar < 180)
					rb.AddRelativeForce(Vector3.left * runForce*moveMod);
				else
					rb.AddRelativeForce(Vector3.right * runForce*moveMod);

			}
		}
		transform.eulerAngles = new Vector3(0, 0, angleChar);
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
	}
}
