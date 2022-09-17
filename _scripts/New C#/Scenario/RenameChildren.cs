using UnityEngine;

public class RenameChildren : MonoBehaviour
{
	public string baseName;
	public int startCount = 0;

	private void Awake()
	{
		Rename();
	}

	private void Rename()
	{
		for (var i = 0; i < transform.childCount; i++)
		{
			var objRename = transform.GetChild(i).gameObject;
			objRename.name = baseName + (i + startCount);
		}
	}
}