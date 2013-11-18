using UnityEngine;
using System.Collections;

public class ObstacleDestroy : MonoBehaviour {
	
	private float obstacleLifeTimer;
	
	// Use this for initialization
	void Start () {
	
		obstacleLifeTimer = Constants.OBSTACLE_DESTROY_TIMER;
	}
	
	// Update is called once per frame
	void Update () {
		
		obstacleLifeTimer -= Time.deltaTime;
		
		if(obstacleLifeTimer <= 0)
			Destroy(gameObject);
	}
}
