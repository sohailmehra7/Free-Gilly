using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;

public class Navigation : MonoBehaviour {
	
	[DllImport ("UniWii")]
	private static extern void wiimote_rumble(int which, float duration);
	
	private GameObject nav;
	private NavMeshAgent navObj;
	private NavMeshAgent obstacleNavObj;
	
	// OvrCameraControler
	private GameObject cameraController;
	
	private Level1_Global globalObj;
	private UniWiiCheck uniWii;
	private Level1_Audio audioScript;
	
	// Obstacles to spawn
	public GameObject SmallObstacleBolts;
	public GameObject SmallObstacleNuts;
	public GameObject SmallObstacleNewspaper;
	public GameObject SmallObstaclePaper;
	public GameObject SmallObstacleBatteries;
	public GameObject LargeObstacleRock1;
	public GameObject LargeObstacleRock2;
	public GameObject LargeObstacleRock3;
	public GameObject LargeObstacleRock4;
	public GameObject LargeObstacleRock5;
	
	// Powerups
	public GameObject healthPowerUp;
	public GameObject staminaPowerUp;
	
	// Particle effects
	public GameObject redParticles;
	public GameObject greenParticles;
	public GameObject bubbleParticles;
	
	// Image effects
	private ScreenOverlay[] so;
	private float overlayTimer;
	private bool overlayToggle;
	
	// Obstacles
	public Vector3 objSpawnPos;
	public float obstacleTimer;
	
	// Powerups
	public float powerUpSpawnTimer;
	
	// Bubble particles
	public float bubbleParticleSpawnTimer;
	
	// Grind
	public bool playGrindSound;
	public float grindTimer;
	
	// Use this for initialization
	void Start () {
		
		GameObject gl = GameObject.Find("Global");
		nav = GameObject.FindGameObjectWithTag("NavAgent");
		navObj = nav.GetComponent<NavMeshAgent>();
		globalObj = gl.GetComponent<Level1_Global>();
		uniWii = gl.GetComponent<UniWiiCheck>();
		obstacleNavObj = GameObject.FindGameObjectWithTag("ObsNavAgent").GetComponent<NavMeshAgent>();
		audioScript = gl.GetComponent<Level1_Audio>();
		
		cameraController = GameObject.Find("OVRCameraController");
		so = cameraController.GetComponentsInChildren<ScreenOverlay>();
		
		// Set the screen overlay parameters
		so[0].enabled = false;
		so[0].blendMode = ScreenOverlay.OverlayBlendMode.AlphaBlend;
		so[0].intensity = 1.0f;
		
		so[1].enabled = false;
		so[1].blendMode = ScreenOverlay.OverlayBlendMode.AlphaBlend;
		so[1].intensity = 1.0f;
		
		overlayTimer = 0.5f;
		overlayToggle = false;
		
		bubbleParticleSpawnTimer = Constants.BUBBLE_PARTICLE_SPAWN_TIMER;
		
		powerUpSpawnTimer = UnityEngine.Random.Range(15.0f, 20.0f);
		
		playGrindSound = false;
		grindTimer = 0.0f;
		
		// Easy
		if(globalObj.branchDiff == 0)
			obstacleTimer = UnityEngine.Random.Range(0.5f, 2.0f);
		
		// Hard
		else if(globalObj.branchDiff == 1)
			obstacleTimer = UnityEngine.Random.Range(0.5f, 1.0f);
		
		// Avoid collision between the navAgent and the obstacles 
		Physics.IgnoreLayerCollision(11, 20, true);
		Physics.IgnoreLayerCollision(10, 20, true);
		Physics.IgnoreLayerCollision(20, 20, true);
		
		Physics.IgnoreLayerCollision(17, 20, true);
	}
	
