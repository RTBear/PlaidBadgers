using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack {

    float m_Damage;             // Damage percent that attack contains
    Vector2 m_launchDir;        // direction the atack the sends the victim
    float m_attackForce;        // force to be applied to victim if launched

    public Attack(float damage, Vector2 launchDirection, float attackForce)
    {
        m_Damage = damage;
        m_launchDir = launchDirection.normalized;
        m_attackForce = attackForce * 300;
    }

    public float getDamage()
    {
        return m_Damage;
    }

    public Vector2 getLaunchDirection()
    {
        return m_launchDir;
    }

    public float getAttackForce()
    {
        return m_attackForce;
    }
                                
}
