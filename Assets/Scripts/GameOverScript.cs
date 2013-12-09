using UnityEngine;
using System.Collections;

public class GameOverScript : MonoBehaviour {
	
	public MovieTexture background;
	
	public string[] LEVEL_ACH;
	public int[] LEVEL_ACH_TRACKER;
	
	private float time;
	private string timeString;
	
	// Use this for initialization
	void Start () {
	
		time = PlayerPrefs.GetFloat("Time");
		
		int minutes = (int)(time / 60);
   		int seconds = (int)(time % 60);
 
   		timeString = string.Format("{0:00}:{1:00}", minutes, seconds);
	}
	
	// Update is called once per frame
	void Update () {
		
		LEVEL_ACH = PlayerPrefsX.GetStringArray("AchievementList");
		LEVEL_ACH_TRACKER = PlayerPrefsX.GetIntArray("AchievementTracker");
	}
	
	void OnGUI() {
	
		// Bonus points
		int bonus = 0;
		
		// Table headers style
		GUIStyle textStyle = new GUIStyle();
		textStyle.fontSize = 12;
		textStyle.fontStyle = FontStyle.Bold;
		textStyle.normal.textColor = Color.white;
		textStyle.alignment = TextAnchor.MiddleCenter;
		
		// Message style
		GUIStyle messageStyle = new GUIStyle();
		messageStyle.fontSize = 14;
		messageStyle.fontStyle = FontStyle.Bold;
		messageStyle.normal.textColor = Color.white;
		messageStyle.alignment = TextAnchor.MiddleCenter;
		
		// Title style
		GUIStyle titleStyle = new GUIStyle();
		titleStyle.fontSize = 24;
		titleStyle.fontStyle = FontStyle.Bold;
		titleStyle.normal.textColor = Color.white;
		titleStyle.alignment = TextAnchor.MiddleCenter;
		
		GUI.DrawTexture(new Rect(-150, -100, Screen.width+Screen.width/5, Screen.height+Screen.height/3), background);
		background.Play();
		
		// Label Area
		GUILayout.BeginArea(new Rect(Screen.width/2 - Screen.width/4, Screen.height/10, Screen.width/2, 100));
		GUILayout.Label("LEVEL " + PlayerPrefs.GetInt("Level") + " SUMMARY:", titleStyle);
		GUILayout.Space(7);
		
		if(PlayerPrefs.GetInt("Complete") == 1)
			GUILayout.Label("Yay, Gilly's free! The unfathomable depths of the ocean are now open for you to explore! Go for it!", messageStyle);
		
		else
			GUILayout.Label("Oh no! Gilly was severly injured. Try again.", messageStyle);
		
		GUILayout.EndArea();
		
		int heightModifer = 0;
		int widthModifer = Screen.width/12;
		int widthModifer1 = Screen.width/3;
		
		// 
		GUILayout.BeginArea(new Rect(Screen.width/2 - Screen.width/4, Screen.height/2 - Screen.height/4, Screen.width/2, Screen.height/2));
		
		// Title row
		GUI.Box(new Rect(0, heightModifer, widthModifer1, 25), "ACHIEVEMENT", textStyle);
		GUI.Box(new Rect(widthModifer1, heightModifer, widthModifer, 25), "PROGRESS", textStyle);
		GUI.Box(new Rect(widthModifer+widthModifer1, heightModifer, widthModifer, 25), "BONUS", textStyle);
		
		heightModifer += 25;
		
		for(int i=0; i<LEVEL_ACH.Length; i++)
		{
			GUI.Box(new Rect(0, heightModifer, widthModifer1, 25), LEVEL_ACH[i]);
			
			if(LEVEL_ACH_TRACKER[i] == 0)
			{
				GUI.Box(new Rect(widthModifer1, heightModifer, widthModifer, 25), "Not Complete");
				GUI.Box(new Rect(widthModifer+widthModifer1, heightModifer, widthModifer, 25), "-");
			}
			
			else if(LEVEL_ACH_TRACKER[i] == 1)
			{
				GUI.Box(new Rect(widthModifer1, heightModifer, widthModifer, 25), "Complete");
				GUI.Box(new Rect(widthModifer+widthModifer1, heightModifer, widthModifer, 25), "+100");
				bonus += 100;
			}
			
			heightModifer += 25;
		}
       	GUILayout.EndArea();
		
		// Final score
		GUILayout.BeginArea(new Rect(Screen.width/2 - Screen.width/4, Screen.height - Screen.height/3, Screen.width/2, 100));
		GUILayout.Label("Score (+Bonus): " + (PlayerPrefs.GetInt("Score")+bonus), messageStyle);
		GUILayout.Label("Time Taken: " + timeString, messageStyle);
		GUILayout.EndArea();
		
		// Navigation Area
		GUILayout.BeginArea(new Rect(Screen.width/2 - Screen.width/4, Screen.height - Screen.height/5, Screen.width/2, 100));
		if(PlayerPrefs.GetInt("Complete") == 1)
		{
			if(GUILayout.Button("Continue"))
			{	
				PlayerPrefs.SetString("SceneToLoad", "Level2");
				Application.LoadLevel("LoadingScreen");
			}
			GUILayout.Space(3);
		}
		
		if(GUILayout.Button("Restart Level"))
		{	
			PlayerPrefs.SetString("SceneToLoad", "Level1");
			Application.LoadLevel("LoadingScreen");
		}
		GUILayout.Space(3);
		
		if(GUILayout.Button("Back to Main Menu"))
		{	
			Application.LoadLevel("TitleScene");
		}
		GUILayout.EndArea();
	}
	
}
