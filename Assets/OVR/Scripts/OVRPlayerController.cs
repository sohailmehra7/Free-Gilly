/************************************************************************************

Filename    :   OVRPlayerController.cs
Content     :   Player controller interface. 
				This script drives OVR camera as well as controls the locomotion
				of the player, and handles physical contact in the world.	
Created     :   January 8, 2013
Authors     :   Peter Giokaris

Copyright   :   Copyright 2013 Oculus VR, Inc. All Rights reserved.

Use of this software is subject to the terms of the Oculus LLC license
agreement provided at the time of installation or download, or which
otherwise accompanies this software in either electronic or hard copy form.

************************************************************************************/

using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController))]

//-------------------------------------------------------------------------------------
// ***** OVRPlayerController
//
// OVRPlayerController implements a basic first person controller for the Rift. It is 
// attached to the OVRPlayerController prefab, which has an OVRCameraController attached
// to it. 
// 
// The controller will interact properly with a Unity scene, provided that the scene has
// collision assigned to it. 
//
// The OVRPlayerController prefab has an empty GameObject attached to it called 
// ForwardDirection. This game object contains the matrix which motor control bases it
// direction on. This game object should also house the body geometry which will be seen
// by the player.
//
public class OVRPlayerController : OVRComponent
{
	// Wii Data
	private UniWiiCheck uniWii;
	
	private Level1_Global globalObj;
	private Level2_Global globalObj2;
	
	// Getting navigating agent
	private NavMeshAgent nav_obj;
	private NavMeshAgent obs_nav_obj;
	
	protected CharacterController 	Controller 		 = null;
	protected OVRCameraController 	CameraController = null;

	public float Acceleration 	   = 0.1f;
	public float Damping 		   = 0.15f;
	public float BackAndSideDampen = 0.5f;
	public float JumpForce 		   = 0.3f;
	public float RotationAmount    = 1.5f;
	public float GravityModifier   = 0.379f;
		
	private float   MoveScale 	   = 1.0f;
	private Vector3 MoveThrottle   = Vector3.zero;
	private float   FallSpeed 	   = 0.0f;
	
	// Initial direction of controller (passed down into CameraController)
	private Quaternion OrientationOffset = Quaternion.identity;			
	// Rotation amount from inputs (passed down into CameraController)
	private float 	YRotation 	 = 0.0f;
	
	// Transfom used to point player in a given direction; 
	// We should attach objects to this if we want them to rotate 
	// separately from the head (i.e. the body)
	protected Transform DirXform = null;
	
	// We can adjust these to influence speed and rotation of player controller
	private float MoveScaleMultiplier     = 10.0f; 
	private float RotationScaleMultiplier = 1.0f; 
	private bool  AllowMouseRotation      = false;
	private bool  HaltUpdateMovement      = false;
	
	// TEST: Get Y from second sensor
	private float YfromSensor2            = 0.0f;
	
	public bool inDrop = false;
	// * * * * * * * * * * * * *
	
	// Awake
	new public virtual void Awake()
	{
		base.Awake();
		
		// We use Controller to move player around
		Controller = gameObject.GetComponent<CharacterController>();
		
		if(Controller == null)
			Debug.LogWarning("OVRPlayerController: No CharacterController attached.");
					
		// We use OVRCameraController to set rotations to cameras, 
		// and to be influenced by rotation
		OVRCameraController[] CameraControllers;
		CameraControllers = gameObject.GetComponentsInChildren<OVRCameraController>();
		
		if(CameraControllers.Length == 0)
			Debug.LogWarning("OVRPlayerController: No OVRCameraController attached.");
		else if (CameraControllers.Length > 1)
			Debug.LogWarning("OVRPlayerController: More then 1 OVRCameraController attached.");
		else
			CameraController = CameraControllers[0];	
	
		// Instantiate a Transform from the main game object (will be used to 
		// direct the motion of the PlayerController, as well as used to rotate
		// a visible body attached to the controller)
		DirXform = null;
		Transform[] Xforms = gameObject.GetComponentsInChildren<Transform>();
		
		for(int i = 0; i < Xforms.Length; i++)
		{
			if(Xforms[i].name == "ForwardDirection")
			{
				DirXform = Xforms[i];
				break;
			}
		}
		
		if(DirXform == null)
			Debug.LogWarning("OVRPlayerController: ForwardDirection game object not found. Do not use.");
	}

