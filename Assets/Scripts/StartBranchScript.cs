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
	
	private Level1_Global globalObj;
	
	// 0 = easy, 1 = hard
	public int branchDifficulty;
	
	// Use this for initialization
	void Start () {
	
		nav = GameObject.FindGameObjectWithTag("NavAgent");
	    nav_obj = nav.GetComponent<NavMeshAgent>();
		spawn_nav_obj = GameObject.FindGameObjectWithTag("ObsNavAgent").GetComponent<NavMeshAgent>();
		
		globalObj = GameObject.Find("Global").GetComponent<Level1_Global>();
		
		destination = endPoint.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
		
	}
	
	void OnTriggerEnter(Collider other)
    {
    	if (other.gameObject.CompareTag("Player"))
		{
			// Set the branch difficulty
			globalObj.branchDiff = branchDifficulty;
			
			if(NavAgentRespawn != null) {
				
				nav_obj.transform.position = NavAgentRespawn.transform.position; //this.transform.position; // + offset
				nav_obj.speed = 4.0f;
			}
			
			nav_obj.SetDestination(destination);
			
			
			if(ObsAgentRespawn != null) {
			
				spawn_nav_obj.transform.position = ObsAgentRespawn.transform.position;// this.transform.position;
				spawn_nav_obj.speed = 8.0f;
			}
			
			spawn_nav_obj.SetDestination(destination);
    	}
    }
}
