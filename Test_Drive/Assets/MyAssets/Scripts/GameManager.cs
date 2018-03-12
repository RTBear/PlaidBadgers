using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;

	public GameObject[] items;
	public GameObject[] players;
	public GameObject[]	planets;
	public GameObject characterPrefab;
	public Mesh[] meshes;
	public int numPlayers;

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
		
//		if (spawnPlayers)
//			SpawnPlayers ();
	}

	void InitGame () {
		planets = GameObject.FindGameObjectsWithTag("Planet");
		string[] controllers = Input.GetJoystickNames ();
		players = new GameObject[controllers.Length];
		meshes = new Mesh[controllers.Length];
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

//	public void SpawnNewPlayers(int numPlayers, Material[] materials)
//	{
//		players = new GameObject[numPlayers];
//		this.materials = materials;
//	}

	public void SpawnPlayers()
	{
		//players = new GameObject[numPlayers];

		for(int i = 0; i < players.Length; i++)
		{
			players[i] = (GameObject)Instantiate (characterPrefab, new Vector2 ((i + 1) * 3,(i + 1) * 3), Quaternion.identity);
			players[i].GetComponent<Char_Code>().playerNumber = i + 1;
			MeshFilter filter = players [i].GetComponent<MeshFilter> ();
			if(filter)
				filter.mesh = meshes [i];
		}
		planets = GameObject.FindGameObjectsWithTag("Planet");
		assignObjectsToPlanets ();
	}

	public void SetupPlayer(Mesh mesh, int playerNumber)
	{
		meshes[playerNumber] = mesh;
	}
}