	// Start
	new public virtual void Start()
	{
		base.Start();
		
		InitializeInputs();	
		SetCameras();
		
		//getting uniwiicheck script
		GameObject gl = GameObject.Find("Global");
		uniWii = gl.GetComponent<UniWiiCheck>();
		
		if(PlayerPrefs.GetInt("Level") == 1)
			globalObj = gl.GetComponent<Level1_Global>();
		
		else if(PlayerPrefs.GetInt("Level") == 2)
			globalObj2 = gl.GetComponent<Level2_Global>();
		
		nav_obj = GameObject.FindGameObjectWithTag("NavAgent").GetComponent<NavMeshAgent>();
		obs_nav_obj = GameObject.FindGameObjectWithTag("ObsNavAgent").GetComponent<NavMeshAgent>();
	}
		
	// Update 
	new public virtual void Update()
	{
		base.Update();
		
		// Test: get Y from sensor 2 
		if(OVRDevice.SensorCount == 2)
		{
			Quaternion q = Quaternion.identity;
			OVRDevice.GetPredictedOrientation(1, ref q);
			YfromSensor2 = q.eulerAngles.y;
		}
		
		UpdateMovement();

		Vector3 moveDirection = Vector3.zero;
		
		float motorDamp = (1.0f + (Damping * DeltaTime));
		MoveThrottle.x /= motorDamp;
		MoveThrottle.y = (MoveThrottle.y > 0.0f) ? (MoveThrottle.y / motorDamp) : MoveThrottle.y;
		MoveThrottle.z /= motorDamp;

		moveDirection += MoveThrottle * DeltaTime;
		
		// Gravity
		if (Controller.isGrounded && FallSpeed <= 0)
			FallSpeed = ((Physics.gravity.y * (GravityModifier * 0.002f)));	
		else
			FallSpeed += ((Physics.gravity.y * (GravityModifier * 0.002f)) * DeltaTime);	

		moveDirection.y += FallSpeed * DeltaTime;

		// Offset correction for uneven ground
		float bumpUpOffset = 0.0f;
		
		if (Controller.isGrounded && MoveThrottle.y <= 0.001f)
		{
			bumpUpOffset = Mathf.Max(Controller.stepOffset, 
									 new Vector3(moveDirection.x, 0, moveDirection.z).magnitude); 
			moveDirection -= bumpUpOffset * Vector3.up;
		}			
	 
		Vector3 predictedXZ = Vector3.Scale((Controller.transform.localPosition + moveDirection), 
											 new Vector3(1, 0, 1));	
		
		// Move contoller
		Controller.Move(moveDirection);
		
		Vector3 actualXZ = Vector3.Scale(Controller.transform.localPosition, new Vector3(1, 0, 1));
		
		if (predictedXZ != actualXZ)
			MoveThrottle += (actualXZ - predictedXZ) / DeltaTime; 
		
		// Update rotation using CameraController transform, possibly proving some rules for 
		// sliding the rotation for a more natural movement and body visual
		UpdatePlayerForwardDirTransform();
	}
		
