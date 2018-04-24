using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tether : MonoBehaviour {

	public GameObject parent;
	public GameObject collidedObject;
	public float m_speed = 10;
	float lifespan = 3;
	float counter = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(!collidedObject)
			transform.Translate (Vector3.up * m_speed * Time.deltaTime);
		counter += Time.deltaTime;
		if (counter >= lifespan && !collidedObject)
			Destroy (this.gameObject);
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.gameObject.tag != "Untagged") 
		{
			if (col.gameObject.tag == "Player")
				col.gameObject.GetComponent<Char_Code> ().isTethered = true;
			collidedObject = col.gameObject;
			parent.GetComponent<Crosshair> ().tetherCollisionLocation = this.transform.position;
			parent.GetComponent<Crosshair>().collidedObject = col.gameObject;
			Destroy (this.gameObject);
		}

	}
}
