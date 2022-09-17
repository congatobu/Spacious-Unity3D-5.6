using UnityEngine;

public class Clouds : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		transform.Translate(new Vector2(0.05f*Time.deltaTime, 0));
	}
}