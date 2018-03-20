using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetherController : GameObjectScript {

	public GameObject prefab;
	private float m_speed = 35;
	private Collider m_collider;
	private Rigidbody m_rb;

	private GameObject collisionParent;

	public bool tetherAttached = false;
	public bool tetherActive = false;
	public bool isFiring = false;


	// Use this for initialization
	void Start () {
		m_collider = GetComponent<Collider> ();
		m_rb = GetComponent<Rigidbody> ();
		m_rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
	}
	
	// Update is called once per frame
	void Update () {
		if (tetherAttached == false) {
			transform.Translate (Vector3.up * m_speed * Time.deltaTime);
		} 
	}

	void OnCollisionEnter(Collision col)
	{
		collisionParent = col.gameObject;
		Debug.Log(ReferenceEquals (collisionParent, col.gameObject));
		Debug.Log ("tether collision entered");
		Debug.Log (collisionParent);
		tetherAttached = true;
		transform.SetParent(collisionParent.transform);

		Rigidbody parent_rb = collisionParent.GetComponent<Rigidbody> ();
		//parent_rb.constraints = RigidbodyConstraints.FreezeAll;
		PlayerController parent_pc = collisionParent.GetComponent<PlayerController> ();
		parent_pc.tethered = true;
		//ignore further collisions
		//rb.detectCollisions = false;
		Destroy (m_rb);

	}

	public void resetTether(){
		Debug.Log ("destroy self call");
		//remove tether object from scene
		if (prefab) {
			Destroy (prefab);
		}

		Debug.Log (collisionParent);
		//Cleanup parent state
		if (collisionParent != null) {
			Debug.Log ("resetting parent");
			Rigidbody parent_rb = transform.GetComponentInParent<Rigidbody> ();
//			Rigidbody parent_rb = collisionParent.GetComponent<Rigidbody> ();
			//parent_rb.constraints = RigidbodyConstraints.FreezeAll;
//			PlayerController parent_pc = collisionParent.GetComponent<PlayerController> ();
			PlayerController parent_pc = transform.GetComponentInParent<PlayerController>();
			parent_pc.tethered = false;
			tetherAttached = false;
		}
	}
		
//	void OnCollisionExit(Collision col)
//	{
//		Debug.Log ("collision exited");
//		tetherAttached = false;
//		Rigidbody parent_rb = collisionParent.GetComponent<Rigidbody> ();
//		parent_rb.constraints = RigidbodyConstraints.None;
//	}
}
