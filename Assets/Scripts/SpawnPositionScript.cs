using UnityEngine;
using System.Collections;

public class SpawnPositionScript : MonoBehaviour {
	
	private GameObject nav;
	private GameObject obsNav;
	private NavMeshAgent nav_obj;
	private NavMeshAgent obs_nav_obj;

	// Agent positions
	public GameObject navAgentPos;
	public GameObject obstacleAgentPos;
	
	// Destination
	public GameObject endPoint;
	
	void Awake() {
		
		//destination = endPoint.transform.position;
		//destination = endPoint.transform.position;
	}
	
	// Use this for initialization
	void Start () {
		
		
		//Debug.Log("sp awake " + destination);
		
		// Get navAgent gameobject
		//nav = GameObject.FindGameObjectWithTag("NavAgent");
	    //nav_obj = nav.GetComponent<NavMeshAgent>();
		
		// Get obstacle navAgent gameobject
		//obsNav = GameObject.FindGameObjectWithTag("ObsNavAgent");
		//obs_nav_obj = obsNav.GetComponent<NavMeshAgent>();
		
		// Set the destination for the navAgent and obstacle navAgent
		//destination = endPoint.transform.position;
		//nav_obj.destination = destination;
		//obs_nav_obj.destination = destination;
		
		// Set initial positions of the agents
		//nav.transform.position = navAgentPos.transform.position;
		//obsNav.transform.position = obstacleAgentPos.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
