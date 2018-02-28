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
			ApplyGravity(o);
        }
		//not garunteed to have items
		if (items != null) {
			foreach (GameObject o in items) {
				ApplyGravity (o);
			}
		}
    }

	void ApplyGravity(GameObject o)
	{
		//in range
		Vector3 dir = (transform.position - o.transform.position);
		if(dir.magnitude <= range)
		{
			if (o.GetComponent<Rigidbody> () != null) {
				o.GetComponent<Rigidbody> ().AddForce (
					dir.normalized * gravitationalPull);
			}
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
