using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;

public class CharacterSelection : GameObjectScript {
	string joystick1Horizontal = "Joystick1Horizontal";
	string joystick1Vertical = "Joystick1Vertical";
	string joystick2Horizontal = "Joystick2Horizontal";
	string joystick2Vertical = "Joystick2Vertical"; 
	public GameObject[] players;
	public GameObject[] newPlayers;
	string[] controllers;
	int numPlayers;
	int charactersSelected;
	MeshFilter[] meshes;
	bool characterSelected = false;
	// Use this for initialization
	void Start () {
		controllers = Input.GetJoystickNames ();
		numPlayers = controllers.Length;
		players = GameObject.FindGameObjectsWithTag("Player");
		Debug.Log ("Number of players: " + players.Length);
		newPlayers = new GameObject[numPlayers];
		meshes = new MeshFilter[numPlayers];
		Debug.Log ("Numbers of controllers attached: " + numPlayers);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetAxis (joystick1Horizontal) != 0 || Input.GetAxis (joystick1Vertical) != 0) 
		{
			float controllerAngle1 = getAngle(new Vector2(Input.GetAxisRaw(joystick1Horizontal), Input.GetAxisRaw(joystick1Vertical)));
			Debug.Log ("Conroller Angle: " + controllerAngle1);
			newPlayers[0] = FindClosestCharacter (controllerAngle1);
			//if(newPlayers[0])
				//ChangePlayerColor (newPlayers [0]);
		}
		if (Input.GetAxis (joystick2Horizontal) != 0 || Input.GetAxis (joystick2Vertical) != 0) 
		{
			float controllerAngle2 = getAngle(new Vector2(Input.GetAxisRaw(joystick2Horizontal), Input.GetAxisRaw(joystick2Vertical)));
			newPlayers [1] = FindClosestCharacter (controllerAngle2);
		}
		if (Input.GetKeyDown ("joystick 1 button 0")) {
			if (newPlayers [0]) {
				meshes [0] = newPlayers [0].GetComponent<MeshFilter> ();
				GameManager.instance.SetupPlayer (meshes [0].mesh, 0);
				charactersSelected++;
			}
		}
		if (Input.GetKeyDown ("joystick 2 button 0")) {
			meshes [1] = newPlayers [1].GetComponent<MeshFilter>();
			GameManager.instance.SetupPlayer (meshes[1].mesh, 1);
			charactersSelected++;
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

	void ChangePlayerColor(GameObject player)
	{
		Renderer renderer = player.GetComponent<Renderer> ();
		renderer.material.color = Color.red;
		MeshRenderer mesh_renderer = player.GetComponent<MeshRenderer> ();
		if(mesh_renderer)
			mesh_renderer.material = renderer.material;
	}

	void UpdateScene()
	{
		EditorSceneManager.LoadScene ("NewMap");
	}

	void SelectCharacter()
	{

	}
}
