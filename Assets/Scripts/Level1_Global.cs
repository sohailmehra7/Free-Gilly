using UnityEngine;
using System.Collections;

static class Constants {
	
	// Bubbles
    public const int MAX_BUBBLES = 5;
	public const float SHOOT_TIME = 0.25f;
	public const float BUBBLE_LIFE_TIME = 7.0f;
	public const float BUBBLE_REGEN_TIME = 3.0f;
	public const float BUBBLE_SPAWN_OFFSET = 1.5f;
	public const float BUBBLE_FORCE = 500.0f;
	
	// Movement Speed
	public const float DEFAULT_FLOW_SPEED = 3.5f;
	public const float DEFAULT_OBSAGENT_SPEED = 10.0f;
	public const float THRUST_SPEED = 20.0f;
	public const float SLOW_SPEED = 2.0f;
	public const float DROP_SPEED = 35.0f;
	
	// Wii Controller rumble;
	public const bool WII_RUMBLE = true;
	
	// Particle effects
	public const float BUBBLE_PARTICLE_LIFE_TIME = 120.0f;
	public const float BUBBLE_PARTICLE_SPAWN_TIMER = 1.5f;
	
	// Health
	public const int HEARTBEAT_HEALTH = 30;
	public const int HEALTH_REWARD = 10;
	public const int MAX_HEALTH = 100;
	
	// Stamina
	public const float STAMINA_DEC_RATE = 0.1f;
	public const float STAMINA_REGEN_TIME = 1.0f;
	public const float STAMINA_REGEN_AMT = 1.0f;
	public const int STAMINA_REWARD = 10;
	public const int MAX_STAMINA = 100;
	
	//Used in Navigation Script 
	public const int MOVE_SPEED = 2;
	
	// Image Effects
	public const bool BLOOD_SPLATTER_TOGGLE = false;
	
	// Obstacles
	public const float SM_OBSTACLE_MOVE_MAGNITUDE = 25.0f;
	public const float LG_OBSTACLE_MOVE_MAGNITUDE = 5.0f;
	public const float OBSTACLE_DESTROY_TIMER = 120.0f;
	
	// Probabilites
	public const float SM_OBSTACLE_PROB = 0.7f;
	public const float LG_OBSTACLE_PROB = 0.3f;
	public const float STAMINA_PROB = 0.6f;
	public const float HEALTH_PROB = 0.4f;
	
	// Power ups
	public const float POWERUP_SPAWN_TIME = 15.0f;
	
	// Debug - Cheat Codes
	public const bool UNLIMITED_HEALTH  = false;
	public const bool UNLIMITED_STAMINA = false;
	public const bool UNLIMITED_BUBBLES = false;
}

public class Level1_Global : MonoBehaviour {

	public GameObject bubble;
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
	
	public int currentLevel = 1;
	
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
	
	// Loading Screen
	public Texture2D loadingScreen;
	public bool loading_on = true;
	
	// Achievements
	public string[] LEVEL1_ACH;
	public int[] LEVEL1_ACH_TRACKER;
	
	// Achievement-related variables
	public int smObsDestroyed;
	
	// Achievement Rewards (H = Max health boost [+10], S = Max stamina boost [+10], HS = Both)
	public string[] LEVEL1_ACH_REWARDS = new string[] {"S", "H", "HS", "HS"};
	
	// SetOVRCameraController
	public void SetOVRCameraController(ref OVRCameraController cameraController)
	{
		CameraController = cameraController;
		CameraController.GetCamera(ref MainCam);
	}
	
	// Use this for initialization
	void Start () {
		
		// Set camera controller
		SetOVRCameraController(ref CameraController);
		
		uniWii = gameObject.GetComponent<UniWiiCheck>();
		audioScript = gameObject.GetComponent<Level1_Audio>();
		
		currentHealth = 10;
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
		
		smObsDestroyed = 0;
		
		// Ignore appropriate collisions
		setUpPhysics();
		
		// Achievements
		LEVEL1_ACH = new string[] {	"Complete the level in under 10 minutes",
									"Shoot and destroy 25 small obstacles",
									"Complete the level without using any health power-ups",
									"Complete the level while selecting the industrial pipes at each junction"};
	
		LEVEL1_ACH_TRACKER = new int[] {1, 0, 1, 1};
		
		// Refresh PlayerPrefs
		PlayerPrefs.SetInt("Level", 1);
		PlayerPrefs.SetInt("Score", score);
		PlayerPrefsX.SetStringArray("AchievementList", LEVEL1_ACH);
		PlayerPrefsX.SetIntArray("AchievementTracker", LEVEL1_ACH_TRACKER);
	}
	
