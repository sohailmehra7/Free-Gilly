using UnityEngine;
using System.Collections;

public class SpawnAgentScript : MonoBehaviour {
	
	public Vector3 dest;
	// Use this for initialization
	void Start () {
	
		dest = GameObject.Find("destination").transform.position;
		GetComponent<NavMeshAgent>().destination = dest;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
