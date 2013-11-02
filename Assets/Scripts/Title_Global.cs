using UnityEngine;
using System.Collections;
using System.IO;

public class Title_Global : MonoBehaviour {

	private GUIStyle buttonStyle;
	
	// Background Texture
	//public GUITexture background;
	
	private Rect levelSelectWindow = new Rect(20, 20, 120, 50);
	private Rect loginWindow = new Rect(20, 20, 120, 50);
	private Rect gameInfoWindow = new Rect(20, 20, 120, 50);
	
	// Window booleans
	private bool showLevelSelectWindow = false;
	private bool showLoginWindow = false;
	private bool showGameInfoWindow = false;
	
	// Player username
	private string username = "default";
	
	// Save file data
	private string loginID = "";
	private int cHealth, mHealth;
	private int cStamina, mStamina;
	private int score;
	private int levelNum, checkpointNum;
	
	
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
		
		GUILayout.BeginArea(new Rect(Screen.width/2 + Screen.width/3, 0, Screen.width/2, 400));
		GUILayout.Label("Player Profile: " + username);
		GUILayout.EndArea();
		
		GUILayout.BeginArea(new Rect(Screen.width/2 - Screen.width/4, Screen.height/2 + Screen.height/8, Screen.width/2, 400));
		
		if (GUILayout.Button("New Game"))
		{
			Application.LoadLevel("Level1");
		}
		GUILayout.Space(7);
		
		if (GUILayout.Button("Continue Game"))
		{
			showLevelSelectWindow = false;
			showLoginWindow = true;
			showGameInfoWindow = false;
		}
		GUILayout.Space(7);
		
		if (GUILayout.Button("Level Select"))
		{
			showLevelSelectWindow = true;
			showLoginWindow = false;
			showGameInfoWindow = false;
		}
		GUILayout.Space(7);
		
		if (GUILayout.Button("High Scores"))
		{
			Application.LoadLevel("HighScoresScene");
		}
		GUILayout.Space(7);
		
		if (GUILayout.Button("Game Info"))
		{
			showLevelSelectWindow = false;
			showLoginWindow = false;
			showGameInfoWindow = true;
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
		
		if(showLevelSelectWindow)
			levelSelectWindow = GUILayout.Window(0, levelSelectWindow, displayLevelSelectWindow, "Level Select");
		
		if(showLoginWindow)
			loginWindow = GUILayout.Window(1, loginWindow, displayLoginWindow, "Login");
		
		if(showGameInfoWindow)
			gameInfoWindow = GUILayout.Window(2, gameInfoWindow, displayGameInfoWindow, "Tutorials");
		
	}
	
	 void displayLevelSelectWindow(int windowID) {
		
        if (GUILayout.Button("Level 1"))
            Application.LoadLevel("Level1");
		
		if (GUILayout.Button("Level 2"))
            Application.LoadLevel("Level1");
		
		if (GUILayout.Button("Level 3"))
            Application.LoadLevel("Level1");
        
    }
	
	void displayLoginWindow(int windowID) {
		
		//username = GUILayout.TextField(username);
		GUILayout.TextField(username);
		
        if (GUILayout.Button("Load Game"))
		{
			username = "sm";
			readSaveFile(username);
			showLoginWindow = false;
		} 
    }
	
	void displayGameInfoWindow(int windowID) {
		
		if (GUILayout.Button("Game Overview"))
            Application.LoadLevel("Level1");
		
        if (GUILayout.Button("Xbox Controller"))
            Application.LoadLevel("Level1");
		
		if (GUILayout.Button("Wii Controller"))
            Application.LoadLevel("Level1");
    }
	
	void readSaveFile(string playerID)
	{
		StreamReader reader = new StreamReader(playerID + "_savefile.txt");
		
		int index = 0;
        string text = "";
		string[] splitStrs;
        while(text != null)
        {
			// Get a line from the highscore file
            text = reader.ReadLine();
			
			// Null test
			if(text == null)
				break;
			
			splitStrs = text.Split(' ');
			
			// Store data
			switch(index)
			{
				case 0:
						loginID = splitStrs[1];
						break;
				case 1:
						cHealth = int.Parse(splitStrs[1]);
						break;
				case 2:
						mHealth = int.Parse(splitStrs[1]);
						break;
				case 3:
						cStamina = int.Parse(splitStrs[1]);
						break;
				case 4:
						mStamina = int.Parse(splitStrs[1]);
						break;
				case 5:
						score = int.Parse(splitStrs[1]);
						break;
				case 6:
						levelNum = int.Parse(splitStrs[1]);
						checkpointNum = int.Parse(splitStrs[3]);
						break;
				case 7:
						break;
				case 8:
						break;
				default:
						break;
			}
		
			index++;
        }
		reader.Close();
	}

}