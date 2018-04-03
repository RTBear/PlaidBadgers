using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;

public class CharacterSelection : GameObjectScript {
	//tempPlayer array is used to get the closest character to where the user is pointing
	public GameObject[] selectedPlayers, closestCharacter, allCharacters;
	string[] horizontalAxes, verticalAxes;
	string[] controllersConnected;
	Vector2[] selectedPlayerLocations;
	Color[] colorArray;
	bool[] isPlayerSelected;
	int numPlayers;
	int charactersSelected;
	// Use this for initialization
	void Start () {
		//controllers returns an array of strings. That is how we can find out how many players there are.
		controllersConnected = Input.GetJoystickNames ();
		numPlayers = controllersConnected.Length;
		horizontalAxes = GetJoystickHorizontalAxes ();
		verticalAxes = GetJoystickVerticalAxes ();
		selectedPlayerLocations = GetSelectedPlayerLocations ();
		CreateTextObjects ();
		allCharacters = GameObject.FindGameObjectsWithTag("Player");
		isPlayerSelected = new bool[numPlayers];
		closestCharacter = new GameObject[numPlayers];
		selectedPlayers = new GameObject[numPlayers];
		Debug.Log ("Numbers of Players: " + numPlayers);
	}
	
	// Update is called once per frame
	void Update () {
		//get player input for all players there has to be a better way to handle this but I just can't think of it at this moment
		for (int i = 0; i < numPlayers; i++)
		{
			if (Input.GetAxis (horizontalAxes [i]) != 0 || Input.GetAxis (verticalAxes [i]) != 0) 
			{
				float controllerAngle = getAngle(new Vector2(Input.GetAxisRaw(horizontalAxes[i]), Input.GetAxisRaw(verticalAxes[i])));
				closestCharacter[i] = FindClosestCharacter (controllerAngle);
			}

			//check if the player has selected a character and disable them from selecting another one
			if (Input.GetKeyDown ("joystick " + (i + 1) + " button 0") && closestCharacter[i] && !isPlayerSelected [i]) 
			{
				isPlayerSelected [i] = true;
				selectedPlayers[i] = Instantiate (closestCharacter [i], selectedPlayerLocations [i], Quaternion.identity);
				GameManager.CharacterType type = selectedPlayers [i].GetComponent<CharacterType> ().type;
				GameManager.instance.AssignCharacterToMap (i + 1, type);
				if (selectedPlayers [i])
					print ("player selected");
				charactersSelected++;
			}

			//if the player presses 'b' then remove their selection so that they can select a new player
			if (Input.GetKeyDown ("joystick " + (i + 1) + " button 1") && selectedPlayers[i] && isPlayerSelected [i]) 
			{
				isPlayerSelected [i] = false;
				GameManager.instance.RemoveCharacterFromMap (i + 1);
				GameObject.Destroy (selectedPlayers [i]);
				print ("player deselected");
				charactersSelected--;
			}

			//rotating effect when a player is selected
			if (selectedPlayers [i]) 
				selectedPlayers [i].transform.Rotate(0, 50 * Time.deltaTime, 0);

		}
			
		//if all characters are selected, player 1 is allowed to start the game by pressing the start button
		if(numPlayers == charactersSelected && Input.GetKeyDown("joystick 1 button 7"))
			UpdateScene ();
	}

	GameObject FindClosestCharacter(float angle)
	{
		GameObject returnCharacter = null;
		float closestAngle = angle;
		Vector2 pos;
		//Debug.Log ("Inside find closest character");
		foreach (GameObject character in allCharacters) 
		{
			Vector2 playerPosition = new Vector2(character.transform.position.x, character.transform.position.y);
			Vector2 controllerPosition = new Vector2 (this.transform.position.x, this.transform.position.y);
			float tempAngle = getAngle(playerPosition - controllerPosition);
			//Debug.Log ("Player Angle: " + tempAngle);
			if (Mathf.Abs (tempAngle - angle) < closestAngle) {
				closestAngle = Mathf.Abs (tempAngle - angle);
				returnCharacter = character;
			}
		}
		return returnCharacter;
	}

	//This could possibly be of some help when setting the players skins
//	void ChangePlayerColor(GameObject player, Color color)
//	{
//		Renderer renderer = player.GetComponent<Renderer> ();
//		renderer.material.color = color;
//		MeshRenderer mesh_renderer = player.GetComponent<MeshRenderer> ();
//		if(mesh_renderer)
//			mesh_renderer.material = renderer.material;
//	}
//	void ResetPlayerColor(GameObject player)
//	{
//		Renderer renderer = player.GetComponent<Renderer> ();
//		renderer.material.color = Color.white;
//		MeshRenderer mesh_renderer = player.GetComponent<MeshRenderer> ();
//		if(mesh_renderer)
//			mesh_renderer.material = renderer.material;
//	}

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

	Vector2[] GetSelectedPlayerLocations()
	{
		Vector2[] locations = new Vector2[4];
		locations [0] = new Vector2 (-7, 3);
		locations [1] = new Vector2 (7, 3);
		locations [2] = new Vector2 (-7, -3);
		locations [3] = new Vector2 (7, 3);
		return locations;
	}
		
	void CreateTextObjects()
	{
		GameObject[] obj = new GameObject[numPlayers];
		GameObject temp = Resources.Load ("Prefabs/TextPrefab") as GameObject;
		for (int i = 0; i < numPlayers; i++)
		{
			obj[i] = Instantiate(temp, new Vector2(selectedPlayerLocations[i].x - .75f, selectedPlayerLocations[i].y - 1.5f), Quaternion.identity);
			obj[i].GetComponent<TextMesh>().text = ("Player " + (i + 1).ToString());
		}
	}
}
