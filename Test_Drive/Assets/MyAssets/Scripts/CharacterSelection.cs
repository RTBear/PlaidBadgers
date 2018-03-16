using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;

public class CharacterSelection : GameObjectScript {
	string joystick1Horizontal = "Joystick1Horizontal";
	string joystick1Vertical = "Joystick1Vertical";
	string joystick2Horizontal = "Joystick2Horizontal";
	string joystick2Vertical = "Joystick2Vertical"; 
	string joystick3Horizontal = "Joystick3Horizontal";
	string joystick3Vertical = "Joystick3Vertical";
	string joystick4Horizontal = "Joystick4Horizontal";
	string joystick4Vertical = "Joystick4Vertical"; 
	public GameObject[] players, newPlayers;
	string[] controllers;
	bool[] playersSelected;
	int numPlayers;
	int charactersSelected;
	// Use this for initialization
	void Start () {
		//controllers returns an array of strings. That is how we can find out how many players there are.
		controllers = Input.GetJoystickNames ();
		numPlayers = controllers.Length;
		players = GameObject.FindGameObjectsWithTag("Player");
		playersSelected = new bool[numPlayers];
		newPlayers = new GameObject[numPlayers];
		Debug.Log ("Numbers of controllers attached: " + numPlayers);
	}
	
	// Update is called once per frame
	void Update () {
		//get player input for all players there has to be a better way to handle this but I just can't think of it at this moment
		if (Input.GetAxis (joystick1Horizontal) != 0 || Input.GetAxis (joystick1Vertical) != 0 && !playersSelected[0]) 
		{
			float controllerAngle = getAngle(new Vector2(Input.GetAxisRaw(joystick1Horizontal), Input.GetAxisRaw(joystick1Vertical)));
			GameObject tempPlayer = newPlayers [0];
			newPlayers[0] = FindClosestCharacter (controllerAngle);
			//if they have changed their selection then reset the other mesh color
			if (newPlayers [0])
				ChangePlayerColor (newPlayers [0], Color.red);
			if (tempPlayer && newPlayers [0] && tempPlayer != newPlayers [0])
				ResetPlayerColor (tempPlayer);
		}
		if (Input.GetAxis (joystick2Horizontal) != 0 || Input.GetAxis (joystick2Vertical) != 0 && !playersSelected[1]) 
		{
			float controllerAngle = getAngle(new Vector2(Input.GetAxisRaw(joystick2Horizontal), Input.GetAxisRaw(joystick2Vertical)));
			newPlayers [1] = FindClosestCharacter (controllerAngle);
		}
		if (Input.GetAxis (joystick3Horizontal) != 0 || Input.GetAxis (joystick3Vertical) != 0 && !playersSelected[2]) 
		{
			float controllerAngle = getAngle(new Vector2(Input.GetAxisRaw(joystick3Horizontal), Input.GetAxisRaw(joystick3Vertical)));
			newPlayers[2] = FindClosestCharacter (controllerAngle);
		}
		if (Input.GetAxis (joystick4Horizontal) != 0 || Input.GetAxis (joystick4Vertical) != 0 && !playersSelected[3]) 
		{
			float controllerAngle = getAngle(new Vector2(Input.GetAxisRaw(joystick4Horizontal), Input.GetAxisRaw(joystick4Vertical)));
			newPlayers [3] = FindClosestCharacter (controllerAngle);
		}

		//check if the player has selected a character and disable them from selecting another one
		for (int i = 0; i < numPlayers; i++) 
		{
			if (Input.GetKeyDown ("joystick " + (i + 1) + " button 0")) 
			{
				if (newPlayers [i]) {
					playersSelected [i] = true;
					GameManager.CharacterType type = newPlayers [i].GetComponent<CharacterType> ().type;
					GameManager.instance.AssignCharacterType (i + 1, type);
					charactersSelected++;
				}
			}
		}

		for (int i = 0; i < numPlayers; i++) 
		{
			if (Input.GetKeyDown ("joystick " + (i + 1) + " button 0")) 
			{
				if (newPlayers [i]) {
					playersSelected [i] = true;
					GameManager.CharacterType type = newPlayers [i].GetComponent<CharacterType> ().type;
					GameManager.instance.AssignCharacterType (i + 1, type);
					charactersSelected++;
				}
			}
		}

		if(numPlayers == charactersSelected)
			UpdateScene ();
	}

	GameObject FindClosestCharacter(float angle)
	{
		GameObject returnPlayer = null;
		float closestAngle = angle;
		Vector2 pos;
		Debug.Log ("Inside find closest character");
		foreach (GameObject player in players) 
		{
			Vector2 playerPosition = new Vector2(player.transform.position.x, player.transform.position.y);
			Vector2 controllerPosition = new Vector2 (this.transform.position.x, this.transform.position.y);
			float tempAngle = getAngle(playerPosition - controllerPosition);
			Debug.Log ("Player Angle: " + tempAngle);
			if (Mathf.Abs (tempAngle - angle) < closestAngle) {
				closestAngle = Mathf.Abs (tempAngle - angle);
				returnPlayer = player;
			}
		}
		return returnPlayer;
	}

	//This could possibly be of some help when setting the players skins
	void ChangePlayerColor(GameObject player, Color color)
	{
		Renderer renderer = player.GetComponent<Renderer> ();
		renderer.material.color = color;
		MeshRenderer mesh_renderer = player.GetComponent<MeshRenderer> ();
		if(mesh_renderer)
			mesh_renderer.material = renderer.material;
	}

	//This could possibly be of some help when setting the players skins
	void ResetPlayerColor(GameObject player)
	{
		Renderer renderer = player.GetComponent<Renderer> ();
		renderer.material.color = Color.white;
		MeshRenderer mesh_renderer = player.GetComponent<MeshRenderer> ();
		if(mesh_renderer)
			mesh_renderer.material = renderer.material;
	}

	void UpdateScene()
	{
		EditorSceneManager.LoadScene ("NewMap");
	}
}
