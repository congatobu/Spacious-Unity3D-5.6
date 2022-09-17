using UnityEngine;

public class ReactorBorder : MonoBehaviour
{
	private ReactorCorrect ReactorCorrect;
	public bool triggeredEnter = false;

	private void Start()
	{
		ReactorCorrect = transform.parent.gameObject.GetComponent<ReactorCorrect>();
	}

	private void OnTriggerEnter2D (Collider2D myCol)
	{
		if (!triggeredEnter)
		{
			if (myCol.gameObject.tag == "Floor")
			{
				Debug.Log(myCol.gameObject.name + "&Enter& " + myCol.gameObject.tag + "&Send& " + this.gameObject.name);
				ReactorCorrect.BordersExploit++;
				triggeredEnter = true;
			}
		}
	}

	/*
	private void OnTriggerStay2D(Collider2D myCol)
	{
		if (myCol.gameObject.tag == "Floor")
		{
			Debug.Log(myCol.gameObject.name + "&Stay& " + myCol.gameObject.tag + "&Пропизделся&" + this.gameObject.name);
		}
	}
	*/
	/*
	private void OnTriggerExit2D (Collider2D myCol)
	{
		if (myCol.gameObject.tag == "Floor")
		{
			Debug.Log(myCol.gameObject.name + "&& " + myCol.gameObject.tag);
		}
	}
	*/
}