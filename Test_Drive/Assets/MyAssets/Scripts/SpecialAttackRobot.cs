using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAttackRobot : SpecialAttack{

	public GameObject Mine;
	GameObject[] AllMines = {null, null, null};
	int[] WhenMinePlaced = {-1, -1, -1};
	int currentMine = 0;

	void Awake(){
		Mine = Resources.Load("Prefabs/Mine") as GameObject;
	}

	public override bool canSpecialAttack(){
		
		return true;
	}

	public override void specialAttack(){
		int smallestMine = currentMine;
		int mineIndex = -1;
		bool foundNull = false;
		for (int i = 0; i < 3; i++) {
			if (AllMines [i] != null) {
				if (WhenMinePlaced [i] < smallestMine) {
					mineIndex = i;
					smallestMine = WhenMinePlaced [i];
				}
			} else {
				mineIndex = i;
				break;
			}
		}
		if (!foundNull) {
			Destroy (AllMines[mineIndex]);
		}
		GameObject nextMine = Instantiate (Mine, transform.position, transform.rotation);
		AllMines [mineIndex] = nextMine;
		WhenMinePlaced [mineIndex] = currentMine;
		currentMine++;
	}


	void OnDestroy(){
		for (int i = 0; i < 3; i++) {
			Destroy (AllMines[i]);
		}
	}
}
