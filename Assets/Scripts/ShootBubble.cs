using UnityEngine;
using System.Collections;

public class ShootBubble : MonoBehaviour {
	
	private Level1_Global globalObj;
	private Vector3 shootDirection;
	
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
		shootDirection = globalObj.direction;
		gameObject.rigidbody.AddForce(shootDirection * 500.0f);
		
		//Debug.Log("bullet force is " + (shootDirection*5000.0f).ToString());
		
		// Remove collision with other bubbles
		Physics.IgnoreLayerCollision(14, 14, true);
		
		// Remove collision with player
		Physics.IgnoreLayerCollision(14, 8, true);
	}
	
	// Update is called once per frame
	void Update () {
		
		
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
			
			// break obstacle, random chance of power up 
			Instantiate(yellowParticles, gameObject.transform.position, Quaternion.identity);
			Destroy(gameObject);
			Destroy(collider.gameObject);
		}
		
		// Collision with large obstacles
		else if(collider.CompareTag("Large Obstacle")) {
			
			Instantiate(yellowParticles, gameObject.transform.position, Quaternion.identity);
			Destroy(gameObject);
			Destroy(collider.gameObject);
		}		
		
		// Collision with large obstacles
		else if(collider.CompareTag("Health PowerUp")) {

			Instantiate(redParticles, gameObject.transform.position, Quaternion.identity);
			Destroy(gameObject);
			Destroy(collider.gameObject);
			
			// Set storedHealthPU to true
			globalObj.storedHealthPU = true;
		}	
		
		// Collision with large obstacles
		else if(collider.CompareTag("Stamina PowerUp")) {
		
			Instantiate(greenParticles, gameObject.transform.position, Quaternion.identity);
			Destroy(gameObject);
			Destroy(collider.gameObject);
			
			// Set storedStaminaPU to true
			globalObj.storedStaminaPU = true;
		}	
	}
}
