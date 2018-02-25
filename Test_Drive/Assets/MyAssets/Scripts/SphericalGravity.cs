using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphericalGravity : MonoBehaviour {

    GameObject[] players;
	GameObject[] items;

    public float gravitationalPull;
	public float range;

    void FixedUpdate()
    {
		//apply spherical gravity to selected objects (set the objects in editor)
        foreach (GameObject o in players)
        {
			if (inRange (o)) {
				ApplyGravity (o);
				//Let the character know it's standing
				Vector2 relativePosition = (o.transform.position - transform.position).normalized;
				o.GetComponent<PlayerController> ().setUprightAngle (relativePosition);
			}
        }
		//not garunteed to have items
		if (items != null) {
			foreach (GameObject o in items) {
				if (inRange (o)) {
					ApplyGravity (o);
				}
			}
		}
    }

	bool inRange(GameObject o){
		Vector3 dir = (transform.position - o.transform.position);
		return dir.magnitude <= range;
	}

	void ApplyGravity(GameObject o)
	{
		if (o.GetComponent<Rigidbody> () != null) {
			Vector3 dir = (transform.position - o.transform.position);
			o.GetComponent<Rigidbody>().AddForce(dir.normalized * gravitationalPull);
		}
	}

    // Use this for initialization
    void Start () {
		players = GameObject.FindGameObjectsWithTag("Player");
		//Uncomment this next line when we have items
		items = GameObject.FindGameObjectsWithTag("Item");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
