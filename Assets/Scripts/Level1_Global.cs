﻿using UnityEngine;
using System.Collections;

public class Level1_Global : MonoBehaviour {

	public GameObject bubble;
	public GameObject g;
	
	public OVRCameraController CameraController = null;
	private Camera MainCam;
	
	public Vector3 direction;
	public Vector3 startPosition;
	
	// Health and stamina stats
	public int currentHealth;
	public int maxHealth;
	public int currentStamina;
	public int maxStamina;
	
	// Power-Ups
	public bool storedHealthPU;
	public bool storedStaminaPU;
	
	// Bubbles
	public int bubblesLeft;
	public int maxBubbles;
	public float bubbleRegenTimer;
	
	// SetOVRCameraController
	public void SetOVRCameraController(ref OVRCameraController cameraController)
	{
		CameraController = cameraController;
		CameraController.GetCamera(ref MainCam);
	}
	
	// Use this for initialization
	void Start () {
	    
		g = GameObject.Find("Gilly");
		SetOVRCameraController(ref CameraController);
		
		currentHealth = 100;
		maxHealth = 100;
		currentStamina = 100;
		maxStamina = 100;
		
		storedHealthPU = false;
		storedStaminaPU = false;
		
		maxBubbles = 5;
		bubblesLeft = maxBubbles;
	}
	
	// Update is called once per frame
	void Update () {
	   
		// Game over condition
		if(currentHealth <= 0 )
		{
			Debug.Log("Game Over!");
			
			// Restart from last checkpoint or return to main menu
		}
		
		//Debug.Log(MainCam.transform.rotation.eulerAngles.ToString());
		if(Input.GetKeyDown(KeyCode.P))
		{
			startPosition= g.transform.position;
			Vector3 startPos = MainCam.transform.position;
			direction = Vector3.forward;
		    direction = MainCam.transform.rotation * direction;
			//direction.Normalize();
			//Debug.Log("bullet direction is " + direction.ToString());
			Vector3 dir = direction;
			dir.Normalize();
			Vector3 offset = dir*25.0f;
			startPosition = startPosition + offset;
		    Instantiate(bubble, startPos + offset, Quaternion.identity) ;	
		}	
	}
}
