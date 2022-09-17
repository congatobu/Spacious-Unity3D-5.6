using System.Collections;
using UnityEngine;

public class ChatText : MonoBehaviour
{
	private TextMesh myText;
	private bool showNew;

	private void Awake()
	{
		HideText();
		myText.GetComponent<Renderer>().sortingLayerID = 7;
	}

	private void Update()
	{
	}

	private void HideText()
	{
		var myTextColor = myText.color;
		myTextColor.a = 0;
	}

	private IEnumerator ShowCharText(string speech)
	{
		showNew = true;

		myText.text = speech;
		var myTextColor = myText.color;
		myTextColor.a = 1;

		var textWords = speech.Split(" "[0]);

		yield return new WaitForSeconds(speech.Length*0.1f);

		showNew = false;

		if (!showNew)
			HideText();
	}
}