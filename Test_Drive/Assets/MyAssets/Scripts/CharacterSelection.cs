using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;

public class CharacterSelection : GameObjectScript {
	public GameObject[] players, newPlayers;
	string[] horizontalAxes, verticalAxes;
	string[] controllersConnected;
	Color[] colorArray;
	bool[] playersSelected;
	int numPlayers;
	int charactersSelected;
	// Use this for initialization
	void Start () {
		//controllers returns an array of strings. That is how we can find out how many players there are.
		controllersConnected = Input.GetJoystickNames ();
		numPlayers = controllersConnected.Length;
		horizontalAxes = GetJoystickHorizontalAxes ();
		verticalAxes = GetJoystickVerticalAxes ();
		colorArray = new Color[4];
		colorArray [0] = Color.red;
		colorArray [1] = Color.blue;
		colorArray [2] = Color.green;
		colorArray [3] = Color.yellow;
		players = GameObject.FindGameObjectsWithTag("Player");
		playersSelected = new bool[numPlayers];
		newPlayers = new GameObject[numPlayers];
		Debug.Log ("Numbers of controllers attached: " + numPlayers);
	}
	
	// Update is called once per frame
	void Update () {
		//get player input for all players there has to be a better way to handle this but I just can't think of it at this moment
		for (int i = 0; i < numPlayers; i++)
		{
			if (Input.GetAxis (horizontalAxes [i]) != 0 || Input.GetAxis (verticalAxes [i]) != 0 && !playersSelected [i]) 
			{
				float controllerAngle = getAngle(new Vector2(Input.GetAxisRaw(horizontalAxes[i]), Input.GetAxisRaw(verticalAxes[i])));
				GameObject tempPlayer = newPlayers [i];
				newPlayers[i] = FindClosestCharacter (controllerAngle);
				//if they have changed their selection then reset the other mesh color
				if (newPlayers [i])
					ChangePlayerColor (newPlayers [i], colorArray[i]);
				if (tempPlayer && newPlayers [i] && tempPlayer != newPlayers [i])
					ResetPlayerColor (newPlayers[i]);
			}
		}

		//check if the player has selected a character and disable them from selecting another one
		for (int i = 0; i < numPlayers; i++) 
		{
			if (Input.GetKeyDown ("joystick " + (i + 1) + " button 0") && newPlayers[i]) 
			{
				playersSelected [i] = true;
				GameManager.CharacterType type = newPlayers [i].GetComponent<CharacterType> ().type;
				GameManager.instance.AssignCharacterType (i + 1, type);
				charactersSelected++;
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

	string[] GetJoystickHorizontalAxes()
	{
		//Hard coding to 4 right now since there will be only be 4 players(
		string[] joystickArray = new string[4];
		for (int i = 0; i < joystickArray.Length; i++) {
			joystickArray [i] = "Joystick" + (i + 1) + "Horizontal";
		}
		return joystickArray;
	}

	string[] GetJoystickVerticalAxes()
	{
		//Hard coding to 4 right now since there will be only be 4 players(
		string[] joystickArray = new string[4];
		for (int i = 0; i < joystickArray.Length; i++) {
			joystickArray [i] = "Joystick" + (i + 1) + "Vertical";
		}
		return joystickArray;
	}
}
