using UnityEngine;
using System.Collections;

public class ShootBubble : MonoBehaviour {
	
	private Level1_Global globalObj;
	private Vector3 shootDirection;
	// Use this for initialization
	void Start () {
		
		GameObject gl = GameObject.Find("Global");
		globalObj = gl.GetComponent<Level1_Global>();
		shootDirection = globalObj.direction ;
	}
	
	// Update is called once per frame
	void Update () {
	
	//	dir *= CrosshairDistance;
		gameObject.rigidbody.AddForce(shootDirection*10);
		
	}
}
