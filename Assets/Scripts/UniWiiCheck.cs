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
	
	[DllImport ("UniWii")]
	private static extern byte wiimote_getIrX(int which);
	
	[DllImport ("UniWii")]
	private static extern float wiimote_getIrY(int which);
	
	[DllImport ("UniWii")]
	private static extern float wiimote_getRoll(int which);
	
	[DllImport ("UniWii")]
	private static extern float wiimote_getPitch(int which);
	
	[DllImport ("UniWii")]
	private static extern float wiimote_getYaw(int which);
	
	[DllImport ("UniWii")]
	private static extern bool wiimote_getButtonA(int which);
	
	[DllImport ("UniWii")]
	private static extern bool wiimote_getButtonB(int which);
	
	[DllImport ("UniWii")]
	private static extern bool wiimote_getButtonUp(int which);
	
	[DllImport ("UniWii")]
	private static extern bool wiimote_getButtonLeft(int which);
	
	[DllImport ("UniWii")]
	private static extern bool wiimote_getButtonRight(int which);
	
	[DllImport ("UniWii")]
	private static extern bool wiimote_getButtonDown(int which);
	
	[DllImport ("UniWii")]
	private static extern bool wiimote_getButton1(int which);
	
	[DllImport ("UniWii")]
	private static extern bool wiimote_getButton2(int which);
	
	[DllImport ("UniWii")]
	private static extern void wiimote_rumble( int which, float duration);
	 
	private String display;
	
	public float XAccel;
	public float YAccel;
	public float ZAccel;
	public float pitch;
	public float roll;
	
	public bool buttonAPressed;
	public bool buttonBPressed;
	public bool button1Pressed;
	public bool button2Pressed;
	public bool buttonUpPressed;
	public bool buttonDownPressed;
	public bool buttonLeftPressed;
	public bool buttonRightPressed;
	
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
        //Debug.Log(display);
		GUI.Label( new Rect(10,Screen.height-100, 500, 100), display);
	}
 
	void Start ()
	{
		wiimote_start();
		wiiCount = wiimote_count();
		float accelX = wiimote_getAccX(0);
		float accelY = wiimote_getAccY(0);
		float accelZ = wiimote_getAccZ(0);
		float irX = wiimote_getIrX(0);
		float irY = wiimote_getIrY(0);
		//float roll = wiimote_getRoll(0);
		//float pitch = wiimote_getPitch(0);
		//float yaw = wiimote_getYaw(0);
		
		buttonAPressed = wiimote_getButtonA(0);
		buttonBPressed = wiimote_getButtonB(0);
	 	button1Pressed = wiimote_getButton1(0);
	 	button2Pressed = wiimote_getButton2(0);
	 	buttonUpPressed = wiimote_getButtonUp(0);
	 	buttonDownPressed = wiimote_getButtonDown(0);
	 	buttonLeftPressed = wiimote_getButtonLeft(0);
	 	buttonRightPressed = wiimote_getButtonRight(0);
	}
	
	void Update()
	{
		XAccel = wiimote_getAccX(0);
		//Debug.Log("X Accel  " + XAccel );
		
		YAccel = wiimote_getAccY(0);
		//Debug.Log("Y Accel  " + YAccel );
		
		ZAccel = wiimote_getAccZ(0);
		//Debug.Log("Z Accel  " + ZAccel );
		
		roll = wiimote_getRoll(0);
		pitch = wiimote_getPitch(0);
		float yaw = wiimote_getYaw(0);
		//Debug.Log("angles are   " + roll+ " " + pitch + " "+ yaw + "Sohail");
		
//		float irX = wiimote_getIrX(0);
//		float irY = wiimote_getIrY(0);
//		Debug.Log("ir X is  " + irX );
//		Debug.Log("ir Y is  " + irY );
//		ZAccel = wiimote_getAccZ(0);
//		if(ZAccel > 180.0f || ZAccel < 110.0f)
//			YAccel = 130.0f;
		
		buttonAPressed = wiimote_getButtonA(0);
		buttonBPressed = wiimote_getButtonB(0);
	 	button1Pressed = wiimote_getButton1(0);
	 	button2Pressed = wiimote_getButton2(0);
	 	buttonUpPressed = wiimote_getButtonUp(0);
	 	buttonDownPressed = wiimote_getButtonDown(0);
	 	buttonLeftPressed = wiimote_getButtonLeft(0);
	 	buttonRightPressed = wiimote_getButtonRight(0);
	}
 
	void OnApplicationQuit() {
		//wiimote_stop();
	}
 
}