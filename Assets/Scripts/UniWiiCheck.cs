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
	
	[DllImport ("UniWii")]
	private static extern bool wiimote_getButtonPlus(int which);
	
	[DllImport ("UniWii")]
	private static extern bool wiimote_getButtonMinus(int which);
	 
	[DllImport ("UniWii")]
	private static extern bool wiimote_getButtonHome(int which);
	 
	private String display;
	
	public float[] XAccel;
	public float[] YAccel;
	public float[] ZAccel;
	public float[] yaw;
	public float[] pitch;
	public float[] roll;
	public float[] irX;
	public float[] irY;
	
	public bool[] buttonAPressed;
	public bool[] buttonBPressed;
	public bool[] button1Pressed;
	public bool[] button2Pressed;
	public bool[] buttonUpPressed;
	public bool[] buttonDownPressed;
	public bool[] buttonLeftPressed;
	public bool[] buttonRightPressed;
	
	public bool[] buttonHomePressed;
	public bool[] buttonPlusPressed;
	public bool[] buttonMinusPressed;
	
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
		XAccel = new float[wiiCount];
		YAccel = new float[wiiCount];
		ZAccel = new float[wiiCount];
		yaw    = new float[wiiCount];
		pitch  = new float[wiiCount];
		roll   = new float[wiiCount];
		irX    = new float[wiiCount];
		irY    = new float[wiiCount];
		
		buttonAPressed      = new bool[wiiCount];
		buttonBPressed      = new bool[wiiCount];
		button1Pressed      = new bool[wiiCount];
		button2Pressed      = new bool[wiiCount];
		buttonUpPressed     = new bool[wiiCount];
		buttonDownPressed   = new bool[wiiCount];
		buttonLeftPressed   = new bool[wiiCount];
		buttonRightPressed  = new bool[wiiCount];
		
		buttonHomePressed  = new bool[wiiCount];
		buttonPlusPressed  = new bool[wiiCount];
		buttonMinusPressed = new bool[wiiCount];
		
		for(int i=0; i < wiiCount; i++)
		{
			XAccel[i] = wiimote_getAccX(i);
			YAccel[i] = wiimote_getAccY(i);
			ZAccel[i] = wiimote_getAccZ(i);
			yaw[i]    = wiimote_getYaw(i);
		    pitch[i]  = wiimote_getPitch(i);
		    roll[i]   = wiimote_getRoll(i);
			irX[i] = wiimote_getIrX(i);
			irY[i] = wiimote_getIrY(i);
			
			buttonAPressed[i]    = wiimote_getButtonA(i);
			buttonBPressed[i]    = wiimote_getButtonB(i);
		 	button1Pressed[i]    = wiimote_getButton1(i);
		 	button2Pressed[i]    = wiimote_getButton2(i);
		 	buttonUpPressed[i]   = wiimote_getButtonUp(i);
		 	buttonDownPressed[i] = wiimote_getButtonDown(i);
		 	buttonLeftPressed[i] = wiimote_getButtonLeft(i);
		 	buttonRightPressed[i] = wiimote_getButtonRight(i);
			
			buttonHomePressed[i]  = wiimote_getButtonHome(i);
			buttonPlusPressed[i]  = wiimote_getButtonPlus(i);
			buttonMinusPressed[i] = wiimote_getButtonMinus(i);
			
		}
		
	}
	
	void Update()
	{
		for(int i=0; i < wiiCount; i++)
		{
			XAccel[i] = wiimote_getAccX(i);
			YAccel[i] = wiimote_getAccY(i);
			ZAccel[i] = wiimote_getAccZ(i);
			yaw[i]    = wiimote_getYaw(i);
		    pitch[i]  = wiimote_getPitch(i);
		    roll[i]   = wiimote_getRoll(i);
			irX[i] = wiimote_getIrX(i);
			irY[i] = wiimote_getIrY(i);
			
			buttonAPressed[i]    = wiimote_getButtonA(i);
			buttonBPressed[i]    = wiimote_getButtonB(i);
		 	button1Pressed[i]    = wiimote_getButton1(i);
		 	button2Pressed[i]    = wiimote_getButton2(i);
		 	buttonUpPressed[i]   = wiimote_getButtonUp(i);
		 	buttonDownPressed[i] = wiimote_getButtonDown(i);
		 	buttonLeftPressed[i] = wiimote_getButtonLeft(i);
		 	buttonRightPressed[i] = wiimote_getButtonRight(i);
			
			buttonHomePressed[i]  = wiimote_getButtonHome(i);
			buttonPlusPressed[i]  = wiimote_getButtonPlus(i);
			buttonMinusPressed[i] = wiimote_getButtonMinus(i);
			
			//Debug.Log("Accels are   " + XAccel[0]+ " " + YAccel[0] + " "+ ZAccel[0] );
			//Debug.Log("angles are   " + roll[0]+ " " + pitch[0] + " "+ yaw[0] );
		}
		//Debug.Log("angles are   " + roll+ " " + pitch + " "+ yaw + "Sohail");
	}
 
	void OnApplicationQuit() {
		//wiimote_stop();
	}
 
}