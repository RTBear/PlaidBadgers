using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Char_Code : GameObjectScript {

    public float jumpVel = 25.0f;
    public Vector3 jump;
    public float runForce = 30f;
    protected bool onGround = false;
    public float maxRunSpeed;
    public bool airJump = true;
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
  
		if (Input.GetKeyUp ("joystick button 2")) {
			Debug.LogWarning ("Pressed attack");
			LaunchAttack (attack_HitBoxes [0]);
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
        airJump = true;
    }

    private void LaunchAttack(Collider collider)
    {
        Collider[] cols = Physics.OverlapBox(collider.bounds.center, collider.bounds.extents, collider.transform.rotation, LayerMask.GetMask("HitBox"));
		if(cols.Length <= 0)
			Debug.LogWarning("No colliders. Hi mom!");
		foreach (Collider c in cols)
		{
			if (c.transform.parent == c)
				print("I hit myself");
			else
			{
				
                var objectsScript = c.GetComponent<GameObjectScript>();
                print(objectsScript);
                if (objectsScript != null)
                {
                    audio.Play();

                    Vector2 knockDir = (c.transform.position - this.transform.position).normalized;
                    Attack basicAttack = new Attack(10, knockDir, 10);

                    objectsScript.attacked(basicAttack);

                }       
                else
				    c.GetComponent<Rigidbody>().AddForce((this.transform.position - c.transform.position).normalized * -1000);
				

			}
		}
    }


}
