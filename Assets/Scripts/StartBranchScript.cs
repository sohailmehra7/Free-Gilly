using UnityEngine;
using System.Collections;

public class StartBranchScript : MonoBehaviour {
	
	public GameObject endPoint;
	public Vector3 destination;
	
	public GameObject NavAgentRespawn;
	public GameObject ObsAgentRespawn;
	private GameObject nav;
	private NavMeshAgent nav_obj;
	private NavMeshAgent spawn_nav_obj;
	
	// Use this for initialization
	void Start () {
	
		nav = GameObject.FindGameObjectWithTag("NavAgent");
	    nav_obj = nav.GetComponent<NavMeshAgent>();
		spawn_nav_obj = GameObject.FindGameObjectWithTag("ObsNavAgent").GetComponent<NavMeshAgent>();
		
		destination = endPoint.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
		
	}
	
	void OnTriggerEnter(Collider other)
    {
    	if (other.gameObject.CompareTag("Player"))
		{
			nav_obj.transform.position = NavAgentRespawn.transform.position; //this.transform.position; // + offset
			nav_obj.SetDestination(destination);
			nav_obj.speed = 4.0f;
			
			spawn_nav_obj.transform.position = ObsAgentRespawn.transform.position;// this.transform.position;
			spawn_nav_obj.SetDestination(destination);
			spawn_nav_obj.speed = 8.0f;
    	}
    }
}
