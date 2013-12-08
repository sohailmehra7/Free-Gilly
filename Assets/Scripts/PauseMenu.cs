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
			GUILayout.BeginArea(new Rect(Screen.width/2, Screen.height/2 - 125, Screen.width/2, 100));
			GUILayout.Label("GAME PAUSED");
       		GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(Screen.width/4, Screen.height/2 + 100, Screen.width/2, 400));

			if(GUILayout.Button("Continue"))
			{
				unPause();
			}
			GUILayout.Space(3);
			
			if(GUILayout.Button("Restart Level"))
			{
				unPause();
				Application.LoadLevel(Application.loadedLevel);
			}
			GUILayout.Space(3);
			
//			if(GUILayout.Button("Save Game"))
//			{
//				saveGame();
//			}
//			GUILayout.Space(7);
			
			if(GUILayout.Button("Achievement List"))
			{
				
			}
			GUILayout.Space(3);
			
			if(GUILayout.Button("Return to Title Screen"))
			{
				unPause();
				Application.LoadLevel("TitleScene");
			}
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
