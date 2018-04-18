using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetherController : MonoBehaviour {

	public GameObject prefab;
	private float m_speed = 50;
	private Rigidbody m_rb;

	private GameObject collisionParent;
	public Transform originalParentTransform;
	public GameObjectScript originalParentCharCode;

	public bool tetherAttached = false;
	public bool tetherActive = false;
	public bool isFiring = false;

	// Use this for initialization
	void Start () {
		m_rb = GetComponent<Rigidbody> ();
		m_rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
	}
	
	// Update is called once per frame
	void Update () {

		if (tetherAttached == false) {
			transform.Translate (Vector3.up * m_speed * Time.deltaTime);
		}
	}

	void OnCollisionEnter(Collision col){
		collisionParent = col.gameObject;

		transform.SetParent(collisionParent.transform);//attach tether to tetheree 
		tetherAttached = true;

		if (collisionParent.CompareTag ("Planet")) {
			originalParentCharCode.GetComponent<Char_Code>().tetherCollisionLocation = transform.position;
		}
			
		//add player and tetheree to tetheringPlayers dictionary so they can pull
		GameManager.instance.tetheringPlayers.Add(originalParentCharCode,collisionParent);

//		Rigidbody parent_rb = collisionParent.GetComponent<Rigidbody> (); 
		//parent_rb.constraints = RigidbodyConstraints.FreezeAll; //freeze input for tetheree

		originalParentCharCode.rb.constraints = RigidbodyConstraints.FreezeAll; //freeze input for player who fired tether

		Destroy (m_rb); 
	}

	public void resetTether(){
		//Debug.Log ("destroy self call");
		//remove tether object from scene
		if (GetComponent<TetherController>().prefab) {
		//	Debug.Log ("destroy prefab");
			Destroy (prefab);
		}

//		Debug.Log (collisionParent);

		//Cleanup parent state
		if (collisionParent != null) {
		//	Debug.Log ("resetting parent");
//			tetherAttached = false;
		}
		tetherAttached = false;
	}
		
}
