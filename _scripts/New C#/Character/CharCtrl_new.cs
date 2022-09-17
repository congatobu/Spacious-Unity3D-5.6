using UnityEngine;

public class CharCtrl_new : MonoBehaviour
{
	private readonly float groundRadius = 0.2f;

	//jet
	private readonly float jetFuel = 100f;
	private readonly float jetFuelWaste = 0.3f;
	private readonly float jumpForce = 500f;
	private readonly float maxRunSpeed = 4.0f;
	//walking / moving
	private readonly float maxWalkSpeed = 2.0f;
	private readonly float speedMultip = 1.0f;
	private Animator anim;
	private bool canJet;

	//booleans
	private bool facingRight = true;
	private Transform groundCheck;

	//grounding
	private bool grounded;
	private bool hasJumped;
	private bool jetOn;

	//effects
	private ParticleSystem jetParticle;

	private float jetUsed;
	private ParticleSystem landParticle;

	private float move;
	private float moveSpeed;
	private float myMaxSpeed;
	private bool running;
	public GameObject shakeDummy;
	private LayerMask whatIsGround;

	private void Start()
	{
		anim = gameObject.GetComponent<Animator>();
		myMaxSpeed = maxWalkSpeed;
		jetOn = false;
		iTween.Init(shakeDummy);
		jetUsed = jetFuel;
	}

	private void FixedUpdate()
	{
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
		anim.SetBool("Ground", grounded);
		anim.SetFloat("vSpeed", GetComponent<Rigidbody2D>().velocity.y);

		move = Input.GetAxis("Horizontal");

		if (move != 0)
		{
			if (moveSpeed < myMaxSpeed)
				moveSpeed += Mathf.Abs(move)*0.05f;
			else
				moveSpeed = myMaxSpeed;

			anim.speed = 1 + moveSpeed/4;
		}
		else
		{
			moveSpeed = 0;
			anim.speed = 1;
		}

		anim.SetFloat("Speed", moveSpeed);

		GetComponent<Rigidbody2D>().velocity = new Vector2(moveSpeed*move*speedMultip, GetComponent<Rigidbody2D>().velocity.y);

		//FLIP
		if (move > 0 && !facingRight)
			Flip();
		else if (move < 0 && facingRight)
			Flip();

		//JET
		if (jetOn)
		{
			iTween.ShakePosition(shakeDummy, iTween.Hash(
				"x",
				10,
				"time",
				5));

			GetComponent<Rigidbody2D>().AddForce(new Vector2(shakeDummy.transform.localPosition.x*10, 0));
			anim.SetBool("Jet", true);
			jetParticle.emissionRate = 20;

			if (jetUsed > 0)
				jetUsed -= jetFuelWaste;
			else
			{
				jetUsed = 0;
				canJet = false;
				jetOn = false;
			}
		}
		else
		{
			if (jetUsed < jetFuel)
				jetUsed += jetFuelWaste*2;
			else
				jetUsed = jetFuel;

			//shakeDummy.transform.localPosition.y = 0;
			shakeDummy.transform.localPosition = new Vector3(0, 0, shakeDummy.transform.position.z);
			//shakeDummy.transform.localPosition.x = 0;
			anim.SetBool("Jet", false);
			if (jetParticle.emissionRate > 0)
				jetParticle.emissionRate -= 0.5f;
			else
				jetParticle.emissionRate = 0;
		}
	}

	private void Update()
	{
		//JUMP
		if (grounded && Input.GetKeyDown(KeyCode.Space))
		{
			anim.SetBool("Ground", false);
			GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce));
			canJet = false;
			hasJumped = true;
		}

		if (!grounded && Input.GetKeyUp(KeyCode.Space))
		{
			jetOn = false;
			canJet = true;
		}

		if (!grounded && canJet && Input.GetKey(KeyCode.Space))
		{
			jetOn = true;
			//make go up
			if (GetComponent<Rigidbody2D>().velocity.y <= 2)
				GetComponent<Rigidbody2D>().
					AddForce(new Vector2(0, jumpForce/20));
		}

		//RUN
		if (grounded && Input.GetKey(KeyCode.LeftShift))
		{
			myMaxSpeed = maxRunSpeed;
			running = true;
		}
		else
			running = false;

		//ACCELERATION AFTER RUNNING
		if (!running && move != 0)
		{
			if (myMaxSpeed > maxWalkSpeed)
				myMaxSpeed -= 0.02f;
			else
				myMaxSpeed = maxWalkSpeed;
		}
	}

	private void Flip()
	{
		facingRight = !facingRight;
		var theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
		landParticle.gameObject.transform.localScale = theScale;
	}

	private void OnCollisionEnter2D(Collision2D coli)
	{
		//landing particles
		if (coli.gameObject.tag == "Floor" && hasJumped)
		{
			landParticle.Play();
			hasJumped = false;
			jetOn = false;
		}
	}
}