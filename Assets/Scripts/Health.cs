using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

	private float maxHealth;
	private float currentHealth;
	
	private float healthBarLength;
	
	//private GameObject gl;
	//private Global globalObj;
	
	// Sounds
	public AudioClip gillyDieSound;
	public AudioClip gillyHeartbeatSound;
	
	// Use this for initialization
	void Start ()  {
		
		// Initialize urrent and max health
		//maxHealth = PlayerPrefs.GetInt("MaxHealth");
		maxHealth = 100.0f;
		currentHealth = maxHealth;
		
		healthBarLength = Screen.width/2.0f;
		
		//gl = GameObject.Find("GlobalObject");
		//globalObj = gl.GetComponent<Global>();
	}
	
	// Update is called once per frame
	void Update () {
		
		
	}
	
	void OnGUI() {
		
		GUI.Box(new Rect(145, 20, healthBarLength, 20), (int)currentHealth + "/" + (int)maxHealth);
	}
}

