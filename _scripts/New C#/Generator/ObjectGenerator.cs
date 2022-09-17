using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class ObjectGenerator : MonoBehaviour
{
	#region RocksSprites

	public Sprite[] Rocks;
	public Sprite[] Rocks2;

	#endregion

	public GameObject CrystalPref;
	public GameObject Enemy;
	public GameObject ReactorPref;

	public GameObject ReactorGO;

	public Transform[] Layers = new Transform[4];
	public List<GameObject> CrystalRocks = new List<GameObject>();
	private GameObject[] _crs;

	public bool activeInit = false;

	public GameObject AliensGO;
	public GameObject AliensPref;
	public GameObject CrystalGO;
	public GameObject CrystalParentPref;
	public GameObject RockPref;
	public GameObject RockParentGO;
	public GameObject Rock2Pref;
	public GameObject Rock2ParentGO;

	public LayerMask SetLayer;

	private void Start()
	{
		ParentsPickups();
	}

	private void Update()
	{
		if (Layers[0] == null)
		{
			GameObject go = GameObject.Find("GENERATOR_GAME_MAP");
			Layers[0] = go.transform;
		}
		if (Layers[1] == null)
		{
			GameObject go = GameObject.Find("GENERATOR_GAME_MAP_1");
			Layers[1] = go.transform;
		}
		if (Layers[2] == null)
		{
			GameObject go = GameObject.Find("GENERATOR_GAME_MAP_2");
			Layers[2] = go.transform;
		}
		if (Layers[3] == null)
		{
			GameObject go = GameObject.Find("GENERATOR_GAME_MAP_3");
			Layers[3] = go.transform;
		}

		if (!activeInit)
		{
			if (Layers[0] != null && Layers[1] != null && Layers[2] != null && Layers[3] != null)
			{
				//Init();//OFF GENERATE THIS
			}
		}

		foreach (GameObject crystal in CrystalRocks)
		{
			if (crystal == null)
				CrystalRocks.Remove(crystal);
		}

		foreach (Transform layer in Layers)
		{
			layer.gameObject.layer = LayerMask.NameToLayer("Floor");
		}
	}

	private int Chanse(int min, int max)
	{
		return Random.Range(min, max);
	}

	private void Init()
	{
		activeInit = true;
		ReactorGO = null;

		AliensGO = Instantiate(AliensPref, transform.position, transform.rotation);
		CrystalGO = Instantiate(CrystalParentPref, transform.position, transform.rotation);
		RockParentGO = new GameObject("RocksParent");
		Rock2ParentGO = new GameObject("Rocks2Parent");

		foreach (Transform layer in Layers)
		{
			layer.gameObject.layer = LayerMask.NameToLayer("Floor");

			foreach (Transform child in layer)
			{
				if (Chanse(0, 3) == 0)
				{
					if (Chanse(0, 3) == 0)
					{
						Destroy(child.gameObject);
						CrystalRocks.Remove(child.gameObject);
					}
					else if (Chanse(0, 3) >= 1)
					{
						if (Chanse(0, 2) == 0)
						{
							GameObject go = Instantiate(RockPref, child.position, child.rotation);
							go.transform.SetParent(RockParentGO.transform);
							go.transform.position = new Vector3(go.transform.position.x, go.transform.position.y - 0.03f, go.transform.position.z);
							go.GetComponent<SpriteRenderer>().sprite = Rocks[Random.Range(0, Rocks.Length)];
							Destroy(child.gameObject);
						}
						else
						{
							GameObject go = Instantiate(Rock2Pref, child.position, child.rotation);
							go.transform.SetParent(Rock2ParentGO.transform);
							go.transform.position = new Vector3(go.transform.position.x, go.transform.position.y + 0.78f - 1.20f, go.transform.position.z);
							go.GetComponent<SpriteRenderer>().sprite = Rocks2[Random.Range(0, Rocks2.Length)];
							Destroy(child.gameObject);
						}
					}
				}
				else if (Chanse(0, 3) == 2)
				{
					if (layer.gameObject.name == "GENERATOR_GAME_MAP" || layer.gameObject.name == "GENERATOR_GAME_MAP_1")
					{
						if (ReactorGO == null)
						{
							ReactorGO = Instantiate(ReactorPref, child.position, child.rotation);
							Destroy(child.gameObject);
							//ReactorCorrect(); // call to correct position
						}
						else
						{
							if (Chanse(0, 7) < 1)
							{
								Destroy(child.gameObject);
								CrystalRocks.Remove(child.gameObject);
							}
							else if (Chanse(0, 7) >= 1)
							{
								GameObject go = Instantiate(Enemy, child.position, child.rotation);
								go.transform.SetParent(AliensGO.transform);
								Destroy(child.gameObject);
							}
						}
					}
					else
					{
						if (Chanse(0, 7) < 1)
						{
							Destroy(child.gameObject);
							CrystalRocks.Remove(child.gameObject);
						}
						else if (Chanse(0, 7) >= 1)
						{
							GameObject go = Instantiate(Enemy, child.position, child.rotation);
							go.transform.SetParent(AliensGO.transform);
							Destroy(child.gameObject);
						}
					}
				}
			}
		}
		
		CrystalCorrect();
	}
	
	private void CrystalCorrect()
	{
		_crs = GameObject.FindGameObjectsWithTag("CrystalRock");

		for (int i = 0; i < _crs.Length - 1; i++)
		{
			if (_crs[i] != null)
			{
				CrystalRocks.Add(_crs[i].gameObject);
			}
		}

		/*
		for (int i = 0; i < CrystalRocks.Count; i++)
		{
			float distance = Vector3.Distance(CrystalRocks[i].transform.position, CrystalRocks[i + 1].transform.position);

			if (distance <= 5)
			{
				Destroy(CrystalRocks[i]);
				CrystalRocks.Remove(CrystalRocks[i]);
				Debug.Log("crystal test " + CrystalRocks[i]);
			}
		}

		for (int i = CrystalRocks.Count; i > 0; i--)
		{
			float distance = Vector3.Distance(CrystalRocks[i].transform.position, CrystalRocks[i + 1].transform.position);

			if (distance <= 5)
			{
				Destroy(CrystalRocks[i]);
				CrystalRocks.Remove(CrystalRocks[i]);
				Debug.Log("crystal test " + CrystalRocks[i]);
			}
		}*/
	}

	private void ParentsPickups()
	{
		AliensGO = Instantiate(AliensPref, transform.position, transform.rotation);
		CrystalGO = Instantiate(CrystalParentPref, transform.position, transform.rotation);
		RockParentGO = new GameObject("RocksParent");
		Rock2ParentGO = new GameObject("Rocks2Parent");
	}
}
