using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {
	
	public bool isPaused;
	
	private string[] buttonNames = {"Continue", "Restart Level", "Achievement List", "Return to Title Screen"};
	private bool[] buttons;
	private int currentSelection = 0;
 
	// Key press precision
	private float keyTimer;
	private bool keyEnabled;
	
	// Use this for initialization
	void Start () {
	
		buttons = new bool[buttonNames.Length];
		isPaused = false;
		
		keyEnabled = true;
	}
	
	// Update is called once per frame
	void Update () {
	
		if(Input.GetKeyDown("0") && isPaused == false)
			pause();
   
		else if(Input.GetKeyDown("0") && isPaused == true) 
   			unPause();
	}
	
	void OnGUI() {
	
		if(isPaused == true)
		{
			GUILayout.BeginArea(new Rect(Screen.width/2, Screen.height/2 - 125, Screen.width/2, 100));
			GUILayout.Label("GAME PAUSED");
       		GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(Screen.width/4, Screen.height/2 + 100, Screen.width/2, 400));
			
			for(int i=0; i <buttonNames.Length; i++) 
			{
       			GUI.SetNextControlName(buttonNames[i]);
       			buttons[i] = GUILayout.Button(buttonNames[i]);
				GUILayout.Space(3);
    		}
			
			if(Input.GetKeyDown(KeyCode.Return))
			{
       			// When the use key is pressed, the selected button will activate
       			buttons[currentSelection] = true;
			}
			
			if(buttons[0])
			{
				unPause();
			}
			
			if(buttons[1])
			{
				unPause();
				PlayerPrefs.SetString("SceneToLoad", PlayerPrefs.GetString("SceneToLoad"));
				Application.LoadLevel("LoadingScreen");
			}
			
			if(buttons[2])
			{
				
			}
			
			if(buttons[3])
			{
				unPause();
				Application.LoadLevel("TitleScene");
			}
			
			// Cycling through buttons
			if(Input.GetKeyDown(KeyCode.DownArrow)) {
				
				Debug.Log("Called +1");
				currentSelection++;
				
				// Loop back to top of list
				if(currentSelection == buttonNames.Length)
					currentSelection = 0;
        		
				GUI.FocusControl(buttonNames[currentSelection]);
    		}
    		if(Input.GetKeyDown(KeyCode.UpArrow)) {
				
				currentSelection--;
				
				// Loop back to bottom of list
				if(currentSelection == -1)
					currentSelection = buttonNames.Length - 1;
				
        		GUI.FocusControl(buttonNames[currentSelection]);
   		 	}
			//Debug.Log(currentSelection);
			GUILayout.EndArea();
		}
	}
	
	public void pause() {
		
		isPaused = true;
   		Time.timeScale = 0;
	}
	
	public void unPause() {
		
		isPaused = false;
   		Time.timeScale = 1;
	}
	
}
