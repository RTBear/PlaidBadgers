using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetherEmitterController : MonoBehaviour {

	public bool isFiring;
	private bool tetherActive;

	public TetherController tether;
	public float tetherSpeed;
	public float tetherLaunchForce;

	public float expirationTime; // time a tether can stay active
	private float currentExpirationTimer; // timer until active tether expires

	public float cooldownTime; //cooldown timer max (time between tether launches)
	private float currentCooldownTimer; //current cooldown time

	public Transform firePoint;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (isFiring) {
			currentCooldownTimer -= Time.deltaTime;
			if (currentCooldownTimer <= 0) {
				currentCooldownTimer = cooldownTime;
				launchTether ();
			}
		} else {
			currentCooldownTimer = 0;
		}
	}

	public void launchTether(){
		if (!tetherActive) {
			tetherActive = true;
			tether = Instantiate (tether, firePoint.position, firePoint.rotation) as TetherController;
			//newTether.speed = tetherSpeed;
			Rigidbody tempRigidBody = tether.GetComponent<Rigidbody>();

			tempRigidBody.AddForce (transform.up * tetherLaunchForce);

		} else {
			currentExpirationTimer -= Time.deltaTime;
			if (currentExpirationTimer <= 0) {
				Destroy (tether);
			}
		}
	}
}
