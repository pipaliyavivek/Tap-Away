//----------------------------------------------------------------------------------
// This is main script. Create window with specified parameters and  elements bucket
//----------------------------------------------------------------------------------

// Make the script also execute in edit mode. It's better to comment this string before release
//[ExecuteInEditMode]

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Aligment in global space
public enum Aligments { none, center_center, center_up, center_down, left_center, left_up, left_down, right_center, right_up, right_down, center_x, center_y, left, right, up, down} 
// Determines  animation at first appearance
public enum StartAnimation { none, move_from_right, move_from_left, move_from_top, move_from_bottom } 
// Basic actions related for windows
public enum Action 
{ 
	none, 						// none
	close, 						// Close  current window
	close_GoToNextWindow,  		// Close current and open window with next index in MenuManager script
	close_GoToPreviousWindow, 	// Close current and open window with previous index in MenuManager script
	GoToNextWindow, 			// Open window with next index in MenuManager script
	GoToPreviousWindow, 		// Open window with previous index in MenuManager script
	GoToWindow, 				// Open window with parameterFloat index in MenuManager script
	close_GoToWindow,           // Close current and open window with parameterFloat index in MenuManager script
	close_MenuManager			// Close/disable whole menu manager and all related menus. 
} 


// Make the script also execute in edit mode. It's better to comment this string before release
//[ExecuteInEditMode]
public class MenuWindow : MonoBehaviour 
{

	public string caption;					// Displayed caption of element
	public Vector2 size;					// Size of Window
	public Vector2 scrollViewSize;			// Size of scollable area inside
	public Vector2 position;				// Determines element position if it isn't preset by globalAligment
	public Aligments globalAligment;  		// DElement aligment in global screen space
	public StartAnimation startAnimation;	// Determines window animation at first appearance
	public float animationSpeed = 10;		// Animation speed
	public GUISkin skin;					// GUI skin, if it isn't  specified - will be used Skin of parent element or default
	public Texture icon;					// To show near/instead of caption
	public bool draggable = false;			// Will be  window  dragable or not
	public bool autoAdjustSize = true;		// Size of window and all elements will be adjusted according to screen resolution
	public AudioClip interactionSound;   	// Plays this sound after any interaction (like button pressing) with any elements from Elements array
											// Please ensure that AudioListener component is attached
	public MenuElement[] Elements;			// Bunch of elements in this window


	// Usefull internal variables
	int index;						// Local windows index. SHOULD BE UNIQUE!
	Action actionToPerform;
	Vector2 currentPosition;
	MenuManager parentElement;
	float parameterFloat=-1;
	Vector2 scrollPosition = Vector2.zero;
	Rect windowRect;
	StartAnimation initialAnimation;
	Vector2 screenSizeMultiplier = new Vector2 (0,0);


	//========================================================================================================
	public void SetParent (MenuManager parent) 
	{
		parentElement = parent;
	}


	//----------------------------------------------------------------------------------
	public void ChangeActive () 
	{
		this.enabled = !this.enabled;
	}

	//----------------------------------------------------------------------------------
	public void SetIndex (int newID) 
	{
		index = newID;
	}

	//----------------------------------------------------------------------------------
	public void SetScreenSizeMultiplier (Vector2 newScreenSizeMultiplier) 
	{
		if (autoAdjustSize) 
		{
			screenSizeMultiplier = newScreenSizeMultiplier;

			if (screenSizeMultiplier != Vector2.zero)
			{
				size.x *= screenSizeMultiplier.x;
				size.y *= screenSizeMultiplier.y;
			}
		}

	}
	//----------------------------------------------------------------------------------
	void Start () 
	{
	//	yield WaitForEndOfFrame();

		if (Elements.Length>0)
			for (var i=0; i<Elements.Length; i++)  
			{
				Elements[i].SetParent(this);
				Elements[i].Init(screenSizeMultiplier);
			}

		animationSpeed *=100;
	}

