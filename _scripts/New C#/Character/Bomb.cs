using System.Collections;
using UnityEngine;

public class Bomb : MonoBehaviour
{
	public Animator anim;
	public AudioClip bipSound;
	public LayerMask bombLayer;
	public bool exploded;
	public AudioClip explosionSound;
	public SpriteRenderer myRenderer;
	public AudioSource mySound;
	public ParticleSystem partic;
	public float startTime;
	public float waitTime = 1f;

	private void Awake()
	{
		anim = GetComponent<Animator>();
		mySound = GetComponent<AudioSource>();
		myRenderer = GetComponent<SpriteRenderer>();
		//partic.gameObject.GetComponent<Renderer>().sortingLayerID = 5;
		partic.Stop();
	}

	private void Start()
	{
		StartCoroutine("Init");
	}

	private void Update()
	{
		if (anim.speed < 2.0f)
			anim.speed = Time.time - startTime;
		else
		{
			if (!exploded)
				StartCoroutine("Explode");
		}
	}

	private IEnumerator Init()
	{
		/*
		if(bombCol != null)
		{
			objToExplode = bombCol.gameObject;
			objToExplode.SendMessage("GonnaExplode");
		}
		*/

		anim.speed = 0;

		mySound.clip = bipSound;

		startTime = Time.time;

		while (!exploded)
		{
			//make bip
			mySound.Play();

			yield return new WaitForSeconds(waitTime);
			waitTime -= 0.12f;
		}
	}

	private IEnumerator Explode()
	{
		exploded = true;
		partic.Play();

		var bombCol = Physics2D.OverlapCircle(transform.position, 0.5f, bombLayer);

		Debug.Log(bombCol);

		if (bombCol != null)
			bombCol.gameObject.SendMessage("Explode");

		myRenderer.enabled = false;

		mySound.clip = explosionSound;

		mySound.Play();

		yield return new WaitForSeconds(0.6f);

		Destroy(gameObject);
	}
}