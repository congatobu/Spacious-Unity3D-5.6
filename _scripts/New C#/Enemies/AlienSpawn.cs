using UnityEngine;

public class AlienSpawn : MonoBehaviour
{
	public int maxAliensOut = 10;

	private void Start()
	{
		//yield return 0;
		//SpawnAliens();///ZAKOMENTUROVAL YA
		//yield return 0;
		if (PlayerPrefsX.GetBool("saved"))
			LoadAliens();
	}


	private void SpawnAliens()
	{
		var n = Random.Range(4, maxAliensOut);

		var usedN = new int[transform.childCount];

		for (var i = 0; i < transform.childCount; i++)
		{
			usedN[i] = i;
		}

		RandomizeList(usedN);

		if (PlayerPrefsX.GetBool("saved"))
		{
			n = PlayerPrefs.GetInt("aliensN");
			usedN = PlayerPrefsX.GetIntArray("aliensUsedN");
		}


		for (var j = 0; j < n; j++)
		{
			var alien = transform.GetChild(usedN[j]).gameObject;
			Destroy(alien);
		}

		PlayerPrefs.SetInt("aliensN", n);
		PlayerPrefsX.SetIntArray("aliensUsedN", usedN);
	}

	private void LoadAliens()
	{
	}

	private void RandomizeList(int[] arr)
	{
		for (int i = arr.Length - 1; i > 0; i--)
		{
			int r = Random.Range(0, i);
			int tmp = arr[i];
			arr[i] = arr[r];
			arr[r] = tmp;
		}
	}
}