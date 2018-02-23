using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Char_Code : MonoBehaviour {

    public float jumpVel = 2.0f;
    public Vector3 jump;
    public Rigidbody rb;
    public float runForce = 30f;
    public GameObject planet;
    public bool onGround = false;
    public float maxRunSpeed;
    public bool canAirJump = false;
    public Collider[] attack_HitBoxes;
    //private bool attackCalled = false;
    public AudioClip whack;
    AudioSource audio;
	public int playerNumber;
	PlayerController pc;

    // Use this for initialization
    void Start () {
     
        rb = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
		pc = GetComponent<PlayerController> ();
		pc.SetController (playerNumber);
    }
	
	// Update is called once per frame
	void Update () {
		
        SetMaxRunSpeed();
  
        if (Input.GetKeyUp("joystick button 2"))
        {
			Debug.LogWarning("Pressed attack");
			LaunchAttack(attack_HitBoxes[0]);
		}
	}

    // This controls the run speed. This will also play havock on 
    void SetMaxRunSpeed()
    {
        if(GetComponent<Rigidbody>().velocity.magnitude > maxRunSpeed)
        {
            GetComponent<Rigidbody>().velocity =  GetComponent<Rigidbody>().velocity.normalized * maxRunSpeed;
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

    private void LaunchAttack(Collider collider)
    {
        Collider[] cols = Physics.OverlapBox(collider.bounds.center, collider.bounds.extents, collider.transform.rotation, LayerMask.GetMask("HitBox"));
		if(cols.Length <= 0)
			Debug.LogWarning("No colliders");
		foreach (Collider c in cols)
		{
			if (c.transform.parent == c)
				print("I hit myself");
			else
			{
				print("You hit " + c.name);
				//if(c.gameObject == punchBag)
				c.GetComponent<Rigidbody>().AddForce((this.transform.position - c.transform.position).normalized * -1000);
				audio.Play();

			}
		}
    }


}
