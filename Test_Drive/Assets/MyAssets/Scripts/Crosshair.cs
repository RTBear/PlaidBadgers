using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour {

	GameObject tetherPrefab;
	GameObject tether;
	public GameObject parent;
	public GameObject collidedObject;
	public Vector3 tetherCollisionLocation;
	public bool shootingTether, shootingProjectile;
	public GameObject projectilePrefab; //stores the template of a tether object
	public GameObject projectile;
	bool canShoot;
	float shootingTimer = 1;
	float shootingCounter;

	private const float LAUNCH_FORCE = 1000; //force applied to tether at launch

	// Use this for initialization
	void Start () {
		tetherPrefab = Resources.Load ("Prefabs/Tether2") as GameObject;
		projectilePrefab = Resources.Load("Prefabs/BaseProjectile") as GameObject;
		shootingTether = false;
		canShoot = true;
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

		if (projectile)
			shootingProjectile = true;
		else
			shootingProjectile = false;
		
		shootingCounter += Time.deltaTime;
		if (shootingCounter >= shootingTimer)
			canShoot = true;
	}

	public void ShootTether()
	{
		Debug.Log ("Shooting tether");
		Vector3 spawnPos = gameObject.transform.Find ("Cube").transform.position;
		tether = (GameObject)Instantiate (tetherPrefab, spawnPos + (transform.up), Quaternion.identity);
		if (tether) 
		{
			tether.GetComponent<Tether> ().parent = this.gameObject;
			tether.GetComponent<Tether>	().pos = this.gameObject.transform.up;
		}
	}

	void PullObjectToYou()
	{
		if (tether && Vector3.Distance (transform.position, collidedObject.transform.position) > .5f) {//if outside tolerable distance
			//This is the actual tether pull
			collidedObject.transform.position = Vector3.Lerp (collidedObject.transform.position, transform.position, (Time.deltaTime * 20) / Vector3.Distance (transform.position, collidedObject.transform.position));
			tether.transform.position = Vector3.Lerp (tether.transform.position, transform.position, (Time.deltaTime * 20) / Vector3.Distance (transform.position, tether.transform.position));
		} else {
			if(collidedObject.GetComponent<Char_Code> ())
				collidedObject.GetComponent<Char_Code> ().isTethered = false;
			Destroy (tether);
			collidedObject = null;
		}
	}

	void PullYouToObject()
	{
		if (!parent) {
			Destroy (tether);
			collidedObject = null;
			return;
		}
			
		Debug.Log ("Pulling to planet");

		if (Vector3.Distance (parent.transform.position, collidedObject.transform.position) > .5f) {//if outside tolerable distance
			//This is the actual tether pull
			parent.transform.position = Vector3.Lerp (parent.transform.position, tetherCollisionLocation, (Time.deltaTime * 20) / Vector3.Distance (parent.transform.position, tetherCollisionLocation));
		} else {
			if(collidedObject.GetComponent<Char_Code> ())
				collidedObject.GetComponent<Char_Code> ().isTethered = false;
			Destroy (tether);
			collidedObject = null;
		}
	}

	public void launchProjectile(){
		if(canShoot)
		{
			canShoot = false;
			projectile = Instantiate (projectilePrefab, transform.position + (transform.up * 2), transform.rotation) as GameObject;

		
			Rigidbody tempRigidBody = projectile.GetComponent<Rigidbody>(); //KEEP for projectile launching
			tempRigidBody.AddForce (projectile.transform.up * LAUNCH_FORCE); //KEEP for projectile launching
		}
	}
}
