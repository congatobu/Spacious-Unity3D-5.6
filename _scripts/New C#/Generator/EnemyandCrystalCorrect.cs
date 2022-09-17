using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyandCrystalCorrect : MonoBehaviour
{
	private string type;
	public GameObject parent;
	public GameObject prefab;
	public GameObject body;

	private void Start()
	{
		type = gameObject.tag;
	}

	private void OnTriggerStay2D(Collider2D myCol)
	{
		if (myCol.gameObject.CompareTag("CrystalRock"))
		{
			if (myCol.gameObject != parent)
			{
				if (type == "CrystalRock_border")
				{
					Destroy(myCol.gameObject);
				}
			}
		}
		else if (myCol.gameObject.CompareTag("Enemy"))
		{
			if (myCol.gameObject != parent)
			{
				if (myCol.gameObject.name != "CheckChar")
				{
					if (myCol.gameObject != body)
					{
						if (type == "Enemy_border")
						{
							Debug.Log(myCol.gameObject.name);
							GameObject go = myCol.gameObject;
							Transform parentt = go.gameObject.transform.parent;
							Destroy(parentt.gameObject);
						}
					}
				}
			}
		}
	}
}
