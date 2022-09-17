using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharCtrl : MonoBehaviour
{
	//Bomb
	public GameObject aBomb;

	//Crystal
	public GameObject aCrystal;
	public Animator anim;
	public GameObject aSpawner;
	public bool canBomb = true;
	public bool canJet;
	public bool canPlaceSpawn = true;
	public bool canRespawn = true;
	public bool charDead;
	public Sprite deadSprite;
	public bool enemyNear = false;

	//booleans
	public bool facingRight = true;
	public Transform groundCheck;

	//grounding
	public bool grounded;
	public float groundRadius = 0.2f;
	public bool hasJumped;
	public Color hurtColor;
	public GameObject jetBar;
	public SpriteRenderer jetBarRender;
	public Color[] jetColors;
	public float jetForce = 30f;
	public float jetFuel = 100f;
	public float jetFuelWaste = 0.3f;
	public bool jetOn;

	//effects
	public ParticleSystem jetParticle;

	//jet
	public AudioClip jetSFX;

	public float jetUsed;
	public float jumpForce = 500f;
	public ParticleSystem landParticle;
	public float lightIntensity;
	//main ctrls
	public MainCtrl mainCTRL;
	public float maxRunSpeed = 4.0f;

	//walking / moving
	public float maxWalkSpeed = 2.0f;

	public float move;
	public float moveSpeed;
	public GUICtrl myGUICtrl;

	//LIGHT
	public Light myLight;
	public float myMaxSpeed;
	public SpriteRenderer myRenderer;
	public bool running;

	public GameObject spawner;
	public float speedMultip = 1.0f;
	public bool switchLight = true;
	public GameObject tilePrefab;
	public bool toCreateDead;
	public string touchingAction = "";
	public LayerMask whatIsGround;

	//-------------------------------//
	//-----------START
	//-------------------------------//

	private void Awake()
	{
		mainCTRL = GameObject.Find("MainCtrl").GetComponent<MainCtrl>();

		myGUICtrl = GameObject.Find("GUI").GetComponent<GUICtrl>();

		anim = GetComponent<Animator>();
		myRenderer = GetComponent<SpriteRenderer>();
		myMaxSpeed = maxWalkSpeed;
		jetOn = false;
		jetUsed = jetFuel;
		jetBarRender = jetBar.GetComponent<SpriteRenderer>();
		lightIntensity = myLight.intensity;

		HideChildren();
		HideChar();

		charDead = true;
		anim.SetBool("Dead", true);
		jetBar.GetComponent<
		Renderer > ().
		enabled = false;
	}

	private void Start()
	{
		//yield return 0;

		//look for the spawner
		if (spawner == null)
			spawner = GameObject.FindWithTag("Respawn");

		if (myGUICtrl.gameSaved)
		{
			Vector3 savedPos = PlayerPrefsX.GetVector3("playerPos");

			if (savedPos != spawner.transform.position)
			{
				transform.position = savedPos;
				CreateSpawner();

				Debug.Log("Char position loaded");
			}

			var reactorSaved = PlayerPrefsX.GetBool("reactorCol");
			if (reactorSaved)
				PickedReactor();
		}

		transform.position = spawner.transform.position;
	}

	//-------------------------------//
	//-----------SPAWN
	//-------------------------------//

	/*public void Spawn()
	{
		StartCoroutine("IESpawn");
	}*/
	
	public IEnumerator Spawn()
	{
		Debug.Log("OLOLOSPAWN!");
		//if char isn't dead, return
		if (!charDead || !canRespawn)
			yield break;

		yield return 0;

		//look for the spawner
		if (spawner == null)
			spawner = GameObject.FindWithTag("Respawn");

		//move the character to its position
		gameObject.transform.position = spawner.transform.position;

		yield return 0;

		spawner.SendMessage("Spawning");
		anim.SetTrigger("Spawn");
		anim.SetBool("Dead", false);

		SendMessage("RenewHealth");

		//make every child active again
		foreach (Transform child in transform)
			child.gameObject.SetActive(true);

		jetBar.gameObject.SetActive(true);

		jetUsed = jetFuel;

		switchLight = true;

		//trigger spawning animation
		myRenderer.enabled = true;

		yield return new WaitForSeconds(0.3f);

		//char isn't dead anymore
		charDead = false;

		groundCheck.gameObject.SendMessage("Spawn");

		canRespawn = false;
	}
	
	public void CanRespawn()
	{
		canRespawn = true;
		//StartCoroutine("Spawn");
	}

	//-------------------------------//
	//-----------MOVEMENT
	//-------------------------------//

	private void FixedUpdate()
	{
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
		anim.SetBool("Ground", grounded);
		anim.SetFloat("vSpeed", GetComponent < Rigidbody2D > ().
		velocity.y)
		;

		if (charDead)
		{
			move = 0;
			return;
		}

		move = Input.GetAxis("Horizontal");

		jetBar.gameObject.transform.localScale = new Vector3(1, jetUsed/100, 1);
		//jetBarRender.color = Color.Lerp(jetColors[1], jetColors[0], jetUsed/100);

		jetBarRender.color = Color.Lerp(Color.green, Color.yellow, Mathf.PingPong(Time.time, 1));

		if (jetUsed >= 100 && grounded)
			jetBar.GetComponent<Renderer> ().enabled = false;
		else
			jetBar.GetComponent<Renderer> ().enabled = switchLight;

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

		GetComponent<Rigidbody2D> ().velocity = new Vector2(moveSpeed*move*speedMultip, GetComponent < Rigidbody2D > ().velocity.y);

		//FLIP
		if (move > 0 && !facingRight)
			Flip();
		else if (move < 0 && facingRight)
			Flip();

		//TURN LIGHT
		if (switchLight && myLight.intensity != lightIntensity)
		{
			if (myLight.intensity < lightIntensity)
			{
				myLight.intensity += 0.02f;
				myRenderer.color = Color.Lerp(Color.black, Color.white, myLight.intensity/lightIntensity);
			}
			else
			{
				myLight.intensity = lightIntensity;
				myRenderer.color = Color.white;
			}
		}

		//JET
		if (jetOn)
		{
			jetParticle.emissionRate = 30;
			if (jetUsed > 0 && transform.position.y <= 4)
				jetUsed -= jetFuelWaste;
			else
			{
				//jetUsed = 0;
				canJet = false;
				jetOn = false;
			}
		}
		else
		{
			if (jetUsed < jetFuel)
				jetUsed += jetFuelWaste*0.4f;
			else
				jetUsed = jetFuel;

			anim.SetBool("Jet", false);
			if (jetParticle.emissionRate > 0)
				jetParticle.emissionRate -= 0.5f;
			else
				jetParticle.emissionRate = 0;
		}
	}

	private void Update()
	{
		if (charDead)
			return;

		//JUMP
		if (grounded && Input.GetKeyDown(KeyCode.Space))
		{
			anim.SetBool("Ground", false);
			SendMessage("Jump");
			GetComponent<
			Rigidbody2D> ().
			AddForce(new Vector2(0, jumpForce));
			canJet = false;
			hasJumped = true;
			//canJet = true;
		}

		if (!grounded && Input.GetKeyUp(KeyCode.Space))
		{
			jetOn = false;
			if (switchLight)
				canJet = true;
		}

		if (!grounded && canJet && Input.GetKey(KeyCode.Space) && switchLight)
		{
			if (!jetOn)
			{
				jetOn = true;
				anim.SetBool("Jet", true);
			}

			//make go up
			if (GetComponent<
			Rigidbody2D > ().
			velocity.y <= 2)
			GetComponent<
			Rigidbody2D > ().
			AddForce(new Vector2(0, jetForce));
		}

		if (grounded && jetOn)
		{
			canJet = false;
			jetOn = false;
		}

		//LIGHT SWITCH
		if (Input.GetKeyDown("e"))
		{
			if (switchLight)
			{
				LightOff();
			}
			else
			{
				switchLight = true;
			}
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

		if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown("w"))
			StartCoroutine("NewSpawner");

		if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown("s"))
			Action();
	}

	public void Flip()
	{
		facingRight = !facingRight;
		var theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
		landParticle.gameObject.transform.localScale = theScale;
	}

	//-------------------------------//
	//-----------DEATH
	//-------------------------------//

	/*public void KillChar()
	{
		StartCoroutine("IEKillChar");
	}*/

	public IEnumerator KillChar()
	{
		//mainCTRL.LowerSoundTrack(0.2f);
		mainCTRL.StartCoroutine("LowerSoundTrack", 0.2f);

		StopChar();

		//LoseCrystals();

		while (myLight.intensity > 0)
		{
			myLight.intensity -= 0.03f;
			myRenderer.color = Color.Lerp(Color.black, Color.white, myLight.intensity/lightIntensity);
			yield return 0;
		}

		mainCTRL.gameObject.SendMessage("DeathSound");

		yield return new WaitForSeconds(1f);

		HideChildren();

		if (toCreateDead)
			CreateDeadCopy();

		anim.SetBool("Dead", true);

		//send message to main ctrl that the char is dead
		mainCTRL.CharIsDead();
	}

	public void StopChar()
	{
		charDead = true;

		GetComponent<
		Rigidbody2D > ().
		velocity = new Vector2(0, 0);

		move = 0;

		jetParticle.gameObject.SetActive(false);

		jetBar.gameObject.SetActive(false);
	}

	public void HideChildren()
	{
		foreach (Transform child in transform)
			child.gameObject.SetActive(false);
		jetBar.gameObject.SetActive(false);
	}

	public void HideChar()
	{
		landParticle.gameObject.SetActive(false);
		myRenderer.enabled = false;
	}

	public void Hurt()
	{
		var origColor = myRenderer.color;
		var hurtTime = 1f;
		while (hurtTime > 0)
		{
			myRenderer.color = Color.Lerp(origColor, hurtColor, hurtTime);
			hurtTime -= 0.01f;
			//yield return 0;
		}
	}

	public void DieAnim()
	{
		anim.SetTrigger("Die");
		toCreateDead = true;
	}

	public void CreateDeadCopy()
	{
		GameObject deadCopy = Instantiate(tilePrefab);
		deadCopy.transform.parent = null;
		deadCopy.transform.position = gameObject.transform.position;
		deadCopy.transform.localScale = gameObject.transform.localScale;
		SpriteRenderer sprRender = deadCopy.GetComponent<SpriteRenderer>();
		sprRender.sprite = deadSprite;
		deadCopy.gameObject.SetActive(true);

		toCreateDead = false;
	}

	public void LoseCrystals()
	{
		//throw some crystals around
		//int numCrystals = Mathf.Floor(mainCTRL.crystalCount / 2);
		int numCrystals = mainCTRL.crystalCount;

		if (numCrystals == 0)
			return;

		//pra metade dos cristais que o char tem
		for (var i = 0; i < numCrystals; i++)
		{
			//instantiate it
			var dropCrystal = Instantiate(aCrystal, myLight.transform.position, Quaternion.identity);
			//not collectable for the total crystal
			dropCrystal.SendMessage("Collectable", false);

			//random force
			float randX = Random.Range(-200, 200);
			float randY = Random.Range(100, 300);
			//add force
			dropCrystal.GetComponent<
			Rigidbody2D > ().
			AddForce(new Vector2(randX, randY));
		}
	}

	//-------------------------------//
	//-----------ACTION
	//-------------------------------//

	public void LightOff()
	{
		switchLight = false;
		myLight.intensity = 0;
		myRenderer.color = Color.black;
	}

	public void LightOn()
	{
		switchLight = true;
	}

	public void PickedCrystal(bool state)
	{
		if (!charDead)
		{
			SendMessage("CrystalSound");

			mainCTRL.AddCrystal(1, state);
		}
	}

	public void PickedReactor()
	{
		SendMessage("CrystalSound");

		SendMessage("ShowCharText", "Reactor found... \n I should get back to the ship now");

		mainCTRL.reactorCollected = true;

		Application.ExternalCall("kongregate.stats.submit", "ReactorFound", 1);

		//achievement FOUND REACTOR
	}

	public void Touching(string what)
	{
		touchingAction = what;

		if (what == "MainSpawner" && mainCTRL.reactorCollected)
			SendMessage("ShowCharText", "I've got the reactor, \n press ACTION and we can get out of here.");
	}

	public void Action()
	{
		if (touchingAction == "MainSpawner")
		{
			//acha o main spawner
			var mainSpawner = GameObject.Find("MainSpawner").transform;
			// mover char pro x do spawner
			transform.position = new Vector3(mainSpawner.transform.position.x, transform.position.y, transform.position.z);
			//game over pro mainctrl
			mainCTRL.StartCoroutine("AskGameOver");
		}

		if (touchingAction == "" || touchingAction == "Rock")
		{
			anim.SetTrigger("Action");
			StartCoroutine("DropBomb");
		}
	}

	public IEnumerator NewSpawner()
	{
		if (!canPlaceSpawn)
			yield break;

		if (mainCTRL.crystalCount <= 0)
		{
			SendMessage("ShowCharText", "I don't have enough crystals");
			yield break;
		}

		if (enemyNear)
		{
			SendMessage("ShowCharText", "This place is too dangerous \n for a spawner");
			yield break;
		}

		//anim.SetTrigger("Action");

		SendMessage("NewSpawnerSound");

		canPlaceSpawn = false;

		CreateSpawner();

		mainCTRL.AddCrystal(-1, false);

		myGUICtrl.SaveGame();

		yield return new WaitForSeconds(5f);

		canPlaceSpawn = true;
	}

	public void CreateSpawner()
	{
		spawner.SendMessage("Collect");

		var newSpawn = Instantiate(aSpawner, transform.position, Quaternion.identity);

		//newSpawn.transform.position.y = transform.position.y + 0.3f;

		newSpawn.transform.position = new Vector3(newSpawn.transform.position.x, transform.position.y + 0.3f, newSpawn.transform.position.z);

		if (facingRight)
			newSpawn.transform.position = new Vector3(transform.position.x + 0.7f, newSpawn.transform.position.y, newSpawn.transform.position.z);
		else
			newSpawn.transform.position = new Vector3(transform.position.x - 0.7f, newSpawn.transform.position.y, newSpawn.transform.position.z);

		spawner = newSpawn;
	}

	public IEnumerator DropBomb()
	{
		if (!canBomb)
		{
			SendMessage("ShowCharText", "Reloading...");
			yield break;
		}

		canBomb = false;

		var newBomb = Instantiate(aBomb, transform.position, Quaternion.identity);
		//newBomb.transform.position.y += 0.3f;
		newBomb.transform.position = new Vector3(newBomb.transform.position.x, newBomb.transform.position.y + 0.3f, newBomb.transform.position.z);

		if (facingRight)
			newBomb.transform.position = new Vector3(newBomb.transform.position.x + 0.3f, newBomb.transform.position.y, newBomb.transform.position.z);
		else
			newBomb.transform.position = new Vector3(newBomb.transform.position.x - 0.3f, newBomb.transform.position.y, newBomb.transform.position.z);


		mainCTRL.bombCount++;

		if (touchingAction == "Rock")
			//newBomb.rigidbody2D.isKinematic = true;

			touchingAction = "";

		//mainCTRL.AddCrystal(-1, false);

		yield return new WaitForSeconds(3f);

		canBomb = true;
	}
}