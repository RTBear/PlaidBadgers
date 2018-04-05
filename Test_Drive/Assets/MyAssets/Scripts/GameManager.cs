using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

	public static GameManager instance = null;

	gameMode currentMode;

	enum gameMode
	{
selectCharacter,
		battle,
		none}

	;

	public enum CharacterType
	{
CUBE,
		SPHERE,
		ROBOT,
		PILL}

	;
	//A dictionary or "map" that you give it the key value of the players id and it will return the players type
	Dictionary<int, CharacterType> characterMap;
	GameObject textPrefab;
	GameObject canvasPrefab;
	public GameObject[] items;
	public GameObject[] players;
	public GameObject[]	planets;

	private LineRenderer tether_LR;
	//when tethered draw 'rope'
	public Dictionary<GameObjectScript, GameObject> tetheringPlayers;
	private const float TETHER_PULL_SPEED = 20;
	private const int TETHER_DISTANCE_TOLERANCE = 1;

	void ManageSceneStuff (Scene scene, LoadSceneMode mode)
	{
		if (scene.name == "CharacterSelect") {
			currentMode = gameMode.selectCharacter;
		} else if (scene.name == "NewMap") {
			currentMode = gameMode.battle;
			InitGame ();
		} else {
			currentMode = gameMode.none;
		}
	}

	// Use this for initialization
	void Awake ()
	{
		SceneManager.sceneLoaded += ManageSceneStuff;

		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
		
		DontDestroyOnLoad (gameObject);

		GeneralInitialization ();
	}

	void GeneralInitialization ()
	{
		textPrefab = Resources.Load ("Prefabs/TextPrefab") as GameObject;
		canvasPrefab = Resources.Load ("Canvas") as GameObject;
		characterMap = new Dictionary<int, CharacterType> ();
		tetheringPlayers = new Dictionary<GameObjectScript, GameObject> ();
		instance.tether_LR = GetComponent<LineRenderer> ();
	}

	void InitGame ()
	{
		planets = GameObject.FindGameObjectsWithTag ("Planet");
		players = GameObject.FindGameObjectsWithTag ("Player");
		items = GameObject.FindGameObjectsWithTag ("Item");
		assignObjectsToPlanets ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (currentMode == gameMode.battle) {
			assignObjectsToPlanets ();
		}
		if (tetheringPlayers.Count > 0) {
			Debug.Log ("game manager update in if");
			pullTethers (); //if people are tethering pull tethers
		}
	}

	//loop through dictionary lerping appropriate objects
	void pullTethers ()
	{
		foreach (KeyValuePair<GameObjectScript,GameObject> obj in tetheringPlayers) {
			GameObjectScript player = obj.Key;
			GameObject tetheree = obj.Value;
			Transform playerTrans = player.GetComponent<Transform> ();
			Transform tethereeTrans = tetheree.GetComponent<Transform> ();
			Debug.Log (player.GetComponent<Char_Code> ().tetherCollisionLocation);
			if (tetheree.GetComponent<GameObjectScript> ()) {//tetheree is an object that can be pulled to player... lerp to player
				Transform destination = playerTrans.position;
				if (Vector3.Distance (destination.position, tethereeTrans.position) > TETHER_DISTANCE_TOLERANCE) {
					Debug.Log ("pullTethers object");
					if (instance.tether_LR) {
						instance.tether_LR.enabled = true;
						instance.tether_LR.SetPosition (0, destination.position);
						instance.tether_LR.SetPosition (1, tethereeTrans.position);
					}

					tethereeTrans.position = Vector3.Lerp (tethereeTrans.position, destination.position, (Time.deltaTime * TETHER_PULL_SPEED) / Vector3.Distance (playerTrans.position, tethereeTrans.position));
				} else {
					if (instance.tether_LR) {
						instance.tether_LR.enabled = false;
					}
					tetheringPlayers.Remove (player);
				}
			} else if (tetheree.CompareTag ("Planet")) { //tetheree is a planet... lerp player to planet
				Transform destination = player.GetComponent<Char_Code> ().tetherCollisionLocation;

				if (Vector3.Distance (destination.position, playerTrans.position) > TETHER_DISTANCE_TOLERANCE) {
					Debug.Log ("pullTethers planet");

					if (instance.tether_LR) {
						instance.tether_LR.enabled = true;
						instance.tether_LR.SetPosition (0, destination.position);
						instance.tether_LR.SetPosition (1, playerTrans.position);
					}
					playerTrans.position = Vector3.Lerp (destination.position, playerTrans.position, (Time.deltaTime * TETHER_PULL_SPEED) / Vector3.Distance (destination.position, playerTrans.position));
				} else {
					if (instance.tether_LR) {
						instance.tether_LR.enabled = false;
					}
					tetheringPlayers.Remove (player);
				}
			}
			
		}
	}

	void assignObjectsToPlanets ()
	{
		//Modify this later
		items = GameObject.FindGameObjectsWithTag ("Item");
		players = GameObject.FindGameObjectsWithTag ("Player");

		foreach (GameObject p in planets) {
			var planet = p.GetComponent<SphericalGravity> ();
			//think of a better way later
			planet.players.Clear ();
			planet.items.Clear ();
		}

		foreach (GameObject player in players) {
			bool assigned = false;
			foreach (GameObject p in planets) {
				var planet = p.GetComponent<SphericalGravity> ();
				if (planet.inRange (player)) {
					planet.players.Add (player);
					assigned = true;
					continue;
				}
			}
			if (!assigned) {
				player.GetComponent<PlayerController> ().notOnPlanet ();
			}
		}

		foreach (GameObject item in items) {
			foreach (GameObject p in planets) {
				var planet = p.GetComponent<SphericalGravity> ();
				if (planet.inRange (item)) {
					planet.items.Add (item);
					continue;
				}
			}
		}
	}

	public void AssignCharacterToMap (int id, CharacterType type)
	{
		characterMap.Add (id, type);
	}

	public void RemoveCharacterFromMap (int id)
	{
		characterMap.Remove (id);
	}

	public void InitialSpawnPlayers ()
	{
		Vector3[] textSpawnLocations = new Vector3[4];
		Vector2[] spawnLocations = new Vector2[4];
		planets = GameObject.FindGameObjectsWithTag ("Planet");
		if (planets [0]) {
			Vector3 planetSize = planets [0].GetComponent<Renderer> ().bounds.size;
			textSpawnLocations [0] = new Vector3 (-planetSize.x / 3f, planetSize.y / 4f, 0);
			textSpawnLocations [1] = new Vector3 (planetSize.x / 7f, planetSize.y / 4f, 0);
			textSpawnLocations [2] = new Vector3 (-planetSize.x / 3f, -planetSize.y / 4f, 0);
			textSpawnLocations [3] = new Vector3 (planetSize.x / 7f, -planetSize.y / 4f, 0);

			spawnLocations [0] = new Vector2 (0, planetSize.y);
			spawnLocations [1] = new Vector2 (0, -planetSize.y);
			spawnLocations [2] = new Vector2 (planetSize.x, 0);
			spawnLocations [3] = new Vector2 (-planetSize.x, 0);
		} else {
			Debug.Log ("There are no planets available to be spawned on");
		}

		for (int i = 0; i < characterMap.Count; i++) {
			int playerId = i + 1; //Account for change of indexing
			GameObject prefab = GetPrefab (characterMap [playerId]);
			GameObject temp = (GameObject)Instantiate (prefab, spawnLocations [i], Quaternion.identity);
			temp.GetComponent<Char_Code> ().playerNumber = playerId;

			/* 	This is some progress made for the canvas. I am sick of messing with it for so long so I'm going 
				to do it a little bit different for right now. I will come back to this if we need to (for the radial health)*/
			
			//			GameObject canvas = Instantiate (canvasPrefab, new Vector3 (0, 0, -1), Quaternion.identity);
			//			canvas.transform.localPosition = new Vector3 (0, 0, -1);
			//			canvas.GetComponent<Canvas> ().worldCamera = Camera.main;
			//			Text tempText = canvas.AddComponent<Text>();
			//			tempText.transform.SetParent (canvas.transform);
			//			tempText.transform.position = textSpawnLocations [i];

			//spawn the text prefab and attach that to a player
			GameObject tempText = (GameObject)Instantiate (textPrefab, textSpawnLocations [i], Quaternion.identity);
			tempText.GetComponent<TextMesh> ().characterSize = .5f;
			temp.GetComponent<Char_Code> ().SetHealthTextObject (tempText);

			temp.layer = LayerMask.NameToLayer ("Player " + playerId);
			Debug.Log ("temp after");
			Debug.Log (temp.layer.ToString ()); 
		}
		players = GameObject.FindGameObjectsWithTag ("Player");
		assignObjectsToPlanets ();
	}

	public void SpawnPlayer (int playerId)
	{
		Vector2 spawnLocation = new Vector2 (0, 0);
		if (planets [0]) {
			//Spawn at the top of the first planet
			spawnLocation = new Vector2 (0, planets [0].GetComponent<Renderer> ().bounds.size.y);
		} else {
			Debug.Log ("There are no planets!");
		}
		GameObject prefab = GetPrefab (characterMap [playerId]);
		GameObject temp = (GameObject)Instantiate (prefab, spawnLocation, Quaternion.identity);

		temp.layer = LayerMask.NameToLayer ("Player " + playerId);
		Debug.Log ("temp after");
		Debug.Log (temp.layer.ToString ()); 
//		temp.GetComponent<PlayerInput> ().SetController (playerId);

		temp.GetComponent<Char_Code> ().playerNumber = playerId;
		players = GameObject.FindGameObjectsWithTag ("Player");
		assignObjectsToPlanets ();
	}

	protected GameObject GetPrefab (CharacterType type)
	{
		switch (type) {
		case CharacterType.CUBE:
			return Resources.Load ("Characters/Cube") as GameObject;
		case CharacterType.ROBOT:
			return Resources.Load ("Characters/Robot") as GameObject;
		case CharacterType.PILL:
			return Resources.Load ("Characters/Pill") as GameObject;
		case CharacterType.SPHERE:
			return Resources.Load ("Characters/Sphere") as GameObject;
		}
		return null;
	}
}
