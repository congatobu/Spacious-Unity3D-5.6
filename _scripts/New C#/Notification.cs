using UnityEngine;
using System.Collections;

public class Notification : MonoBehaviour {
	
	public GameObject sender;
	public string message;
	public Hashtable data;
	
	public Notification(GameObject pSender, string pMessage)
	{
		sender = pSender;
		message = pMessage;
	}
	public Notification(GameObject pSender, string pMessage, Hashtable pData)
	{
		sender = pSender;
		message = pMessage;
		data = pData;
	}
}
