using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Char_Code : GameObjectScript {

    AudioSource audio;
	public int playerNumber;
	PlayerController pc;
	PlayerInput input;
	public Collider[] attack_HitBoxes;

    // Use this for initialization
    void Start () {
     
        rb = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
		pc = GetComponent<PlayerController> ();
		input = GetComponent<PlayerInput>();
		input.SetController(playerNumber);
    }
	
	// Update is called once per frame
	void Update () {
		RespondToInputs();
	}

	// Update is called once per frame
	void RespondToInputs () {
		//add tether here? ect?

		//Check if the user has applied input on their controller
		if (input.MoveTriggered()) {
			pc.Move(input.GetMoveAxis());
		}

		//if (Input.GetKeyDown ("joystick button 2"))
		//	Debug.Log (horizontal + " pressed x");
		
		//To get the joystick mapping correct the format needs to be "joystick # button 0"
		if (input.JumpTriggered()) {
			//sound effect here
			//animation here
			//ect
			//ect
			pc.Jump();
		}

		if (input.AttackTriggered())
		{
			Debug.LogWarning("Pressed attack");
			Collider collider = pc.GetAttackCollider(attack_HitBoxes[0]);
			if (collider != null) {
				//sound effect
				//ect
				pc.LaunchAttack(collider);
			}
		}
	}

}
