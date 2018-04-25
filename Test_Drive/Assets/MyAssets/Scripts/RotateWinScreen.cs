using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWinScreen : MonoBehaviour {
	GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		player.transform.Rotate(0, Time.deltaTime * 20, 0);
	}
}
