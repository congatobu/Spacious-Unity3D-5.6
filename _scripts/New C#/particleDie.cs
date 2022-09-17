using UnityEngine;

public class particleDie : MonoBehaviour
{
	private float counter;
	private float mySc = 0;

	private void Start()
	{
		counter = 0;
	}

	private void Update()
	{
		counter += 0.05f;


		if (counter < 3)
		{
			transform.localScale = new Vector3(1 - counter/3, 1 - counter/6, 0.5f - counter/6);
			//alpha goes down
		}
		else
			Destroy(gameObject);
	}
}