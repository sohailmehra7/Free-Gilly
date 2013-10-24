using UnityEngine;
using System.Collections;

public class NavAgentScript : MonoBehaviour {
	
	public Vector3 dest;
	// Use this for initialization
	void Start () {
	//dest.transform.position = 
		dest = GameObject.Find("destination").transform.position;
		GetComponent<NavMeshAgent>().destination = dest;
	}
	
	// Update is called once per frame
	void Update () {
	 
	}
}
