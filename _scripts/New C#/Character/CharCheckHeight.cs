using UnityEngine;

public class CharCheckHeight : MonoBehaviour
{
	public float hurtHeight = 4.5f;
	private CharCtrl charCtrl;
	public float fallHeight;
	private float hitHeight;
	public ParticleSystem landParticle;
	private Color myColor;
	private float myVelo;
	private bool touch;

	private void Awake()
	{
		charCtrl = transform.parent.gameObject.GetComponent<CharCtrl>();
		myColor = landParticle.startColor;
	}

	private void Start()
	{
		//yield return 0;
		fallHeight = transform.position.y;
	}

	public void Spawn()
	{
		fallHeight = transform.position.y;
	}

	private void Update()
	{
		if (charCtrl.grounded)
			return;

		myVelo = Mathf.Floor(transform.parent.gameObject.GetComponent<Rigidbody2D>().
			velocity.y);

		if (myVelo < 0 && myVelo > -3)
			fallHeight = transform.position.y;
	}

	private void OnTriggerEnter2D(Collider2D floorCol)
	{
		if (charCtrl.charDead)
			return;

		if (floorCol.gameObject.tag == "Floor" && !touch) // MAYBY BUG
		{
			Debug.Log("Floor Hit On Check Height Char!");

			touch = true;

			SendMessageUpwards("TouchFloor");

			hitHeight = fallHeight - transform.position.y;

			//Debug.Log(hitHeight);

			if (charCtrl.switchLight)
				landParticle.startColor = myColor;
			else
				landParticle.startColor = Color.black;

			if (transform.parent.gameObject.GetComponent<Rigidbody2D>().velocity.y < -1)
			{
				//landParticle.emissionRate = 0.5f + (hitVelo * -20);
				landParticle.Emit(30);
				//landParticle.Play();
				touch = true;
			}

			if (hitHeight > hurtHeight)
			{
				var hitDamage = (int) Mathf.Floor(hitHeight/hurtHeight);
				if (hitHeight > hurtHeight*3)
					SendMessageUpwards("FallingDie");

				SendMessageUpwards("TakeDamage", hitDamage);

				//Debug.Log("Ouch!");
			}

			fallHeight = transform.position.y;
		}
	}

	private void OnTriggerExit2D(Collider2D floorCol)
	{
		touch = false;
	}
}