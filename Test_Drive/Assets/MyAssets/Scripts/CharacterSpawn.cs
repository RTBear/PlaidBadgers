using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawn : MonoBehaviour {

	GameObject textPrefab;
	// Use this for initialization
	void Start () {
		GameManager.instance.InitialSpawnPlayers ();
		//reenable the GameManager script
		GameManager.instance.enabled = true;
		textPrefab = Resources.Load ("Prefabs/TextPrefab") as GameObject;
		//canvas = FindObjectOfType ("Canvas") as GameObject;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
		
}
