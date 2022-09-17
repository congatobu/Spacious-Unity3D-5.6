using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
	public BoxCollider2D myCol;
	public GameObject mySprite;
	public AudioSource mySound;

	void Awake()
	{
		myCol = mySprite.GetComponent<BoxCollider2D>();
		mySprite.GetComponent< Animation > ()["spawner"].wrapMode = WrapMode.Loop;
		mySound = GetComponent<AudioSource>();
		myCol.isTrigger = true;
	}
	/*
	public void Spawning()
	{
		StartCoroutine("IESpawning");
	}

	public void Collect()
	{
		StartCoroutine("IECollect");
	}
	*/
	private IEnumerator Spawning()
	{
		mySound.Play();
		myCol.isTrigger = false;

		yield return new WaitForSeconds(8f);

		myCol.isTrigger = true;
	}

	private IEnumerator Collect()
	{
		yield return new WaitForSeconds(0.5f);

		Destroy(gameObject);
	}
}