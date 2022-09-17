using UnityEngine;

public class AlienACheckChar : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnTriggerEnter2D(Collider2D myCol)
	{
		if (myCol.gameObject.tag == "Player")
			SendMessageUpwards("CharCloseBy", true);
	}

	private void OnTriggerExit2D(Collider2D myCol)
	{
		if (myCol.gameObject.tag == "Player")
			SendMessageUpwards("CharCloseBy", false);
	}
}