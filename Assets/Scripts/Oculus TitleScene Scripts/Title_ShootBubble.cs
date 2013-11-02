using UnityEngine;
using System.Collections;

public class TitleShootBubble : MonoBehaviour {
	
	//private Level1_Global globalObj;
	private Vector3 shootDirection;
	
	// Particle effects
	public GameObject redParticles;
	public GameObject greenParticles;
	public GameObject yellowParticles;
	public GameObject blueParticles;
	
	// Use this for initialization
	void Start () {
		
		// Get the aim direction and add force to the bubble object
		//GameObject gl = GameObject.Find("Global");
		//globalObj = gl.GetComponent<Level1_Global>();
		//shootDirection = globalObj.direction;
		//gameObject.rigidbody.AddForce(shootDirection * 500.0f);
		
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
		if(collider.CompareTag("MenuButton")) {
			
			// Destroy bubble
			//Destroy (gameObject);
		}	
	}
}
