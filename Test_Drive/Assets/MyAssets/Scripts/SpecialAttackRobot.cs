using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttackRobot : SpecialAttack{

	public GameObject Mine = Resources.Load("Prefabs/Mine") as GameObject;

	public override bool canSpecialAttack(){
		return true;
	}

	public override void specialAttack(){
		Instantiate (Mine, transform.position, transform.rotation);
	}
}