	//----------------------------------------------------------------------------------
	// Align element and  setup start position/animation
	void OnEnable () 
	{
		initialAnimation = startAnimation;

		if (Elements.Length>0)
			for (var i=0; i<Elements.Length; i++)  
				Elements[i].PrepareAnimations();

		switch (globalAligment)
		{
		case Aligments.center_center: 
			position.x = (Screen.width-size.x)/2;
			position.y = (Screen.height-size.y)/2;
			break;

		case Aligments.center_up: 
			position.x = (Screen.width-size.x)/2;
			position.y = 0;
			break;  

		case Aligments.center_down: 
			position.x = (Screen.width-size.x)/2;
			position.y = Screen.height-size.y;
			break;   

		case Aligments.left_center: 
			position.x = 0;
			position.y = (Screen.height-size.y)/2;
			break;   

		case Aligments.left_up: 
			position.x = 0;
			position.y = 0;
			break; 

		case Aligments.left_down: 
			position.x = 0;
			position.y = Screen.height-size.y;
			break; 

		case Aligments.right_center: 
			position.x = Screen.width-size.x;
			position.y = (Screen.height-size.y)/2;
			break;   

		case Aligments.right_up: 
			position.x = Screen.width-size.x;
			position.y = 0;
			break; 

		case Aligments.right_down: 
			position.x = Screen.width-size.x;
			position.y = Screen.height-size.y;
			break; 

		case Aligments.left: 
			position.x = 0;
			break; 

		case Aligments.center_y: 
			position.x = (Screen.height-size.y)/2;
			position.y = (Screen.height-size.y)/2;
			break;   

		case Aligments.center_x: 
			position.x = (Screen.width-size.x)/2;
			break;  

		case Aligments.right: 
			position.x = Screen.width-size.x;
			break; 

		case Aligments.down: 
			position.y = Screen.height-size.y;
			break; 

		case Aligments.up: 
			position.y = 0;
			break;       
		}


		switch (initialAnimation)
		{
		case StartAnimation.none:
			currentPosition = position;
			break;

		case StartAnimation.move_from_left:
			currentPosition = new Vector2(0-size.x, position.y);
			break;

		case StartAnimation.move_from_right:
			currentPosition = new Vector2(Screen.width+size.x, position.y);
			break;

		case StartAnimation.move_from_top:
			currentPosition = new Vector2(position.x, 0-size.y);
			break;

		case StartAnimation.move_from_bottom:
			currentPosition = new Vector2(position.x, Screen.height+size.y);
			break;
		}


		if (scrollViewSize == Vector2.zero)   scrollViewSize = size;

		windowRect = new Rect( currentPosition.x, currentPosition.y, size.x, size.y) ;



	}


	//----------------------------------------------------------------------------------
	// Animate element if animation specified
	void Update () 
	{

		if (initialAnimation!=StartAnimation.none)
		{
			if (Time.timeScale == 0) currentPosition = position;
			else
				switch (initialAnimation)
			{
			case StartAnimation.move_from_left:
				currentPosition.x += Time.deltaTime * animationSpeed;
				if  (currentPosition.x >= position.x) 
				{
					initialAnimation=StartAnimation.none;
					currentPosition = position;
				}
				break;

			case StartAnimation.move_from_right:
				currentPosition.x -= Time.deltaTime * animationSpeed;
				if  (currentPosition.x <= position.x)  
				{
					initialAnimation=StartAnimation.none;
					currentPosition = position;
				}
				break;

			case StartAnimation.move_from_top:
				currentPosition.y += Time.deltaTime * animationSpeed;
				if  (currentPosition.y >= position.y)  
				{
					initialAnimation=StartAnimation.none;
					currentPosition = position;
				}
				break;

			case StartAnimation.move_from_bottom:
				currentPosition.y -= Time.deltaTime * animationSpeed;
				if  (currentPosition.y <= position.y)  
				{
					initialAnimation=StartAnimation.none;
					currentPosition = position;
				}
				break;

			}
			windowRect = new Rect( currentPosition.x, currentPosition.y, size.x, size.y) ;
		}


		if (Elements.Length>0)
			for (var i=0; i<Elements.Length; i++) Elements[i].Update();

		if (actionToPerform!=Action.none) 
		if(parentElement) 
		{
			parentElement.SetAction(actionToPerform);
			//actionToPerform = Action.none;
		}

		if (actionToPerform == Action.close) 
		if(!parentElement) 
		{
			this.enabled = false;
			actionToPerform = Action.none;
		}


	}
	//----------------------------------------------------------------------------------
	// Set window  action and/or paramenter for it
	public void SetAction (Action action, float param)
	{
		actionToPerform = action;
		parameterFloat = param;
		if (GetComponent<AudioSource>()) 
			GetComponent<AudioSource>().PlayOneShot(interactionSound);
		else 
			Debug.Log ("If you want to have  sound feedback - Please ensure that AudioSource component is attached to " + gameObject.name);
	}

	public void SetAction (Action action)
	{
		actionToPerform = action;
		if (GetComponent<AudioSource>())
			GetComponent<AudioSource>().PlayOneShot(interactionSound);
	}

	//----------------------------------------------------------------------------------
	// Get window  action and/or paramenter for it
	public Action GetAction ()
	{
		return actionToPerform;
	}

	public float GetActionParameter ()
	{
		return parameterFloat;
	}

	//----------------------------------------------------------------------------------
	// Draw window
	void OnGUI () 
	{
		if(skin) GUI.skin = skin;

		windowRect = GUI.Window (index, windowRect, WindowFunction, caption);

	}

	//----------------------------------------------------------------------------------
	// Process all elements inside window
	void WindowFunction (int windowID ) 
	{
		if (icon) 
			GUI.DrawTexture(new Rect (0, 0, 30, 30), icon, ScaleMode.ScaleToFit, true);

		scrollPosition = GUI.BeginScrollView (new Rect (0, 0, size.x, size.y), scrollPosition, new Rect (0, 0, scrollViewSize.x, scrollViewSize.y));

		if (Elements.Length>0)
			for (int i=0; i<Elements.Length; i++) 
				Elements[i].OnGUI();

		GUI.EndScrollView ();

		// Make the windows be draggable.
		if (draggable) 
			GUI.DragWindow ();         
	}


}
