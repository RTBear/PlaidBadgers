using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetherEmitterController : MonoBehaviour {

	public bool isFiring;
	private bool tetherActive;

	public TetherController tether;
	public float tetherSpeed;

	public float timeBetweenShots;
	private float shotCounter; //countdown until I can fire again

	public Transform firePoint;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (isFiring) {
			shotCounter -= Time.deltaTime;
			if (shotCounter <= 0) {
				shotCounter = timeBetweenShots;
				TetherController newTether = Instantiate (tether, firePoint.position, firePoint.rotation) as TetherController;
				newTether.speed = tetherSpeed;
			}
		} else {
			shotCounter = 0;
		}
	}
}
