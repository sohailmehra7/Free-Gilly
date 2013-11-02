using UnityEngine;
using System.Collections;

public class CreditsUI : MonoBehaviour {
	
	// Background Texture
	//public GUITexture background;
	
	// Use this for initialization
	void Start () {
	
		// Set the background
		int backgroundWidth = Screen.width;
		int backgroundHeight = Screen.height;
		//background.pixelInset = new Rect(0, 0, backgroundWidth, backgroundHeight);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void Awake () {
    
		//background = GameObject.Find("Background").GetComponent("GUITexture") as GUITexture;
	}
	
	void OnGUI () {
		
		GUILayout.BeginArea(new Rect(Screen.width/4, Screen.height/20, Screen.width/2, 400));
		if (GUILayout.Button("Back to Title Screen"))
		{
			Application.LoadLevel("TitleScene");
		}
		GUILayout.EndArea();
	}
}
