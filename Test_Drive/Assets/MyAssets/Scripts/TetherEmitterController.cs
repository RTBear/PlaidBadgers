using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetherEmitterController : Char_Code {

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
		tether = Instantiate(tether, transform) as TetherController;
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

		//Debug.Log (tether.prefab);
		if (tether.tetherActive) {
			Debug.Log ("tether.tetheractive"); 
			currentExpirationTimer -= Time.deltaTime;
			Debug.Log("time--");
			//if (!tether.isFiring) {//if tether expires or if trigger is released before tether expires
				//tether.destroySelf();
				//if (!tether.prefab) {//make sure tether is actually deleted
				//	tetherActive = false;
				//	Debug.Log ("destroyed tether");
				//}
			//}
		} else if(tether.prefab != null){ 
			tether.resetTether ();
		}
	}



	public void launchTether(){
		tether.isFiring = true;
		if (!tether.tetherActive) {
			currentExpirationTimer = expirationTime;
			tether.prefab = Instantiate (tetherPrefab, firePoint.position, firePoint.rotation) as GameObject;
			//make sure child knows where parent is
			//Debug.Log(firePoint.position);
//			Debug.Log (firePoint.);
			//tether.p_firePoint = new Vector3(0,3.7f,0);
//			tether.p_firePoint = firePoint.position;


			//make sure tether is actually created
			Debug.Log("launch tether");
			if (tether.prefab != null) {
				Debug.Log ("tether active");
				tether.tetherActive = true;
			}

//			Debug.Log (LayerMask.NameToLayer ("Player " + (playerNumber + 1)));
			Debug.Log (LayerMask.NameToLayer ("Player 1"));

			Debug.Log ("tether before");
			Debug.Log (tether.prefab.layer.ToString());
//			tether.prefab.layer = LayerMask.NameToLayer("Player " + (playerNumber + 1));//convert player number to player number layer
			tether.prefab.layer = LayerMask.NameToLayer("Player 1");//convert player number to player number layer
//			tether.prefab.GetComponent<LayerMask>() = LayerMask.NameToLayer("Player " + (playerNumber + 1));
			Debug.Log("tether after");
			Debug.Log (tether.prefab.layer.ToString());

			//tether.prefab.layer = tetherMask.value; 
			//Debug.Log("tetherMask: " + tetherMask.value);
			Debug.Log("pn: " + playerNumber);



			//SphericalGravity.getItems(); //TODO: OPTIMIZATION: Add new tether to planet upon creation


			//Rigidbody tempRigidBody = tether.prefab.GetComponent<Rigidbody>(); //KEEP for projectile launching
			//tempRigidBody.AddForce (transform.up * tetherLaunchForce); //KEEP for projectile launching
		}
	}
}
