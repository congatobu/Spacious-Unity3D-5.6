using UnityEngine;

public class CharHitWall : MonoBehaviour
{
	private CharCtrl charCtrl;
	public float hitForce = 200f;
	private float hitX;
	private float hitY;
	//private float hitDirection;
	private GameObject myChar;
	private bool topHit;

	private void Awake()
	{
		myChar = transform.parent.gameObject;
		charCtrl = myChar.GetComponent<CharCtrl>();
	}

	private void Update()
	{
	}

	private void HitTop()
	{
		hitY = -1;
		topHit = true;
	}

	private void OnTriggerEnter2D(Collider2D wallCol)
	{
		if (wallCol.gameObject.tag == "Floor")
		{
			Debug.Log("Enter Floor");

			if (charCtrl.grounded)
				return;

			if (!topHit)
			{
				if (charCtrl.facingRight)
					hitX = -1;
				else
					hitX = 1;
			}

			SendMessageUpwards("TouchFloor");
			charCtrl.canJet = false;
			//hitDirection = wallCol.transform.position.x - transform.position.x;
			//hitDirection = hitDirection / Mathf.Abs(hitDirection);
			myChar.GetComponent<Rigidbody2D>().AddForce(new Vector2(hitX*hitForce, hitY*hitForce*0.2f));
		}
	}

	private void OnTriggerExit2D(Collider2D wallCol)
	{
		if (wallCol.gameObject.tag == "Floor")
		{
			Debug.Log("Exit Floor");

			charCtrl.canJet = true;
			hitX = 0;
			hitY = 0;
			topHit = false;
		}
	}
}