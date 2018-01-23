using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShericalGravity : MonoBehaviour {

    public List<GameObject> objects;
    public GameObject planet;
    public bool singlePlanet;

    public float gravitationalPull;

    void FixedUpdate()
    {
        if (!(singlePlanet)) {
        //apply spherical gravity to selected objects (set the objects in editor)
        foreach (GameObject o in objects)
        {
            if (o.GetComponent<Rigidbody>() != null)
            {
                o.GetComponent<Rigidbody>().AddForce((planet.transform.position - o.transform.position).normalized * gravitationalPull);
            }
        }
    }
    else
    {
        
        //or apply gravity to all game objects with rigidbody
        foreach (GameObject o in UnityEngine.Object.FindObjectsOfType<GameObject>())
        {
            
            if (o.GetComponent<Rigidbody>() != null && o != planet)
            {
                o.GetComponent<Rigidbody>().AddForce((planet.transform.position - o.transform.position).normalized * gravitationalPull);
            }
        }
    
    }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
