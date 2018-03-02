using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectScript : MonoBehaviour {
    public float health;
    public GameObject planet;
    public Rigidbody rb;

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    // Given an vector it will return the angle of that vector
    protected float getAngle(Vector2 vector)
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

    // Given a vector it will return the unit vector (exp. SQRT(X^2 + Y^2) == 1
    protected Vector2 getUnitVector(float x, float y)
    {
        float magnitude = Mathf.Sqrt(x * x + y * y);

        return new Vector2(x / magnitude, y / magnitude);
    }

    public void attacked(Attack attack)
    {
        health += attack.getDamage();

        float launchModifier = 1 + health/100.0f;

        Vector2 LaunchDir = attack.getLaunchDirection();
        float attackForce = attack.getAttackForce();

        // apply new velocity to char when hit
        Vector3 currentSpeed = rb.velocity;
        //rb.velocity = new Vector3(currentSpeed[0] + LaunchDir[0]*attackForce*launchModifier, currentSpeed[1] + LaunchDir[1]*attackForce*launchModifier, 0);
        rb.AddForce(LaunchDir[0] * attackForce * launchModifier, LaunchDir[1] * attackForce * launchModifier, 0);
    }
}