	// Update is called once per frame
	void Update () {
		
		// Blood splatter effect
		if(overlayToggle == true)
		{
			so[0].enabled = true;
			so[1].enabled = true;
			
			overlayTimer -= Time.deltaTime;
			if(overlayTimer <= 0.0f)
			{
				overlayTimer = 0.5f;
				overlayToggle = false;
				so[0].enabled = false;
				so[1].enabled = false;
			}
		}
		
		Vector3 ePos = nav.transform.position;
		Vector3 sPos = gameObject.transform.position;
		Vector3 flowDir = new Vector3(0,0,0);
		float distance = 0;
		
		NavMeshHit hit =  new NavMeshHit();
		
		objSpawnPos = obstacleNavObj.transform.position 
			+ new Vector3(UnityEngine.Random.Range(-0.25f, 0.25f), UnityEngine.Random.Range(-0.5f, 6.0f), 0.0f);
		
		obstacleTimer -= Time.deltaTime;
		bubbleParticleSpawnTimer -= Time.deltaTime;
		powerUpSpawnTimer -=Time.deltaTime;
		
		if(powerUpSpawnTimer <= 0.0f)
		{
			powerUpSpawnTimer = UnityEngine.Random.Range(15.0f, 20.0f);
			
			if(obstacleNavObj.remainingDistance >= 5.0f)
				InstantiatePowerUps();
		}
		
		if(obstacleTimer <= 0.0f)
		{
			// Easy
			if(globalObj.branchDiff == 0)
				obstacleTimer = UnityEngine.Random.Range(0.5f, 2.0f);
		
			// Hard
			else if(globalObj.branchDiff == 1)
				obstacleTimer = UnityEngine.Random.Range(0.5f, 1.0f);
		
			if(obstacleNavObj.remainingDistance >= 5.0f)
				InstantiateObstacles();
		}
		
		if(bubbleParticleSpawnTimer <= 0)
		{
			float theta = UnityEngine.Random.Range(0.0f, 360.0f);
			Vector3 bubbleParticleOffset = new Vector3((float)(4.0f*Math.Sin(theta)), UnityEngine.Random.Range(1.0f, 6.0f), (float)(4.0f*Math.Cos(theta)));
			Instantiate(bubbleParticles, obstacleNavObj.transform.position + bubbleParticleOffset, Quaternion.identity);
			bubbleParticleSpawnTimer =  Constants.BUBBLE_PARTICLE_SPAWN_TIMER;
		}
		
		grindTimer += Time.deltaTime;
		
		if(grindTimer > 0.1f)
		{
			playGrindSound = false;
			audioScript.audio4.Stop();
		}
		
		//vel.Normalize();
		float dist = navObj.remainingDistance; 
		bool hasReached = false;
		
		if (Vector3.Distance(navObj.transform.position,navObj.destination) <= 10.0f)
		     hasReached = true;
			
		//Debug.Log(hasReached);	
		if(!hasReached)
		{
			flowDir = (ePos - sPos);
			
			// Offset the Y by 4 units to center the vector in the scene
			flowDir.y += 4.0f;	
			
			distance = flowDir.magnitude * 0.7f;
			flowDir.Normalize();
			gameObject.rigidbody.velocity = -flowDir * distance;
		}
		else
			gameObject.rigidbody.velocity = gameObject.transform.forward*0.75f;
	}
	
	void InstantiatePowerUps()
	{
		float r = UnityEngine.Random.Range(0.0f, 1.0f);

		// Select between spawning halth/ stamina powerups with a certain probability
		int pType = 0;
		if(r <= Constants.HEALTH_PROB)
			pType = 0;
		else
			pType = 1;
		
		// Health
		if(pType == 0)
			Instantiate(healthPowerUp, objSpawnPos, Quaternion.identity);
		
		// Stamina
		else if(pType == 1)
			Instantiate(staminaPowerUp, objSpawnPos, Quaternion.identity);
			
	}
	
