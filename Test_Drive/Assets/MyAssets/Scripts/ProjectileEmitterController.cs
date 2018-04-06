using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEmitterController : MonoBehaviour {

	public GameObject projectilePrefab; //stores the template of a tether object
	public ProjectileController projectile; //the tether script

	private const float LAUNCH_FORCE = 200; //force applied to tether at launch

	//public float cooldownTime; //cooldown timer max (time between tether launches)
	//private float currentCooldownTimer; //current cooldown time

	public Transform firePoint;
	public GameObject crosshair;

	// Use this for initialization
	void Start () {
		projectilePrefab = Resources.Load("Prefabs/BaseProjectile") as GameObject;
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void launchProjectile(){
		Debug.Log ("launchProjectile call");
		if (!projectile.projectileActive) { 
			Debug.Log ("inside if");

			projectile.prefab = Instantiate (projectilePrefab, firePoint.position, firePoint.rotation) as GameObject;
			Debug.Log (firePoint.position);
			//make sure tether is actually created
			if (projectile.prefab != null) {
				projectile.projectileActive = true;
			}

			projectile.prefab.layer = LayerMask.NameToLayer("Player " + GetComponentInParent<Char_Code>().playerNumber);//convert player number to player number layer

			//SphericalGravity.getItems(); //TODO: OPTIMIZATION: Add new tether to planet upon creation

			Rigidbody tempRigidBody = projectile.GetComponent<ProjectileController>().prefab.GetComponent<Rigidbody>(); //KEEP for projectile launching
			tempRigidBody.AddForce (projectile.prefab.transform.up * LAUNCH_FORCE); //KEEP for projectile launching
		}
	}
}
