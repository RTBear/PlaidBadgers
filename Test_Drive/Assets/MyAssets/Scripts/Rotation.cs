using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(0, Time.deltaTime, 0);

		if(gameObject.tag == "Planet")
			transform.Rotate(Time.deltaTime * 5,Time.deltaTime * 3, Time.deltaTime * 3);
	}
}
