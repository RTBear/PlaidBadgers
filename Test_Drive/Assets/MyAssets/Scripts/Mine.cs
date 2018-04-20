using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour {
	GameObject explosion;
	float setupTime = .5f;
	float time = 0f;

	// Use this for initialization
	void Start () {
		transform.eulerAngles = new Vector3(0,0,0);
		explosion = Resources.Load ("Prefabs/Explosion 1") as GameObject;
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (0, 0, Time.deltaTime * 100);
		time += setupTime;
	}

	void OnTriggerEnter (Collider c) {
		if (time > setupTime) {
			Instantiate (explosion, transform.position, transform.rotation);
			Destroy (gameObject);

			var objectsScript = c.GetComponent<GameObjectScript>();
			print(objectsScript);
			if (objectsScript != null) {
				
				Vector2 knockDir = (c.transform.position - this.transform.position).normalized;
				Attack basicAttack = new Attack (10, knockDir, 10);

				objectsScript.attacked (basicAttack);
			}
		}
	}
}
