using UnityEngine;
using System.Collections;

public class ShootBubble : MonoBehaviour {
	
	private Level1_Global globalObj;
	private Vector3 shootDirection;
	
	// Use this for initialization
	void Start () {
		
		// Get the aim direction and add force to the bubble object
		GameObject gl = GameObject.Find("Global");
		globalObj = gl.GetComponent<Level1_Global>();
		shootDirection = globalObj.direction;
		gameObject.rigidbody.AddForce(shootDirection * 5000.0f);
		
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
			
			// break obstacle by instatiating smaller pieces , random chance of power up 
			Destroy (gameObject);
		}
		
		// Collision with large obstacles
		else if(collider.CompareTag("Large Obstacle")) {
			
			//Debug.Log("Collided with large object");
			Destroy (gameObject);
		}		
		
		// Collision with large obstacles
		else if(collider.CompareTag("Health PowerUp")) {
			
			//Debug.Log("Collided with large object");
			Destroy (gameObject);
		}	
		
		// Collision with large obstacles
		else if(collider.CompareTag("Stamina PowerUp")) {
			
			//Debug.Log("Collided with large object");
			Destroy (gameObject);
		}	
	}
}
