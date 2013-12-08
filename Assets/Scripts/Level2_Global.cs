using UnityEngine;
using System.Collections;

public class Level2_Global : MonoBehaviour {
	
//	// SetOVRCameraController
//	public void SetOVRCameraController(ref OVRCameraController cameraController)
//	{
//		CameraController = cameraController;
//		CameraController.GetCamera(ref MainCam);
//	}
	
	// Use this for initialization
	void Start () {
	
		// Refresh PlayerPrefs
		PlayerPrefs.SetInt("Level", 2);
		PlayerPrefs.SetInt("Complete", 0);
		//PlayerPrefs.SetInt("Score", score);
		//PlayerPrefs.SetFloat("Time", timer);
		//PlayerPrefsX.SetStringArray("AchievementList", LEVEL1_ACH);
		//PlayerPrefsX.SetIntArray("AchievementTracker", LEVEL1_ACH_TRACKER);
	}
	
	void Awake() {
		
		// Play the game
		Time.timeScale = 1;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void shootBubble()
	{
//		if(bubblesLeft > 0)
//		{		
//			Vector3 startPos = MainCam.transform.position;
//			direction = Vector3.forward;
//		    direction = MainCam.transform.rotation * direction;
//			
//			Vector3 dir = direction;
//			dir.Normalize();
//			Vector3 offset = dir * Constants.BUBBLE_SPAWN_OFFSET;
//		    
//			// Create bubble
//			Instantiate(bubble, startPos + offset, Quaternion.identity);
//			
//			// Play sound
//			AudioSource.PlayClipAtPoint(audioScript.bubblePopSound, startPos + offset);
//				
//			// Update bubbles left counter
//			if(Constants.UNLIMITED_BUBBLES == false)
//				bubblesLeft--;
//		}
	}
	
	void useHealthPowerUp()
	{
//		if(storedHealthPU == true)
//		{
//			currentHealth += 25;
//				
//			if(currentHealth > 100)
//				currentHealth = 100;
//				
//			storedHealthPU = false;
//			
//			// Update achievement tracker
//			LEVEL1_ACH_TRACKER[1] = 0;
//		}	
	}
	
	void useStaminaPowerUp()
	{
//		if(storedStaminaPU == true)
//		{
//			currentStamina += 25;
//				
//			if(currentStamina > 100)
//				currentStamina = 100;
//				
//			storedStaminaPU = false;
//		}
	}
	
	void setUpPhysics() {
	
		Physics.IgnoreLayerCollision(10, 10, true);
		Physics.IgnoreLayerCollision(10, 11, true);
		Physics.IgnoreLayerCollision(10, 12, true);
		Physics.IgnoreLayerCollision(10, 13, true);
		Physics.IgnoreLayerCollision(11, 11, true);
		Physics.IgnoreLayerCollision(11, 12, true);
		Physics.IgnoreLayerCollision(11, 13, true);
		Physics.IgnoreLayerCollision(12, 12, true);
		Physics.IgnoreLayerCollision(12, 13, true);
		Physics.IgnoreLayerCollision(13, 13, true);
	}
	
}
