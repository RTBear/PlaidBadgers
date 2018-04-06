using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphericalGravity : MonoBehaviour {

    public List<GameObject> players;
	public List<GameObject> items;

    public float gravitationalPull;
	public float range;

    void FixedUpdate()
    {
		//apply spherical gravity to selected objects (set the objects in editor)
        foreach (GameObject o in players)
        {
			ApplyGravity (o);
			//Let the character know it's standing
			Vector2 relativePosition = (o.transform.position - transform.position).normalized;
			o.GetComponent<PlayerController> ().setUprightAngle (relativePosition);
        }
		//not garunteed to have items
		foreach (GameObject o in items) {
			if (inRange (o)) {
				ApplyGravity (o);
			}
		}
    }

	public bool inRange(GameObject o){
		if (o) 
		{
			Vector3 dir = (transform.position - o.transform.position);
			return dir.magnitude <= range;
		}
		return false;
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
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
