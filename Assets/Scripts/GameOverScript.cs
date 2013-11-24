using UnityEngine;
using System.Collections;

public class GameOverScript : MonoBehaviour {
	
	public MovieTexture background;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI() {
	
			GUI.DrawTexture(new Rect(-150, -100, Screen.width+Screen.width/5, Screen.height+Screen.height/3), background);
			background.Play();
		
			GUILayout.BeginArea(new Rect(Screen.width/2-Screen.width/35 , Screen.height/2 - Screen.height/10, Screen.width/10, Screen.height/10));
			GUILayout.Label("GAME OVER!!");
       		GUILayout.EndArea();
			
			GUILayout.BeginArea(new Rect(Screen.width/2-Screen.width/20, Screen.height/2-Screen.height/20, Screen.width/10, Screen.height/10));

			if(GUILayout.Button("Restart Level"))
			{
				Application.LoadLevel("Level1");
			}
			GUILayout.Space(7);

			if(GUILayout.Button("Quit to Title Screen"))
			{
				Application.LoadLevel("TitleScene");
			}
			GUILayout.EndArea();
	}
	
}
