using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Char_Code : MonoBehaviour {

    public float jumpVel = 2.0f;
    public Vector3 jump;
    public Rigidbody rb;
    public float runForce = 30f;
    public GameObject planet;
    private bool onGround = false;
    public float maxRunSpeed;
    public bool airJump = true;
    public Collider[] attack_HitBoxes;
    private bool attackCalled = false;
    public AudioClip whack;
    AudioSource audio;




    // Use this for initialization
    void Start () {
     
        rb = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();

    }
	
	// Update is called once per frame
	void Update () {

   

        SetMaxRunSpeed();
   
        transform.eulerAngles = Move_sideways();

        Jump();

        

        if (Input.GetKeyDown(KeyCode.Z))
        {
            LaunchAttack(attack_HitBoxes[0]);
        }
        if (Input.GetKeyUp(KeyCode.Z))
        {
            if (attackCalled)
                attackCalled = false;
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

    Vector3 Move_sideways()
    {
        Vector2 relative_position = rb.transform.position - planet.transform.position;
 
        


        // This is the generic way of moving side to side
        /**
            if (onGround)
            {

                rb.AddRelativeForce(Vector3.right * runForce * Input.GetAxis("Horizontal"));
            }
            else
            {
                rb.AddRelativeForce(Vector3.right * (runForce / 2) * Input.GetAxis("Horizontal"));
                // Vector3 relative_position = rb.transform.position - planet.transform.position;
            }

            if (onGround && Input.GetAxis("Horizontal") == 0)
            {
                GetComponent<Rigidbody>().velocity *= 0;
            }
    **/

 // This is how our charactor will move with analog sticks
        float angleChar = getAngle(relative_position);

        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            Vector2 directionRun = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            float angleRunDir = getAngle(directionRun);
       
            //check if they desired location is the same as current location
            if ((int)angleChar + 8 >= (int)angleRunDir && (int)angleRunDir >= (int)angleChar - 8)
            {
                //Jump();
                if (onGround)
                    rb.velocity = new Vector2(0, 0);
            }
            else // Run to given location
            {
                float moveMod = 1f;
                if (!onGround)
                    moveMod = 0.5f;


                if (angleChar>angleRunDir)
                {
                    if (angleChar - angleRunDir < 180)
                        rb.AddRelativeForce(Vector3.right * runForce*moveMod);
                    else
                        rb.AddRelativeForce(Vector3.left * runForce*moveMod);
                }
                else
                {
                    if(angleRunDir - angleChar < 180)
                        rb.AddRelativeForce(Vector3.left * runForce*moveMod);
                    else
                        rb.AddRelativeForce(Vector3.right * runForce*moveMod);

                }

            }
        }
        


                
            
        return new Vector3(0, 0, angleChar);
    }

    float getAngle(Vector2 vector)
    {
        float angle;

        if (vector[0] == 0)
        {
            if (vector[1] > 0)
            {
                angle = 0;
            }
            else
            {
                angle = 180;
            }
        }
        else if (vector[0] > 0)       //check if char is to right of planet
        {
            if (vector[1] > 0) //   Quadrant I
            {
                angle = 270 + (Mathf.Atan(vector[1] / vector[0]) * 180) / Mathf.PI;
            }
            else   // Quadrant II
            {
                angle = 270 + (Mathf.Atan(vector[1] / vector[0]) * 180) / Mathf.PI;
            }

        }
        else
        {
            if (vector[1] > 0) //   Quadrant IIV
            {
                angle = 90 + (Mathf.Atan(vector[1] / vector[0]) * 180) / Mathf.PI;
            }
            else  // Quadrand III
            {
                angle = 90 + (Mathf.Atan(vector[1] / vector[0]) * 180) / Mathf.PI;
            }
        }
        return angle;
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

    //This is to convert and return unit vectors
    Vector2 getUnitVector(float x, float y)
    {
        float magnitude = Mathf.Sqrt(x * x + y * y);

        return new Vector2(x / magnitude, y / magnitude);
    }

    void Jump()
    {
       // jump from ground logic
        if ((onGround | airJump) && (Input.GetKeyDown(KeyCode.UpArrow) | Input.GetKeyDown("joystick button 0")))
        if (onGround | airJump)
        {
            //determine where on planet the Char is
            Vector3 relative_position = rb.transform.position - planet.transform.position;
          //DELETE AFTER USE// float magnitude = Mathf.Sqrt(relative_position[0] * relative_position[0] + relative_position[1] * relative_position[1]);
            Vector2 jumpUnitVector = getUnitVector(relative_position[0], relative_position[1]);
            // jumpDirection is the direction normal to the surface
            Vector3 jumpDirection = new Vector3(jumpUnitVector[0]*jumpVel, jumpUnitVector[1]*jumpVel, 0);
  

            // Get current speed of Char
            Vector3 currentSpeed = GetComponent<Rigidbody>().velocity;


            //if moving we need to combine the current momentum into the jump
            rb.velocity = new Vector3(currentSpeed[0] + jumpDirection[0], currentSpeed[1] + jumpDirection[1], 0);


                onGround = false;

            // Air jump logic
            if (airJump)
                airJump = false;
        }
    }

    private void LaunchAttack(Collider collider)
    {
        if (attackCalled == false)
        {
            attackCalled = true;
            Collider[] cols = Physics.OverlapBox(collider.bounds.center, collider.bounds.extents, collider.transform.rotation, LayerMask.GetMask("HitBox"));
            foreach (Collider c in cols)
            {

                if (c.transform.parent == c)
                    print("I hit myself");
                else
                {
                    print("You hit " + c.name);
                    //if(c.gameObject == punchBag)
                    c.GetComponent<Rigidbody>().AddForce((this.transform.position - c.transform.position).normalized *-1000);
                    audio.Play();
                
                }
                    
            }
        }
    }


}
