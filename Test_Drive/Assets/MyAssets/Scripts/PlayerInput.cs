﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour {
	string horizontal;
	string vertical;
	string keyboardHorizontal;
	string keyboardVertical;
	string aimHorizontal;
	string aimVertical;
	string fireTether;

	int playerNumber;

	// Use this for initialization
	void Start () {
		
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
		aimHorizontal = "Joystick" + number + "AimHorizontal";
		aimVertical = "Joystick" + number + "AimVertical";
		fireTether = "Joystick" + number + "FireTether";
		playerNumber = number;
	}

	//check if input is still being received from the player
	public bool isReceivingTetherFiringInput(){
		return (Input.GetAxisRaw (fireTether) == 1);
	}



	public bool fireTetherTriggered(){
		return (Input.GetAxisRaw (fireTether) == 1);
	}

	public bool AimTriggered(){
		return (Input.GetAxis (aimHorizontal) != 0 || Input.GetAxis (aimVertical) != 0);
	}

	public Vector2 GetAimAxis(){
		if (Input.GetAxis (aimHorizontal) != 0 || Input.GetAxis (aimVertical) != 0) {
			return new Vector2 (Input.GetAxisRaw (aimHorizontal), Input.GetAxisRaw (aimVertical));
		} else {
			return new Vector2 (0,0);
		}
	}

	public bool MoveTriggered(){
		return 
			(Input.GetAxis (horizontal) != 0 ||
				Input.GetAxis (vertical) != 0 ||
				Input.GetAxis (keyboardHorizontal) != 0 ||
				Input.GetAxis (keyboardVertical) != 0);
	}

	public Vector2 GetMoveAxis(){
		if (Input.GetAxis (horizontal) != 0 || Input.GetAxis (vertical) != 0)
			return new Vector2 (Input.GetAxisRaw (horizontal), Input.GetAxisRaw (vertical));
		else
			return new Vector2 (Input.GetAxisRaw (keyboardHorizontal), Input.GetAxisRaw (keyboardVertical));
	}

	//all logic to determine if should Jump
	public bool JumpTriggered(){
		return
			(Input.GetKeyDown ("joystick " + playerNumber + " button 0") ||
		Input.GetKeyDown (KeyCode.Space));
	}

	public bool AttackTriggered(){
		return Input.GetKeyUp ("joystick button 2") && playerNumber == 1;
	}
}