using UnityEngine;
using System.Collections;

public class StartBranchScript : MonoBehaviour {
	
	public GameObject endPoint;
	private GameObject nav;
	private NavMeshAgent nav_obj;
	public Vector3 destination ;
	// Use this for initialization
	void Start () {
	
		nav = GameObject.Find("NavAgent");
	    nav_obj = nav.GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
		
		
	}
	
	void OnTriggerEnter(Collider other)
    {
    if (other.gameObject.CompareTag("Player"))
    {
			Debug.Log ("Fish colided");
        destination = endPoint.transform.position ;
			Debug.Log(destination);
			Debug.Log(gameObject.transform.position);
			nav_obj.transform.position = this.transform.position;
			nav_obj.ResetPath ();
			nav_obj.SetDestination(destination);
			nav_obj.speed = 4.0f;
			
    }
    }
}
