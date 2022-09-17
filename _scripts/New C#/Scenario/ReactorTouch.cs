using UnityEngine;

public class ReactorTouch : MonoBehaviour
{
	/*
	private void Update()
	{
		RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, 5f);

		if (hit.collider.gameObject != this.gameObject && hit.collider.gameObject.name != "CheckChar" &&
		    hit.collider.gameObject.name != "Reactor(Clone)")
		{
			Debug.Log(" ПОПАЛ" + hit.collider.gameObject.name);
			//Debug.DrawLine(transform.position, hit.collider.transform.position, Color.white, 5f, false);
		}
	}
	*/

	private void OnTriggerEnter2D(Collider2D myCol)
	{
		if (myCol.gameObject.tag == "Player")
			SendMessageUpwards("CharClose", true);
	}

	private void OnTriggerExit2D(Collider2D myCol)
	{
		if (myCol.gameObject.tag == "Player")
			SendMessageUpwards("CharClose", false);
	}
}