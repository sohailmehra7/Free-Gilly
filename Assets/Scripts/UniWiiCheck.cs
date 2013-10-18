using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;
 
public class UniWiiCheck : MonoBehaviour {
 
	[DllImport ("UniWii")]
	private static extern void wiimote_start();
 
	[DllImport ("UniWii")]
	private static extern void wiimote_stop();
 
	[DllImport ("UniWii")]
	private static extern int wiimote_count();
	
	[DllImport ("UniWii")]
	private static extern byte wiimote_getAccX(int which);
	
	[DllImport ("UniWii")]
	private static extern byte wiimote_getAccY(int which);
	
	[DllImport ("UniWii")]
	private static extern byte wiimote_getAccZ(int which);
 
	private String display;
	
	public float YAccel;
	public float ZAccel;
	
	public int wiiCount;
 
	void OnGUI() {
		int c = wiimote_count();
		if (c>0) {
			display = "";
			for (int i=0; i<=c-1; i++) {
				display += "Wiimote " + i + " found!\n";
			}
		}
		else display = "Press the '1' and '2' buttons on your Wii Remote.";
        Debug.Log(display);
		GUI.Label( new Rect(10,Screen.height-100, 500, 100), display);
	}
 
	void Start ()
	{
		wiimote_start();
		wiiCount = wiimote_count();
	}
	
	void Update()
	{
		YAccel = wiimote_getAccY(0);
		Debug.Log("the acceleeration is " + ZAccel );
		ZAccel = wiimote_getAccZ(0);
		if(ZAccel > 180.0f || ZAccel < 110.0f)
			YAccel = 130.0f;
	}
 
	void OnApplicationQuit() {
		//wiimote_stop();
	}
 
}