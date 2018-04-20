using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour {

	public GameObject prefab;
	private Rigidbody m_rb;

	public bool projectileActive = false;
	public bool collided = false;

	public ProjectileEmitterController parentCode;

	private const float DAMAGE = 10;
	private const float ATTACK_FORCE = 5;

	// Use this for initialization
	void Start () {
		m_rb = GetComponent<Rigidbody> ();
		m_rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
	}

	// Update is called once per frame
	void Update () {
		

	}

	public void resetProjectile(){
		Debug.Log ("resetting projectile");
		if (prefab) {
			Destroy (prefab);
		}
		parentCode.projectile.projectileActive = false;
		parentCode.projectile.collided = false;
	}

	void OnCollisionEnter(Collision col){
//		Debug.Log ("collision with: " + col.gameObject.ToString ());
		var collisionParent = col.collider.GetComponent<GameObjectScript>();
		 
//		Debug.Log ("collisionParent: " + collisionParent);
		if(collisionParent){//make sure it collided with a player or item

			Vector2 knockDir = (collisionParent.transform.position - transform.position).normalized;
			Attack basicProjectileImpact = new Attack (DAMAGE, knockDir, ATTACK_FORCE);


			collisionParent.attacked (basicProjectileImpact);
		}

		parentCode.projectile.collided = true;

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