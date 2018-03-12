using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawn : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameManager.instance.SpawnPlayers ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
		
}
