using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : GameObjectScript {
	Rigidbody rb;
	bool inPlanetGravity;
	private Vector2 relativePos;

	public TetherEmitterController tetherEmitter;
	public ProjectileEmitterController projectileEmitter;

	int jmpForce = 2000;
	bool canAirJump = true;
	bool onGround = false;
	public bool facingClockwise = true;

	float runForce = 30f;
	float maxRunSpeed = 100;
	float sprintForce = 1f;

	public AudioClip whack;

	private DeathTimer dt;
	private float deathTime = 2f;

	// Use this for initialization
	void Awake () {
		rb = GetComponentInParent<Rigidbody>();
		relativePos = new Vector3 (0,0,0);
		if(tetherEmitter)
			tetherEmitter.transform.position = transform.GetComponent<Renderer> ().bounds.center;
		if(projectileEmitter)
			projectileEmitter.transform.position = transform.GetComponent<Renderer> ().bounds.center;
		dt = gameObject.AddComponent<DeathTimer>() as DeathTimer;
		dt.Init(deathTime, gameObject);
	}

	//Methods for outside access
	//These affect player movement
	public void setUprightAngle(Vector2 pos){
		if (!inPlanetGravity) {
			inPlanetGravity = true;
			dt.ResetAndOff();
		}

		float angleChar = getAngle (pos);

        if (facingClockwise)
            transform.eulerAngles = new Vector3(0, 0, angleChar);
        else
        {
            // MUST BE IN THIS ORDER TO WORK PROPERLY!!!
            transform.eulerAngles = new Vector3(0, 0, angleChar);   // 1. apply global rotation
            transform.Rotate(0, 180, 0, Space.Self);                // 2. apply local rotation
        }
        relativePos = pos;
    }

	

	public void notOnPlanet(){
		if (inPlanetGravity) {
			inPlanetGravity = false;
			dt.On ();
		}
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
		if (tetherEmitter.tether) {//if the tether is no longer a child of the player, that means it is attached to something else.
			if (inPlanetGravity && !tethered && !tetherEmitter.tether.tetherActive) {
				return true;
			}
		}
		return false;//if this point is reached, the player should be paralized
	}

	public void Aim(Vector2 directionAim){
		float angleCrosshair = getAngle (directionAim);
		tetherEmitter.transform.eulerAngles = new Vector3(0, 0, angleCrosshair);
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
            {
                facingClockwise = false;
                //rb.AddRelativeForce(Vector3.left * runForce * moveMod);
            }
            else
            {
                facingClockwise = true;
                //rb.AddRelativeForce(Vector3.right * runForce * moveMod);
            }
            rb.AddRelativeForce(Vector3.right * runForce * moveMod * sprintForce);
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
		return canMove() && (onGround || canAirJump);
	}
		
	public void Jump()
	{
		rb.AddForce (relativePos* jmpForce);
		onGround = false;
		// Air jump logic
		if (canAirJump)
			canAirJump = false;
	}

	//would be nice to seperate so we can split it apart for
	//sound effects, ect,
	//but idk how it works well enough yet
	/*public Collider GetAttackCollider(Collider collider){
		Collider[] cols = Physics.OverlapBox(collider.bounds.center, collider.bounds.extents, collider.transform.rotation, LayerMask.GetMask("HitBox"));
		if (cols.Length <= 0) {
			Debug.LogWarning ("No colliders. Hi mom!");
			return null;
		}
		//not sure I fixed this right
		foreach (Collider c in cols) {
			if (c.transform.parent == c) {
				print ("I hit myself");
			}
			else {
				return c;
			}
		}
		return null;
	}*/
	
	public void LaunchAttack(GameObject target, float attackMultiplier)
	{
		
		var objectsScript = target.GetComponent<GameObjectScript>();
		print(objectsScript);
		if (objectsScript != null) {
			//audio.Play();

			Vector2 knockDir = (target.transform.position - this.transform.position).normalized;
			Attack basicAttack = new Attack (10 * attackMultiplier, knockDir, 10);

			objectsScript.attacked (basicAttack);
		}
		else {
			target.GetComponent<Rigidbody> ().
			AddForce ((this.transform.position - target.transform.position).normalized * -1000);
		}
	}

	public void AddSprint(){
		sprintForce = 10f;
		rb.AddRelativeForce (Vector3.down*250);
	}

	public void RemoveSprint(){
		sprintForce = 1f;
	}
}
