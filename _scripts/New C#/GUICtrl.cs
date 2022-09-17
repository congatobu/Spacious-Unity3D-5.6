using System.Collections;
using UnityEngine;

public class GUICtrl : MonoBehaviour
{
	public GUITexture blackScreen;
	private CharCtrl charCTRL;

	public TextMesh[] credits;

	public float fuelMeter = 1.0f;
	private float fuelWaste = -0.0001f;

	private bool gameOver;

	public bool gameSaved;
	public TextMesh guiCrystals;
	public Transform guiFuel;
	public GameObject guiHealth;

	public Transform guiLayout;
	public TextMesh guiLive;
	public TextMesh guiOver;
	public TextMesh guiTexter;
	public TextMesh guiTimer;
	public TextMesh guiTitle;
	public GameObject[] healthBars;
	public int healthCount;

	public float hours;
	private MainCtrl mainCTRL;
	public int maxHealth;
	public float minutes;
	public float savedTime;
	private GUIScore scoreCTRL;
	public float seconds;

	public float startTime;
	public string textTimer;
	public float theTime;
	public bool timing;

	public float loadingTimer;

	private void Awake()
	{
		if (mainCTRL == null)
			mainCTRL = GameObject.Find("MainCtrl").GetComponent<MainCtrl>();

		if (charCTRL == null)
			charCTRL = GameObject.Find("Character").GetComponent<CharCtrl>();

		scoreCTRL = GetComponent<GUIScore>();
		/*
		//make the gui text show in front of everythig
		guiLive.GetComponent<Renderer>().sortingLayerID = 7;
		guiCrystals.GetComponent<Renderer>().sortingLayerID = 7;
		guiTimer.GetComponent<Renderer>().sortingLayerID = 7;
		guiTexter.GetComponent<Renderer>().sortingLayerID = 7;
		guiTitle.GetComponent<Renderer>().sortingLayerID = 7;
		guiOver.GetComponent<Renderer>().sortingLayerID = 7;
		
		for (var i = 0; i < credits.Length; i++)
		{
			var cre = credits[i];
			cre.GetComponent<Renderer>().sortingLayerID = 7;
		}*/

		guiOver.GetComponent<Renderer>().enabled = false;

		iTween.Init(gameObject);

		Color blackScreenColor = blackScreen.color;
		blackScreenColor.a = 1.0f;

		//blackScreen.color.a = 1.0f;
		blackScreen.pixelInset = new Rect(0, 0, Screen.width, Screen.height);

		//guiTexter.color.a = 0;
		Color guiTexterColor = guiTexter.color;
		guiTexterColor.a = 0;
	}

	private void Start()
	{
		//CreateHealth();
		StartCoroutine("CreateHealth");
		ShowGUI(false);

		//temp to delete all saves
		//PlayerPrefs.DeleteAll();


		if (PlayerPrefsX.GetBool("saved"))
		{
			Debug.Log("Loading game");
			gameSaved = PlayerPrefsX.GetBool("saved");
			LoadGame();
		}

		///--LOAGIND--//
		NewText("LOADING");
		loadingTimer = 1f;
	}

	private void FixedUpdate()
	{
		if (charCTRL.jetOn)
		{
			if (fuelMeter > 0)
			{
				fuelMeter += fuelWaste;
				guiFuel.localScale = new Vector3(fuelMeter, 1, 1);
			}
		}

		if (fuelMeter <= 0 && !gameOver)
		{
			gameOver = true;
			//mainCTRL.GameOver();
			mainCTRL.StartCoroutine("GameOver");
		}
	}

	private void Update()
	{
		if (timing)
		{
			theTime = Time.time + savedTime - startTime;
			hours = Mathf.Floor(theTime/3600);
			minutes = theTime/60 - 60*hours;
			seconds = theTime%60;

			textTimer = string.Format("{0:0}:{1:00}:{2:00}", hours, minutes, seconds);
			guiTimer.text = textTimer;
		}

		if (loadingTimer != 2)
		{
			loadingTimer -= Time.deltaTime;
			//Debug.Log("LOADING: " + loadingTimer + "%");
		}
		               
		if (loadingTimer <= 0)
		{
			loadingTimer = 2f;
			Debug.Log("LOADING: " + "100%");
			NewText("PRESS 'SPACE' TO START");
		}
	}

	public void TitleScreen()
	{
		//FadeIn(0);
		StartCoroutine("FadeIn", 0);

		//show title
		//ShowCredits(true);

		//show message "press space to start"
		NewText("PRESS 'SPACE' TO START");

		//show Credits
	}

	public void NewGame()
	{
		//show normal HUD
		ShowGUI(true);

		//start timer
		startTime = Time.time;
		timing = true;
	}

