using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {
	public Camera fpCamera;
	public Camera tpCamera;
	public float movementSpeed = 30;
	public float mouseSensitivity = 2;
	public float gravity = 10.0f;

	private enum CONTROLS{ FP, TP};
	private CONTROLS controlMode = CONTROLS.FP;
	private float verticalAngle = 0;
	private float angleLimit = 80.0f;
	private float verticalDisplacement = 0;


	// Use this for initialization
	void Start () {
		InitializeControlMode();
	}

	// Update is called once per frame
	void Update () {
		HandleUserControls(controlMode);
	}

	void HandleUserControls(CONTROLS control){
		if(controlMode == CONTROLS.FP){
			HandleFirstPersonControls();
		}
		else if(controlMode == CONTROLS.TP){
			HandleThirdPersonControls();
		}
	}

	#region First Person Methods
	void HandleFirstPersonControls(){
		CharacterController control = GetComponent<CharacterController>();
		HandleFPMouseControls(control);
		HandleFPKeyboardControls(control);
	}

	void HandleFPMouseControls(CharacterController controller){
		
		float lrRotation = Input.GetAxis("Mouse X") * mouseSensitivity;
		verticalAngle -= Input.GetAxis("Mouse Y") * mouseSensitivity;

		verticalAngle = Mathf.Clamp(verticalAngle, -angleLimit, angleLimit);

		transform.Rotate(0, lrRotation, 0);
		fpCamera.transform.localRotation = Quaternion.Euler(verticalAngle, 0, 0);
	}

	void HandleFPKeyboardControls(CharacterController controller){

		float forwardDisplacement = Input.GetAxis("Vertical") * movementSpeed;
		float sidewayDisplacement = Input.GetAxis("Horizontal") * movementSpeed;
		if(Input.GetKey(KeyCode.Space) && controller.isGrounded){

			verticalDisplacement = 0;
			verticalDisplacement += 10;
		}
		verticalDisplacement -= gravity * Time.deltaTime;
		Vector3 displacement = new Vector3(sidewayDisplacement, verticalDisplacement, forwardDisplacement);
		displacement = transform.rotation * displacement;
		controller.Move(displacement * Time.deltaTime);

		if(Input.GetKeyDown("y")){
			ToggleControlMode();
		}
		if(Input.GetKeyDown("e")){
			GameObject go = GameObject.Find("Iris Door");
			IrisDoor door = go.GetComponent<IrisDoor>();
			door.ToggleDoorStatus();
		}

	}
	#endregion

	#region Third Person Methods

	void HandleThirdPersonControls(){
		if(Input.GetKeyDown("y")){
			ToggleControlMode();
		}
	}
	#endregion

	#region General Low Level Methods
	void InitializeControlMode(){
		if(controlMode == CONTROLS.FP){
			SetControlMode(CONTROLS.FP);

		}
		else if(controlMode == CONTROLS.TP){
			SetControlMode(CONTROLS.FP);
		}
	}
	void ToggleControlMode(){
		if(controlMode == CONTROLS.FP){
			SetControlMode(CONTROLS.TP);
		}
		else if(controlMode == CONTROLS.TP){
			SetControlMode(CONTROLS.FP);
		}
	}

	void SetControlMode(CONTROLS control){
		if(control == CONTROLS.FP){
			controlMode = CONTROLS.FP;
			fpCamera.enabled = true;
			tpCamera.enabled = false;
			//Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
		else if(control == CONTROLS.TP){
			controlMode = CONTROLS.TP;
			tpCamera.enabled = true;
			fpCamera.enabled = false;
			Cursor.visible = true;
			//Cursor.lockState = CursorLockMode.None;

		}
	}


	#endregion
}
