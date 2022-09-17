using UnityEngine;
using System.Collections;

public class AlienTutorial : MonoBehaviour
{
	public bool charClose = false;
	public AlienACtrl mainAlien;
	public bool text1Show = false;
	public bool text2Show = false;

	void Awake()
	{
		mainAlien = GetComponent<AlienACtrl>();
	}

	void FixedUpdate()
	{

		if (text2Show)
			return;

		if (charClose)
		{
			if (mainAlien.attacking)
			{
				if (!text2Show)
				{
					ShowTutorial();
					text2Show = true;
				}
			}
		}
	}
	
	private IEnumerator CharCloseBy(bool state)
	{
		if (charClose == state)
			yield break;

		charClose = state;

		if (charClose)
		{
			if (!text1Show)
			{
				text1Show = true;

				mainAlien.myChar.SendMessage("LightOff");

				mainAlien.myChar.SendMessage("ShowCharText", "System failing on me again?");

				yield return new WaitForSeconds(3f);

				mainAlien.myChar.SendMessage("ShowCharText", "Fixed it");

				mainAlien.myChar.SendMessage("LightOn");
			}

		}
		else
		{
			mainAlien.myChar.SendMessage("HideText");
		}
	}
	
	void ShowTutorial()
	{
		mainAlien.myChar.SendMessage("ShowCharText", "I don't wanna get too \n close to this... THING");
	}
}