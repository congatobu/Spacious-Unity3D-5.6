using UnityEngine;

public class GUIScore : MonoBehaviour
{
	private GUICtrl guiCTRL;
	private MainCtrl mainCTRL;
	public TextMesh scClones;
	public TextMesh scCrystals;
	public Transform scFuel;

	public Transform scoreLayout;
	public TextMesh scPoints;
	public TextMesh scTime;

	private void Awake()
	{
		if (mainCTRL == null)
			mainCTRL = GameObject.Find("MainCtrl").GetComponent<MainCtrl>();

		guiCTRL = GetComponent<GUICtrl>();

		scClones.GetComponent<MeshRenderer>().sortingLayerName = "Floor";
		scCrystals.GetComponent<MeshRenderer>().sortingLayerName = "Floor";
		scPoints.GetComponent<MeshRenderer>().sortingLayerName = "Floor";
		scTime.GetComponent<MeshRenderer>().sortingLayerName = "Floor";

		//make the gui text show in front of everythig
		scClones.GetComponent<MeshRenderer>().sortingLayerID = 0;
		scCrystals.GetComponent<MeshRenderer>().sortingLayerID = 0;
		scPoints.GetComponent<MeshRenderer>().sortingLayerID = 0;
		scTime.GetComponent<MeshRenderer>().sortingLayerID = 0;
	}

	private void Start()
	{
		ShowScoreGUI(false);
	}

	public void SetScore()
	{
		scTime.text = guiCTRL.textTimer;
		scClones.text = (mainCTRL.maxLives - mainCTRL.livesCount).ToString();
		scFuel.localScale = new Vector3(1 - guiCTRL.fuelMeter, 1, 1);
		scCrystals.text = mainCTRL.crystalCount.ToString();
		scPoints.text = SetPoints().ToString();
		ShowScoreGUI(true);
	}

	private void Update()
	{
	}

	private int SetPoints()
	{
		var ptCrystals = mainCTRL.crystalCount*15;
		var ptClones = mainCTRL.livesCount*10;
		var ptFuel = guiCTRL.fuelMeter*15;
		var ptTime = guiCTRL.minutes*0.7f;

		float totalPoints = Mathf.Floor(ptCrystals + ptClones + ptFuel - ptTime);

		if (mainCTRL.crystalCount == 0)
			totalPoints = 0;

		if (mainCTRL.reactorCollected)
			totalPoints += 50;

		//temp
		Debug.Log("time - " + guiCTRL.theTime);

		if (mainCTRL.shipFixed)
		{
			totalPoints += 100;
			Application.ExternalCall("kongregate.stats.submit", "Time", Mathf.Floor(guiCTRL.theTime));
			Application.ExternalCall("kongregate.stats.submit", "Fuel", Mathf.Floor(scFuel.localScale[0]*100));
			Application.ExternalCall("kongregate.stats.submit", "ShipFixed", 1);
		}

		Application.ExternalCall("kongregate.stats.submit", "Crystals", mainCTRL.crystalCount);

		Application.ExternalCall("kongregate.stats.submit", "Score", totalPoints);
		return (int)totalPoints;
	}

	private void ShowScoreGUI(bool state)
	{
		foreach (Transform child in scoreLayout.transform)
			child.gameObject.SetActive(state);

		scoreLayout.gameObject.SetActive(state);
	}
}