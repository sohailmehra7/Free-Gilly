using UnityEngine;
using System.Collections;

public class Level1_Global : MonoBehaviour {

	public GameObject bubble;
	
	public OVRCameraController CameraController = null;
	
	private Camera MainCam;
	
	public Vector3 direction;
		// SetOVRCameraController
	public void SetOVRCameraController(ref OVRCameraController cameraController)
	{
		CameraController = cameraController;
		CameraController.GetCamera(ref MainCam);
	
	}
	
	// Use this for initialization
	void Start () {
	
		SetOVRCameraController(ref CameraController);
	}
	
	// Update is called once per frame
	void Update () {
	   
		
		if(Input.GetKeyDown(KeyCode.P))
		{
			Vector3 startPos = MainCam.transform.position;
			direction = Vector3.forward;
		    direction = MainCam.transform.rotation * direction;
			
		    Instantiate(bubble, startPos, Quaternion.identity) ;	
		}
		
		
	}
}
