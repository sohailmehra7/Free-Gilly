using UnityEngine;
using System.Collections;

public class Navigation : MonoBehaviour {

	public float moveSpeed;
	
	// Use this for initialization
	void Start () {
		
		moveSpeed = 1.0f ;
		gameObject.rigidbody.AddForce(new Vector3(0,0,-3));
	}
	
	// Update is called once per frame
	void Update () {
		
		gameObject.rigidbody.AddForce(new Vector3(0,0,-3));
		if(Input.GetAxisRaw ("Vertical") > 0 )
		{
			gameObject.transform.Translate (0,moveSpeed,0);
			
		}
		if(Input.GetAxisRaw ("Vertical") < 0 )
		{
			gameObject.transform.Translate (0,-moveSpeed,0);
			
		}
		if(Input.GetAxisRaw ("Horizontal") > 0 )
		{
			gameObject.transform.Translate (moveSpeed,0,0);
			
		}
		if(Input.GetAxisRaw ("Horizontal") < 0 )
		{
			gameObject.transform.Translate (-moveSpeed,0,0);
			
		}
		//gameObject.transform.position.Set(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z + 200.0f ); 
	}
}
