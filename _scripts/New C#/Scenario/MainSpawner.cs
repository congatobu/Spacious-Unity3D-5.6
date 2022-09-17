using System.Collections;
using UnityEngine;

public class MainSpawner : MonoBehaviour
{
	public Transform doorPos;
	public SpriteRenderer guiInfo;
	public TextMesh guiMission;
	public SpriteRenderer guiReactor;
	public bool guiShown = true;
	public AudioSource mySound;
	public GameObject mySprite;
	public BoxCollider2D spriteCol;

	private void Awake()
	{
		spriteCol = mySprite.GetComponent<BoxCollider2D>();
		mySprite.GetComponent<Animation>()["spawner"].wrapMode = WrapMode.Loop;
		mySound = GetComponent<AudioSource>();

		transform.position = doorPos.position;

		iTween.Init(gameObject);

		mySprite.GetComponent<Renderer>().enabled = false;

		//guiMission.GetComponent<Renderer>().sortingLayerID = 7;

		guiReactor.GetComponent<Animation>()["reactor_pulse"].wrapMode = WrapMode.Loop;

		GuiShow(false);
	}

	private void Start()
	{
		if (PlayerPrefsX.GetBool("saved"))
		{
			guiShown = false;
			GuiShow(false);
		}
	}

	private void Spawning()
	{
		mySound.Play();
	}

	private void Collect()
	{
		gameObject.tag = "Untagged";
	}

	public IEnumerator GoDown()
	{
		mySprite.GetComponent<Renderer>().enabled = true;
		yield return new WaitForSeconds(1f);
		//iTween.MoveTo(gameObject, "y", 0.7f, "time", 5.0f, "ignoretimescale", true);
		iTween.MoveTo(gameObject, iTween.Hash ("y", 0.7f, "time", 5.0f, "ignoretimescale", true));
		GuiShow(true);
	}

	public void GoUp()
	{
		iTween.MoveTo(gameObject, iTween.Hash ("y", doorPos.position.y, "time", 5.0f, "ignoretimescale", true));
		GuiShow(false);
	}

	private void OnTriggerEnter2D(Collider2D charCol)
	{
		if (charCol.gameObject.tag == "Player")
		{
			charCol.SendMessage("Touching", "MainSpawner");
			if (!guiShown)
			{
				GuiShow(true);
				guiShown = true;
			}
		}
	}

	private void OnTriggerExit2D(Collider2D charCol)
	{
		if (charCol.gameObject.tag == "Player")
		{
			charCol.SendMessage("Touching", "");
			if (guiShown)
			{
				GuiShow(false);
				guiShown = false;
			}
		}
	}

	private void GuiShow(bool show)
	{
		/*
		guiInfo.color.a = show;
		guiMission.color.a = show;
		guiReactor.color.a = show;
		*/

		guiInfo.enabled = show;
		guiMission.gameObject.SetActive(show);
		guiReactor.enabled = show;
	}
}