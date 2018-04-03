using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class SpawnTimer : Timer {
//}

public class DeathTimer : Timer {
	protected GameObject target;

	public void Init(float deathTime, GameObject targetObject){
		targetTime = deathTime;
		target = targetObject;
	}

	protected override void timerFinished ()
	{
		Destroy (target);
		Destroy (this);
	}
}

public abstract class Timer : MonoBehaviour {

	protected float currentTime = 0;
	protected float targetTime;
	protected bool running = false;

	public void On(){
		running = true;
	}

	public void ResetAndOff(){
		currentTime = 0;
		running = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (running) {
			currentTime += Time.deltaTime;
			if (currentTime >= targetTime) {
				timerFinished();
			}
		}
	}

	protected abstract void timerFinished();
}