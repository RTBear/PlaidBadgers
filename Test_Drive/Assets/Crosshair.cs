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
	public GameObject projectilePrefab; //stores the template of a tether object
	public GameObject projectile;

	private const float LAUNCH_FORCE = 1000; //force applied to tether at launch
	private const float EXPIRATION_TIME = 1;
	private float expirationTimer = EXPIRATION_TIME;

	// Use this for initialization
	void Start () {
		tetherPrefab = Resources.Load ("Prefabs/Tether2") as GameObject;
		projectilePrefab = Resources.Load("Prefabs/BaseProjectile") as GameObject;
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
		Debug.Log ("Shooting tether");
		Vector3 spawnPos = gameObject.transform.Find ("Cube").transform.position;
		tether = (GameObject)Instantiate (tetherPrefab, spawnPos, Quaternion.identity);
		if (tether) 
		{
			tether.GetComponent<Tether> ().parent = this.gameObject;
			tether.GetComponent<Tether>	().pos = this.gameObject.transform.up;
		}
	}

	void PullObjectToYou()
	{
		if (Vector3.Distance (transform.position, collidedObject.transform.position) > .5f) {//if outside tolerable distance
			//This is the actual tether pull
			collidedObject.transform.position = Vector3.Lerp (collidedObject.transform.position, transform.position, (Time.deltaTime * 20) / Vector3.Distance (transform.position, collidedObject.transform.position));
			tether.transform.position = Vector3.Lerp (tether.transform.position, transform.position, (Time.deltaTime * 20) / Vector3.Distance (transform.position, tether.transform.position));
		} else {
			parent.GetComponent<Char_Code> ().isTethered = false;
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
			
		if (Vector3.Distance (parent.transform.position, collidedObject.transform.position) > .5f) {//if outside tolerable distance
			//This is the actual tether pull
			parent.transform.position = Vector3.Lerp (parent.transform.position, tetherCollisionLocation, (Time.deltaTime * 20) / Vector3.Distance (parent.transform.position, tetherCollisionLocation));
		} else {
			parent.GetComponent<Char_Code> ().isTethered = false;
			Destroy (tether);
			collidedObject = null;
		}
	}

	public void launchProjectile(){
		if(!projectile)
		{
			projectile = Instantiate (projectilePrefab, transform.position + transform.up, transform.rotation) as GameObject;

			//make sure tether is actually created
			if (projectile != null) {
			//	projectile.GetComponent<ProjectileController>().projectileActive = true;
				expirationTimer = EXPIRATION_TIME;
			}

			projectile.layer = LayerMask.NameToLayer("Player " + GetComponentInParent<Char_Code>().playerNumber);//convert player number to player number layer

			//SphericalGravity.getItems(); //TODO: OPTIMIZATION: Add new tether to planet upon creation

			Rigidbody tempRigidBody = projectile.GetComponent<Rigidbody>(); //KEEP for projectile launching
			tempRigidBody.AddForce (projectile.transform.up * LAUNCH_FORCE); //KEEP for projectile launching
		}
	}
}
