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
		gameObject.rigidbody.AddForce(shootDirection * 10000.0f);
		
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
		
		// Collision with environment
		if(collider.CompareTag("Environment")) {
			
			// Destroy bubble
			Destroy (gameObject);
		}
		
		// Collision with small obstacles
		else if(collider.CompareTag("Small Obstacle")) {
			
			Destroy (collider.gameObject);
			
			// break obstacle, random chance of power up 
			Destroy (gameObject);
			Instantiate(yellowParticles, collider.gameObject.transform.position, Quaternion.identity);	
		}
		
		// Collision with large obstacles
		else if(collider.CompareTag("Large Obstacle")) {
			Destroy (collider.gameObject);
			//Debug.Log("Collided with large object");
			Destroy (gameObject);
			Instantiate(yellowParticles, collider.gameObject.transform.position, Quaternion.identity);	
		}		
		
		// Collision with large obstacles
		else if(collider.CompareTag("Health PowerUp")) {
			
			Destroy (collider.gameObject);
			//Debug.Log("Collided with large object");
			Destroy (gameObject);
			Instantiate(redParticles, collider.gameObject.transform.position, Quaternion.identity);
			
			// Set storedHealthPU to true
			globalObj.storedHealthPU = true;
		}	
		
		// Collision with large obstacles
		else if(collider.CompareTag("Stamina PowerUp")) {
			
			Destroy (collider.gameObject);
			//Debug.Log("Collided with large object");
			Destroy (gameObject);
			Instantiate(greenParticles, collider.gameObject.transform.position, Quaternion.identity);
			
			// Set storedStaminaPU to true
			globalObj.storedStaminaPU = true;
		}	
	}
}
