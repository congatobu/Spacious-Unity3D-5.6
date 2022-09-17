using UnityEngine;

public class ReactorSpawn : MonoBehaviour
{
	public GameObject reactor;
	public GameObject[] spawns;

	private void Start()
	{
		//yield return 0;
		PickRandomRock();
	}


	private void PickRandomRock()
	{
		//int n = Random.Range(0, transform.childCount);
		int n = Random.Range(0, spawns.Length);

		bool reactorPicked = false;
		/*
		if (PlayerPrefsX.GetBool("saved"))
		{
			n = PlayerPrefs.GetInt("reactorRock");
			reactorPicked = PlayerPrefsX.GetBool("reactorCol");
		}
		*/

		//GameObject rock = transform.GetChild(n).gameObject;
		GameObject rock = spawns[n].gameObject;

		if (!reactorPicked)
		{
			GameObject newRector = Instantiate(reactor, rock.transform.position, Quaternion.identity);

			PlayerPrefsX.SetVector3("reactorPos", rock.transform.position);
		}

		Destroy(rock);
		rock = null;

		PlayerPrefs.SetInt("reactorRock", n);
	}
}