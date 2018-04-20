using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEmitterController : MonoBehaviour {

	public GameObject projectilePrefab; //stores the template of a projectile object
	public ProjectileController projectile; //the projectile script

	private const float LAUNCH_FORCE = 1000; //force applied to projectile at launch

	//public float cooldownTime; //cooldown timer max (time between projectile launches)
	//private float currentCooldownTimer; //current cooldown time

	public Transform firePoint;
	public GameObject crosshair;

	private const float EXPIRATION_TIME = 1;
	private float expirationTimer = EXPIRATION_TIME;

	private const float COOLDOWN_TIME = 0.8f;
	private float cooldownTimer = 0;

	// Use this for initialization
	void Start () {
		projectilePrefab = Resources.Load("Prefabs/BaseProjectile") as GameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if (cooldownTimer > 0) {
			cooldownTimer -= Time.deltaTime;
		}
		if (projectile) {
			if (projectile.projectileActive) {
				Debug.Log (projectile.collided);
				if (expirationTimer < 0 || projectile.collided) {
					Debug.Log ("time to remove");
					projectile.resetProjectile ();
					expirationTimer = EXPIRATION_TIME;
					cooldownTimer = COOLDOWN_TIME;
				} else {
					expirationTimer -= Time.deltaTime;
				}
			}
		}
	}

	public void launchProjectile(){
		if (!projectile.projectileActive && cooldownTimer <= 0) { 
			
			projectile.GetComponent<ProjectileController> ().parentCode = GetComponentInParent<ProjectileEmitterController>();

			projectile.prefab = Instantiate (projectilePrefab, firePoint.position, firePoint.rotation) as GameObject;

			//make sure projectile is actually created
			if (projectile.prefab != null) {
				projectile.projectileActive = true;
				expirationTimer = EXPIRATION_TIME;
			}

			projectile.prefab.layer = LayerMask.NameToLayer("Player " + GetComponentInParent<Char_Code>().playerNumber);//convert player number to player number layer

			//SphericalGravity.getItems(); //TODO: OPTIMIZATION: Add new projectile to planet upon creation

			Rigidbody tempRigidBody = projectile.GetComponent<ProjectileController>().prefab.GetComponent<Rigidbody>();
			tempRigidBody.AddForce (projectile.prefab.transform.up * LAUNCH_FORCE);
		}
	}
}
