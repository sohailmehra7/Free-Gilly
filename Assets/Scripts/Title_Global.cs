using UnityEngine;
using System.Collections;

public class Title_Global : MonoBehaviour {

	private GUIStyle buttonStyle;
	
	// Background Texture
	public GUITexture background;
	
	// Use this for initialization
	void Start () {
		
		// Set the background
		int backgroundWidth = Screen.width;
		int backgroundHeight = Screen.height;
		background.pixelInset = new Rect(0, 0, backgroundWidth, backgroundHeight);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void Awake () {
    
		background = GameObject.Find("Background").GetComponent("GUITexture") as GUITexture;
	}
	
	void OnGUI () {
		
		GUILayout.BeginArea(new Rect(Screen.width/2 - Screen.width/4, Screen.height/2 + Screen.height/4, Screen.width/2, 400));
		
		if (GUILayout.Button("New Game"))
		{
			Application.LoadLevel("GameScene");
		}
		GUILayout.Space(7);
		
		if (GUILayout.Button("High Scores"))
		{
			Application.LoadLevel("HighScoresScene");
		}
		GUILayout.Space(7);
		
		if (GUILayout.Button("Game Info"))
		{
			Application.LoadLevel("RulesScene");
		}
		GUILayout.Space(7);
		
		/*if (GUILayout.Button("Options"))
		{
			Application.LoadLevel("HighScoresScene");
		}
		GUILayout.Space(7);*/
		
		if (GUILayout.Button("Credits"))
		{
			Application.LoadLevel("CreditsScene");
		}
		GUILayout.Space(7);
	
		if (GUILayout.Button("Exit"))
		{
			Application.Quit();
			Debug.Log ("Application Quit");
		}
		GUILayout.EndArea();
	}
}