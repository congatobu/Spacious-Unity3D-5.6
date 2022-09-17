using UnityEngine;
using System.Collections;

public class SecretPath : MonoBehaviour
{
	public bool pathHidden = true;

	private void Start()
	{
	}

	private void Update()
	{
	}

	// Quando o char entrar em contato com o colider entao sumir com todos os filhos
	private void OnTriggerEnter2D(Collider2D myCol)
	{
		if (myCol.gameObject.tag == "Player")
		{
			if (!pathHidden)
				return;

			foreach (Transform child in transform)
			{
				var chRenderer = child.gameObject.GetComponent<SpriteRenderer>();
				chRenderer.enabled = false;
			}

			pathHidden = false;
		}
	}

	private void OnTriggerExit2D(Collider2D myCol)
	{
		if (myCol.gameObject.tag == "Player")
		{
			StartCoroutine("TriggerExit");
		}
	}

	private IEnumerator TriggerExit()
	{
		// char saiu do caminho
		pathHidden = true;

		//Delay para esconder novamente
		yield return new WaitForSeconds(3f);

		//se o Char voltou ao caminho entao cancelar
		if (!pathHidden)
			yield break;

		foreach (Transform child in transform)
		{
			var chRenderer = child.gameObject.GetComponent<SpriteRenderer>();
			chRenderer.enabled = true;
		}

		pathHidden = true;
	}
}