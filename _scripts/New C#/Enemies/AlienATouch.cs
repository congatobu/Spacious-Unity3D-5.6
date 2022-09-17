using System.Collections;
using UnityEngine;

public class AlienATouch : MonoBehaviour
{
	private Animator bodyAnim;

	private bool touched;

	private void Awake()
	{
		bodyAnim = GetComponent<Animator>();
	}

	private void Update()
	{
	}

	private void OnTriggerEnter2D(Collider2D myCol)
	{
		if (touched)
			return;

		if (myCol.gameObject.tag == "Player")
		{
			SendMessageUpwards("CharTouched");
			touched = true;
			//yield return 0;
		}

		if (myCol.gameObject.tag == "Bomb")
		{
			touched = true;
			SendMessageUpwards("StopAlien");
			myCol.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
			myCol.gameObject.GetComponent<Renderer>().enabled = false;
			bodyAnim.SetTrigger("Eat");
		}
	}

	private void BackToNormal()
	{
		touched = false;
	}

	private IEnumerator Explode()
	{
		SendMessageUpwards("KillAlien");

		yield return new WaitForSeconds(0.4f);
		SendMessageUpwards("ExplodeParticle");
	}
}