	void Awake() {
		
		// Select a random spawn position
		int pos = (int) Random.Range(0, spawnPositions.Length);
		setSpawnPoint(pos);
		
		// Play the game
		Time.timeScale = 1;
	}
	
	// Update is called once per frame
	void Update () {
		
		// Update timer
		timer = Time.time - startTime;
		
		// Game over condition
		if(currentHealth <= 0)
		{	
			// Fail the appropriate achievements
			for(int i=0; i<LEVEL1_ACH.Length; i++)
				LEVEL1_ACH_TRACKER[i] = 0;
			
			// Store data
			PlayerPrefs.SetInt("Level", currentLevel);
			PlayerPrefs.SetInt("Complete", 0);
			PlayerPrefs.SetInt("Score", score);
			PlayerPrefs.SetFloat("Time", timer);
			PlayerPrefsX.SetStringArray("AchievementList", LEVEL1_ACH);
			PlayerPrefsX.SetIntArray("AchievementTracker", LEVEL1_ACH_TRACKER);
			
			// Load Gameover scene
			Application.LoadLevel("GameOverScene");
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
		
		/////// Achievement checks ///////
		if(smObsDestroyed >= 25)
			LEVEL1_ACH_TRACKER[1] = 1;
		
		if(timer > 600.0f)
			LEVEL1_ACH_TRACKER[0] = 0;
		//////////////////////////////////
			
		// Keyboard input
		if(Input.GetKeyDown(KeyCode.P))
			shootBubble();
		
		if(Input.GetKeyDown(KeyCode.U))
			useHealthPowerUp();
		
		if(Input.GetKeyDown(KeyCode.I))
			useStaminaPowerUp();
		
		// Wii inputs
		// Use stamina power-up
		if((uniWii.wiiCount > 1) && (uniWii.buttonPlusPressed[0] || uniWii.buttonPlusPressed[1]))
			useStaminaPowerUp();
		
		// Use health power-up
		if((uniWii.wiiCount > 1) && (uniWii.buttonMinusPressed[0] || uniWii.buttonMinusPressed[1]))	
			useHealthPowerUp();
	}
	
	void OnGUI ()
	{
		if(loading_on)
		{
       		GUI.matrix.SetTRS(Vector3.zero, Quaternion.identity, new Vector3(1.0f*Screen.width/1.0f, 1.0f*Screen.height/1.0f, 1.0f));
       		GUI.depth = -10;
       		GUI.Box(new Rect(0,0,1024,768), loadingScreen);
    	}
    	if(!Application.isLoadingLevel)
       		loading_on = false;
	}
	
	void setSpawnPoint(int pos)
	{
		SpawnPositionScript s = spawnPositions[pos].GetComponent<SpawnPositionScript>();
		
		// Place the player at the spawn position
		//Instantiate(player, spawnPositions[pos].transform.position, Quaternion.identity); 
		player.transform.position = spawnPositions[pos].transform.position;
		
		// Place the navAgents at the appropriate positions
		GameObject na = (GameObject) Instantiate(navAgent, s.navAgentPos.transform.position, Quaternion.identity) as GameObject;
		na.GetComponent<NavMeshAgent>().SetDestination(s.endPoint.transform.position);
		
		GameObject ona = (GameObject) Instantiate(obsNavAgent, s.obstacleAgentPos.transform.position, Quaternion.identity);
		ona.GetComponent<NavMeshAgent>().SetDestination(s.endPoint.transform.position);
	}
	
	void shootBubble()
	{
		if(bubblesLeft > 0)
		{		
			Vector3 startPos = MainCam.transform.position;
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
			
			// Update achievement tracker
			LEVEL1_ACH_TRACKER[1] = 0;
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
