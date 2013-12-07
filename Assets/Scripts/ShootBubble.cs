using UnityEngine;
using System.Collections;

public class ShootBubble : MonoBehaviour {
	
	private Level1_Global globalObj;
	private Level1_Audio audioScript;
	
	private NavMeshAgent navObj;
	private Vector3 shootDirection;
	
	// Life timer
	private float bubbleLifeTimer =  Constants.BUBBLE_LIFE_TIME;
	
	// Particle effects
	public GameObject redParticles;
	public GameObject greenParticles;
	public GameObject yellowParticles;
	public GameObject blueParticles;
	
	// Use this for initialization
	void Start () {
		
		// Get the aim direction and add force to the bubble object
		GameObject gl = GameObject.Find("Global");
		globalObj = gl.GetComponent<Level1_Global>();
		audioScript = gl.GetComponent<Level1_Audio>();
		
		navObj = GameObject.FindGameObjectWithTag("NavAgent").GetComponent<NavMeshAgent>();
		shootDirection = globalObj.direction;
		
		gameObject.rigidbody.AddForce(shootDirection * Constants.BUBBLE_FORCE * (navObj.speed/Constants.DEFAULT_FLOW_SPEED));
		
		// Remove collision with other bubbles
		Physics.IgnoreLayerCollision(14, 14, true);
		
		// Remove collision with player
		Physics.IgnoreLayerCollision(14, 8, true);
	}
	
	// Update is called once per frame
	void Update () {
		
		if(bubbleLifeTimer <= 0)
			Destroy(gameObject);
		
		else
			bubbleLifeTimer -= Time.deltaTime;
		
	}
	
	void OnCollisionEnter(Collision collision)
	{
		// Get collider info
		Collider collider = collision.collider;
		Vector3 particleOffset = new Vector3(0, 0, 0);
		
		// Collision with environment
		if(collider.CompareTag("Environment")) {
			
			// Destroy bubble
			Destroy(gameObject);
		}
		
		// Collision with small obstacles
		else if(collider.CompareTag("Small Obstacle")) {
			
			// Play sound
			AudioSource.PlayClipAtPoint(audioScript.obstacleHitSound, collider.gameObject.transform.position);
			
			// break obstacle, random chance of power up 
			Instantiate(yellowParticles, gameObject.transform.position, Quaternion.identity);
			Destroy(gameObject);
			Destroy(collider.gameObject);
			
			// Add points to score
			globalObj.score += 10;
		}
		
		// Collision with large obstacles
		else if(collider.CompareTag("Large Obstacle")) {
			
			// Play sound
			AudioSource.PlayClipAtPoint(audioScript.bubbleLargeObsHit, collider.gameObject.transform.position);
			
			//Instantiate(yellowParticles, gameObject.transform.position, Quaternion.identity);
			Destroy(gameObject);
			//Destroy(collider.gameObject);
		}		
		
		// Collision with large obstacles
		else if(collider.CompareTag("Health PowerUp")) {
			
			// Play sound
			AudioSource.PlayClipAtPoint(audioScript.powerPickUpSound, collider.gameObject.transform.position);
			
			Instantiate(redParticles, gameObject.transform.position, Quaternion.identity);
			Destroy(gameObject);
			Destroy(collider.gameObject);
			
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
		
		// Collision with large obstacles
		else if(collider.CompareTag("Stamina PowerUp")) {
		
			// Play sound
			AudioSource.PlayClipAtPoint(audioScript.powerPickUpSound, collider.gameObject.transform.position);
			
			Instantiate(greenParticles, gameObject.transform.position, Quaternion.identity);
			Destroy(gameObject);
			Destroy(collider.gameObject);
			
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
	}
}
