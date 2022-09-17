using System.Collections;
using UnityEngine;

public class CrystalRockTutorial : MonoBehaviour
{
	public bool msgShown;
	public CrystalRock rockMain;

	private void Awake()
	{
		rockMain = GetComponent<CrystalRock>();
	}

	private IEnumerator TouchingChar(bool state)
	{
		if (rockMain.myChar == null)
			yield break;

		if (state)
		{
			if (msgShown)
				yield break;

			msgShown = true;

			rockMain.myChar.SendMessage("ShowCharText", "I could try using a BOMB\n on this crystal rock");
		}
		else
		{
			yield return new WaitForSeconds(3f);
			msgShown = false;
		}
	}
}