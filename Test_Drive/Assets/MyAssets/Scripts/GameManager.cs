using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;

	public GameObject[] items;
	public GameObject[] players;
	public GameObject[]	planets;

	// Use this for initialization
	void Awake () {
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);

		DontDestroyOnLoad(gameObject);

		InitGame();
	}

	void InitGame () {
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
}
