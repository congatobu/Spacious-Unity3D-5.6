using UnityEngine;
using System.Collections;

public class NotificationCenter : MonoBehaviour {
	
	private static NotificationCenter defaultCenter;
	private Hashtable notifications = new Hashtable();
	
	public static NotificationCenter DefaultCenter()
	{
		if(!defaultCenter)
		{
			GameObject notificationObject = new GameObject("DefaultNotificationCenter");
			notificationObject.AddComponent<NotificationCenter>();
			//notificationObject.AddComponent("NotificationCenter");
			defaultCenter = notificationObject.GetComponent<NotificationCenter>();
		}		  
		return defaultCenter; 
	}
	
	//==============
	// ADD OBSERVERS
	//==============
	public void AddObserver(GameObject pObserver, string pMessage)
	{
		AddObserver(pObserver, pMessage, null);
	}
	public void AddObserver(GameObject pObserver, string pMessage, GameObject pSender)
	{
		//verificando se o nome é válido
		if( pMessage == null || pMessage == "") { Debug.Log("Null name specified for notification in AddObserver."); return; }
		
		//verificando a existencia da mensagem
		if(notifications[pMessage] == null) {notifications[pMessage] = new ArrayList();}
		
		ArrayList notifyList = (ArrayList)notifications[pMessage];
		if(!notifyList.Contains(pObserver)) {notifyList.Add(pObserver);}
	}
	
	//================
	//REMOVE OBSERVERS
	//=================
	public void RemoveObserver(GameObject pObserver, string pMessage)
	{
		ArrayList notifyList = (ArrayList)notifications[pMessage];
		if(notifyList != null)
		{
			if(notifyList.Contains(pObserver)) {notifyList.Remove(pObserver); }
			if(notifyList.Count == 0) { notifications.Remove(pObserver); }
		}
	}
	
	//==================
	//POST NOTIFICATIONS
	//==================
	public void PostNotification(GameObject pSender, string pMessage){PostNotification(pSender, pMessage, null); } 
	public void PostNotification(GameObject pSender, string pMessage, Hashtable pData)
	{
		this.gameObject.AddComponent<Notification>();
		Notification notification = this.gameObject.GetComponent<Notification>();
		
		notification.message = pMessage;
		notification.sender = pSender;
		notification.data = pData;
		
		PostNotification(notification);
		
		Destroy(notification);
	}
	public void PostNotification(Notification pNotification)
	{
		//validando message
		if(pNotification.message == null || pNotification.message == "") {print("Null message sent to Notification."); return; }
		
		//validando notify list
		ArrayList notifyList = (ArrayList)notifications[pNotification.message];
		if(notifyList == null) { Debug.Log("Notify list not found in PostNotification."); return; }
		
		ArrayList observersToRemove = new ArrayList();
		
		foreach(GameObject observer in notifyList)
		{
			if(!observer)
			{
				observersToRemove.Add(observer);
			}else{
				observer.SendMessage(pNotification.message, pNotification, SendMessageOptions.DontRequireReceiver);
			}
		}
		foreach(GameObject observer in observersToRemove)
		{
			notifyList.Remove(observer);
		}
	}
}
