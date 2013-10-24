using UnityEngine;
using System.Collections;

static class Constants {
	
    public const int MAX_BUBBLES = 5;
	public const float BUBBLE_REGEN_TIME = 3.0f;
	public const float BUBBLE_SPAWN_OFFSET = 25.0f;
	
	//Used in Navigation Script 
	public const int MOVE_SPEED = 2;
}

public class Level1_Global : MonoBehaviour {

	public GameObject bubble;
	public GameObject g;
	
	public OVRCameraController CameraController = null;
	private Camera MainCam;
	
	public Vector3 direction;
	public Vector3 startPosition;
	
	// Health and stamina stats
	public int currentHealth;
	public int maxHealth;
	public int currentStamina;
	public int maxStamina;
	
	// Power-Ups
	public bool storedHealthPU;
	public bool storedStaminaPU;
	
	// Bubbles
	public int bubblesLeft;
	public float bubbleRegenTimer;
	
	// Score
	public int score;
	
	// SetOVRCameraController
	public void SetOVRCameraController(ref OVRCameraController cameraController)
	{
		CameraController = cameraController;
		CameraController.GetCamera(ref MainCam);
	}
	
	// Use this for initialization
	void Start () {
	    
		g = GameObject.Find("Gilly");
		SetOVRCameraController(ref CameraController);
		
		currentHealth = 100;
		maxHealth = 100;
		currentStamina = 100;
		maxStamina = 100;
		
		storedHealthPU = false;
		storedStaminaPU = false;
		
		bubblesLeft = Constants.MAX_BUBBLES;
		bubbleRegenTimer = Constants.BUBBLE_REGEN_TIME;
		
		score = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
		// Game over condition
		if(currentHealth <= 0 )
		{
			Debug.Log("Game Over!");
			
			// Restart from last checkpoint or return to main menu
		}
		
		if(bubblesLeft < Constants.MAX_BUBBLES)
		{
			bubbleRegenTimer -= Time.deltaTime;
			if(bubbleRegenTimer <= 0)
			{
				// Add a bubble and reset timer
				bubblesLeft++;
				bubbleRegenTimer = Constants.BUBBLE_REGEN_TIME;
			}
		}
		
		// Shoot bubble
		if(Input.GetKeyDown(KeyCode.P))
		{
			if(bubblesLeft > 0)
			{
				startPosition= g.transform.position;
				Vector3 startPos = MainCam.transform.position;
				direction = Vector3.forward;
		    	direction = MainCam.transform.rotation * direction;
			
				Vector3 dir = direction;
				dir.Normalize();
				Vector3 offset = dir * Constants.BUBBLE_SPAWN_OFFSET;
				startPosition = startPosition + offset;
		    
				// Create bubble
				Instantiate(bubble, startPos + offset, Quaternion.identity);
			
				// Update bubbles left counter
				bubblesLeft--;
			}
		}	
	}
	

}
