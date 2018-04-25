using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSelect : MonoBehaviour {

	GameObject[] availableMaps;
	int currentMapSelection;
	GameObject mapSelector;
	bool canSwitchMap = true;
	float mapCooldown = 0;
	// Use this for initialization
	void Start () {
		availableMaps = GameObject.FindGameObjectsWithTag("Map");

		Debug.Log ("Available maps array: ");
		foreach (GameObject map in availableMaps)
			Debug.Log (map.name);
	
		//I was having issues with the available maps giving me the incorrect map when selecting one
		//I am going to sort them by the way they are layed out in the MapSelect scene
		//Earth
		//Fire and Ice
		//All Four Planets
		//Earth and Moon
		SortMapsByMapSelectionLayout ();
		Debug.Log("Amount of available maps: " + availableMaps.Length);
	}
	
	// Update is called once per frame
	void Update () {
		//Only allow player one to switch maps
		if((Input.GetAxis("Joystick1Horizontal") == -1 || Input.GetAxis("Joystick1Horizontal") == 1) && canSwitchMap)
		{
			canSwitchMap = false;
			SwitchCurrentMapSelection(Input.GetAxis("Joystick1Horizontal"));


		}

		if(!canSwitchMap)
			mapCooldown += Time.deltaTime;

//		if (canSwitchMap) 
//		{
//			transform.Rotate(0, 90 * Input.GetAxis(, 0);
//		}

		if(mapCooldown >= .25f)
		{
			canSwitchMap = true;
			mapCooldown = 0;
		}
		if(Input.GetKeyDown("joystick 1 button 0"))
			SelectMap();


	}

	void SwitchCurrentMapSelection(float value)
	{
		
		transform.Rotate(0, 90 * value, 0);

		currentMapSelection += (int)value;
		if(currentMapSelection > availableMaps.Length - 1) //make it zero indexed
			currentMapSelection = 0;
		if(currentMapSelection < 0)
			currentMapSelection = availableMaps.Length - 1;

		foreach (GameObject map in availableMaps)
			map.transform.Rotate (0, 90 * value, 0);

		Debug.Log("Current map: " + availableMaps[currentMapSelection].name);
	}

	void SelectMap()
	{
		SceneManager.LoadScene(availableMaps[currentMapSelection].name);
	}

	//This is a stupid way to find them but I am tired and can't think of anything right now...
	void SortMapsByMapSelectionLayout()
	{
		GameObject[] tempArray = new GameObject[availableMaps.Length];

		for (int i = 0; i < availableMaps.Length; i++) 
		{
			switch (availableMaps [i].name) 
			{
			case "Earth":
				tempArray [0] = availableMaps [i];
				break;
			case "FireAndIce":
				tempArray[1] = availableMaps [i];
				break;
			case "AllFourPlanets":
				tempArray[2] = availableMaps [i];
				break;
			case "EarthAndMoon":
				tempArray[3] = availableMaps [i];
				break;
			}
		}
		availableMaps = tempArray;
	}
}
