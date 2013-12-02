﻿using UnityEngine;
using System.Collections;

// Vimu bhai check

static class Constants {
	
	// Bubbles
    public const int MAX_BUBBLES = 5;
	public const float SHOOT_TIME = 0.25f;
	public const float BUBBLE_LIFE_TIME = 5.0f;
	public const float BUBBLE_REGEN_TIME = 3.0f;
	public const float BUBBLE_SPAWN_OFFSET = 1.5f;
	public const float BUBBLE_FORCE = 500.0f;
	
	// Wii Controller rumble;
	public const bool WII_RUMBLE = true;
	
	// Particle effects
	public const float BUBBLE_PARTICLE_LIFE_TIME = 30.0f;
	public const float BUBBLE_PARTICLE_SPAWN_TIMER = 2.0f;
	
	// Health
	public const int HEARTBEAT_HEALTH = 30;
	public const int HEALTH_REWARD = 10;
	
	// Stamina
	public const float STAMINA_DEC_RATE = 0.1f;
	public const float STAMINA_REGEN_TIME = 1.0f;
	public const float STAMINA_REGEN_AMT = 2.0f;
	public const int STAMINA_REWARD = 10;
	
	//Used in Navigation Script 
	public const int MOVE_SPEED = 2;
	
	// Image Effects
	public const bool BLOOD_SPLATTER_TOGGLE = false;
	
	// Obstacles
	public const float SM_OBSTACLE_MOVE_MAGNITUDE = 25.0f;
	public const float LG_OBSTACLE_MOVE_MAGNITUDE = 5.0f;
	public const float OBSTACLE_DESTROY_TIMER = 30.0f;
	
	// Probabilites
	public const float SM_OBSTACLE_PROB = 0.7f;
	public const float LG_OBSTACLE_PROB = 0.3f;
	public const float STAMINA_PROB = 0.6f;
	public const float HEALTH_PROB = 0.4f;
	
	// Power ups
	public const float POWERUP_SPAWN_TIME = 15.0f;
	
	// Achievements
//	public const string[] LEVEL1_ACH = new string[] {"Shoot and destroy 25 small obstacles (Level1)",
//												     "Complete the level without using any health power-ups (Level1)",
//												     "Complete the level while selecting the industrial pipes at each junction (Level1)",
//												     "Find and collect the bonus item (Level1)"};
//	
//	// Achievement Rewards (H = Max health boost [+10], S = Max stamina boost [+10], HS = Both)
//	public const string[] LEVEL1_ACH_REWARDS = new string[] {"S", "H", "HS", "HS"};
	
	// Debug - Cheat Codes
	public const bool UNLIMITED_HEALTH  = false;
	public const bool UNLIMITED_STAMINA = false;
	public const bool UNLIMITED_BUBBLES = true;
}

public class Level1_Global : MonoBehaviour {

	public GameObject bubble;
	public bool gameOver ;
	public Texture fadeTexture;
	
	// Player controller
	public GameObject player;
	
	// navAgents
	public GameObject navAgent;
	public GameObject obsNavAgent;
	
	public OVRCameraController CameraController = null;
	private Camera MainCam;
	
	private UniWiiCheck uniWii;
	
	private Level1_Audio audioScript;
	
	public Vector3 direction;
	public Vector3 startPosition;
	
	// Health and stamina stats
	public int currentHealth;
	public int maxHealth;
	public float currentStamina;
	public float maxStamina;
	public float staminaRegenTimer;
	
	// Power-Ups
	public bool storedHealthPU;
	public bool storedStaminaPU;
	
	// Bubbles
	public float shootTimer;
	public bool shootEnabled;
	public int bubblesLeft;
	public float bubbleRegenTimer;
	
	// Score & Timer
	public int score;
	public float startTime;
	public float timer;
	
	// Spawn point array
	public GameObject[] spawnPositions;
	
	// Branch difficulty
	public int branchDiff;
	
	// SetOVRCameraController
	public void SetOVRCameraController(ref OVRCameraController cameraController)
	{
		CameraController = cameraController;
		CameraController.GetCamera(ref MainCam);
	}
	
	// Use this for initialization
	void Start () {
	    gameOver = false;
		// Set camera controller
		SetOVRCameraController(ref CameraController);
		
		uniWii = gameObject.GetComponent<UniWiiCheck>();
		audioScript = gameObject.GetComponent<Level1_Audio>();
		
		currentHealth = 100;
		maxHealth = 100;
		currentStamina = 100.0f;
		maxStamina = 100.0f;
		
		storedHealthPU = false;
		storedStaminaPU = false;
		
		bubblesLeft = Constants.MAX_BUBBLES;
		bubbleRegenTimer = Constants.BUBBLE_REGEN_TIME;
		staminaRegenTimer = Constants.STAMINA_REGEN_TIME;
		shootTimer = Constants.SHOOT_TIME;
		shootEnabled = true;
		
		// Default setting = easy
		branchDiff = 0;
		
		score = 0;
		startTime = Time.time;
		
		// Ignore appropriate collisions
		setUpPhysics();
	}
	
	void Awake() {
		
		// Select a random spawn position
		int pos = (int) Random.Range(0, spawnPositions.Length);
		setSpawnPoint(pos);
	}
	
