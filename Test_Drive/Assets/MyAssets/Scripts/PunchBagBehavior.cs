using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchBagBehavior : MonoBehaviour {

    private float m_dmgMultiplier = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void addDamage(float damage)
    {
        m_dmgMultiplier += damage;
    }

    public float getDamage()
    {
        return m_dmgMultiplier;
    }
}
