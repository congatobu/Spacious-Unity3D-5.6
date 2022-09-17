using UnityEngine;
using System.Collections;

public class LightSpine : MonoBehaviour
{
	public Color[] lightColors;
	//private CircleCollider2D myColl;

	void Start()
	{
		//myColl = gameObject.GetComponent<CircleCollider2D>();
	}

	void Update()
	{

	}

	void OnTriggerStay2D(Collider2D spriteCol)
	{
		if (spriteCol.gameObject.tag == "Floor")
		{
			SpriteRenderer spColor = spriteCol.gameObject.GetComponent<SpriteRenderer>();
			float spDistance = Vector2.Distance(gameObject.transform.position, spriteCol.gameObject.transform.position);
			spColor.color = lightColors[(int)Mathf.Floor(spDistance)];
		}
	}

	void OnTriggerExit2D(Collider2D spriteCol)
	{
		if (spriteCol.gameObject.tag == "Floor")
		{
			SpriteRenderer spColor = spriteCol.gameObject.GetComponent<SpriteRenderer>();
			spColor.color = lightColors[lightColors.Length - 1];
		}
	}
}