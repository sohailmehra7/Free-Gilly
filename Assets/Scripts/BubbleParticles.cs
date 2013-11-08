using UnityEngine;
using System.Collections;

public class BubbleParticles : MonoBehaviour {
	
	private float timer;
	
	// Use this for initialization
	void Start () {
	
		timer = Constants.BUBBLE_PARTICLE_LIFE_TIME;
	}
	
	// Update is called once per frame
	void Update () {
	
		if(timer <= 0)
			Destroy(gameObject);
		
		else
			timer -= Time.deltaTime;
	}
}
