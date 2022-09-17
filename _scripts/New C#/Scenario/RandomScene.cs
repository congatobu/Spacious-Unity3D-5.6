using UnityEngine;
using System.Collections;

public class RandomScene : MonoBehaviour
{
	public GameObject tilePrefab;
	public Sprite[] floorSprites;
	public int tileNumber = 5;

	void Start()
	{
		CreateFloor();
	}

	void Update()
	{

	}

	void CreateFloor()
	{
		for (int i = 0; i < tileNumber; i++)
		{
			GameObject newTile = Instantiate(tilePrefab);
			newTile.tag = "Floor";
			newTile.transform.localPosition = new Vector2(i, 0);
			newTile.transform.parent = gameObject.transform;
			SpriteRenderer sprRender = newTile.GetComponent<SpriteRenderer>();
			int spNumb = Random.Range(0, floorSprites.Length);
			sprRender.sprite = floorSprites[spNumb];
		}

		gameObject.AddComponent< BoxCollider2D > ();
		BoxCollider2D col = gameObject.GetComponent<BoxCollider2D>();
		col.size = new Vector2(tileNumber, 1);
		col.offset = new Vector2(gameObject.transform.position.x + tileNumber / 2, 0);
	}
}