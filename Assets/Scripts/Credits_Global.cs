using UnityEngine;
using System.Collections;

public class Credits_Global : MonoBehaviour {
	
	// Background Texture
	public Texture background;
	
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void Awake () {
    
	}
	
	void OnGUI () {
		
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), background);
		
		GUILayout.BeginArea(new Rect(Screen.width/4, Screen.height/20, Screen.width/2, 100));
		if (GUILayout.Button("Back to Title Screen"))
		{
			Application.LoadLevel("TitleScene");
		}
		GUILayout.EndArea();
	}
}
