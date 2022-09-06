//----------------------------------------------------------------------------------
//  Atomic class of menu elements
//  All basic functionality integrated already
//----------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class MenuElement 
{
	// Type of element determines it appearnce and functionality. You can extend it as you want
	public enum ElementTypes 
	{ 
		button_CloseGoTo,  	// Create button that closes current menu window and opens window with index parameterFloat in MenuManager script
		button_GoTo,       	// Create button that opens window with index parameterFloat in MenuManager script
		button_CloseBack,  	// Create button that closes current menu window and opens window with previous index in MenuManager script
		button_CloseNext,  	// Create button that closes current menu window and opens window with next index in MenuManager script
		button_Close,
		button_CloseEverything,  // Close/disable whole menu manager and all related menus.
		button_Back, 		// Create button that opens window with previous index in MenuManager script
		button_Next, 		// Create button that opens window with next index in MenuManager script
		button_ExitGame,   	// Create button that close application
		button_LoadLevel,  	// Create button that load level with index parameterFloat
		button_SetQuality, 	// Create button that set quality level according to parameter (Fastest, Fast, ... Fantastic)
		button_DecQuality, 	// Create button that decrease quality level 
		button_IncQuality, 	// Create button that increase quality level  
		button_Resume,     	// Create button that close current menu and set time-scale to 1
		button_Restart,    	// Create button that restart current level
		button_OpenURL,
		button_SendMessage,
		label,             	// Create text label  
		textArea,
		image,
		stars,
		scroll_Resolutions,	// Create scroll with list of all avaiable resolutions. Click will change gurrent resolution to choosen one
		toggle_Fullscreen, 	// Create toggle that turn on/off fullscreen mode
		toggle_Sound,
		toggle_InvertMouse, 
		slider_MouseSens,  	// Create slider that can be used for Mouse sensitivity adjustment
		slider_masterBrightness, 
		slider_masterVolume
	} 


	public string caption;					// Displayed caption of element
	public Texture icon;					// To show near/instead of caption
	public ElementTypes type;				// Type of element 
	public Vector2 size;					// Element size
	public Aligments globalAligment;		// Element aligment in parent space
	public Vector2 position;				// Determines element position if it isn't preset by globalAligment
	public StartAnimation startAnimation;   // Determines element animation at first appearance
	public float animationSpeed = 10;		// Animation speed
	public GUISkin skin;					// GUI skin, if it isn't  specified - will be used Skin of parent element
	public string parameter;				// Additional string parameter, should be  specified  for  some  types of elements
	public float parameterFloat;			// Additional float parameter, should be  specified  for  some  types of elements
	public bool locked= false;				// Lock element if true. I.e. player can't use it. If lockImage assigned - also draw it above
	public Texture lockImage;				// Image that indicate locked element
	//public GameObject menuButtons;
	//public GameObject optionsMenu;
	//public GameObject noSaveDialog;
	//public string level;
	//public bool isOptions = false;
	//public bool isSaveDialog = false;

	MenuWindow parentElement;
	float ambientLight;
	Vector2 parentSize;
	Vector2 currentPosition;
	Vector2 scrollPosition  = Vector2.zero;
	bool Fullscreen_toggleBool ;
	float MouseSens = 1;
	StartAnimation initialAnimation;
	bool beenClicked = false;
	//AudioListener masterVolume;
	float oldMasterVolume;
	bool isMouseInverted;
	int MouseInverted = 0;

	//========================================================================================================
	public void SetParent (MenuWindow parent) 
	{
		parentElement = parent;
	}


	//----------------------------------------------------------------------------------
	// Preparing element to be animated on enabling
	public void PrepareAnimations () 
	{
		initialAnimation = startAnimation;

		switch (initialAnimation)
		{
			case StartAnimation.none:
				currentPosition = position;
				break;

			case StartAnimation.move_from_left:
				currentPosition = new Vector2(0-size.x, position.y);
				break;

			case StartAnimation.move_from_right:
				currentPosition = new Vector2(parentSize.x+size.x, position.y);
				break;

			case StartAnimation.move_from_top:
				currentPosition = new Vector2(position.x, 0-size.y);
				break;

			case StartAnimation.move_from_bottom:
				currentPosition = new Vector2(position.x, parentSize.y+size.y);
				break;
			}
	}


	//----------------------------------------------------------------------------------
	// Align element and  setup start position/animation
	public void Init (Vector2 screenSizeMultiplier) 
	{
		if (!parentElement)
		{
			parentSize.x = Screen.width;
			parentSize.y = Screen.height;
		}
		else
			parentSize = parentElement.size;


		if (screenSizeMultiplier != Vector2.zero)
		{
			size.x *= screenSizeMultiplier.x;
			size.y *= screenSizeMultiplier.y;

			position.x *= screenSizeMultiplier.x;
			position.y *= screenSizeMultiplier.y;
		}


		switch (globalAligment)
		{
			case Aligments.center_center: 
				position.x = (parentSize.x-size.x)/2;
				position.y = (parentSize.y-size.y)/2;
				break;
			case Aligments.center_up: 
				position.x = (parentSize.x-size.x)/2;
				position.y = 0;
				break;  
			case Aligments.center_down: 
				position.x = (parentSize.x-size.x)/2;
				position.y = parentSize.y-size.y;
				break;   

			case Aligments.left_center: 
				position.x = 0;
				position.y = (parentSize.y-size.y)/2;
				break;   
			case Aligments.left_up: 
				position.x = 0;
				position.y = 0;
				break; 
			case Aligments.left_down: 
				position.x = 0;
				position.y = parentSize.y-size.y;
				break; 

			case Aligments.right_center: 
				position.x = parentSize.x-size.x;
				position.y = (parentSize.y-size.y)/2;
				break;   
			case Aligments.right_up: 
				position.x = parentSize.x-size.x;
				position.y = 0;
				break; 
			case Aligments.right_down: 
				position.x = parentSize.x-size.x;
				position.y = parentSize.y-size.y;
				break; 

			case Aligments.left: 
				position.x = 0;
				break; 

			case Aligments.center_y: 
				position.x = (parentSize.x-size.y)/2;
				position.y = (parentSize.y-size.y)/2;
				break;   

			case Aligments.center_x: 
				position.x = (parentSize.x-size.x)/2;
				break;  

			case Aligments.right: 
				position.x = parentSize.x-size.x;
				break; 

			case Aligments.down: 
				position.y = parentSize.y-size.y;
				break; 

			case Aligments.up: 
				position.y = 0;
				break;           
		}


		PrepareAnimations ();


		//masterVolume = GameObject.FindObjectOfType<AudioListener>();

		if (PlayerPrefs.HasKey("MouseSens"))  MouseSens = PlayerPrefs.GetFloat("MouseSens");
		else
			MouseSens = 1.01f;


		if (PlayerPrefs.HasKey("MouseInverted"))  MouseInverted = PlayerPrefs.GetInt("MouseInverted");
		else
			MouseInverted = 1;


		isMouseInverted = MouseInverted > 0 ? false : true;

		animationSpeed *=100;
	}

	//----------------------------------------------------------------------------------
	// Animate element if animation specified
	public void Update () 
	{
		if (Time.timeScale == 0) 
			currentPosition = position;
		else
			if (initialAnimation!=StartAnimation.none)
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

		/*if(isOptions)
		{
			optionsMenu.SetActive(false);
			menuButtons.SetActive(true);
			isOptions = false;
		}


		if(isSaveDialog)
		{
			noSaveDialog.SetActive(false);
			menuButtons.SetActive(true);
			isSaveDialog = false;
		}*/

	}

	//----------------------------------------------------------------------------------
	// Draw and handle  element according to it type
	public void OnGUI ()
	{

		if(skin) GUI.skin = skin;

		switch (type)
		{
			case ElementTypes.button_Close: 
				if (GUI.Button (new Rect (currentPosition.x, currentPosition.y, size.x, size.y), new GUIContent (caption, icon)))
				if (!locked) parentElement.SetAction (Action.close, parameterFloat);         
				break;

			case ElementTypes.button_CloseGoTo: 
				if (GUI.Button (new Rect (currentPosition.x, currentPosition.y, size.x, size.y), new GUIContent (caption, icon)))
				if (!locked) parentElement.SetAction (Action.close_GoToWindow, parameterFloat);

				break;

			case ElementTypes.button_GoTo: 
				if (GUI.Button (new Rect (currentPosition.x, currentPosition.y, size.x, size.y), new GUIContent (caption, icon)))
				if (!locked) parentElement.SetAction (Action.GoToWindow, parameterFloat);
				break;


			case ElementTypes.button_Back: 
				if (GUI.Button (new Rect (currentPosition.x, currentPosition.y, size.x, size.y), new GUIContent (caption, icon)))
				if (!locked) parentElement.SetAction (Action.GoToPreviousWindow);
				break;

			case ElementTypes.button_Next: 
				if (GUI.Button (new Rect (currentPosition.x, currentPosition.y, size.x, size.y), new GUIContent (caption, icon)))
				if (!locked) parentElement.SetAction (Action.GoToNextWindow);
				break;


			case ElementTypes.button_CloseBack: 
				if (GUI.Button (new Rect (currentPosition.x, currentPosition.y, size.x, size.y), new GUIContent (caption, icon)))
				if (!locked) parentElement.SetAction (Action.close_GoToPreviousWindow);
				break;

			case ElementTypes.button_CloseNext: 
				if (GUI.Button (new Rect (currentPosition.x, currentPosition.y, size.x, size.y), new GUIContent (caption, icon)))
				if (!locked) parentElement.SetAction (Action.close_GoToNextWindow);
				break; 

			case ElementTypes.button_CloseEverything: 
				if (GUI.Button (new Rect (currentPosition.x, currentPosition.y, size.x, size.y), new GUIContent (caption, icon)))
				if (!locked) 
				{ 
					parentElement.SetAction (Action.close_MenuManager);
				}
				break;  

			case ElementTypes.button_ExitGame: 
				if (GUI.Button (new Rect (currentPosition.x, currentPosition.y, size.x, size.y), new GUIContent (caption, icon)))
				if (!locked) Application.Quit();
				break; 

			case ElementTypes.button_LoadLevel: 
				if (GUI.Button (new Rect (currentPosition.x, currentPosition.y, size.x, size.y), new GUIContent (caption, icon)))
				if (!locked) 
				{
					if (parentElement.gameObject.GetComponent<AudioSource>()) 
					{
						beenClicked = true;
						parentElement.gameObject.GetComponent<AudioSource>().Play();
					}
					else
						SceneManager.LoadScene((int)parameterFloat);
				}

				if (parentElement)
					if (parentElement.gameObject.GetComponent<AudioSource>() && beenClicked)  
						if (!parentElement.gameObject.GetComponent<AudioSource>().isPlaying) 
							SceneManager.LoadScene((int)parameterFloat);
				break; 


			case ElementTypes.button_SetQuality: 
				if (GUI.Button (new Rect (currentPosition.x, currentPosition.y, size.x, size.y), new GUIContent (caption, icon)))
				if (!locked) 
				{
					QualitySettings.SetQualityLevel((int)parameterFloat);				
				}
				break;  


			case ElementTypes.button_IncQuality: 
				if (GUI.Button (new Rect (currentPosition.x, currentPosition.y, size.x, size.y), new GUIContent (caption, icon)))
				if (!locked)  QualitySettings.IncreaseLevel();
				break; 


			case ElementTypes.button_DecQuality: 
				if (GUI.Button (new Rect (currentPosition.x, currentPosition.y, size.x, size.y), new GUIContent (caption, icon)))
				if (!locked) QualitySettings.DecreaseLevel();
				break; 


			case ElementTypes.scroll_Resolutions: 
				if (!locked) 
				{
					if (parameterFloat <= 0)  parameterFloat = 20;
					Resolution[] resolutions  = Screen.resolutions;
					scrollPosition  = GUI.BeginScrollView (new Rect (currentPosition.x, currentPosition.y, size.x, size.y),
						scrollPosition, new Rect (0, 0, size.x*0.8f, resolutions.Length*parameterFloat));

					int i = 0;
					foreach (var res in resolutions) 
					{
						if (GUI.RepeatButton (new Rect (0, i*parameterFloat, size.x*0.9f, parameterFloat*1.1f), res.width + "x" + res.height + " " + res.refreshRate + "Hz")) 
						{
							Screen.SetResolution (res.width, res.height, Screen.fullScreen);
							SceneManager.LoadScene (SceneManager.GetActiveScene().name);							
						}

						i++;
					}
					GUI.EndScrollView();
				}
				break; 



			case ElementTypes.toggle_Fullscreen: 
				bool isFullScreen = Screen.fullScreen;
				Fullscreen_toggleBool  = GUI.Toggle (new Rect (currentPosition.x, currentPosition.y, size.x, size.y), isFullScreen, new GUIContent (caption, icon));
				if (!locked) 
				{ if (Fullscreen_toggleBool != isFullScreen) Screen.fullScreen = Fullscreen_toggleBool;}
				break; 


			case ElementTypes.slider_MouseSens:                                                           
				GUI.Label (new Rect (currentPosition.x, currentPosition.y, caption.Length*parameterFloat, size.y), new GUIContent (caption, icon));
				if (!locked)
				{
					MouseSens = GUI.HorizontalSlider(new Rect(currentPosition.x+caption.Length*parameterFloat, currentPosition.y, size.x - caption.Length*parameterFloat, size.y), MouseSens, 0.01f, 2.01f);
					if (MouseSens != 1)  PlayerPrefs.SetFloat("MouseSens", MouseSens);
				}		
				break;	                                                                                                                                                                                                                                                                                                                                        

			case ElementTypes.button_Resume: 
				if (GUI.Button (new Rect (currentPosition.x, currentPosition.y, size.x, size.y), new GUIContent (caption, icon)))
				if (!locked) 
				{ 
					Time.timeScale = 1;
					parentElement.SetAction (Action.close);
				}
				break; 

			case ElementTypes.button_Restart: 
				if (GUI.Button (new Rect (currentPosition.x, currentPosition.y, size.x, size.y), new GUIContent (caption, icon)))
				if (!locked) 
				{ 
					Time.timeScale = 1;
					SceneManager.LoadScene (SceneManager.GetActiveScene().name);	
				}
				break; 

			case ElementTypes.button_OpenURL: 
				if (GUI.Button (new Rect (currentPosition.x, currentPosition.y, size.x, size.y), new GUIContent (caption, icon)))
				if (!locked) 
				{ 
					Application.OpenURL(parameter);
				}
				break; 


			case ElementTypes.label: 
				GUI.Label (new Rect (currentPosition.x, currentPosition.y, size.x, size.y), new GUIContent (caption + parameter, icon)); 
				break; 


			case ElementTypes.stars: 
				for (int i = 0; i<parameterFloat; i++)
				{
					GUI.DrawTexture(new Rect((currentPosition.x-(parameterFloat*size.x*1.1f-size.x)/2)+i*size.x*1.1f, currentPosition.y, size.x, size.y), icon, ScaleMode.StretchToFill, true);
				}
				break;                      


			case ElementTypes.textArea: 
				scrollPosition  = GUI.BeginScrollView (new Rect (currentPosition.x, currentPosition.y, size.x, size.y), scrollPosition, new Rect (0, 0, size.x-18, size.y*parameterFloat));
				string textAreaString = GUI.TextArea (new Rect (0, 0, size.x-15, size.y*(parameterFloat+1)), parameter);   
				GUI.EndScrollView();

				if (!locked) 
				{ 
					parameter = textAreaString;
				}
				else
				{
					GUI.SetNextControlName ("none");
					GUI.FocusControl ("none");    
				}
				break; 


			case ElementTypes.image: 
				if (icon) GUI.DrawTexture(new Rect (currentPosition.x, currentPosition.y, size.x, size.y), icon, ScaleMode.StretchToFill, true);
				break;    


			case ElementTypes.toggle_Sound: 
					bool toggleState = GUI.Toggle (new Rect (currentPosition.x, currentPosition.y, size.x, size.y), (AudioListener.volume == 0), new GUIContent (caption, icon));
					if (!locked) 			   
					{
						Debug.Log(toggleState);
						if (toggleState)
						{
							if (AudioListener.volume > 0) oldMasterVolume = AudioListener.volume;
								AudioListener.volume = 0;
						}
						else
							if (AudioListener.volume == 0)  AudioListener.volume = oldMasterVolume;

					}
				break;


			case ElementTypes.slider_masterVolume:                                                           
				GUI.Label (new Rect (currentPosition.x, currentPosition.y, caption.Length*parameterFloat, size.y), new GUIContent (caption, icon));
				if (!locked)   
					AudioListener.volume  = GUI.HorizontalSlider(new Rect(currentPosition.x+caption.Length*parameterFloat, currentPosition.y, size.x - caption.Length*parameterFloat, size.y), AudioListener.volume , 0, 1 );		
				break;	  

			case ElementTypes.slider_masterBrightness:                                                          
				GUI.Label (new Rect (currentPosition.x, currentPosition.y, caption.Length*parameterFloat, size.y), new GUIContent (caption, icon));
				if (!locked)  
					ambientLight = GUI.HorizontalSlider(new Rect(currentPosition.x+caption.Length*parameterFloat, currentPosition.y, size.x - caption.Length*parameterFloat, size.y), ambientLight, 0f, 0.2f);
				RenderSettings.ambientLight = new Color(ambientLight, ambientLight, ambientLight, 0.5f);                         
				break;  



			case ElementTypes.toggle_InvertMouse: 
				isMouseInverted = GUI.Toggle (new Rect (currentPosition.x, currentPosition.y, size.x, size.y), isMouseInverted, new GUIContent (caption, icon));

				if (!locked)   
				if (MouseInverted != 0)  PlayerPrefs.SetInt("MouseInverted", isMouseInverted ? -1 : 1);

				break;

			case ElementTypes.button_SendMessage:
				if (GUI.Button (new Rect (currentPosition.x, currentPosition.y, size.x, size.y), new GUIContent (caption, icon)))
				if (!locked) 
				{ 
					parentElement.gameObject.SendMessage(parameter, parameterFloat, SendMessageOptions.DontRequireReceiver);
				}
				break;                            
		}

		if (locked) 
		if (lockImage) GUI.DrawTexture(new Rect (currentPosition.x, currentPosition.y, size.x, size.y), lockImage, ScaleMode.ScaleToFit, true);

	}

	//----------------------------------------------------------------------------------
	// Lock or unlock element
	public void Locked (bool state)
	{
		locked = state;

	}

	//----------------------------------------------------------------------------------
}
