using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSelect : MonoBehaviour {

	GameObject[] availableMaps;
	int currentMapSelection;
	GameObject mapSelector;
	bool changeMap = false;
	bool canSwitchMap = true;
	float mapCooldown = 0;
	// Use this for initialization
	void Start () {
		availableMaps = GameObject.FindGameObjectsWithTag("Map");
		//this will default earth to be the first selected map
		for(int i = 0; i < availableMaps.Length; i++)
		{
			currentMapSelection = i;
			if(availableMaps[i].name == "Earth")
				break;
		}
		Debug.Log("Amount of available maps: " + availableMaps.Length);
	}
	
	// Update is called once per frame
	void Update () {
		//Only allow player one to switch maps
		if((Input.GetAxis("Joystick1Horizontal") == -1 || Input.GetAxis("Joystick1Horizontal") == 1) && canSwitchMap)
		{
			SwitchCurrentMapSelection(Input.GetAxis("Joystick1Horizontal"));
			canSwitchMap = false;
		}

		if(!canSwitchMap)
			mapCooldown += Time.deltaTime;

		if(mapCooldown >= .5f)
		{
			canSwitchMap = true;
			mapCooldown = 0;
		}
		if(Input.GetKeyDown("joystick 1 button 0"))
			SelectMap();


	}

	void SwitchCurrentMapSelection(float value)
	{
		Debug.Log("Changing map");
		transform.Rotate(0, 90 * value, 0);

		currentMapSelection += (int)value;
		if(currentMapSelection >= availableMaps.Length - 1) //make it zero indexed
			currentMapSelection = 0;
		if(currentMapSelection < 0)
			currentMapSelection = availableMaps.Length - 1;
	}

	void SelectMap()
	{
		SceneManager.LoadScene(availableMaps[currentMapSelection].name);
	}
}