	// Update is called once per frame
	void Update () {
		
		// Update timer
		timer = Time.time - startTime;
		
		// Game over condition
		if(currentHealth <= 0)
		{
			gameOver = true;
			Application.LoadLevel ("GameOverScene");
			
			// Restart from last checkpoint or return to main menu
			// Application.LoadLevel("GameOverScene");
		}
		
		if(currentStamina <= 0)
			currentStamina = 0;
		
		if(currentStamina < maxStamina)
		{
			staminaRegenTimer -= Time.deltaTime;
			if(staminaRegenTimer <= 0)
			{
				currentStamina += Constants.STAMINA_REGEN_AMT;
				staminaRegenTimer = Constants.STAMINA_REGEN_TIME;
			}
		}
		
		if(bubblesLeft < Constants.MAX_BUBBLES)
		{
			bubbleRegenTimer -= Time.deltaTime;
			if(bubbleRegenTimer <= 0)
			{
				// Add a bubble and reset timer
				bubblesLeft++;
				bubbleRegenTimer = Constants.BUBBLE_REGEN_TIME;
			}
		}
		
		if(shootEnabled == false)
		{
			shootTimer -= Time.deltaTime;
			if(shootTimer <= 0)
			{
				shootTimer = Constants.SHOOT_TIME;
				shootEnabled = true;
			}
		}
		
		// Shoot bubble
		if(uniWii.wiiCount > 1 && uniWii.buttonBPressed[1])
		{
			if(shootEnabled == true)
			{
				shootBubble();
				shootEnabled = false;
			}
		}
		
		// Keyboard input
		if(Input.GetKeyDown(KeyCode.P))
			shootBubble();
		
		if(Input.GetKeyDown(KeyCode.U))
			useHealthPowerUp();
		
		if(Input.GetKeyDown(KeyCode.I))
			useStaminaPowerUp();
		
		// Wii inputs
		// Use health power-up
		if(uniWii.wiiCount > 1 && uniWii.buttonPlusPressed[1])
			useHealthPowerUp();
		
		// Use stamina power-up
		if(uniWii.wiiCount > 1 && uniWii.buttonMinusPressed[1])
			useStaminaPowerUp();
	}
	
	void setSpawnPoint(int pos)
	{
		SpawnPositionScript s = spawnPositions[pos].GetComponent<SpawnPositionScript>();
		
	 	//GameObject navObj, obsNavObj;
	 	//NavMeshAgent nav, obsNav;
	
		//navObj = GameObject.Find("NavAgent");
		//obsNavObj = GameObject.Find("SpawnNavAgent");
	    //nav = navObj.GetComponent<NavMeshAgent>();
		//obsNav = obsNavObj.GetComponent<NavMeshAgent>();
		
		// Place the player at the spawn position
		//Instantiate(player, spawnPositions[pos].transform.position, Quaternion.identity); 
		player.transform.position = spawnPositions[pos].transform.position;
		
		// Place the navAgents at the appropriate positions
		GameObject na = (GameObject) Instantiate(navAgent, s.navAgentPos.transform.position, Quaternion.identity) as GameObject;
		na.GetComponent<NavMeshAgent>().SetDestination(s.endPoint.transform.position);
		
		GameObject ona = (GameObject) Instantiate(obsNavAgent, s.obstacleAgentPos.transform.position, Quaternion.identity);
		ona.GetComponent<NavMeshAgent>().SetDestination(s.endPoint.transform.position);
		
		//Debug.Log(s.destination);
		//navObj.transform.position = s.navAgentPos.transform.position;
		//obsNavObj.transform.position = s.obstacleAgentPos.transform.position;
	}
	
	void shootBubble()
	{
		if(bubblesLeft > 0)
		{		
			Vector3 startPos = MainCam.transform.position;
			//Debug.Log(startPos);
			direction = Vector3.forward;
		    direction = MainCam.transform.rotation * direction;
			
			Vector3 dir = direction;
			dir.Normalize();
			Vector3 offset = dir * Constants.BUBBLE_SPAWN_OFFSET;
		    
			// Create bubble
			Instantiate(bubble, startPos + offset, Quaternion.identity);
			
			// Play sound
			AudioSource.PlayClipAtPoint(audioScript.bubblePopSound, startPos + offset);
				
			// Update bubbles left counter
			if(Constants.UNLIMITED_BUBBLES == false)
				bubblesLeft--;
		}
	}
	
	void useHealthPowerUp()
	{
		if(storedHealthPU == true)
		{
			currentHealth += 25;
				
			if(currentHealth > 100)
				currentHealth = 100;
				
			storedHealthPU = false;
		}	
	}
	
	void useStaminaPowerUp()
	{
		if(storedStaminaPU == true)
		{
			currentStamina += 25;
				
			if(currentStamina > 100)
				currentStamina = 100;
				
			storedStaminaPU = false;
		}
	}
	
//	void onGUI()
//	{
//		if(gameOver == true)
//		{
//			GUI.color = Color.black;
//			GUI.color.a = (float)Mathf.Lerp(1.0f,0.0f,(Time.time - startTime));
//			
//			GUI.DrawTexture (new Rect(0,0,Screen.width,Screen.height),fadeTexture);
//			
//		
//			
//		}
//		
//	}
	
	void setUpPhysics() {
	
		Physics.IgnoreLayerCollision(10, 10, true);
		Physics.IgnoreLayerCollision(10, 11, true);
		Physics.IgnoreLayerCollision(10, 12, true);
		Physics.IgnoreLayerCollision(10, 13, true);
		Physics.IgnoreLayerCollision(11, 11, true);
		Physics.IgnoreLayerCollision(11, 12, true);
		Physics.IgnoreLayerCollision(11, 13, true);
		Physics.IgnoreLayerCollision(12, 12, true);
		Physics.IgnoreLayerCollision(12, 13, true);
		Physics.IgnoreLayerCollision(13, 13, true);
	}
	
}
