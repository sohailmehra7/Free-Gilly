using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {
	
	public bool isPaused;
	
	// Use this for initialization
	void Start () {
	
		isPaused = false;
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
			GUILayout.BeginArea(new Rect(Screen.width/2 - Screen.width/10, Screen.height/2 - 125, Screen.width/2 - 125, 100));
			GUILayout.Label("				GAME PAUSED");
       		GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(Screen.width/2 - Screen.width/10, Screen.height/2 + 100, Screen.width - 800, 400));

			if(GUILayout.Button("Continue"))
			{
				unPause();
			}
			GUILayout.Space(7);
			
			if(GUILayout.Button("Restart Level"))
			{
				unPause();
				Application.LoadLevel(Application.loadedLevel);
			}
			GUILayout.Space(7);
			
			if(GUILayout.Button("Save Game"))
			{
				saveGame();
			}
			GUILayout.Space(7);
			
			if(GUILayout.Button("Achievement List"))
			{
				
			}
			GUILayout.Space(7);
			
			if(GUILayout.Button("Return to Title Screen"))
			{
				unPause();
				Application.LoadLevel("TitleScene");
			}
			GUILayout.EndArea();
		}
	}
	
	void pause() {
		
		isPaused = true;
   		Time.timeScale = 0;
	}
	
	void unPause() {
		
		isPaused = false;
   		Time.timeScale = 1;
	}
	
	void saveGame()
	{
		
	}
	
}