	// UpdateMovement
	//
	// COnsolidate all movement code here
	//
	static float sDeltaRotationOld = 0.0f;
	public virtual void UpdateMovement()
	{
		// Do not apply input if we are showing a level selection display
		if(HaltUpdateMovement == true)
			return;
	
		bool moveForward = false;
		bool moveLeft  	 = false;
		bool moveRight   = false;
		bool moveBack    = false;
		bool moveUp   	 = false;
		bool moveDown    = false;
		
		MoveScale = 1.0f;

		if(nav_obj.remainingDistance >= 3.0f && inDrop == false)
		{
			if(nav_obj.speed > Constants.DEFAULT_FLOW_SPEED)
				nav_obj.speed -= 0.25f;
			
			if(nav_obj.speed < Constants.DEFAULT_FLOW_SPEED)
				nav_obj.speed = Constants.DEFAULT_FLOW_SPEED;
			
			//nav_obj.speed = Constants.DEFAULT_FLOW_SPEED;
			obs_nav_obj.speed = Constants.DEFAULT_OBSAGENT_SPEED;
		}	
			
		// * * * * * * * * * * *
		// Keyboard input
			
		// Move
			
		// WASD
		if (Input.GetKey(KeyCode.W)) moveUp 	 = true;
		if (Input.GetKey(KeyCode.A)) moveLeft	 = true;
		if (Input.GetKey(KeyCode.S)) moveDown 	 = true; 
		if (Input.GetKey(KeyCode.D)) moveRight 	 = true;
		if (Input.GetKey(KeyCode.R)) moveBack 	 = true;
		
		if(PlayerPrefs.GetInt("Level") == 1)
		{
			if (Input.GetKey(KeyCode.T) && globalObj.currentStamina > 0) moveForward = true;
		}
		else
		{
			if (Input.GetKey(KeyCode.T)) moveForward = true;
		}
		
		// Arrow keys
		if (Input.GetKey(KeyCode.UpArrow))    	moveUp 	  	= true;
		if (Input.GetKey(KeyCode.LeftArrow))  	moveLeft 	= true;
		if (Input.GetKey(KeyCode.DownArrow))  	moveDown 	= true; 
		if (Input.GetKey(KeyCode.RightArrow)) 	moveRight   = true; 
		
		// Wii inputs
		int c = uniWii.wiiCount;
		if(c > 1) 
		{
			// Move forward (buttons)
			if(PlayerPrefs.GetInt("Level") == 1)
			{
				if (uniWii.buttonAPressed[0] && globalObj.currentStamina > 0) 
					moveForward = true;
			}
			else
			{
				if (uniWii.buttonAPressed[0]) 
					moveForward = true;
			}
		
			// Move back (buttons)
			if (uniWii.buttonBPressed[0]) 
				moveBack = true;  
			
			// Move back (motion)
			//else if((uniWii.ZAccel < 95.0f || uniWii.ZAccel > 180.0f) && (uniWii.pitch < -125.0f && uniWii.pitch > -180.0f) || (uniWii.pitch > 155.0f && uniWii.pitch < 180.0f))
				//moveBack = true;
			
			// If both buttons are pressed
			if(uniWii.button1Pressed[0] && uniWii.button1Pressed[1])
			{   Debug.Log ("up executed");
				if(((uniWii.ZAccel[0] < 80.0f || uniWii.ZAccel[0] > 130.0f) && (uniWii.roll[0] < 45 && uniWii.roll[0] > -45)) && 
					((uniWii.ZAccel[1] < 80.0f || uniWii.ZAccel[1] > 130.0f) && (uniWii.roll[1] < 45 && uniWii.roll[1] > -45)))
					moveUp = true;
				
				else if(((uniWii.ZAccel[0] < 80.0f || uniWii.ZAccel[0] > 140.0f) && ((uniWii.roll[0] < -125 && uniWii.roll[0] > -180) || (uniWii.roll[0] < 180 && uniWii.roll[0] > 125))) &&
					 (((uniWii.ZAccel[0] < 980.0f || uniWii.ZAccel[0] > 140.0f) && ((uniWii.roll[0] < -125 && uniWii.roll[0] > -180) || (uniWii.roll[0] < 180 && uniWii.roll[0] > 125)))))
					moveDown = true;
				
				if( (uniWii.roll[0] < 45 && uniWii.roll[0] > -45)    && (uniWii.roll[1] < 45 && uniWii.roll[1] > -45))
					moveUp = true;
				
				else if(((uniWii.roll[0] < -125 && uniWii.roll[0] > -180) || (uniWii.roll[0] < 180 && uniWii.roll[0] > 125)) &&
					 ( ((uniWii.roll[0] < -125 && uniWii.roll[0] > -180) || (uniWii.roll[0] < 180 && uniWii.roll[0] > 125)))  )
					moveDown = true;
			}
			
			// If one button is pressed
			else if(uniWii.button1Pressed[0] || uniWii.button1Pressed[1])
			{
				// Move left (motion)	
				//if(uniWii.roll < -45 && uniWii.roll > -145)	
				if (uniWii.button1Pressed[0] && ((uniWii.YAccel[0] > 150.0f) && (uniWii.pitch[0] > 45.0f && uniWii.pitch[0] < 150.0f)))
					moveLeft = true;
			
				// Move right (motion)
				//else if(uniWii.roll > 45 && uniWii.roll < 145)
				else if (uniWii.button1Pressed[1] && ((uniWii.YAccel[1] > 150.0f) && (uniWii.pitch[1] > 45.0f && uniWii.pitch[1] < 150.0f)))	
					moveRight = true;
			}
	
			// Move forward (motion)
			//((uniWii.roll < -120 &&  uniWii.roll > -180) || (uniWii.roll > 120 &&  uniWii.roll < 180))
			//uniWii.YAccel < 120.0f ||	
			else if(((uniWii.YAccel[0] > 155.0f) && (uniWii.pitch[0] > 45.0f && uniWii.pitch[0] < 150.0f)) &&
			  ((uniWii.YAccel[1] > 155.0f) && (uniWii.pitch[1] > 45.0f && uniWii.pitch[1] < 150.0f)))
				moveForward = true;
			
			// D-pad
			if(uniWii.buttonUpPressed[0] || uniWii.buttonUpPressed[1])    		moveUp 	      = true;
			if(uniWii.buttonLeftPressed[0] || uniWii.buttonLeftPressed[1])  	moveLeft 	  = true;
			if(uniWii.buttonDownPressed[0] || uniWii.buttonDownPressed[1])  	moveDown 	  = true; 
			if(uniWii.buttonRightPressed[0] || uniWii.buttonRightPressed[1]) 	moveRight     = true; 
		}
		
			
		if ((moveForward && moveLeft) || (moveForward && moveRight) || (moveBack && moveLeft) || (moveBack && moveRight))
			MoveScale = 0.70710678f;
			
		// No positional movement if we are in the air
		//if (!Controller.isGrounded)	
			//MoveScale = 0.0f;
			
		MoveScale *= DeltaTime;
			
		// Compute this for key movement
		float moveInfluence = Acceleration * 0.1f * MoveScale * MoveScaleMultiplier;
			
		// Run!
		if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
			moveInfluence *= 2.0f;
			
		if(DirXform != null)
		{
			if(moveForward)
			{
				if(PlayerPrefs.GetInt("Level") == 1)
				{
					if(nav_obj.remainingDistance >= 10.0f)
					{	   
						// Thrust without drop
						if(inDrop == false)
						{
							nav_obj.speed = Constants.THRUST_SPEED;
							obs_nav_obj.speed = Constants.THRUST_SPEED;
						}
						// Thrust while in drop
						else
						{
							nav_obj.speed = Constants.DROP_SPEED;
							obs_nav_obj.speed = Constants.DROP_SPEED;	
						}
					
						// Decrease stamina
						if(!Constants.UNLIMITED_STAMINA)
							globalObj.currentStamina -= Constants.STAMINA_DEC_RATE;
					}
					else
					{
						MoveThrottle += DirXform.TransformDirection(Vector3.forward * moveInfluence);
					}
				}
				else
				{
					MoveThrottle += DirXform.TransformDirection(Vector3.forward * moveInfluence);
				}
			}
			if (moveBack)
			{
				if(PlayerPrefs.GetInt("Level") == 1)
				{
					if(nav_obj.remainingDistance >= float.Epsilon)
					{
						if(inDrop == false)
						{
							nav_obj.speed = Constants.SLOW_SPEED;
						}
					
						// Decrease stamina
						globalObj.currentStamina -= Constants.STAMINA_DEC_RATE;
					}
					else
						MoveThrottle += DirXform.TransformDirection(Vector3.back * moveInfluence) * BackAndSideDampen;
				}
				else
				{
					MoveThrottle += DirXform.TransformDirection(Vector3.back * moveInfluence) * BackAndSideDampen;
				}
			}
			if (moveLeft)
			{
				if(PlayerPrefs.GetInt("Level") == 1)
				{
					if(nav_obj.remainingDistance >= 10.0f)
					{
						nav_obj.transform.position += DirXform.TransformDirection(Vector3.left * moveInfluence) * BackAndSideDampen * 10.0f;
					}
					else
					{
						MoveThrottle += DirXform.TransformDirection(Vector3.left * moveInfluence) * BackAndSideDampen;
					}
				}
				else
					MoveThrottle += DirXform.TransformDirection(Vector3.left * moveInfluence) * BackAndSideDampen;
					
			}
			if (moveRight)
			{
				if(PlayerPrefs.GetInt("Level") == 1)
				{
					if(nav_obj.remainingDistance >= 10.0f)
					{
						nav_obj.transform.position +=DirXform.TransformDirection(Vector3.right * moveInfluence) * BackAndSideDampen * 10.0f;
					}
					else
					{
						MoveThrottle += DirXform.TransformDirection(Vector3.right * moveInfluence) * BackAndSideDampen;
					}
				}
				else
					MoveThrottle += DirXform.TransformDirection(Vector3.right * moveInfluence) * BackAndSideDampen;
					
			}
			if (moveUp)
				MoveThrottle += DirXform.TransformDirection(Vector3.up * moveInfluence);
			if (moveDown)
				MoveThrottle += DirXform.TransformDirection(Vector3.down * moveInfluence) * BackAndSideDampen;
		}
			
		// Rotate
			
		// compute for key rotation
		float rotateInfluence = DeltaTime * RotationAmount * RotationScaleMultiplier;
			
		//reduce by half to avoid getting ill
		if (Input.GetKey(KeyCode.Q)) 
			YRotation -= rotateInfluence * 0.5f;  
		if (Input.GetKey(KeyCode.E)) 
			YRotation += rotateInfluence * 0.5f; 
		
		// * * * * * * * * * * *
		// Mouse input
			
		// Move
			
		// Rotate
		float deltaRotation = 0.0f;
		if(AllowMouseRotation == false)
			deltaRotation = Input.GetAxis("Mouse X") * rotateInfluence * 3.25f;
			
		float filteredDeltaRotation = (sDeltaRotationOld * 0.0f) + (deltaRotation * 1.0f);
		YRotation += filteredDeltaRotation;
		sDeltaRotationOld = filteredDeltaRotation;
			
		// * * * * * * * * * * *
		// XBox controller input	
			
		// Compute this for xinput movement
		moveInfluence = Acceleration * 0.1f * MoveScale * MoveScaleMultiplier;
			
		// Run!
		moveInfluence *= 1.0f + 
					     OVRGamepadController.GPC_GetAxis((int)OVRGamepadController.Axis.LeftTrigger);
			
		// Move
		if(DirXform != null)
		{
			float leftAxisY = 
				OVRGamepadController.GPC_GetAxis((int)OVRGamepadController.Axis.LeftYAxis);
				
			float leftAxisX = 
			OVRGamepadController.GPC_GetAxis((int)OVRGamepadController.Axis.LeftXAxis);
						
			if(leftAxisY > 0.0f)
	    		MoveThrottle += leftAxisY *
				DirXform.TransformDirection(Vector3.forward * moveInfluence);
				
			if(leftAxisY < 0.0f)
	    		MoveThrottle += Mathf.Abs(leftAxisY) *		
				DirXform.TransformDirection(Vector3.back * moveInfluence) * BackAndSideDampen;
				
			if(leftAxisX < 0.0f)
	    		MoveThrottle += Mathf.Abs(leftAxisX) *
				DirXform.TransformDirection(Vector3.left * moveInfluence) * BackAndSideDampen;
				
			if(leftAxisX > 0.0f)
				MoveThrottle += leftAxisX *
				DirXform.TransformDirection(Vector3.right * moveInfluence) * BackAndSideDampen;

		}
			
		float rightAxisX = 
		OVRGamepadController.GPC_GetAxis((int)OVRGamepadController.Axis.RightXAxis);
			
		// Rotate
		YRotation += rightAxisX * rotateInfluence;
		
		// Auto orientation
		if(nav_obj.remainingDistance >= 5.0f) //15//&& (nav_obj.remainingDistance != float.NegativeInfinity && nav_obj.remainingDistance != float.PositiveInfinity))
		{
			YRotation = nav_obj.transform.rotation.eulerAngles.y;
		}
		
		// Update cameras direction and rotation
		SetCameras();

	}

