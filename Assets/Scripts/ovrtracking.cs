using UnityEngine;
using System.Collections;

public class ovrtracking : MonoBehaviour {
	GameObject myPlayer;
	// Use this for initialization
	void Start () {
	myPlayer = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		Transform playerTransform = myPlayer.transform;
		//gameObject.transform.position.Set(playerTransform.position.x,playerTransform.position.y,playerTransform.position.z);
		//gameObject.transform.eulerAngles.Set(playerTransform.eulerAngles.x,playerTransform.eulerAngles.y,playerTransform.eulerAngles.z);
		gameObject.transform.parent = myPlayer.transform;
	}
}
