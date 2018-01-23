using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Char_Code : MonoBehaviour {

    public float jumpVel = 2.0f;
    public Vector3 jump;
    public Rigidbody rb;
    public GameObject planet;
    private bool onGround = false;
    public float maxRunSpeed;
    public bool airJump = true;



    // Use this for initialization
    void Start () {
     
        rb = GetComponent<Rigidbody>();

    }
	
	// Update is called once per frame
	void Update () {

        SetMaxRunSpeed();
   
        transform.eulerAngles = Move_sideways();

        Jump();

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
        if (onGround)
        {
            rb.AddRelativeForce(Vector3.right * 6 * Input.GetAxis("Horizontal"));
        }
        else
        {
            rb.AddRelativeForce(Vector3.right * 3 * Input.GetAxis("Horizontal"));
           // Vector3 relative_position = rb.transform.position - planet.transform.position;
        }

        if (onGround && Input.GetAxis("Horizontal") == 0)
        {
            GetComponent<Rigidbody>().velocity *= 0;
        }

        float neededAngle;
       
        Vector3 relative_position = rb.transform.position - planet.transform.position;
       
            if (relative_position[0] == 0)
        {
            if (relative_position[1] > 0)
            {
                neededAngle = 0;
            }
            else
            {
                neededAngle = 180;
            }
        }
            else if (relative_position[0] > 0)       //check if char is to right of planet
        {
            if (relative_position[1] > 0) //   Quadrant I
            {
                neededAngle = 270 + (Mathf.Atan(relative_position[1] / relative_position[0]) * 180)/Mathf.PI;
            }
            else   // Quadrant II
            {
                neededAngle = (Mathf.Atan(relative_position[1] / relative_position[0]) * 180)/ Mathf.PI - 90;
            }
   
        }
        else
        {
            if(relative_position[1] > 0) //   Quadrant IIV
            {
                neededAngle = 90 + (Mathf.Atan(relative_position[1] / relative_position[0]) * 180)/ Mathf.PI;
            }
            else  // Quadrand III
            {
                neededAngle = 90 + (Mathf.Atan(relative_position[1] / relative_position[0]) * 180) / Mathf.PI;
            }
        }
        return new Vector3(0, 0, neededAngle);
    }

    void OnCollisionEnter(Collision collider)
    {
        onGround = true;
        //airJump = true;    // moving to when jumped
        print("Enter called.");
    }

   
    void OnCollisionExit(Collision collider)
    {
        onGround = false;
        airJump = true;
    }
    

    void Jump()
    {
       // jump from ground logic
        if ((onGround | airJump) && Input.GetKeyDown(KeyCode.UpArrow))
        {
            //determine where on planet the Char is
            Vector3 relative_position = rb.transform.position - planet.transform.position;
           float magnitude = Mathf.Sqrt(relative_position[0] * relative_position[0] + relative_position[1] * relative_position[1]);
            // jumpDirection is the direction normal to the surface
            Vector3 jumpDirection = new Vector3((relative_position[0] / magnitude)*jumpVel, (relative_position[1] / magnitude)*jumpVel, 0);
            print("Jump Direction: " + jumpDirection);

            // Get current speed of Char
            Vector3 currentSpeed = GetComponent<Rigidbody>().velocity;
            print("Current Speed: " + currentSpeed);

            //if moving we need to combine the current momentum into the jump
            rb.velocity = new Vector3(currentSpeed[0] + jumpDirection[0], currentSpeed[1] + jumpDirection[1], 0);
            print("Jump Velocity: " + rb.velocity);

                onGround = false;

            // Air jump logic
            if (airJump)
                airJump = false;
        }
    }


}
