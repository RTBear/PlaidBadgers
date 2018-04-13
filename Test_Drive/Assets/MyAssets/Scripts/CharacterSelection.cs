using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelection : GameObjectScript {
	//These arrays are in the format of (variable[playerNumber])
	public GameObject[] selectedPlayers, closestCharacter, allCharacters, displayedCharacter;
	string[] horizontalAxes, verticalAxes;
	string[] controllersConnected;
	Vector2[] selectedPlayerLocations;
	Color[] colorArray;
	bool[] isPlayerSelected;
	int numPlayers;
	int charactersSelected;
	//the array is used as the selected player
	int[] materialPositions;
	Material[] materials;

	// Use this for initialization
	void Start () {
		//controllers returns an array of strings. That is how we can find out how many players there are.
		controllersConnected = Input.GetJoystickNames ();
		numPlayers = controllersConnected.Length;
		horizontalAxes = GetJoystickHorizontalAxes ();
		verticalAxes = GetJoystickVerticalAxes ();
		selectedPlayerLocations = GetSelectedPlayerLocations ();
		materials = GetAvailableMaterials();
		CreateTextObjects ();
		allCharacters = GameObject.FindGameObjectsWithTag("Player");
		isPlayerSelected = new bool[numPlayers];
		closestCharacter = new GameObject[numPlayers];
		selectedPlayers = new GameObject[numPlayers];
		displayedCharacter = new GameObject[numPlayers];
		materialPositions = new int[numPlayers];
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

			if(!isPlayerSelected[i] && displayedCharacter[i] != null && closestCharacter[i] != displayedCharacter[i])
			{
				Destroy(displayedCharacter[i]);
				displayedCharacter[i] = null;
			}

			if(!isPlayerSelected[i] && closestCharacter[i] && displayedCharacter[i] == null)
			{
				displayedCharacter[i] = Instantiate (closestCharacter [i], selectedPlayerLocations [i], Quaternion.identity);
			}

			//check if the player has selected a character and disable them from selecting another one
			if (Input.GetKeyDown ("joystick " + (i + 1) + " button 0") && closestCharacter[i] && !isPlayerSelected [i]) 
			{
				SelectCharacter(i);
			}

			//if the player presses 'b' then remove their selection so that they can select a new player
			if (Input.GetKeyDown ("joystick " + (i + 1) + " button 1") && selectedPlayers[i] && isPlayerSelected [i]) 
			{
				DeselectCharacter(i);
			}

			//If the user has selected a player, then allow them to change the color of their player
			if(isPlayerSelected[i] && Input.GetKeyDown("joystick " + (i + 1) + " button 5"))
			{
				//cycle to the right
				SetPlayerMaterial(materialPositions[i] + 1, i);
			}
			else if(isPlayerSelected[i] && Input.GetKeyDown("joystick " + (i + 1) + " button 4"))
			{
				//cycle to the left
				SetPlayerMaterial(materialPositions[i] - 1, i);
			}


			//rotating effect when a player is selected
			if (selectedPlayers [i]) 
				selectedPlayers [i].transform.Rotate(0, 50 * Time.deltaTime, 0);

		}
			
		//if all characters are selected, player 1 is allowed to start the game by pressing the start button
		if(numPlayers == charactersSelected && Input.GetKeyDown("joystick 1 button 7"))
			UpdateScene ();
	}
		
	void SelectCharacter(int whichCharacter)
	{
		CharacterAttributes attributes = new CharacterAttributes();
		isPlayerSelected [whichCharacter] = true;
		selectedPlayers[whichCharacter] = Instantiate (closestCharacter [whichCharacter], selectedPlayerLocations [whichCharacter], Quaternion.identity);
		selectedPlayers[whichCharacter].GetComponent<Transform>().localScale = new Vector3(1.25f, 1.25f, 1.25f);
		Debug.Log ("Selected character type: " + selectedPlayers [whichCharacter].GetComponent<CharacterType> ().type.ToString ());
		//actually assigning values and putting the select character into the map
		attributes.m_CharacterType = selectedPlayers [whichCharacter].GetComponent<CharacterType> ().type;
		attributes.SetPrefab ();
		GameManager.instance.AssignCharacterToMap (whichCharacter + 1, attributes);

		if (selectedPlayers [whichCharacter])
			print ("player selected");
		if(displayedCharacter[whichCharacter])
		{
			Destroy(displayedCharacter[whichCharacter]);
			displayedCharacter[whichCharacter] = null;
		}
		charactersSelected++;
	}

	void DeselectCharacter(int whichCharacter)
	{
		isPlayerSelected [whichCharacter] = false;
		GameManager.instance.RemoveCharacterFromMap (whichCharacter + 1);
		GameObject.Destroy (selectedPlayers [whichCharacter]);
		print ("player deselected");
		charactersSelected--;
	}

	GameObject FindClosestCharacter(float angle)
	{
		GameObject returnCharacter = null;
		float closestAngle = angle;
		foreach (GameObject character in allCharacters) 
		{
			Vector2 playerPosition = new Vector2(character.transform.position.x, character.transform.position.y);
			Vector2 controllerPosition = new Vector2 (this.transform.position.x, this.transform.position.y);
			float tempAngle = getAngle(playerPosition - controllerPosition);
			if (Mathf.Abs (tempAngle - angle) < closestAngle) {
				closestAngle = Mathf.Abs (tempAngle - angle);
				returnCharacter = character;
			}
		}
		return returnCharacter;
	}
		
	void SetPlayerMaterial(int materialPos, int whichPlayer)
	{
		//if materialPos is beyond the array of materials, loop back around
		if(materialPos < 0)
			materialPos = 3;
		if(materialPos > 3)
			materialPos = 0;
		
		MeshRenderer renderer = selectedPlayers[whichPlayer].GetComponent<MeshRenderer>();
		if (renderer) {
			renderer.material = materials [materialPos];
			CharacterAttributes attributes = GameManager.instance.GetAttributesFromMap (whichPlayer + 1);
			attributes.m_Material = renderer.material;
			Debug.Log ("Changing material");
			GameManager.instance.UpdateCharacterMap (whichPlayer + 1, attributes);
		}
		materialPositions[whichPlayer] = materialPos;
	}
		
	void UpdateScene()
	{
		SceneManager.LoadScene("NewMap");
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

	Material[] GetAvailableMaterials()
	{
		Material[] tempMaterials = new Material[4];
		tempMaterials[0] = Resources.Load("Materials/Blue") as Material;
		tempMaterials[1] = Resources.Load("Materials/Red") as Material;
		tempMaterials[2] = Resources.Load("Materials/Green") as Material;
		tempMaterials[3] = Resources.Load("Materials/Yellow") as Material;
		return tempMaterials;
	}
}
