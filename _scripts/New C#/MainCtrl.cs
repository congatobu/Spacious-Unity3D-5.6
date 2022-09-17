using System.Collections;
using UnityEngine;

public class MainCtrl : MonoBehaviour
{
	public bool askGOver;
	public MainAudioCtrl audioCtrl;

	public GameObject bgMusic;
	public int bombCount = 0;
	public Transform camTarget;
	public bool canRestart;
	public bool charDead;

	public int crystalCount;
	public int enemiesKilled;
	public bool gameOver;
	public bool gamePaused;
	//public int myPoints = 0;
	private GameObject killerEnemy;
	public int livesCount;

	public Camera mainCamera;

	public MainSpawner mainSpawner;
	//public int crystalTotal = 0;
	public int maxLives = 3;
	public GameObject myChar;
	public GUICtrl myGUICtrl;
	public bool reactorCollected = false;
	public Animator shipDoor;
	public bool shipFixed;
	public AudioSource soundTrack;
	private bool titleScreen;

	//-------------------------------//
	//-----------START
	//-------------------------------//

	private void Awake()
	{
		myGUICtrl = GameObject.Find("GUI").GetComponent<GUICtrl>();

		if (myChar == null)
			myChar = GameObject.Find("Character");

		audioCtrl = GetComponent<MainAudioCtrl>();
		soundTrack = bgMusic.GetComponent<AudioSource>();

		if (mainCamera == null)
			mainCamera = Camera.main;

		iTween.Init(mainCamera.gameObject);

		livesCount = maxLives;
		crystalCount = 0;
		//crystalTotal = 0;
	}

	private void Start()
	{
		StartCoroutine("TitleScreen");
	}

	public IEnumerator TitleScreen()
	{
		myGUICtrl.TitleScreen();

		yield return new WaitForSeconds(2f);

		titleScreen = true;
	}

	public void StartCamera()
	{
		myGUICtrl.HideText();

		myGUICtrl.ShowCredits(false);

		soundTrack.Play();

		StartCoroutine("LowerSoundTrack", 0.7f);

		iTween.MoveTo(mainCamera.gameObject, iTween.Hash(
			"x",
			camTarget.position.x,
			"y",
			camTarget.position.y,
			"time",
			8.0f,
			"oncomplete",
			"NewGame",
			"oncompletetarget",
			gameObject,
			"easetype",
			"easeOutQuad"));
	}

	public IEnumerator NewGame()
	{
		mainCamera.SendMessage("FollowPlayer", true);

		shipDoor.SetTrigger("Open");

		yield return new WaitForSeconds(1f);

		//mainSpawner.GoDown();
		mainSpawner.StartCoroutine("GoDown");

		myGUICtrl.NewGame();

		UpdateGUI();

		CharRespawn();
		//charDead = true;
	}

	//-------------------------------//
	//-----------UPDATE
	//-------------------------------//

	private void Update()
	{
		if (titleScreen)
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				Debug.Log("Start!");
				StartCamera();
				titleScreen = false;
			}
		}

		if (charDead && !gameOver)
		{
			if (Input.anyKeyDown)
				CharRespawn();
		}

		if (askGOver)
		{
			if (Input.GetKeyDown("y"))
			{
				myGUICtrl.guiOver.text = "MISSION COMPLETED";
				StartCoroutine("GameOver");
				shipFixed = true;
				//achievement FIXED SHIP
			}

			if (Input.GetKeyDown("n"))
				CancelGameOver();
		}

		if (gameOver && canRestart)
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				Application.LoadLevel(0);
			}
		}

		if (Input.GetKeyDown("p"))
		{
			if (!gamePaused)
			{
				myGUICtrl.NewText("Game Paused");
				myGUICtrl.SaveGame();
				PauseGame();
			}
			else
			{
				UnpauseGame();
				myGUICtrl.HideText();
			}
		}
	}

	//-------------------------------//
	//-----------DEATH
	//-------------------------------//

	public void CharIsDead()
	{
		charDead = true;

		livesCount--;

		//crystalCount = 0;

		UpdateGUI();

		if (livesCount > 0)
		{
			myChar.SendMessage("CanRespawn");
			myGUICtrl.DeadRespawn();
		}
		else
		{
			StartCoroutine("GameOver");
			Debug.Log("Game Over!");
		}
	}

	public void CharRespawn()
	{
		if (livesCount <= 0)
			return;

		charDead = false;

		myChar.SendMessage("Spawn");

		myGUICtrl.HideText();

		LowerSoundTrack(0.7f);

		//yield return 0;

		if (killerEnemy != null)
		{
			killerEnemy.SendMessage("BackToNormal");
			killerEnemy = null;
		}
	}

	public void KilledChar(GameObject enemy)
	{
		killerEnemy = enemy;
	}

	public IEnumerator GameOver()
	{
		if (askGOver)
		{
			myGUICtrl.HideText();
			mainSpawner.GoUp();
			askGOver = false;
		}

		myChar.SendMessage("StopChar");
		myChar.SendMessage("LightOff");

		charDead = true;
		gameOver = true;

		UnpauseGame();

		myGUICtrl.timing = false;

		Debug.Log("Game Over");

		yield return new WaitForSeconds(2f);

		mainCamera.SendMessage("FollowPlayer", false);

		//myGUICtrl.GameOver();
		myGUICtrl.StartCoroutine("GameOver");

		yield return new WaitForSeconds(3f);

		canRestart = true;
	}

	public void CancelGameOver()
	{
		askGOver = false;
		myGUICtrl.HideText();
		UnpauseGame();
	}

	public void KilledEnemy()
	{
		enemiesKilled++;
	}

	//-------------------------------//
	//-----------MUSIC
	//-------------------------------//

	public IEnumerator LowerSoundTrack(float sdVol)
	{
		if (sdVol < soundTrack.volume)
		{
			while (soundTrack.volume > sdVol)
			{
				soundTrack.volume -= 0.004f;
				yield return 0;
			}
		}
		else
		{
			while (soundTrack.volume < sdVol)
			{
				soundTrack.volume += 0.004f;
				yield return 0;
			}
		}
	}

	//-------------------------------//
	//-----------GUI
	//-------------------------------//

	public void AddCrystal(int n, bool state)
	{
		crystalCount += n;

		if (state)
			//crystalTotal += n;

			UpdateGUI();
	}

	public void UpdateGUI()
	{
		myGUICtrl.ChangeLives(livesCount);
		myGUICtrl.ChangeCrystals(crystalCount);
	}

	public IEnumerator AskGameOver()
	{
		if (!reactorCollected)
		{
			myGUICtrl.NewText("WE CAN'T LEAVE WITHOUT THE REACTOR");

			yield return new WaitForSeconds(2f);

			myGUICtrl.HideText();
		}
		else
		{
			myGUICtrl.NewText("ARE YOU READY TO LEAVE AND END THE MISSION? ( Y / N )");
			askGOver = true;
			PauseGame();
		}
	}

	public void PauseGame()
	{
		gamePaused = true;
		soundTrack.Pause();
		Time.timeScale = 0;
	}

	public void UnpauseGame()
	{
		gamePaused = false;
		soundTrack.Play();
		Time.timeScale = 1;
	}
}