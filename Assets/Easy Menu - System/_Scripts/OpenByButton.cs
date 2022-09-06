//----------------------------------------------------------------------------------------------------------------------------------
// Script should be assigned to active object
// If this object contains MenuWindow script - everything will work already. 
// Or you can control another object (that contains MenuWindow script) - it should be assigned to MenuObject. 
// This object should be active, but attached MenuWindow script should be disabled
//----------------------------------------------------------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenByButton : MonoBehaviour
{
	// Use MenuManager type instead MenuWindow of if  you want  to operate  with whole menu system on scene
	public MenuWindow MenuObject;
	public KeyCode buttonCode = KeyCode.Escape;
	public bool pauseGame = true;

	//=================================================================
	void Start () 
	{
		if(!MenuObject) 
			MenuObject = gameObject.GetComponent<MenuWindow>();
		
		if (MenuObject) 
			MenuObject.enabled = false;
		else
			Debug.Log ("Sorry but there is no MenuWindow script attached to current object and no assigned to ", MenuObject);  

	}

	//----------------------------------------------------------------------------------------------------------------------------------
	void Update () 
	{
		if (Input.GetKeyUp(buttonCode))
		{

			if (MenuObject)  
				MenuObject.enabled = !MenuObject.enabled;
			else
				Debug.Log ("Sorry but there's no MenuWindow script attached to current object and no assigned to ", MenuObject);

			if (pauseGame)
				if (MenuObject.enabled)
				
			Time.timeScale = 0; else Time.timeScale = 1;
		}
	}

	//----------------------------------------------------------------------------------------------------------------------------------

}
