using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactorCorrect : MonoBehaviour
{
	public int BordersExploit = 0;
	public int BorderMaxExploit;

	private void Update()
	{
		if (BordersExploit == BorderMaxExploit)
		{
			CorrectPosition();
		}
	}

	private void CorrectPosition()
	{
		Debug.Log("Restart Level Generated.");
		Application.LoadLevel(Application.loadedLevel);
	}
}
