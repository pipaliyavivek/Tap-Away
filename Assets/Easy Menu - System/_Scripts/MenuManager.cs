//----------------------------------------------------------------------------------
// Global script for window management. Can be abandoned if you have only one menu window
//----------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour 
{


	public MenuWindow[] windows;		// List of all windows
	public int activeWindow;			// Start/current window index
	public bool autoIndex = false; 		// All windows will be indexed automatically according to their  order in windows array
	public Vector2 defaultScreenSize = new Vector2 (800,480); // Default size of Screen. Size of all windows (and their elements)
																// will be adjusted according to it. IF windows autoAdjustSize = true 

	Vector2 screenSizeMultiplier = new Vector2 (0,0);
	Action actionToPerform;
	int lastActive = -1;


	//========================================================================================================
	// Init all windows
	void Start () 
	{
		Time.timeScale = 1;

		screenSizeMultiplier.x = Screen.width/defaultScreenSize.x;
		screenSizeMultiplier.y = Screen.height/defaultScreenSize.y;

		if (windows.Length>0)
		{
			for (int i=0; i < windows.Length; i++)  
			{
				windows[i].SetParent(this);
				windows[i].enabled = false;
				windows[i].SetIndex(i);
				windows[i].SetScreenSizeMultiplier(screenSizeMultiplier);
			}

			if(activeWindow >= 0)  windows[activeWindow].enabled = true;
		}

		// lastActive = activeWindow;
	}


	//----------------------------------------------------------------------------------
	// Process actions sended by windows 
	void Update ()
	{

		if (actionToPerform!=Action.none)
		{

			if (windows.Length > 0)
				for (int i=0; i<windows.Length; i++)  
					if (windows[i].GetAction() != Action.none) 
					{
						actionToPerform =  windows[i].GetAction();
						windows[i].SetAction(Action.none);
						lastActive = i;
						break;
					}


			int WinParam;    

			switch (actionToPerform)
			{
				case Action.close:
					windows[lastActive].enabled = false;
					break;

				case Action.close_GoToWindow:
					WinParam = (int)windows[lastActive].GetActionParameter();
					windows[lastActive].enabled = false;
					if (windows.Length >= WinParam) 
					{
						windows[WinParam].enabled = true;
						activeWindow = WinParam;
					}
					break;


				case Action.GoToWindow:
					WinParam = (int)windows[lastActive].GetActionParameter();

					if (windows.Length >= WinParam) 
					{
						windows[WinParam].enabled = true;
						activeWindow = WinParam;
					}
					break;


				case Action.close_GoToNextWindow:
					windows[lastActive].enabled = false;
					if (windows.Length >= lastActive+1) 
					{
						windows[lastActive+1].enabled = true;
						activeWindow = lastActive+1;
					}
					break;

				case Action.close_GoToPreviousWindow:
					windows[lastActive].enabled = false;
					if (lastActive-1 >= 0) 
					{
						windows[lastActive-1].enabled = true;	 
						activeWindow = lastActive-1;
					}
					break;

				case Action.GoToNextWindow:
					if (windows.Length >= lastActive+1) 
					{
						windows[lastActive+1].enabled = true;
						activeWindow = lastActive+1;
					}
					break;

				case Action.GoToPreviousWindow:
					if (lastActive-1 >= 0) 
					{
						windows[lastActive-1].enabled = true;	 
						activeWindow = lastActive-1;
					}
					break;

				case Action.close_MenuManager:

					if (windows.Length>0)
						for (int i=0; i<windows.Length; i++)  
							windows[i].enabled = false;

					this.enabled = false;

					break;
				}


			actionToPerform = Action.none;

		}

	}

	//----------------------------------------------------------------------------------
	void OnEnable () 
	{
		if (activeWindow >= 0  &&  windows[activeWindow]) 
			windows[activeWindow].enabled = true;
	}


	void OnDisable () 
	{
		if (activeWindow >= 0  &&  windows[activeWindow]) 
			windows[activeWindow].enabled = false;
	}

	//----------------------------------------------------------------------------------
	// Functions  for set/get local action variable
	public void SetAction (Action _action) 
	{
		actionToPerform = _action;
	}

	Action GetAction ()
	{
		return actionToPerform;
	}

	//----------------------------------------------------------------------------------
}
