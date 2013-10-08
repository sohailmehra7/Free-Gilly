using UnityEngine;
using System.Collections;

public class Stamina : MonoBehaviour {

	private float maxStamina;
	private float currentStamina;
	
	private float staminaBarLength;
	
	//private GameObject gl;
	//private Global globalObj;
	
	// Use this for initialization
	void Start ()  {
		
		// Initialize current and max stamina
		//maxStamina = PlayerPrefs.GetInt("MaxStamina");
		maxStamina = 100.0f;
		currentStamina = maxStamina;
		
		staminaBarLength = Screen.width/2.0f;
		
		//gl = GameObject.Find("GlobalObject");
		//globalObj = gl.GetComponent<Global>();
	}
	
	// Update is called once per frame
	void Update () {
		
		
	}
	
	void OnGUI() {
		
		GUI.Box(new Rect(145, 60, staminaBarLength, 20), (int)currentStamina + "/" + (int)maxStamina);
	}
}
