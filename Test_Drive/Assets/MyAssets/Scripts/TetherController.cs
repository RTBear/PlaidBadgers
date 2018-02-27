using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetherController : MonoBehaviour {

	public float speed;
	public GameObject prefab;
	// Use this for initialization
	void Start () {
		prefab = Resources.Load ("Prefabs/Tether") as GameObject;
	}
	
	// Update is called once per frame
	void Update () {
		
		//transform.Translate (Vector3.up * speed * Time.deltaTime);
	}
}
