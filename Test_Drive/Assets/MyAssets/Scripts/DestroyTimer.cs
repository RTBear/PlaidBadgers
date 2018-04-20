using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTimer : MonoBehaviour{
	float deathTime;
	float currentTime = 0f;
	GameObject thingToDestroy;

	public DestroyTimer(float timeout, GameObject gameObject){
		deathTime = timeout;
		thingToDestroy = gameObject;
	}

	public void reset(){
		currentTime = 0;
	}
		

	public void run(){
		currentTime += Time.deltaTime;
		if (currentTime >= deathTime) {
			Destroy(thingToDestroy);
		}
	}
}