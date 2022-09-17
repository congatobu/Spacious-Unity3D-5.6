using System.Collections;
using UnityEngine;

public class AlienACtrl : MonoBehaviour
{
	public float attackDistance = 2f;
	public bool attacking;

	public AudioSource audioCtrl;

	public GameObject body;
	private Animator bodyAnim;
	private bool charClose;
	public CharCtrl charCtrl;
	public bool charDead;

	public float charDistance;

	public CircleCollider2D closeCollider;
	private float colliderRad;
	public GameObject eyes;
	private Animator eyesAnim;

	private bool eyesUp;
	//main ctrls
	public GameObject mainCTRL;

	public GameObject myChar;

	public Color myColor;

	private void Awake()
	{
		bodyAnim = body.gameObject.GetComponent<Animator>();
		eyesAnim = eyes.gameObject.GetComponent<Animator>();

		if (mainCTRL == null)
			mainCTRL = GameObject.Find("MainCtrl");

		if (myChar == null)
			myChar = GameObject.Find("Character");

		charCtrl = myChar.gameObject.GetComponent<CharCtrl>();

		audioCtrl = GetComponent<AudioSource>();
	}

	private void FixedUpdate()
	{
		if (charClose)
		{
			if (charDead)
			{
				Attack(false);
				CharCloseBy(false);
				return;
			}

			//calcular distancia do char
			charDistance = Vector2.Distance(gameObject.transform.position, myChar.gameObject.transform.position);

			if (charCtrl.switchLight)
			{
				if (eyesUp)
				{
					eyesAnim.SetTrigger("EyesDown");
					eyesUp = false;
				}
			}
			else
			{
				if (!eyesUp)
				{
					eyesAnim.SetTrigger("EyesUp");
					eyesUp = true;
				}
			}


			//se estiver perto o suficiente entao rodar a animacao
			if (charDistance < attackDistance)
			{
				PlaySound();
				if (!attacking)
					Attack(true);
			}
			else
			{
				if (attacking)
					Attack(false);
			}
		}
		else
		{
			if (eyesUp)
			{
				eyesAnim.SetTrigger("EyesDown");
				eyesUp = false;
			}
		}
	}

	private void CharCloseBy(bool state)
	{
		if (charClose == state)
			return;

		charClose = state;
	}

	private void Attack(bool state)
	{
		if (state)
			bodyAnim.SetTrigger("Attack");
		else
			bodyAnim.SetTrigger("AttackBack");

		charCtrl.enemyNear = state;

		attacking = state;
	}

	private void CharTouched()
	{
		iTween.MoveTo(myChar.gameObject, iTween.Hash ("position", gameObject.transform.position, "time", 0.5f, "oncomplete", "KillChar", "oncompletetarget", gameObject));
	}

	private void KillChar()
	{
		if (charDead)
			return;

		charDead = true;
		//Kill the char
		//myChar.gameObject.transform.position = gameObject.transform.position;
		myChar.SendMessage("Death");
		myChar.SendMessage("HideChar");

		bodyAnim.SetTrigger("Eat");

		//send message to main ctrl that I killed the char
		mainCTRL.SendMessage("KilledChar", gameObject);
	}


	//function to receive message that char has respawned and can act normal again
	private void BackToNormal()
	{
		charDead = false;
		body.SendMessage("BackToNormal");
	}

	private void PlaySound()
	{
		audioCtrl.pitch = Random.Range(0.9f, 1.2f);
		audioCtrl.Play();
	}

	private void StopAlien()
	{
		charDead = true;
	}

	private IEnumerator KillAlien()
	{
		StopAlien();

		if (eyesUp)
			eyesAnim.SetTrigger("EyesDown");

		eyes.GetComponent<Renderer>().
		enabled = false;

		bodyAnim.SetTrigger("Die");

		mainCTRL.SendMessage("KilledEnemy");

		transform.parent.SendMessage("ObjGone", gameObject.name);

		yield return new WaitForSeconds(0.7f);

		DestroyMe();
	}

	private void DestroyMe()
	{
		Destroy(gameObject);
	}
}