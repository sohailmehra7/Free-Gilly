using UnityEngine;
using System.Collections;

public class Level1_Global : MonoBehaviour {

	public AudioClip backGroundMusic; 
	
	// Use this for initialization
	void Start () {
	
		AudioSource.PlayClipAtPoint(backGroundMusic,gameObject.transform.position);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
