using System.Collections;
using UnityEngine;

public class CharHealth : MonoBehaviour
{
	public int charHealth = 5;
	public bool dead;
	public int maxHealth = 5;
	public GUICtrl myGUICtrl;
	public string[] hurtTexts = new[] { "Ouch!", "", "Ai", "That hurt", "", "Son of a...", "..." };

	private void Awake()
	{
		myGUICtrl = GameObject.Find("GUI").GetComponent<GUICtrl>();
		myGUICtrl.maxHealth = maxHealth;
	}

	private void Start()
	{
		//yield return 0;

		if (myGUICtrl.gameSaved)
			charHealth = PlayerPrefs.GetInt("health");
		else
			charHealth = maxHealth;
	}

	public void TakeDamage(int damage)
	{
		charHealth -= damage;

		SendMessage("Hurt");

		if (charHealth <= 0 && !dead)
			FallingDie();
		else
		{
			SendMessage("ShowCharText", hurtTexts[Random.Range(0, hurtTexts.Length - 1)]);
			UpdateGUI();

			if (charHealth == 1)
				//FunText();
				StartCoroutine("FunText");
		}
	}

	public void FallingDie()
	{
		SendMessage("DieAnim");
		//yield return 0;
		Death();
	}

	public void Death()
	{
		//Debug.Log("DIED");
		dead = true;
		charHealth = 0;

		UpdateGUI();

		//matar jogador
		SendMessage("KillChar");
	}

	public void UpdateGUI()
	{
		myGUICtrl.ChangeHealth(charHealth);
		//Debug.Log(charHealth);
	}

	public void RenewHealth()
	{
		if (charHealth <= 0)
			charHealth = maxHealth;

		UpdateGUI();
		dead = false;
	}

	public IEnumerator FunText()
	{
		yield return new WaitForSeconds(1f);

		SendMessage("ShowCharText", "If you want to recharge the health bar \n you can wait for 30 minutes...");
		yield return 0;
		SendMessage("ShowCharText", "...or you can buy it for only 1.99f");
		yield return 0;
		SendMessage("ShowCharText", "hahaha Just kidding. \n There is no way to recover it.");
	}
}