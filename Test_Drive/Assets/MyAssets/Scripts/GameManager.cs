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

	//A dictionary or "map" that you give it the key value of the players id and it will return the players type
	Dictionary<int, CharacterAttributes> characterMap;
	GameObject textPrefab;
	GameObject canvasPrefab;
	public GameObject[] items;
	public GameObject[] players;
	public GameObject[]	planets;
	Dictionary<int, RespawnTimer> respawnTimers = new Dictionary<int, RespawnTimer>();
	int numPlayers = 4;

	float respawnTime = 1f;

	private LineRenderer tether_LR;//when tethered draw 'rope'
	public Dictionary<GameObjectScript, GameObject> tetheringPlayers;
	private const float TETHER_PULL_SPEED = 20;
	private const float TETHER_DISTANCE_TOLERANCE = 0.5f;
	public const float TETHER_HOLD_TIME = 2;


	void ManageSceneStuff (Scene scene, LoadSceneMode mode)
	{
		if (scene.name == "CharacterSelect") {
			currentMode = gameMode.selectCharacter;
		} else if (scene.name == "NewMap 1") {
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
		characterMap = new Dictionary<int, CharacterAttributes> ();
		tetheringPlayers = new Dictionary<GameObjectScript, GameObject> ();
		instance.tether_LR = GetComponent<LineRenderer> ();
	}

	void InitGame () {
		//this is just for starting the game without character selection screen
		CharacterAttributes tempAttributes = new CharacterAttributes();
		tempAttributes.m_CharacterType = CharacterAttributes.CharacterType.ROBOT;
		tempAttributes.SetPrefab ();
		if(characterMap.Count == 0) AssignCharacterToMap(1, tempAttributes);

		//set up the respawn timers
		for (int i = 1; i <= characterMap.Count; i++) {
 			RespawnTimer timer = gameObject.AddComponent<RespawnTimer>();
			timer.Init (respawnTime, i);
			respawnTimers[i] = timer;
		}

		numPlayers = characterMap.Count;
		planets = GameObject.FindGameObjectsWithTag("Planet");
	}
	
	// Update is called once per frame
	void Update () {
		
		if (currentMode == gameMode.battle) {
			//Modify this later
			items = GameObject.FindGameObjectsWithTag("Item");
			players = GameObject.FindGameObjectsWithTag ("Player");
			//change to be dynamic with number of players
			if (players.Length < numPlayers) {
				int playerId = findMissingPlayerId();
				if(playerId != 0){
					respawnTimers [playerId].On ();
				};
			}
			assignObjectsToPlanets();
		}
		if (tetheringPlayers.Count > 0) {
			pullTethers (); //if people are tethering pull tethers
		}
	}

	//remove an object from being tethered
	public void removeValFromTether(GameObjectScript obj){
		Debug.Log ("remove tether call");
		foreach (KeyValuePair<GameObjectScript,GameObject> pair in tetheringPlayers) {
			if (pair.Value == obj) {
				tetheringPlayers.Remove (pair.Key);
				Debug.Log ("broke tether for: " + obj.ToString ());
			}
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

			if (tetheree.GetComponent<GameObjectScript> ()) {//tetheree is an object that can be pulled to player... lerp to player
				Vector3 destination = playerTrans.position + playerTrans.right;// place destination in front of player.
				tetheree.GetComponent<GameObjectScript>().tethered = true;//disable tetheree
				if (Vector3.Distance (destination, tethereeTrans.position) > TETHER_DISTANCE_TOLERANCE) {//if outside tolerable distance
					Debug.Log ("pullTethers object");
					if (instance.tether_LR) {
						instance.tether_LR.enabled = true;//draw rope
						instance.tether_LR.SetPosition (0, destination);
						instance.tether_LR.SetPosition (1, tethereeTrans.position);
					}
					//This is the actual tether pull
					tethereeTrans.position = Vector3.Lerp (tethereeTrans.position, destination, (Time.deltaTime * TETHER_PULL_SPEED) / Vector3.Distance (destination, tethereeTrans.position));
				} else {
					if (instance.tether_LR) {
						instance.tether_LR.enabled = false;
					}
					if (player.GetComponent<Char_Code> ().tetherHoldTimer > 0) {
						player.GetComponent<Char_Code> ().tetherHoldTimer--;
					} else {
						player.GetComponent<Char_Code> ().tetherHoldTimer = TETHER_HOLD_TIME;
						player.rb.constraints = RigidbodyConstraints.FreezePositionZ;
						player.GetComponent<Char_Code>().pc.tetherEmitter.tether.resetTether();

						tetheringPlayers.Remove (player);
					}
				}
			} else if (tetheree.CompareTag ("Planet")) { //tetheree is a planet... lerp player to planet
				Vector3 destination = player.GetComponent<Char_Code> ().tetherCollisionLocation.position;//location to tether to

				if (Vector3.Distance (destination, playerTrans.position) > TETHER_DISTANCE_TOLERANCE) {//if outside tolerable distance
					Debug.Log ("pullTethers planet");

					if (instance.tether_LR) {
						instance.tether_LR.enabled = true;//draw rope
						instance.tether_LR.SetPosition (0, destination);
						instance.tether_LR.SetPosition (1, playerTrans.position);
					}
					//This is the actual tether pull
					playerTrans.position = Vector3.Lerp (destination, playerTrans.position, (Time.deltaTime * TETHER_PULL_SPEED) / Vector3.Distance (destination, playerTrans.position));
				} else {
					if (instance.tether_LR) {
						instance.tether_LR.enabled = false;
					}
					player.rb.constraints = RigidbodyConstraints.FreezePositionZ;
					player.GetComponent<Char_Code>().pc.tetherEmitter.tether.resetTether();
					tetheringPlayers.Remove (player);
				}
			}
			
		}
	}

	private int findMissingPlayerId(){
		for (int i = 1; i <= numPlayers; i++) {
			bool found = false;
			foreach (GameObject player in players){
				if (player.GetComponent<Char_Code>().playerNumber == i){
					found = true;
					break;
				}
			}
			if (!found){
				//if a timer is already running then we want to see if anyone else is dead
				if(!respawnTimers[i].isRunning()) return i;
			}
		}
		return 0;
	}

	void assignObjectsToPlanets () {
		foreach(GameObject p in planets){
			var planet = p.GetComponent<SphericalGravity>();
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

	public void AssignCharacterToMap (int id, CharacterAttributes attributes)
	{
		characterMap.Add (id, attributes);
	}

	public void RemoveCharacterFromMap (int id)
	{
		characterMap.Remove (id);
	}

	public void UpdateCharacterMap(int id, CharacterAttributes attributes)
	{
		characterMap [id] = attributes;
	}

	public CharacterAttributes GetAttributesFromMap(int id)
	{
		return characterMap [id];
	}

	public void InitialSpawnPlayers ()
	{
		Vector3[] textSpawnLocations = new Vector3[4];
		Vector2[] spawnLocations = new Vector2[4];
		planets = GameObject.FindGameObjectsWithTag ("Planet");
		if (planets [0]) {
			Vector3 planetSize = planets [0].GetComponent<Renderer> ().bounds.size;
			Vector3 planetLocation = planets[0].GetComponent<Transform>().position;
			textSpawnLocations [0] = new Vector3 (planetLocation.x - planetSize.x / 3f, planetLocation.y + planetSize.y / 4f, 0);
			textSpawnLocations [1] = new Vector3 (planetLocation.x + planetSize.x / 7f, planetLocation.y + planetSize.y / 4f, 0);
			textSpawnLocations [2] = new Vector3 (planetLocation.x - planetSize.x / 3f, planetLocation.y - planetSize.y / 4f, 0);
			textSpawnLocations [3] = new Vector3 (planetLocation.x + planetSize.x / 7f, planetLocation.y - planetSize.y / 4f, 0);

			spawnLocations [0] = new Vector2 (0, planetLocation.y + planetSize.y);
			spawnLocations [1] = new Vector2 (0, planetLocation.y - planetSize.y);
			spawnLocations [2] = new Vector2 (planetLocation.x + planetSize.x, 0);
			spawnLocations [3] = new Vector2 (planetLocation.x - planetSize.x, 0);
		} else {
			Debug.Log ("There are no planets available to be spawned on");
		}

		for (int i = 0; i < characterMap.Count; i++) {
			int playerId = i + 1; //Account for change of indexing
			GameObject prefab = characterMap[playerId].m_Prefab;
			GameObject temp = (GameObject)Instantiate (prefab, spawnLocations [i], Quaternion.identity);
			temp.GetComponent<Char_Code> ().playerNumber = playerId;
			MeshRenderer renderer = temp.GetComponent<MeshRenderer> ();
			if(renderer)
				renderer.material = characterMap [playerId].m_Material;

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
			Vector3 planetSize = planets [0].GetComponent<Renderer> ().bounds.size;
			Vector3 planetLocation = planets[0].GetComponent<Transform>().position;
			spawnLocation = new Vector2 (0, planetLocation.y + planetSize.y);
		} else {
			Debug.Log ("There are no planets!");
		}
		GameObject prefab = characterMap[playerId].m_Prefab;
		GameObject temp = (GameObject)Instantiate (prefab, spawnLocation, Quaternion.identity);
		temp.GetComponent<MeshRenderer> ().material = characterMap [playerId].m_Material;

		temp.layer = LayerMask.NameToLayer ("Player " + playerId);
		Debug.Log ("temp after");
		Debug.Log (temp.layer.ToString ()); 

		temp.GetComponent<Char_Code> ().playerNumber = playerId;
		players = GameObject.FindGameObjectsWithTag ("Player");
		assignObjectsToPlanets ();
	}
}
