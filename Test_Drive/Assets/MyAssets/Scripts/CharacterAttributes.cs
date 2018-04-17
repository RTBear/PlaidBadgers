using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttributes{
	public enum CharacterType
	{
		CUBE,
		SPHERE,
		ROBOT,
		PILL}
	;

	public GameObject m_Prefab;
	public Material m_Material = Resources.Load("Materials/BaseMaterial") as Material;
	public CharacterType m_CharacterType;

	public void SetPrefab ()
	{
		switch (m_CharacterType) {
			case CharacterType.CUBE:
				m_Prefab = Resources.Load ("Characters/Cube") as GameObject;
				break;
			case CharacterType.ROBOT:
				m_Prefab = Resources.Load ("Characters/Robot") as GameObject;
				break;
			case CharacterType.PILL:
				m_Prefab = Resources.Load ("Characters/Pill") as GameObject;
				break;
			case CharacterType.SPHERE:
				m_Prefab = Resources.Load ("Characters/Sphere") as GameObject;
				break;
		}
	}

}
