using UnityEngine;
using System.Collections;

public class Reactor : MonoBehaviour
{
	public Light myLight;
	public float origIntensity;
	public bool charCloseBy = false;
	public bool picked = false;

	void Awake()
	{
		origIntensity = myLight.intensity;
		myLight.intensity = 0;
	}

	private IEnumerator LightOn()
	{
		while (myLight.intensity < origIntensity)
		{
			myLight.intensity += 0.05f;
			yield return 0;
		}

		myLight.intensity = origIntensity;
	}

	private IEnumerator LightOff()
	{

		while (myLight.intensity > 0)
		{
			myLight.intensity -= 0.05f;
			yield return 0;
		}

		myLight.intensity = 0;
	}

	public void CharClose(bool state)
	{
		if (charCloseBy == state)
			return;

		charCloseBy = state;

		if (state)
			StartCoroutine("LightOn");
		else
			StartCoroutine("LightOff");

	}

	private void OnTriggerEnter2D(Collider2D charCol)
	{
		if (charCol.gameObject.tag == "Player")
		{
			StartCoroutine("TriggerEnter", charCol.gameObject);
		}
	}

	private IEnumerator TriggerEnter(GameObject enterObject)
	{
		if (picked)
			yield break;

		picked = true;

		CharCtrl myChar = enterObject.gameObject.GetComponent<CharCtrl>();

		if (myChar.charDead)
			yield break;

		//send message to char
		myChar.PickedReactor();

		//wait
		yield return new WaitForSeconds(0.2f);

		//destroy
		Destroy(gameObject);
	}
}