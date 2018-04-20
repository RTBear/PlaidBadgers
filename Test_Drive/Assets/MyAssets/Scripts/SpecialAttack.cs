using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class SpecialAttack : MonoBehaviour {
	public abstract bool canSpecialAttack();

	public abstract void specialAttack();
}
