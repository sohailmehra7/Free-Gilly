using UnityEngine;
using System.Collections;

public class Level1_Audio : MonoBehaviour {

	public AudioSource audio1; // Underwater Background
	public AudioSource audio2; // Hearbeat audio
	public AudioSource audio3;  //Drop Sound
	
	Level1_Global globalObj;
	
	//Audio tracks
	private bool playHeartBeat = false;
	private bool isplayingBeat = false;
	
	// Audio clips
	public AudioClip obstacleHitSound;
	public AudioClip bubblePopSound;
	public AudioClip powerPickUpSound;
	public AudioClip bloodSplatterSound;
	
	
	// Voiceover
	public AudioClip ouch; 						// "OUCH!"
	public AudioClip dropComment; 				// "That's a drop! Wwwwhhoooooo!!"
	public AudioClip lowHealthComment;			// "I'm not feeling too good."
	public AudioClip lowStaminaComment;			// "I'm getting tired out. I should take it easy for a little while."
	public AudioClip closeDodgeComment;			// "That was a close one."
	public AudioClip useHealthPowerUpComment;	// "That feels much better."
	public AudioClip useStaminaPowerUpC0mment;	// "Ready to race!"
	
	// Use this for initialization
	void Start () {
		
		globalObj = gameObject.GetComponent<Level1_Global>();
		//audio2.Play();
	}
	
	// Update is called once per frame
	void Update () {

		if(globalObj.currentHealth <= Constants.HEARTBEAT_HEALTH && isplayingBeat == false)
		{
			isplayingBeat = true;
			audio2.Play();
		}
		
		if(globalObj.currentHealth >= Constants.HEARTBEAT_HEALTH && isplayingBeat == true)
		{
			isplayingBeat = false;
			audio2.Stop();
		}
		
	
	}
}
