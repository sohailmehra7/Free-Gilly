using UnityEngine;
using System.Collections;

public class level1Global : MonoBehaviour {
	public AudioClip backGroundMusic; 
	// Use this for initialization
	void Start () {
	AudioSource.PlayClipAtPoint(backGroundMusic,gameObject.transform.position);
	//AudioSource.	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
