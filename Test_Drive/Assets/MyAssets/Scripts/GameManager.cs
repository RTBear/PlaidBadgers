using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;

	public enum CharacterType{CUBE, SPHERE, CYLINDAR, PILL};
	//A dictionary or "map" that you give it the key value of the players id and it will return the players type
	Dictionary<int, CharacterType> characterMap;

	public GameObject[] items;
	public GameObject[] players;
	public GameObject[]	planets;
	public GameObject cubePrefab, spherePrefab, cylindarPrefab, pillPrefab;

	// Use this for initialization
	void Awake () {
		Debug.Log ("Reloading...");
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);

		DontDestroyOnLoad(gameObject);

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
		
	public void AssignCharacterType(int id, CharacterType type)
	{
		characterMap.Add (id, type);
	}

	public void InitialSpawnPlayers()
	{
		for(int i = 0; i < characterMap.Count; i++)
		{
			GameObject prefab = GetPrefab(characterMap [i + 1]);
			GameObject temp = (GameObject)Instantiate (prefab, new Vector2 ((i + 1) * 3,(i + 1) * 3), Quaternion.identity);
			temp.GetComponent<Char_Code>().playerNumber = i + 1;
			players [i] = temp;
		}
		//players = GameObject.FindGameObjectsWithTag("Player");
		planets = GameObject.FindGameObjectsWithTag("Planet");
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
			return cubePrefab;
			break;
		case CharacterType.CYLINDAR:
			return cylindarPrefab;
			break;
		case CharacterType.PILL:
			return pillPrefab;
			break;
		case CharacterType.SPHERE:
			return spherePrefab;
			break;
		}
		return null;
	}
}
