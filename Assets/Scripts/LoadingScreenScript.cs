using UnityEngine;
using System.Collections;

public class LoadingScreenScript: MonoBehaviour {

	private AsyncOperation ao;
	
	public Texture background;

	void Start()
	{
		ao = Application.LoadLevelAsync(PlayerPrefs.GetString("SceneToLoad"));
	}
	
	void OnGUI() {
		
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), background);
//		if(ao != null)
//		{
//			Debug.Log(ao.progress);
//			GUI.Box(new Rect(0, Screen.height - 40, ao.progress * Screen.width, 40), "");
//		}
	}

}