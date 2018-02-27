using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetherEmitterController : MonoBehaviour {

	//public bool isFiring = false;
	private bool tetherActive = false;
	private bool tetherCollide = false;


	public GameObject tetherPrefab; //stores the template of a tether object
	public TetherController tether; //the tether script

	private float tetherLaunchForce = 2000; //force applied to tether at launch

	public float expirationTime; // time a tether can stay active
	private float currentExpirationTimer; // timer until active tether expires

	//public float cooldownTime; //cooldown timer max (time between tether launches)
	//private float currentCooldownTimer; //current cooldown time

	public Transform firePoint;
	public GameObject crosshair;

	// Use this for initialization
	void Start () {
		tetherPrefab = Resources.Load("Prefabs/Tether") as GameObject;
	}
	
	// Update is called once per frame
	void Update () {

		if (tetherActive) {
			currentExpirationTimer -= Time.deltaTime;
			if (currentExpirationTimer <= 0 || (Input.GetAxisRaw ("RightTrigger") == 0) && !tetherCollide) {//if tether expires or if trigger is released before tether expires
				Destroy(tether.prefab);
				if (!tether.prefab) {//make sure tether is actually deleted
					tetherActive = false;
				}
					Debug.Log ("destroyed tether");
			}
		}
	}

	public void launchTether(){


		if (!tetherActive) {
			currentExpirationTimer = expirationTime;
			tether.prefab = Instantiate (tetherPrefab, firePoint.position, firePoint.rotation) as GameObject;

			//make sure tether is actually created
			if (tether.prefab) {
				tetherActive = true;
			}

			Rigidbody tempRigidBody = tether.prefab.GetComponent<Rigidbody>();
			//SphericalGravity.getItems(); //TODO: OPTIMIZATION: Add new tether to planet upon creation
			tempRigidBody.AddForce (transform.up * tetherLaunchForce);
		}
	}
}
