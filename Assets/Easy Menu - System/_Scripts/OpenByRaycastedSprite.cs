//----------------------------------------------------------------------------------------------------------------------------------
// Script should be assigned to active object
// If this object contains MenuWindow script - everything will work already. 
// Or you can control another object (that contains MenuWindow script) - it should be assigned to MenuObject. 
// This object should be active, but attached MenuWindow script should be disabled
//----------------------------------------------------------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class OpenByRaycastedSprite : MonoBehaviour 
{

	// Use MenuManager type instead MenuWindow of if  you want  to operate  with whole menu system on scene
	public MenuWindow MenuObject;  
	public Collider2D triggerObject;
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

	//-----------------------------------------------------------------
	void Update () 
	{
		if (Input.GetMouseButtonDown(0)) 
			RaycastSprite ();

	}

	//-----------------------------------------------------------------
	void RaycastSprite ()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit2D hit = Physics2D.GetRayIntersection(ray,Mathf.Infinity);

		if(hit.collider == triggerObject) 
			OpenCloseMenu ();

	}

	//-----------------------------------------------------------------						
	void OpenCloseMenu () 
	{
		if (MenuObject)  
			MenuObject.enabled = !MenuObject.enabled;
		else
			Debug.Log ("Sorry but there is no MenuWindow script attached to current object and no assigned to ", MenuObject);

		if (pauseGame)
			if (MenuObject.enabled) 
				Time.timeScale = 0; else Time.timeScale = 1;

	}

	//-----------------------------------------------------------------
}
