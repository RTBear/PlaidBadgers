using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : GameObjectScript {
	Rigidbody rb;

	bool inPlanetGravity;
	private Vector2 relativePos;

	public int jmpForce;
	public float jumpVel = 25.0f;
	bool canAirJump = true;
	bool onGround = false;

	public float runForce = 30f;
	public float maxRunSpeed;

	public AudioClip whack;

	// Use this for initialization
	void Start () {
		rb = GetComponentInParent<Rigidbody>();
		relativePos = new Vector3 (0,0,0);
	}

	//Methods for outside access
	//These affect player movement
	public void setUprightAngle(Vector2 pos){
		inPlanetGravity = true;
		float angleChar = getAngle (pos);
		transform.eulerAngles = new Vector3 (0, 0, angleChar);
		relativePos = pos;
	}

	public void notOnPlanet(){
		inPlanetGravity = false;
	}

	void OnCollisionEnter(Collision collider)
	{
		onGround = true;
	}

	void OnCollisionExit(Collision collider)
	{
		onGround = false;
		canAirJump = true;
	}

	//can make this more complex, set methods for is paralized, ect
	public bool canMove(){
		return inPlanetGravity;
	}
		
	public void Move(Vector2 directionRun)
	{
		// This is how our charactor will move with analog sticks
		float angleChar = getAngle(relativePos);
		float angleRunDir = getAngle(directionRun);

		//check if they desired location is the same as current location
		if ((int)angleChar + 8 >= (int)angleRunDir && (int)angleRunDir >= (int)angleChar - 8)
		{
			if (onGround)
				rb.velocity = new Vector2(0, 0);
		}
		else // Run to given location
		{
			float moveMod = 1f;
			if (!onGround)
				moveMod = 0.5f;

			float angleDiff = angleRunDir - angleChar;
			if (angleDiff < 0)
				angleDiff += 360;
			if (angleDiff < 180 && angleDiff > 0)
				rb.AddRelativeForce(Vector3.left * runForce*moveMod);
			else
				rb.AddRelativeForce(Vector3.right * runForce*moveMod);
		}
		SetMaxRunSpeed ();
	}

	// This controls the run speed. This will also play havock on 
	void SetMaxRunSpeed()
	{
		if(rb.velocity.magnitude > maxRunSpeed)
		{
			rb.velocity =  rb.velocity.normalized * maxRunSpeed;
		}
	}

	public bool canJump(){
		return inPlanetGravity && (onGround || canAirJump);
	}
		
	public void Jump()
	{
		rb.AddForce (relativePos* jmpForce);
		onGround = false;
		// Air jump logic
		if (canAirJump)
			canAirJump = false;
	}

	public Collider GetAttackCollider(Collider collider){
		Collider[] cols = Physics.OverlapBox(collider.bounds.center, collider.bounds.extents, collider.transform.rotation, LayerMask.GetMask("HitBox"));
		if (cols.Length <= 0) {
			Debug.LogWarning ("No colliders. Hi mom!");
			return null;
		}
		//not sure I fixed this right
		foreach (Collider c in cols) {
			if (c.transform.parent == c) {
				print ("I hit myself");
				return null;
			}
			else {
				return c;
			}
		}
		return null;
	}

	public void LaunchAttack(Collider c)
	{
		var objectsScript = c.GetComponent<GameObjectScript> ();
		if (objectsScript != null) {
			Vector2 knockDir = (c.transform.position - this.transform.position).normalized;
			Attack basicAttack = new Attack(10, knockDir, 10);
			objectsScript.attacked(basicAttack);
		} else {
			c.GetComponent<Rigidbody> ()
				.AddForce ((this.transform.position - c.transform.position).normalized * -1000);
		}
	}
}
