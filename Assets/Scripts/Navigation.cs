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
	public GameObject SmallObstacle;
	public GameObject LargeObstacle;
	public GameObject LargeObstacleRock1;
	public GameObject LargeObstacleRock2;
	public GameObject LargeObstacleRock3;
	public GameObject LargeObstacleRock4;
	public GameObject LargeObstacleRock5;
	
	// Particle effects
	public GameObject redParticles;
	public GameObject greenParticles;
	public GameObject bubbleParticles;
	
	// Image effects
	private ScreenOverlay[] so;
	private float overlayTimer;
	private bool overlayToggle;
	
	// Obstacles
	public Vector3 ObjSpawnPos;
	public float ObstacleTimer;
	
	// Bubble particles
	public float bubbleParticleSpawnTimer;
	
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
		
		bubbleParticleSpawnTimer =  Constants.BUBBLE_PARTICLE_SPAWN_TIMER;
		
		//gameObject.rigidbody.AddForce(0, 0, -1000);//new Vector3(Random.Range (-10,10),0,Random.Range (-4,-5)));//
		// Avoid collision between the navAgent and the obstacles 
		Physics.IgnoreLayerCollision(11, 20, true);
		Physics.IgnoreLayerCollision(10, 20, true);
		Physics.IgnoreLayerCollision(20, 20, true);
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
		Vector3 flowDir = new Vector3(0,0,0);// = (ePos - sPos);
		float distance = 0;//flowDir.magnitude;
		
		NavMeshHit hit =  new NavMeshHit();
		
		ObjSpawnPos = obstacleNavObj.transform.position 
			+ new Vector3(UnityEngine.Random.Range(-0.4f, 0.4f), UnityEngine.Random.Range(0.2f, 1.5f), 0.0f);// sPos + flowDir ; //new Vector3(0,0,10);//
		
		ObstacleTimer += Time.deltaTime;
		bubbleParticleSpawnTimer -= Time.deltaTime;
		
		if(ObstacleTimer > 2.0f)
		{
			ObstacleTimer = 0.0f;
		
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
	
	void InstantiateObstacles()
	{
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
			obstacle = (GameObject) Instantiate(SmallObstacle, ObjSpawnPos, Quaternion.identity) as GameObject;
			
			// Scale obstacle randomly
			obstacle.transform.localScale += new Vector3(UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f), UnityEngine.Random.Range(0.0f, 1.0f));
		}
		
		// Large obstacle
		if(oType == 1)
		{
			int randomObs = (int)UnityEngine.Random.Range(0.0f, 5.0f);
			
			switch(randomObs)
			{
				case 0:
						obstacle = (GameObject)Instantiate(LargeObstacleRock1, ObjSpawnPos, Quaternion.identity) as GameObject;
						break;
				case 1:
						obstacle = (GameObject)Instantiate(LargeObstacleRock2, ObjSpawnPos, Quaternion.identity) as GameObject;
						break;
				case 2:
						obstacle = (GameObject)Instantiate(LargeObstacleRock3, ObjSpawnPos, Quaternion.identity) as GameObject;
						break;
				case 3:
						obstacle = (GameObject)Instantiate(LargeObstacleRock4, ObjSpawnPos, Quaternion.identity) as GameObject;
						break;
				case 4:
						obstacle = (GameObject)Instantiate(LargeObstacleRock5, ObjSpawnPos, Quaternion.identity) as GameObject;
						break;
				default:
						obstacle = (GameObject)Instantiate(LargeObstacleRock1, ObjSpawnPos, Quaternion.identity) as GameObject;
						break;
			}
			
			// Scale obstacle randomly
			obstacle.transform.localScale += new Vector3(UnityEngine.Random.Range(0.5f, 2.0f), UnityEngine.Random.Range(0.5f, 2.0f), UnityEngine.Random.Range(0.5f, 2.0f));
		}
		
		// Rotate obstacle randomly
		obstacle.transform.Rotate(new Vector3(UnityEngine.Random.Range(-90.0f, 90.0f), UnityEngine.Random.Range(-90.0f, 90.0f), UnityEngine.Random.Range(-90.0f, 90.0f)));
		
		
	}
	
	void OnControllerColliderHit(ControllerColliderHit hit)
	{
		//Collider collider = collision.collider;
		Vector3 particleOffset = new Vector3(0, 0, 0);
		
		if(hit.gameObject.tag == "Environment") {
			
			globalObj.currentHealth -= 0;
			
			//Destroy (gameObject);
		}
		
		// Collision with small obstacles
		else if(hit.gameObject.tag == ("Small Obstacle")) {
			
			globalObj.currentHealth -= 5;
			
			// Controller rumble
			if(Constants.WII_RUMBLE)
				wiimote_rumble(0, 0.5f);
			
			// Image effect
			if(Constants.BLOOD_SPLATTER_TOGGLE)
				overlayToggle = true;
			
			// Break obstacle 
			Destroy (hit.gameObject);
			//Instantiate(yellowParticles, collider.gameObject.transform.position, Quaternion.identity);
		}
		
		else if(hit.gameObject.tag == ("Large Obstacle")) {
			
			globalObj.currentHealth -= 10;
			
			// Controller rumble
			if(Constants.WII_RUMBLE)
				wiimote_rumble(0, 0.5f);
			
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
			
			// Set storedHealthPU to true
			globalObj.storedHealthPU = true;
		}
		
		else if(hit.gameObject.tag == ("Stamina PowerUp")) {
			
			// Destroy object and instantiate particles
			Destroy(hit.gameObject);
			Instantiate(greenParticles, collider.gameObject.transform.position, Quaternion.identity);
			
			// Set storedStaminaPU to true
			globalObj.storedStaminaPU = true;
		}
		else if(hit.gameObject.tag == ("StartDrop")) {
			
			
		    audioScript.audio3.Play ();
			
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
