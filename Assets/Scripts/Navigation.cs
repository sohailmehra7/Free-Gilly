using UnityEngine;
using System.Collections;

public class Navigation : MonoBehaviour {

	public float moveSpeed;
	
	private Level1_Global globalObj;
	
	// Particle effects
	public GameObject redParticles;
	public GameObject greenParticles;
	
	// Use this for initialization
	void Start () {
		
		GameObject gl = GameObject.Find("Global");
		globalObj = gl.GetComponent<Level1_Global>();
		moveSpeed = 10.0f;
		gameObject.rigidbody.AddForce(0, 0, -10000);//new Vector3(Random.Range (-10,10),0,Random.Range (-4,-5)));//
	}
	
	// Update is called once per frame
	void Update () {
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
	
	void OnControllerColliderHit(ControllerColliderHit hit)
	{
		//Debug.Log("collison has occured ") ;
		//Collider collider = collision.collider;
		
		if(hit.gameObject.tag == "Environment") {
			
			globalObj.currentHealth -= 0;
			
			//Destroy (gameObject);
		}
		
		// Collision with small obstacles
		else if(hit.gameObject.tag == ("Small Obstacle")) {
			
			globalObj.currentHealth -= 5;
			
			// Break obstacle 
			Destroy (hit.gameObject);
		}
		
		else if(hit.gameObject.tag == ("Large Obstacle")) {
			
			globalObj.currentHealth -= 10;

			//Destroy(gameObject);
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
