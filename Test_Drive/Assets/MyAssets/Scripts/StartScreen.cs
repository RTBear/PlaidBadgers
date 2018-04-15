using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown("joystick 1 button 7"))
			SceneManager.LoadScene("CharacterSelect");

		/* Mainly used for start screen*/
		if (gameObject.tag == "Planet") 
		{
			if(gameObject.layer == LayerMask.NameToLayer("Earth"))
				transform.Rotate (Time.deltaTime * 10, Time.deltaTime * 2, Time.deltaTime * 13);
			if(gameObject.layer == LayerMask.NameToLayer("Lava"))
				transform.Rotate (Time.deltaTime * 7, Time.deltaTime * 6, Time.deltaTime * 4);
			if(gameObject.layer == LayerMask.NameToLayer("Moon"))
				transform.Rotate (Time.deltaTime * 13, Time.deltaTime * 10, Time.deltaTime * 3);
		}
	}
}
