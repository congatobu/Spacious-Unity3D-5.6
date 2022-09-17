using UnityEngine;

public class HitWallTopCheck : MonoBehaviour
{
	private void Start()
	{
	}

	private void OnTriggerEnter2D(Collider2D wallCol)
	{
		if (wallCol.gameObject.tag == "Floor")
		{
			SendMessageUpwards("HitTop");
		}
	}
}