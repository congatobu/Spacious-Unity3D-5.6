using UnityEngine;
using System.Collections;

public class CrystalRock : MonoBehaviour
{
	public GameObject aCrystal;
	public int crystalsNum = 6;
	private bool exploding;
	public GameObject myChar;
	public GameObject myParticles;


	public SpriteRenderer myRenderer;
	public Sprite spriteDestroied;

	//public GameObject border;

	private void Awake()
	{
		myRenderer = GetComponent<SpriteRenderer>();
	}

	private void GonnaExplode()
	{
		exploding = true;
		TouchingChar(false);
	}

	/*
	public void DELETE()
	{
		if (border == null)
		{
			foreach (Transform child in transform)
			{
				if (child.name == "Border")
				{
					border = child.gameObject;
				}
			}	
		}

		border.GetComponent<EnemyandCrystalCorrect>().DELETE();
	}
	*/

	public void Explode()
	{
		Debug.Log(("Explode with Bomb suckes"));
		//Debug.Log("XPLODE");
		MakeCrystals();
		myParticles.SendMessage("ExplodeParticle");

		transform.parent.SendMessage("ObjGone", gameObject.name);

		DestroyMe();
	}

	private void DestroyMe()
	{
		myRenderer.sprite = spriteDestroied;
		gameObject.layer = 0;
		Destroy(this);
	}

	private void OnTriggerEnter2D(Collider2D myCol)
	{
		if (myCol.gameObject.tag == "Player" && !exploding)
		{
			if (myChar == null)
				myChar = myCol.gameObject;

			SendMessage("TouchingChar", true);
		}
	}

	private void OnTriggerExit2D(Collider2D myCol)
	{
		if (myCol.gameObject.tag == "Player")
			SendMessage("TouchingChar", false);
	}

	private void TouchingChar(bool state)
	{
		if (myChar == null)
			return;

		if (state)
			myChar.SendMessage("Touching", "Rock");
		else
			myChar.SendMessage("Touching", "");
	}

	private void MakeCrystals()
	{
		//throw some crystals around
		var numCrystals = Random.Range(crystalsNum - 2, crystalsNum);

		//pra metade dos cristais que o char tem
		for (var i = 0; i < numCrystals; i++)
		{
			//instantiate it
			var dropCrystal = Instantiate(aCrystal, transform.position, Quaternion.identity);
			//random force
			float randX = Random.Range(-200, 200);
			float randY = Random.Range(100, 150);
			//add force
			dropCrystal.GetComponent<Rigidbody2D>().AddForce(new Vector2(randX, randY));
		}
	}
}