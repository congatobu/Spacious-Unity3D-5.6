using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
	public ObjectGenerator ObjectGenerator;
	public GameObject CrystalPref;
	public GameObject EnemyPref;

	public GameObject ReactorPref;
	//public GameObject ReactorGO;
	//private GameObject AliensGO;
	public GameObject AliensPref;
	//private GameObject CrystalGO;
	public GameObject CrystalParentPref;
	public GameObject RockPref;
	//private GameObject RockParentGO;
	public GameObject Rock2Pref;
	//private GameObject Rock2ParentGO;

	#region RocksSprites

	public Sprite[] Rocks;
	public Sprite[] Rocks2;

	#endregion

	private void Start()
	{
		if (ObjectGenerator == null)
			ObjectGenerator = Camera.main.GetComponent<ObjectGenerator>();

		Init(); // TO DO START GENERATE RANDOM PICKUP
	}

	private int Chanse(int min, int max)
	{
		return Random.Range(min, max);
	}

	private void Init()
	{
		if (Chanse(0, 3) == 0)
		{
			Debug.Log("test randomize1");
			if (Chanse(0, 3) == 0)
			{
				GameObject go = Instantiate(CrystalPref, transform.position, transform.rotation);
				go.transform.SetParent(ObjectGenerator.CrystalGO.transform);
			}
			else if (Chanse(0, 3) >= 1)
			{
				if (Chanse(0, 2) == 0)
				{
					GameObject go = Instantiate(RockPref, transform.position, transform.rotation);
					go.transform.SetParent(ObjectGenerator.RockParentGO.transform);
					go.transform.position = new Vector3(go.transform.position.x, go.transform.position.y - 0.03f, go.transform.position.z);
					go.GetComponent<SpriteRenderer>().sprite = Rocks[Random.Range(0, Rocks.Length)];
				}
				else
				{
					GameObject go =Instantiate(Rock2Pref, transform.position, transform.rotation);
					go.transform.SetParent(ObjectGenerator.Rock2ParentGO.transform);
					go.transform.position = new Vector3(go.transform.position.x, go.transform.position.y + 0.78f - 1.20f, 0f);
					go.GetComponent<SpriteRenderer>().sprite = Rocks2[Random.Range(0, Rocks2.Length)];
				}
			}
		}
		else if (Chanse(0, 3) == 2)
		{
			Debug.Log("test randomize2");
			if (ObjectGenerator.ReactorGO == null)
			{
				GameObject reactr = Instantiate(ReactorPref, transform.position, transform.rotation);
				ObjectGenerator.ReactorGO = reactr;
			}
			else
			{
				GameObject go = Instantiate(EnemyPref, transform.position, transform.rotation);
				go.transform.SetParent(ObjectGenerator.AliensGO.transform);
			}
		}
	}
}
