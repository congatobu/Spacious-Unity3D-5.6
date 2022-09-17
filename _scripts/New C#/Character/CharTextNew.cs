using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharTextNew : MonoBehaviour
{
	public int counter;
	public TextMesh myText;
	//private Array textArr = new Array();
	private List<string> textArr = new List<string>();

	private void Awake()
	{
		HideText();
		/*myText.GetComponent<Renderer>().sortingLayerID = 7;*/
	}

	private void HideText()
	{
		myText.text = "";
		Color myTextColor = myText.color;
		myTextColor.a = 0;
	}

	private void ShowCharText(string newText) //adiciona um novo texto a lista
	{
		if (textArr.Count > 0)
		{
			if (newText == textArr[textArr.Count])
				return;
		}

		//textArr.Push(newText);
		textArr.Add(newText);

		if (textArr.Count == 1)
			//PrintText();
			StartCoroutine("PrintText");
	}

	private IEnumerator PrintText() //mostra o texto
	{
		string speech = textArr[counter];

		myText.text = speech;
		Color myTextColor = myText.color;
		myTextColor.a = 1;

		var textWords = speech.Split(" "[0]);

		yield return new WaitForSeconds(speech.Length*0.15f);

		counter++;

		CheckList();
	}

	private void CheckList() //verifica se tem mais na lista
	{
		if (textArr.Count > counter)
			PrintText();
		else
		{
			HideText();
			counter = 0;
			textArr = new List<string>();
		}
	}
}