	public IEnumerator GameOver()
	{
		if (timing)
			timing = false;

		//FadeOut(0.5f);
		StartCoroutine("FadeOut", 0.5f);
		ShowGUI(false);
		//show game over message
		guiOver.GetComponent<Renderer>().enabled = true;

		yield return new WaitForSeconds(3f);
		//show score
		ShowScore();
	}

	public void ShowScore()
	{
		scoreCTRL.SetScore();
		PlayerPrefs.DeleteAll();
		NewText("PRESS 'SPACE' TO RESTART");
	}

	public void DeadRespawn()
	{
		//press any button to spawn a new clone
		NewText("PRESS ANY KEY TO SPAWN A NEW CLONE");
	}

	public void ChangeLives(int newLive)
	{
		guiLive.text = newLive.ToString();
	}

	public void ChangeCrystals(int newCrystal)
	{
		guiCrystals.text = newCrystal.ToString();
	}

	public void ChangeHealth(int newHealth)
	{
		healthCount = newHealth;

		for (var i = 0; i < healthBars.Length; i++)
		{
			var hBar = healthBars[i];
			if (i < newHealth)
				hBar.GetComponent<Renderer>().enabled = true;
			else
				hBar.GetComponent<Renderer>().enabled = false;
		}
	}

	public void NewText(string newT)
	{
		guiTexter.text = newT;
		Color guiTexterColor = guiTexter.color;
		guiTexterColor.a = 255;
	}

	public void HideText()
	{
		Color guiTexterColor = guiTexter.color;
		guiTexterColor.a = 0;

		guiTexter.text = "";
	}

	public void ShowGUI(bool state)
	{
		foreach (Transform child in guiLayout.transform)
			child.gameObject.SetActive(state);

		guiLayout.gameObject.SetActive(state);
	}

	public IEnumerator CreateHealth()
	{
		if (maxHealth == 0)
			Debug.Log("Falha HEALTH");

		healthBars = new GameObject[maxHealth];

		healthCount = maxHealth;

		for (var i = 0; i < maxHealth; i++)
		{
			var newBar = Instantiate(guiHealth, guiHealth.transform.position, Quaternion.identity);
			//newBar.transform.position.x += i*0.15f;
			newBar.transform.position = new Vector3(newBar.transform.position.x + i * 0.15f, newBar.transform.position.y, newBar.transform.position.z);
			newBar.transform.parent = guiLayout;
			healthBars[i] = newBar;
			yield return 0;
		}

		Destroy(guiHealth.gameObject);
	}

	public void ShowCredits(bool state)
	{
		for (var i = 0; i < credits.Length; i++)
		{
			var cre = credits[i];
			cre.GetComponent<Renderer>().enabled = state;
		}
	}

	public IEnumerator FadeIn(float fade)
	{
		//blackScreen.color.a = 1;
		while (blackScreen.color.a > fade)
		{
			//blackScreen.color.a -= 0.005f;
			Color newColor = blackScreen.color;
			newColor.a -= 0.005f;
			yield return 0;
		}
		Color newColor1 = blackScreen.color;
		newColor1.a = fade;
	}

	public IEnumerator FadeOut(float fade)
	{
		//blackScreen.color.a = 1;
		while (blackScreen.color.a < fade)
		{
			Color newColor = blackScreen.color;
			newColor.a -= 0.005f;
			yield return 0;
		}
		Color newColor1 = blackScreen.color;
		newColor1.a = fade;
	}

	//-------------------------------//
	//-----------LOAD / SAVE
	//-------------------------------//

	public void SaveGame()
	{
		PlayerPrefsX.SetBool("saved", true);
		PlayerPrefs.SetInt("clones", mainCTRL.livesCount);
		PlayerPrefs.SetInt("crystals", mainCTRL.crystalCount);
		PlayerPrefs.SetInt("health", healthCount);
		PlayerPrefs.SetFloat("timer", theTime);
		PlayerPrefs.SetFloat("fuel", fuelMeter);
		PlayerPrefsX.SetBool("reactorCol", mainCTRL.reactorCollected);

		if (!charCTRL.charDead)
			PlayerPrefsX.SetVector3("playerPos", charCTRL.gameObject.transform.position);
		else
			PlayerPrefsX.SetVector3("playerPos", charCTRL.spawner.transform.position);

		NotificationCenter.DefaultCenter().PostNotification(gameObject, "SaveGame");

		PlayerPrefs.Save();

		Debug.Log("Game Saved");
	}

	public void LoadGame()
	{
		Debug.Log("Loading");

		mainCTRL.livesCount = PlayerPrefs.GetInt("clones");
		mainCTRL.crystalCount = PlayerPrefs.GetInt("crystals");
		//fuel
		fuelMeter = PlayerPrefs.GetFloat("fuel");
		savedTime = PlayerPrefs.GetFloat("timer");

		Debug.Log("GUI loaded");
	}
}