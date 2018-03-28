using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;

	public enum CharacterType{CUBE, SPHERE, ROBOT, PILL};
	//A dictionary or "map" that you give it the key value of the players id and it will return the players type
	Dictionary<int, CharacterType> characterMap;
	GameObject textPrefab;
	GameObject canvasPrefab;
	public GameObject[] items;
	public GameObject[] players;
	public GameObject[]	planets;

	// Use this for initialization
	void Awake () {
		Debug.Log ("Reloading...");
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);

		DontDestroyOnLoad(gameObject);

		textPrefab = Resources.Load ("Prefabs/TextPrefab") as GameObject;
		canvasPrefab = Resources.Load ("Canvas") as GameObject;
		InitGame();
	}

	void Start()
	{

	}

	void InitGame () {
		characterMap = new Dictionary<int, CharacterType> ();
		planets = GameObject.FindGameObjectsWithTag("Planet");
		players = GameObject.FindGameObjectsWithTag("Player");
		items = GameObject.FindGameObjectsWithTag("Item");
		assignObjectsToPlanets();
	}
	
	// Update is called once per frame
	void Update () {
		assignObjectsToPlanets();
	}

	void assignObjectsToPlanets () {
		//Modify this later
		items = GameObject.FindGameObjectsWithTag("Item");

		foreach(GameObject p in planets){
			var planet = p.GetComponent<SphericalGravity>();
			//think of a better way later
			planet.players.Clear();
			planet.items.Clear();
		}

		foreach (GameObject player in players) {
			bool assigned = false;
			foreach(GameObject p in planets){
				var planet = p.GetComponent<SphericalGravity>();
				if (planet.inRange (player)) {
					planet.players.Add(player);
					assigned = true;
					continue;
				}
			}
			if (!assigned) {
 				player.GetComponent<PlayerController>().notOnPlanet();
			}
		}

		foreach (GameObject item in items) {
			foreach (GameObject p in planets) {
				var planet = p.GetComponent<SphericalGravity> ();
				if (planet.inRange (item)) {
					planet.items.Add(item);
					continue;
				}
			}
		}
	}
		
	public void AssignCharacterToMap(int id, CharacterType type)
	{
		characterMap.Add (id, type);
	}

	public void RemoveCharacterFromMap(int id)
	{
		characterMap.Remove (id);
	}

	public void InitialSpawnPlayers()
	{
		Vector3[] textSpawnLocations = new Vector3[4];
		Vector2[] spawnLocations = new Vector2[4];
		planets = GameObject.FindGameObjectsWithTag("Planet");
		if (planets [0]) 
		{
			Vector3 planetSize = planets [0].GetComponent<Renderer> ().bounds.size;
			textSpawnLocations [0] = new Vector3 (-planetSize.x / 4f, planetSize.y / 4f, 0);
			textSpawnLocations [1] = new Vector3 (planetSize.x / 4f, planetSize.y / 4f, 0);
			textSpawnLocations [2] = new Vector3 (-planetSize.x / 4f, -planetSize.y / 4f, 0);
			textSpawnLocations [3] = new Vector3 (planetSize.x / 4f, -planetSize.y / 4f, 0);

			spawnLocations [0] = new Vector2 (0, planetSize.y);
			spawnLocations [1] = new Vector2 (0, -planetSize.y);
			spawnLocations [2] = new Vector2 (planetSize.x, 0);
			spawnLocations [3] = new Vector2 (-planetSize.x, 0);
		} 
		else 
		{
			Debug.Log ("There are no planets available to be spawned on");
		}

		for(int i = 0; i < characterMap.Count; i++)
		{
			GameObject prefab = GetPrefab(characterMap [i + 1]);
			GameObject temp = (GameObject)Instantiate (prefab, spawnLocations[i], Quaternion.identity);
			temp.GetComponent<Char_Code>().playerNumber = i + 1;

			/* 	This is some progress made for the canvas. I am sick of messing with it for so long so I'm going 
				to do it a little bit different for right now. I will come back to this if we need to (for the radial health)*/
			//			GameObject canvas = Instantiate (canvasPrefab, new Vector3 (0, 0, -1), Quaternion.identity);
			//			canvas.transform.localPosition = new Vector3 (0, 0, -1);
			//			canvas.GetComponent<Canvas> ().worldCamera = Camera.main;
			//			Text tempText = canvas.AddComponent<Text>();
			//			tempText.transform.SetParent (canvas.transform);
			//			tempText.transform.position = textSpawnLocations [i];

			//spawn the text prefab and attach that to a player
			GameObject tempText = (GameObject)Instantiate (textPrefab,textSpawnLocations [i], Quaternion.identity);
			tempText.GetComponent<TextMesh> ().characterSize = .5f;
			temp.GetComponent<Char_Code> ().SetHealthTextObject(tempText);
			players [i] = temp;
		}
		players = GameObject.FindGameObjectsWithTag("Player");
		assignObjectsToPlanets ();
	}

	public void SpawnPlayer(int playerId)
	{
		GameObject prefab = GetPrefab (characterMap [playerId]);
		GameObject temp = (GameObject)Instantiate (prefab, new Vector2 (0,5), Quaternion.identity);
		temp.GetComponent<Char_Code> ().playerNumber = playerId;
		players = GameObject.FindGameObjectsWithTag ("Player");
		assignObjectsToPlanets ();
	}

	protected GameObject GetPrefab(CharacterType type)
	{
		switch (type) 
		{
			case CharacterType.CUBE:
				return Resources.Load("Characters/Cube") as GameObject;
			case CharacterType.ROBOT:
				return Resources.Load("Characters/Robot") as GameObject;
			case CharacterType.PILL:
				return Resources.Load("Characters/Pill") as GameObject;
			case CharacterType.SPHERE:
				return Resources.Load("Characters/Sphere") as GameObject;
		}
		return null;
	}
}