	// UpdatePlayerControllerRotation
	// This function will be used to 'slide' PlayerController rotation around based on 
	// CameraController. For now, we are simply copying the CameraController rotation into 
	// PlayerController, so that the PlayerController always faces the direction of the 
	// CameraController. When we add a body, this will change a bit..
	public virtual void UpdatePlayerForwardDirTransform()
	{
		if ((DirXform != null) && (CameraController != null))
		{
			Quaternion q = Quaternion.identity;
			q = Quaternion.Euler(0.0f, YfromSensor2, 0.0f);
			DirXform.rotation = q * CameraController.transform.rotation;
		}
	}
	
	///////////////////////////////////////////////////////////
	// PUBLIC FUNCTIONS
	///////////////////////////////////////////////////////////
	
	// Jump
	public bool Jump()
	{
		if (!Controller.isGrounded)
			return false;

		MoveThrottle += new Vector3(0, JumpForce, 0);

		return true;
	}

	// Stop
	public void Stop()
	{
		Controller.Move(Vector3.zero);
		MoveThrottle = Vector3.zero;
		FallSpeed = 0.0f;
	}	
	
	// InitializeInputs
	public void InitializeInputs()
	{
		// Get our start direction
		OrientationOffset = transform.rotation;
		// Make sure to set y rotation to 0 degrees
		YRotation = 0.0f;
	}
	