	void InstantiateObstacles()
	{
		Vector3 ePos = obstacleNavObj.transform.position;
		Vector3 sPos = gameObject.transform.position;
		Vector3 flowDir = ePos - sPos;
		flowDir.Normalize();
		
		float r = UnityEngine.Random.Range(0.0f, 1.0f);
		
		// Obstacle that will be instantiated
		GameObject obstacle = null;
		
		// Select between spawning small/large obstacle with a certain probability
		int oType = 0;
		if(r <= Constants.SM_OBSTACLE_PROB)
			oType = 0;
		else
			oType = 1;
		
		// Small obstacle
		if(oType == 0)
		{
			int randomObs = (int)UnityEngine.Random.Range(0.0f, 4.0f);

			switch(randomObs)
			{
				case 0:
						obstacle = (GameObject)Instantiate(SmallObstacleBolts, objSpawnPos, Quaternion.identity) as GameObject;
						break;
				case 1:
						obstacle = (GameObject)Instantiate(SmallObstacleNuts, objSpawnPos, Quaternion.identity) as GameObject;
						break;
				case 2:
						obstacle = (GameObject)Instantiate(SmallObstacleNewspaper, objSpawnPos, Quaternion.identity) as GameObject;
						break;
				case 3:
						obstacle = (GameObject)Instantiate(SmallObstaclePaper, objSpawnPos, Quaternion.identity) as GameObject;
						break;
//				case 4:
//						obstacle = (GameObject)Instantiate(LargeObstacleRock5, objSpawnPos, Quaternion.identity) as GameObject;
//						break;
				default:
						obstacle = (GameObject)Instantiate(SmallObstacleBolts, objSpawnPos, Quaternion.identity) as GameObject;
						break;
			}
			
			// Scale obstacle randomly
			obstacle.transform.localScale += new Vector3(UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f));
		
			// Rotate obstacle randomly
			obstacle.transform.Rotate(new Vector3(UnityEngine.Random.Range(-90.0f, 90.0f), UnityEngine.Random.Range(-90.0f, 90.0f), UnityEngine.Random.Range(-90.0f, 90.0f)));
			
			// Add random movement force/ torque
			obstacle.rigidbody.AddForce(flowDir * Constants.SM_OBSTACLE_MOVE_MAGNITUDE);
			obstacle.rigidbody.AddTorque(UnityEngine.Random.Range(5.0f, 15.0f), UnityEngine.Random.Range(5.0f, 15.0f), UnityEngine.Random.Range(5.0f, 15.0f));
		}
		
