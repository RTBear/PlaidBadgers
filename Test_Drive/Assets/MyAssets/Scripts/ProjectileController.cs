using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour {

	public GameObject prefab;
	private Rigidbody m_rb;

	public bool projectileActive = false;
	private const float EXPIRATION_TIME = 2;
	private float expirationTimer = EXPIRATION_TIME;

	// Use this for initialization
	void Start () {
		m_rb = GetComponent<Rigidbody> ();
		m_rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
	}

	// Update is called once per frame
	void Update () {
		if (expirationTimer > 0) {
			Debug.Log ("expT--");
			expirationTimer--;
		} else {
			resetProjectile ();
		}

	}

	void resetProjectile(){
		expirationTimer = EXPIRATION_TIME;
		Destroy (m_rb); 
		Destroy (prefab);
		projectileActive = false;
	}

	void OnCollisionEnter(Collision col){
		var collisionParent = col.collider.GetComponent<GameObjectScript>();
		 
		if(collisionParent){//make sure it collided with a player or item
			Vector2 knockDir = (col.transform.position - collisionParent.transform.position).normalized;
			Attack basicProjectileImpact = new Attack (10, knockDir, 5);

			collisionParent.GetComponent<GameObjectScript> ().attacked (basicProjectileImpact);
		}

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
		resetProjectile();
	}
}