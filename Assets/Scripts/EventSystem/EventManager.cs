using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using IUserInterface;

public class EventManager : MonoBehaviour {

	#region enum
	//-------------------------------------------
	//enum defining all possible game events
    public enum EVENT_TYPE
	{
		FSM_ENTER,
		FSM_EXIT,
		FSM_UPDATE,
		Props
	}
	
	#endregion
	
 

	#region variables
    //-------------------------------------------
	//singleton design pattern
	private static EventManager instance = null;
	
	//Array of listener object
	public Dictionary<EVENT_TYPE,List<OnEvent>> Listeners = new Dictionary<EVENT_TYPE, List<OnEvent>>();

	//Defining a delegate type for events
	public delegate void OnEvent(EVENT_TYPE Event_Type, Component Sender = null, object Param = null);
	
    #endregion
	
	
		
    #region Properties Setting
	//-------------------------------------------
    //public access to instace
    public static EventManager Instance
	{
		get { return instance; }
	}

	#endregion
	
	

	private void Awake()
	{
		//If no instance exists,then assign this instance
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(this);
		}
		else
		{
			DestroyImmediate(this);
		}
	}
	//-------------------------------------------

	
	
	/// <summary>
	/// Add listener_object to array of listeners
	/// </summary>
	/// <param name="Event_Type">Event to Listen</param>
	/// <param name="Listener">Object to Listen for event</param>
	public void AddListener(EVENT_TYPE Event_Type, OnEvent Listener)
	{
		List<OnEvent> ListenerList = null;
		if (Listeners.TryGetValue(Event_Type, out ListenerList))
		{
			ListenerList.Add(Listener);
			return;
		}
		
		ListenerList = new List<OnEvent>();
		ListenerList.Add(Listener);
		Listeners.Add(Event_Type, ListenerList);
	}
	//-------------------------------------------
	
	

	/// <summary>
	///Funiction to post event to listeners 
	/// </summary>
	/// <param name="Event_Type">event to invoke</param>
	/// <param name="Sender">Object of invokeing event</param>
	/// <param name="Param">Optional argument</param>
	public void PostNotification(EVENT_TYPE Event_Type, Component Sender = null, object Param = null)
	{
		List<OnEvent> ListenerList = null;
		
		//If no entry exists, then exit
		if (!Listeners.TryGetValue(Event_Type, out ListenerList))
		{
			return;
		}

		for (int i = 0; i < ListenerList.Count; i++)
		{
			if (!ListenerList[i].Equals(null))
				ListenerList[i](Event_Type, Sender, Param);
		}
	}
	//-------------------------------------------
	
	
	

}
