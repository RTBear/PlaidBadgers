﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Char_Code : GameObjectScript {

    AudioSource audio;
	public int playerNumber;
	public PlayerController pc;
	protected PlayerInput input;
	public Collider[] attack_HitBoxes;
	SpecialAttack specialAttack;
	public Text healthTextFromCanvas;
	
	GameObject healthTextObject;

	public Vector3 tetherCollisionLocation; //used to determine where tether collided with planet
	public float tetherHoldTimer = GameManager.TETHER_HOLD_TIME;

    // Use this for initialization
    void Start () {
//		tetherCollisionLocation = GetComponent<Transform> ();
        audio = GetComponent<AudioSource>();
		pc = GetComponent<PlayerController> ();
		input = GetComponent<PlayerInput>();
		input.SetController(playerNumber);
		rb = GetComponent<Rigidbody> ();
		//this will be dynamic based on type
		specialAttack = gameObject.AddComponent<SpecialAttackRobot>();
    }
	
	// Update is called once per frame
	// this is the central update class for the character
	void Update () {
		RespondToInputs();
		SetHealthText ();
//		Debug.Log (pc.tetherEmitter.tether.m_tetherToPlanet);
//		if(pc.tetherEmitter.tether.m_tetherToPlanet){
//			tetherToPlanet();
//		}
	}

	//tether player to planet
//	public void tetherToPlanet(){
//		Debug.Log ("tether to planet called");
//		GetComponent<GameObjectScript> ().setTetherDestination (pc.tetherEmitter.tether.m_collisionLocation);
//	}

	// Update is called once per frame
	void RespondToInputs () {

		if (input.isReceivingTetherFiringInput() == true) {
			if (pc.tetherEmitter.tether.tetherAttached == false) {
				pc.tetherEmitter.tether.isFiring = false;
			}
		}

		//Check if the user has applied input on their controller
		if (input.MoveTriggered () && pc.canMove ()) {
			pc.Move (input.GetMoveAxis ());
		}

		//Check if the user has applied aim input
		if (input.AimTriggered ()) { 
			pc.Aim (input.GetAimAxis ());
		}

		if (input.isReceivingTetherFiringInput ()) {
			pc.tetherEmitter.launchTether ();
		}

		if (input.isReceivingProjectileFiringInput ()) {
			pc.projectileEmitter.launchProjectile ();
		}

		if (input.SpecialAttackTriggered ()) {
			if (specialAttack.canSpecialAttack ()) {
				specialAttack.specialAttack ();
			}
		}

		//if (Input.GetKeyDown ("joystick button 2"))
		//	Debug.Log (horizontal + " pressed x");
		
		//To get the joystick mapping correct the format needs to be "joystick # button 0"
		if (input.JumpTriggered () && pc.canJump ()) {
			//sound effect here
			//animation here
			//ect
			//ect
			pc.Jump ();
		}

		if (input.AttackTriggered ()) {
			Debug.LogWarning ("Pressed attack");
			//Collider collider = pc.GetAttackCollider(attack_HitBoxes[0]);
			//if (collider != null) {
			//sound effect
			//ect
			pc.LaunchAttack (attack_HitBoxes [0]);
			//}
		}

		if (input.SprintTriggered ()) {
			pc.AddSprint ();
		} else {
			pc.RemoveSprint ();
		}
	}

	public void SetHealthTextObject(GameObject txt)
	{
		healthTextObject = txt;
	}

	void SetHealthText() {
		if (healthTextFromCanvas)
			healthTextFromCanvas.text = health.ToString () + "%";
		else if(healthTextObject)
			healthTextObject.GetComponent<TextMesh>().text = health.ToString() + "%";
	}
}
