using UnityEngine;
using System.Collections;

public class CharCheckSpeed : MonoBehaviour
{
	private float hitVelo;
	float hurtVelocity = -8f;
	ParticleSystem landParticle;
	private CharCtrl charCtrl;
	private Color myColor;

	private bool touch = false;

	void Start()
	{
		charCtrl = transform.parent.gameObject.GetComponent<CharCtrl>();
		myColor = landParticle.startColor;
	}

	void Update()
	{

	}

	void OnTriggerEnter2D(Collider2D floorCol)
	{
		if (charCtrl.charDead)
			return;

		if (touch)
			return;

		if (floorCol.gameObject.tag == "Floor")
		{
			SendMessageUpwards("TouchFloor");

			hitVelo = transform.parent.gameObject.GetComponent< Rigidbody2D > ().velocity.y;

			if (charCtrl.switchLight)
				landParticle.startColor = myColor;
			else
				landParticle.startColor = Color.black;

			if (hitVelo < -1)
			{
				//landParticle.emissionRate = 0.5f + (hitVelo * -20);
				landParticle.Emit((int)hitVelo * -5);
				//landParticle.Play();
				touch = true;
			}

			if (hitVelo < hurtVelocity)
			{
				int hitDamage = (int)Mathf.Floor(hitVelo / hurtVelocity);
				if (hitVelo < hurtVelocity * 4)
					SendMessageUpwards("FallingDie");

				SendMessageUpwards("TakeDamage", hitDamage);

				//Debug.Log("Ouch!");
			}
		}
	}

	void OnTriggerExit2D(Collider2D floorCol)
	{
		touch = false;
	}
}