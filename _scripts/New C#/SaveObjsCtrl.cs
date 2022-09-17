using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SaveObjsCtrl : MonoBehaviour
{
	public string mainName;
	private List<string> objsArr;
	public int objsSaved;

	private void Awake()
	{
		NotificationCenter.DefaultCenter().AddObserver(gameObject, "SaveGame");
	}

	private void Start()
	{
		objsArr = new List<string>();

		if (PlayerPrefsX.GetBool("saved"))
			LoadObjs();
	}

	public void ObjGone(string rock)
	{
		objsArr.Add(rock);
	}

	public void SaveGame()
	{
		if (objsSaved == objsArr.Count)
			return;

		PlayerPrefsX.SetStringArray(mainName, objsArr.ToArray());
		objsSaved = objsArr.Count;
	}

	public void LoadObjs()
	{
		Debug.Log("Loading");
		var objsList = PlayerPrefsX.GetStringArray(mainName);
		objsArr = new List<string>();

		for (var i = 0; i < objsArr.Count; i++)
		{
			var newObj = GameObject.Find(objsArr[i]);
			newObj.SendMessage("DestroyMe");
		}
	}
}