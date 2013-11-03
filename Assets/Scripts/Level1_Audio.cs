using UnityEngine;
using System.Collections;

public class Level1_Audio : MonoBehaviour {

	public AudioSource audio1; // Underwater Background
	public AudioSource audio2; // Hearbeat audio
	//public AudioSource audio3;
	Level1_Global globalObj;
	
	//Audio tracks
	private bool playHeartBeat = false;
	private bool isplayingBeat = false;
	
	// Use this for initialization
	void Start () {
		globalObj = gameObject.GetComponent<Level1_Global>();
		//audio2.Play();
	}
	
	// Update is called once per frame
	void Update () {

			if(globalObj.currentHealth <= 90 && isplayingBeat == false)
		{
			isplayingBeat = true;
			audio2.Play();
		}
		
		if(globalObj.currentHealth >= 90 && isplayingBeat == true)
		{
			isplayingBeat = false ;
			audio2.Stop ();
		}
	}
}
