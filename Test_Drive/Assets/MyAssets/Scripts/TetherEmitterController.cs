using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetherEmitterController : MonoBehaviour {

	public GameObject tetherPrefab; //stores the template of a tether object
	public TetherController tether; //the tether script

	private float tetherLaunchForce = 2000; //force applied to tether at launch

	private float expirationTime = 1; // time a tether can stay active
	private float currentExpirationTimer; // timer until active tether expires

	//public float cooldownTime; //cooldown timer max (time between tether launches)
	//private float currentCooldownTimer; //current cooldown time

	public Transform firePoint;
	public GameObject crosshair;

	// Use this for initialization
	void Start () {
//		tether = GetComponent<TetherController> ();
		tetherPrefab = Resources.Load("Prefabs/Tether") as GameObject;
	}
	
	// Update is called once per frame
	void Update () {
		//check if destroy tether
		if (currentExpirationTimer <= 0) { 
			tether.tetherActive = false;
		}
		if (tether.isFiring == false && tether.tetherAttached == false) {
			tether.tetherActive = false;
		}

		if (tether.tetherActive) {
			currentExpirationTimer -= Time.deltaTime;


		} else if(tether.prefab != null){ 
			tether.resetTether ();
		}
	}



	public void launchTether(){
		tether.isFiring = true;
		if (!tether.tetherActive) {
			currentExpirationTimer = expirationTime;
			tether.GetComponent<TetherController> ().originalParentTransform = transform;
			tether.GetComponent<TetherController> ().originalParentCharCode = GetComponentInParent<Char_Code>();

			tether.prefab = Instantiate (tetherPrefab, firePoint.position, firePoint.rotation) as GameObject;

			//make sure tether is actually created
			Debug.Log("launch tether");
			if (tether.prefab != null) {
				Debug.Log ("tether active");
				tether.tetherActive = true;
			}

			tether.prefab.layer = LayerMask.NameToLayer("Player " + GetComponentInParent<Char_Code>().playerNumber);//convert player number to player number layer

//			Debug.Log("pn: " + GetComponentInParent<Char_Code>().playerNumber);



			//SphericalGravity.getItems(); //TODO: OPTIMIZATION: Add new tether to planet upon creation


			//Rigidbody tempRigidBody = tether.prefab.GetComponent<Rigidbody>(); //KEEP for projectile launching
			//tempRigidBody.AddForce (transform.up * tetherLaunchForce); //KEEP for projectile launching
		}
	}
}
