using UnityEngine;
using System.IO;
using System.Collections;

public class HighScores_Global : MonoBehaviour {

	// Background Texture
	public Texture background;
	
	private string[] highScoreData;
	
	int selGrid = 0;
	string[] selStrings = new string[] {"Username", "Score", "Level1_Time", "Level2_Time", "Level3_Time"};
	
	// Use this for initialization
	void Start () {
	
		highScoreData = new string[11];
		
		readHighScoreData();
		
		// Set the background
		//int backgroundWidth = Screen.width;
		//int backgroundHeight = Screen.height;
		//background.pixelInset = new Rect(0, 0, backgroundWidth, backgroundHeight);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void Awake () {
    
		//background = GameObject.Find("Background").GetComponent("GUITexture") as GUITexture;
	}
	
	void OnGUI()
    {	
		// Table headers style
		GUIStyle textStyle = new GUIStyle();
		textStyle.fontSize = 12;
		textStyle.fontStyle = FontStyle.Bold;
		textStyle.normal.textColor = Color.white;
		textStyle.alignment = TextAnchor.MiddleCenter;
		
		// Title style
		GUIStyle titleStyle = new GUIStyle();
		titleStyle.fontSize = 24;
		titleStyle.fontStyle = FontStyle.Bold;
		titleStyle.normal.textColor = Color.white;
		titleStyle.alignment = TextAnchor.MiddleCenter;
		
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), background);
		
		// HighScore Label Area
		GUILayout.BeginArea(new Rect(Screen.width/2 - Screen.width/4, Screen.height/10, Screen.width/2, 100));
        GUILayout.Label("HIGH SCORES:", titleStyle);
		GUILayout.EndArea();
		
		int heightModifer = 0;
		int widthModifer = Screen.width/12;
		
		// Username/Score Area
		GUILayout.BeginArea(new Rect(Screen.width/2 - Screen.width/4, Screen.height/2 - Screen.height/4, Screen.width/2, Screen.height/2));
		
		// Title row
		GUI.Box(new Rect(0, heightModifer, widthModifer, 25), "RANK", textStyle);
		GUI.Box(new Rect(widthModifer, heightModifer, widthModifer, 25), "USERNAME", textStyle);
		GUI.Box(new Rect(widthModifer*2, heightModifer, widthModifer, 25), "SCORE", textStyle);
		GUI.Box(new Rect(widthModifer*3, heightModifer, widthModifer, 25), "LEVEL 1 TIME", textStyle);
		GUI.Box(new Rect(widthModifer*4, heightModifer, widthModifer, 25), "LEVEL 2 TIME", textStyle);
		GUI.Box(new Rect(widthModifer*5, heightModifer, widthModifer, 25), "LEVEL 3 TIME", textStyle);
		heightModifer += 25;
		
		for(int i=1; i<11; i++)
		{
			string[] dataSplit = highScoreData[i].Split();

			GUI.Box(new Rect(0, heightModifer, widthModifer, 25), i + ". ");
			GUI.Box(new Rect(widthModifer, heightModifer, widthModifer, 25), dataSplit[0]);
			GUI.Box(new Rect(widthModifer*2, heightModifer, widthModifer, 25), dataSplit[1]);
			GUI.Box(new Rect(widthModifer*3, heightModifer, widthModifer, 25), dataSplit[2]);
			GUI.Box(new Rect(widthModifer*4, heightModifer, widthModifer, 25), dataSplit[3]);
			GUI.Box(new Rect(widthModifer*5, heightModifer, widthModifer, 25), dataSplit[4]);
			
			heightModifer += 25;
			//GUILayout.Space(15);
		}
       	GUILayout.EndArea();
		
		// Navigation Area
		GUILayout.BeginArea(new Rect(Screen.width/2 - Screen.width/4, Screen.height - Screen.height/10, Screen.width/2, 100));
		if(GUILayout.Button("Back to Main Menu"))
		{	
			Application.LoadLevel("TitleScene");
		}
		GUILayout.EndArea();
    }
	
	void readHighScoreData () {
	
		StreamReader reader = new StreamReader("highScores.txt");
		
		int index = 0;
        string text = "";
        while(text != null)
        {
			// Get a line from the highscore file
            text = reader.ReadLine();
			
			// Null test
			if(text == null)
				break;
			
			// Store all data
			highScoreData[index++] = text;
        }
		reader.Close();
	}

}
