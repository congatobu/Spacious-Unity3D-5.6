using UnityEngine;
using System.Collections;

public class Crystal : MonoBehaviour
{
	public bool picked = false;
	public BoxCollider2D myCol;
	public bool canCollect = true;

	void Awake()
	{
		myCol = GetComponent<BoxCollider2D>();
	}

	void Start()
	{
		transform.localScale = new Vector3(Random.Range(0.8f, 1.2f), Random.Range(0.7f, 1.2f), 1);
	}

	void OnCollisionEnter2D(Collision2D charCol)
	{
		if (charCol.gameObject.tag == "Player")
		{
			StartCoroutine("OnColEnter", charCol.gameObject);
		}
	}

	private IEnumerator OnColEnter(GameObject enterObject)
	{
		if (picked)
			yield break;

		CharCtrl myChar = enterObject.GetComponent<CharCtrl>();

		if (myChar.charDead)
			yield break;

		//send message to char
		myChar.PickedCrystal(canCollect);

		picked = true;

		//colliders to trigger
		myCol.isTrigger = true;

		//add force up
		GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 200));

		//wait
		yield return new WaitForSeconds(0.4f);

		//destroy
		Destroy(gameObject);
	}

	void Collectable(bool state)
	{
		canCollect = state;
	}
}