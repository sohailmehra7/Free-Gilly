using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;


public class Navigation : MonoBehaviour {
	
	[DllImport ("UniWii")]
	private static extern void wiimote_rumble(int which, float duration);
	
	private GameObject nav;
	private NavMeshAgent nav_obj;
	private NavMeshAgent spawn_nav_obj;
	
	private Level1_Global globalObj;
	private UniWiiCheck uniWii;
	
	//Obstacles to spawn
	public GameObject SmallObstacle;
	public GameObject LargeObstacle;
	public GameObject LargeObstacleRock1;
	public GameObject LargeObstacleRock2;
	public GameObject LargeObstacleRock3;
	public GameObject LargeObstacleRock4;
	public GameObject LargeObstacleRock5;
	
	//   Particle effects
	public GameObject redParticles;
	public GameObject greenParticles;
	
	public Vector3 ObjSpawnPos;
	
	public float ObstacleTimer;
	
	// Use this for initialization
	void Start () {
		
		GameObject gl = GameObject.Find("Global");
		nav = GameObject.Find("NavAgent");
		globalObj = gl.GetComponent<Level1_Global>();
		uniWii = gl.GetComponent<UniWiiCheck>();
		spawn_nav_obj = GameObject.Find("SpawnNavAgent").GetComponent<NavMeshAgent>();
		
		//gameObject.rigidbody.AddForce(0, 0, -1000);//new Vector3(Random.Range (-10,10),0,Random.Range (-4,-5)));//
		// Avoid collision between the navAgent and the obstacles 
		Physics.IgnoreLayerCollision(11, 20, true);
		Physics.IgnoreLayerCollision(10, 20, true);
		Physics.IgnoreLayerCollision(20, 20, true);
	}
	
	// Update is called once per frame
	void Update () {
		
		Vector3 ePos = nav.transform.position;
		Vector3 sPos = gameObject.transform.position;
		Vector3 flowDir = new Vector3(0,0,0);// = (ePos - sPos);
		float distance = 0;//flowDir.magnitude;
		nav_obj = nav.GetComponent<NavMeshAgent>();
		
		NavMeshHit hit =  new NavMeshHit();
		
		//if(distance > 8.0f)
		//{
		//	nav_obj.speed = 2.0f;
		//} 
		//else if(distance < 3.0f)
		//{
		//	nav_obj.speed = 5.0f;
		//}
		ObjSpawnPos = spawn_nav_obj.transform.position 
			+ new Vector3(UnityEngine.Random.Range(-0.4f,0.4f),UnityEngine.Random.Range(0.2f,1.5f),0.0f);// sPos + flowDir ; //new Vector3(0,0,10);//
		
		ObstacleTimer += Time.deltaTime;
		
		//Debug.DrawLine(probePosition, hit.position, Color.red);
		if( ObstacleTimer > 2.0f) //NavMesh.SamplePosition(ePos, out hit,100.0f,1 << NavMesh.GetNavMeshLayerFromName("Default")) )//&&
		{
			//Vector3 v = flowDir;
			//v.Set (flowDir.x*45, 4.5f, flowDir.z*45);
			//ObjSpawnPos =hit.position + v;
	    	//Debug.DrawLine(ePos, hit.position, Color.blue);
			ObstacleTimer = 0.0f;
		
			if(spawn_nav_obj.remainingDistance >= 5.0f)
				InstantiateObstacles();
		}
			
		//vel.Normalize();
		if(nav_obj.remainingDistance >= 1.0f)// && (nav_obj.remainingDistance != float.NegativeInfinity && nav_obj.remainingDistance != float.PositiveInfinity))
		{
			flowDir = (ePos - sPos);
			distance = flowDir.magnitude;
			flowDir.Normalize();
			gameObject.rigidbody.velocity = -flowDir*distance;
		}
		else
			gameObject.rigidbody.velocity = -gameObject.transform.forward*0.25f;
		
		//Debug.Log("rEMAINING DIST = " + nav_obj.remainingDistance);
		//Debug.Log("Path status = " + nav_obj.pathStatus);
			
		//gameObject.rigidbody.AddForce(0,0,-50);//new Vector3(Random.Range (-10,10),0,Random.Range (-4,-5)));//
		/*
		
		if(Input.GetAxisRaw ("Vertical") > 0 ) // Up
		{
			gameObject.rigidbody.AddForce(new Vector3(0, -moveSpeed,0));//gameObject.transform.Translate (0,moveSpeed,0);
			
		}
		if(Input.GetAxisRaw ("Vertical") < 0 )
		{
			gameObject.rigidbody.AddForce(new Vector3(0, moveSpeed,0));
			
		}
		if(Input.GetAxisRaw ("Horizontal") > 0 )
		{
			gameObject.transform.Translate (moveSpeed,0,0);
			
		}
		if(Input.GetAxisRaw ("Horizontal") < 0 )
		{
			gameObject.transform.Translate (-moveSpeed,0,0);
			
		}*/
		//gameObject.transform.position.Set(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z + 200.0f ); 
	}
	
	void InstantiateObstacles()
	{
		float r = UnityEngine.Random.Range(0.0f, 1.0f);
		
		// Select between spawning small/large obstacle with a certain probability
		int oType = 0;
		if(r <= 0.9f)
			oType = 0;
		else
			oType = 1;
		
		// Small obstacle
		if(oType == 0)
		{
			Instantiate(SmallObstacle, ObjSpawnPos, Quaternion.identity);
		}
		
		// Large obstacle
		if(oType == 1)
		{
			int randomObs = (int)UnityEngine.Random.Range(0.0f, 5.0f);
			
			switch(randomObs)
			{
				case 0:
						Instantiate(LargeObstacleRock1, ObjSpawnPos, Quaternion.identity);
						break;
				case 1:
						Instantiate(LargeObstacleRock2, ObjSpawnPos, Quaternion.identity);
						break;
				case 2:
						Instantiate(LargeObstacleRock3, ObjSpawnPos, Quaternion.identity);
						break;
				case 3:
						Instantiate(LargeObstacleRock4, ObjSpawnPos, Quaternion.identity);
						break;
				case 4:
						Instantiate(LargeObstacleRock5, ObjSpawnPos, Quaternion.identity);
						break;
				default:
						Instantiate(LargeObstacleRock1, ObjSpawnPos, Quaternion.identity);
						break;
			}
			
		}
	}
	
	void OnControllerColliderHit(ControllerColliderHit hit)
	{
		//Debug.Log("collison has occured ") ;
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
			
			// Break obstacle 
			Destroy (hit.gameObject);
			//Instantiate(yellowParticles, collider.gameObject.transform.position, Quaternion.identity);
		}
		
		else if(hit.gameObject.tag == ("Large Obstacle")) {
			
			globalObj.currentHealth -= 10;
			
			// Controller rumble
			if(Constants.WII_RUMBLE)
				wiimote_rumble(0, 0.5f);

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
		
	}
}