	// SetCameras
	public void SetCameras()
	{
		if(CameraController != null)
		{
			// Make sure to set the initial direction of the camera 
			// to match the game player direction
			CameraController.SetOrientationOffset(OrientationOffset);
			CameraController.SetYRotation(YRotation);
		}
	}
	
	// Get/SetMoveScaleMultiplier
	public void GetMoveScaleMultiplier(ref float moveScaleMultiplier)
	{
		moveScaleMultiplier = MoveScaleMultiplier;
	}
	public void SetMoveScaleMultiplier(float moveScaleMultiplier)
	{
		MoveScaleMultiplier = moveScaleMultiplier;
	}
	
	// Get/SetRotationScaleMultiplier
	public void GetRotationScaleMultiplier(ref float rotationScaleMultiplier)
	{
		rotationScaleMultiplier = RotationScaleMultiplier;
	}
	public void SetRotationScaleMultiplier(float rotationScaleMultiplier)
	{
		RotationScaleMultiplier = rotationScaleMultiplier;
	}
	
	// Get/SetAllowMouseRotation
	public void GetAllowMouseRotation(ref bool allowMouseRotation)
	{
		allowMouseRotation = AllowMouseRotation;
	}
	public void SetAllowMouseRotation(bool allowMouseRotation)
	{
		AllowMouseRotation = allowMouseRotation;
	}
	
	// Get/SetHaltUpdateMovement
	public void GetHaltUpdateMovement(ref bool haltUpdateMovement)
	{
		haltUpdateMovement = HaltUpdateMovement;
	}
	public void SetHaltUpdateMovement(bool haltUpdateMovement)
	{
		HaltUpdateMovement = haltUpdateMovement;
	}

}

