using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour {

	GameObject tetherPrefab;
	GameObject tether;
	public GameObject parent;
	public GameObject collidedObject;
	public Vector3 tetherCollisionLocation;
	public bool shootingTether;

	// Use this for initialization
	void Start () {
		tetherPrefab = Resources.Load ("Prefabs/Tether2") as GameObject;
		shootingTether = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (collidedObject && collidedObject.tag != "Planet")
			PullObjectToYou ();
		else if (collidedObject && collidedObject.tag == "Planet")
			PullYouToObject ();

		if (tether)
			shootingTether = true;
		else
			shootingTether = false;
	}

	public void ShootTether()
	{
		tether = (GameObject)Instantiate (tetherPrefab, transform.position, Quaternion.identity);
		if (tether) 
		{
			tether.GetComponent<Tether> ().parent = this.gameObject;
		}
	}

	void PullObjectToYou()
	{
		if (Vector3.Distance (transform.position, collidedObject.transform.position) > .5f) {//if outside tolerable distance
			//This is the actual tether pull
			collidedObject.transform.position = Vector3.Lerp (collidedObject.transform.position, transform.position, (Time.deltaTime * 20) / Vector3.Distance (transform.position, collidedObject.transform.position));
		} else {
			parent.GetComponent<Char_Code> ().isTethered = false;
			collidedObject = null;
		}
	}

	void PullYouToObject()
	{
		if (Vector3.Distance (parent.transform.position, collidedObject.transform.position) > .5f) {//if outside tolerable distance
			//This is the actual tether pull
			parent.transform.position = Vector3.Lerp (parent.transform.position, tetherCollisionLocation, (Time.deltaTime * 20) / Vector3.Distance (parent.transform.position, tetherCollisionLocation));
		} else {
			parent.GetComponent<Char_Code> ().isTethered = false;
			collidedObject = null;

		}
	}

}
