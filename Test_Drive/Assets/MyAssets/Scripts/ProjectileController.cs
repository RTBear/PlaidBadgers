﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour {

	public bool collided = false;
	public float timer = 2;
	public float counter;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		
		counter += Time.deltaTime;
		if (counter >= timer)
			Destroy (this.gameObject);
	}

	void OnCollisionEnter(Collision col){
		Debug.Log ("collision with: " + col.gameObject.ToString ());
		var collisionParent = col.collider.GetComponent<GameObjectScript>();
		 
		Debug.Log ("collisionParent: " + collisionParent);
		if(collisionParent){//make sure it collided with a player or item

			Vector2 knockDir = (col.transform.position - collisionParent.transform.position).normalized;
			Attack basicProjectileImpact = new Attack (10, knockDir, 25);

			collisionParent.attacked (basicProjectileImpact);
		}

		Destroy (this.gameObject);

//		var objectsScript = c.GetComponent<GameObjectScript>();
//		print(objectsScript);
//		if (objectsScript != null) {
//			//audio.Play();
//
//			Vector2 knockDir = (c.transform.position - this.transform.position).normalized;
//			Attack basicAttack = new Attack (10, knockDir, 10);
//
//			objectsScript.attacked (basicAttack);
//
//		}
	}
}