		// Large obstacle
		if(oType == 1)
		{
			int randomObs = (int)UnityEngine.Random.Range(0.0f, 5.0f);
			
			switch(randomObs)
			{
				case 0:
						obstacle = (GameObject)Instantiate(LargeObstacleRock1, objSpawnPos, Quaternion.identity) as GameObject;
						break;
				case 1:
						obstacle = (GameObject)Instantiate(LargeObstacleRock2, objSpawnPos, Quaternion.identity) as GameObject;
						break;
				case 2:
						obstacle = (GameObject)Instantiate(LargeObstacleRock3, objSpawnPos, Quaternion.identity) as GameObject;
						break;
				case 3:
						obstacle = (GameObject)Instantiate(LargeObstacleRock4, objSpawnPos, Quaternion.identity) as GameObject;
						break;
				case 4:
						obstacle = (GameObject)Instantiate(LargeObstacleRock5, objSpawnPos, Quaternion.identity) as GameObject;
						break;
				default:
						obstacle = (GameObject)Instantiate(LargeObstacleRock1, objSpawnPos, Quaternion.identity) as GameObject;
						break;
			}
			
			// Scale obstacle randomly
			obstacle.transform.localScale += new Vector3(UnityEngine.Random.Range(0.5f, 2.0f), UnityEngine.Random.Range(0.5f, 2.0f), UnityEngine.Random.Range(0.5f, 2.0f));
		
			// Rotate obstacle randomly
			obstacle.transform.Rotate(new Vector3(UnityEngine.Random.Range(-90.0f, 90.0f), UnityEngine.Random.Range(-90.0f, 90.0f), UnityEngine.Random.Range(-90.0f, 90.0f)));
		
			// Add random movement force/ torque
			obstacle.rigidbody.AddForce(flowDir * Constants.LG_OBSTACLE_MOVE_MAGNITUDE);
			obstacle.rigidbody.AddTorque(UnityEngine.Random.Range(2.0f, 5.0f), UnityEngine.Random.Range(2.0f, 5.0f), UnityEngine.Random.Range(2.0f, 5.0f));
		}
	}
	
	void OnControllerColliderHit(ControllerColliderHit hit)
	{
		//Collider collider = collision.collider;
		Vector3 particleOffset = new Vector3(0, 0, 0);
		
		if(hit.gameObject.tag == "FinalDestination") {
			
			// Store data
			PlayerPrefs.SetInt("Level", globalObj.currentLevel);
			PlayerPrefs.SetInt("Complete", 1);
			PlayerPrefs.SetInt("Score", globalObj.score);
			PlayerPrefs.SetFloat("Time", globalObj.timer);
			PlayerPrefsX.SetStringArray("AchievementList", globalObj.LEVEL1_ACH);
			PlayerPrefsX.SetIntArray("AchievementTracker", globalObj.LEVEL1_ACH_TRACKER);
			
			// Load Gameover scene
			Application.LoadLevel("GameOverScene");
		}
		
		else if(hit.gameObject.tag == "Environment") {
			
			grindTimer = 0.0f;
			
			if(playGrindSound == false)
			{
				playGrindSound = true;
				
				// Play sound
				audioScript.audio4.Play();
			}
		}
		
		// Collision with small obstacles
		else if(hit.gameObject.tag == ("Small Obstacle")) {
			
			globalObj.currentHealth -= 5;
			
			// Controller rumble
			if(Constants.WII_RUMBLE){
				wiimote_rumble(0, 0.5f);
				wiimote_rumble(1, 0.5f);}
			
			// Play sound
			AudioSource.PlayClipAtPoint(audioScript.fishObsHit, hit.gameObject.transform.position);
			
			// Image effect
			if(Constants.BLOOD_SPLATTER_TOGGLE)
				overlayToggle = true;
			
			// Break obstacle 
			Destroy (hit.gameObject);
		}
		
		else if(hit.gameObject.tag == ("Large Obstacle")) {
			
			globalObj.currentHealth -= 10;
			
			// Controller rumble
			if(Constants.WII_RUMBLE){
				wiimote_rumble(0, 0.5f);
				wiimote_rumble(1, 0.5f);}
			
			// Play sound
			AudioSource.PlayClipAtPoint(audioScript.fishObsHit, hit.gameObject.transform.position);
			
			// Image effect
			if(Constants.BLOOD_SPLATTER_TOGGLE)
				overlayToggle = true;
			
			// Break obstacle 
			Destroy (hit.gameObject);
		}
		
		else if(hit.gameObject.tag == ("Health PowerUp")) {
			
			// Destroy object and instantiate particles
			Destroy(hit.gameObject);
			Instantiate(redParticles, collider.gameObject.transform.position, Quaternion.identity);
			
			// Play sound
			AudioSource.PlayClipAtPoint(audioScript.powerPickUpSound, hit.gameObject.transform.position);
			
			// If there is already a power-up stored use the new one immeadiately
			if(globalObj.storedHealthPU == true)
			{
				globalObj.currentHealth += 25;
				
				// Clamp to max health
				if(globalObj.currentHealth > Constants.MAX_HEALTH)
					globalObj.currentHealth = Constants.MAX_HEALTH;
			}
			
			// Set storedHealthPU to true
			else
				globalObj.storedHealthPU = true;
		}
		
		else if(hit.gameObject.tag == ("Stamina PowerUp")) {
			
			// Destroy object and instantiate particles
			Destroy(hit.gameObject);
			Instantiate(greenParticles, collider.gameObject.transform.position, Quaternion.identity);
			
			// Play sound
			AudioSource.PlayClipAtPoint(audioScript.powerPickUpSound, hit.gameObject.transform.position);
			
			// If there is already a power-up stored use the new one immeadiately
			if(globalObj.storedStaminaPU == true)
			{
				globalObj.currentStamina += 25;
				
				// Clamp to max stamina
				if(globalObj.currentStamina > Constants.MAX_STAMINA)
					globalObj.currentStamina = Constants.MAX_STAMINA;
			}
			
			// Set storedStaminaPU to true
			else
				globalObj.storedStaminaPU = true;
		}
		
		else if(hit.gameObject.tag == ("StartDrop")) {
			
		    audioScript.audio3.Play();
			
			// Destroy object and instantiate particles
			Destroy(hit.gameObject);
			OVRPlayerController s = gameObject.GetComponent<OVRPlayerController>();
			s.inDrop = true;
			navObj.speed = 40.0f;
			obstacleNavObj.speed = 40.0f;
		}
		
		else if(hit.gameObject.tag == ("EndDrop")) {
			
			audioScript.audio3.Stop();
			
			// Destroy object and instantiate particles
			Destroy(hit.gameObject);
			OVRPlayerController s = gameObject.GetComponent<OVRPlayerController>();
			s.inDrop = false;
			navObj.speed = 7.0f;
			obstacleNavObj.speed = 14.0f;
		}
		
	}
}
