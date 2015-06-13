using UnityEngine;
using System.Collections;
[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour {
	public Camera fpCamera;
	public Camera tpCamera;
	public float movementSpeed = 30;
	public float mouseSensitivity = 5;
	public float gravity = 10.0f;
	public float jumpHeight = 15.0f;
	public Vector3 camPosition = new Vector3(10, 70, 10);

	private enum CONTROLS{ FP, TP};
	private CONTROLS controlMode = CONTROLS.FP;
	private Vector3 displacement = new Vector3(0, 0, 0);
	//First Person Variables
	private float verticalAngle = 0;
	private float angleLimit = 70.0f;
	private float verticalDisplacement = 0;
	private RaycastHit rayHit;
	private float verticalVelocity = 0;
	private CharacterController controller;
	//Third Person Variables
	bool test = true;


	// Use this for initialization
	void Start () {
		Initialize();
	}

	// Update is called once per frame
	void Update () {
		HandleUserControls(controlMode);
		SetGravityFactoredVerticalVelocity();
		MoveCharacterController();
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
		GameObject go = GetObjectFromCrossHair();
		HandleFPInteraction(go);
		HandleFPMouseControls();
		HandleFPKeyboardControls();
	}

	GameObject GetObjectFromCrossHair(){
		Ray ray = fpCamera.ScreenPointToRay(new Vector3(Screen.width/2, Screen.height/2, 0));
		GameObject foundObject = null;
		if(Physics.Raycast(ray, out rayHit)){
			foundObject = rayHit.collider.gameObject;
			//Debug.Log (foundObject.name);
		}

		return foundObject;
	}

	void HandleFPInteraction(GameObject go){
		if(Input.GetKeyDown("e") && go!= null){
			IInteraction interaction = go.GetComponent<IInteraction>();
			if(interaction != null){
				interaction.Interact();
			}
			else{
				Debug.Log ("Interaction not found!");
			}
		}
	}
	void HandleFPMouseControls(){
		
		float lrRotation = Input.GetAxis("Mouse X") * mouseSensitivity;
		verticalAngle -= Input.GetAxis("Mouse Y") * mouseSensitivity;

		verticalAngle = Mathf.Clamp(verticalAngle, -angleLimit, angleLimit);

		transform.Rotate(0, lrRotation, 0);
		fpCamera.transform.localRotation = Quaternion.Euler(verticalAngle, 0, 0);
	}

	void HandleFPKeyboardControls(){
		float forwardDisplacement = Input.GetAxis("Vertical") * movementSpeed;
		float sidewayDisplacement = Input.GetAxis("Horizontal") * movementSpeed;
		float jumpBurst = 0;

		if(Input.GetKey(KeyCode.Space) && controller.isGrounded){

			if(controller.isGrounded){
				Debug.Log("Captured: " + transform.position.y);
				verticalVelocity = jumpHeight;
				jumpBurst = 3.0f * jumpHeight - gravity;
			}
			else{
				if(test){
					Debug.Log("invalid capt : " + transform.position.y);
				}
				//Debug.Log ("char not grounded");
			}
			//verticalDisplacement -= gravity * Time.deltaTime;
			//Vector3 displacement = new Vector3(sidewayDisplacement, verticalDisplacement + verticalVelocity, forwardDisplacement);


			//verticalVelocity -= gravity;
		}



		if(Input.GetKeyDown("y")){
			ToggleControlMode();
		}

		displacement = new Vector3(sidewayDisplacement, (verticalVelocity + jumpBurst), forwardDisplacement);
		displacement = transform.rotation * displacement;
		Debug.Log(verticalVelocity);
	//		if(Input.GetKeyDown("e")){
//			GameObject go = GameObject.Find("Iris Door");
//			IrisDoor door = go.GetComponent<IrisDoor>();
//			door.ToggleDoorStatus();
//		}

	}
	#endregion

	#region Third Person Methods

	void HandleThirdPersonControls(){
//		if(Input.GetKeyDown("y")){
//			ToggleControlMode();
//		}
		CameraFollowPlayer();
		GameObject go = GetObjectFromCrossHair();
		HandleFPInteraction(go);
		HandleFPMouseControls();
		HandleFPKeyboardControls();
	}

	void CameraFollowPlayer(){
		tpCamera.transform.position = new Vector3(transform.position.x + camPosition.x, transform.position.y + camPosition.y, transform.position.z + camPosition.z);
		tpCamera.transform.LookAt(transform.position);
	}
	#endregion

	#region General Low Level Methods
	void Initialize(){
		InitializeControlMode();
		InitializeCharacterController();
	}
	void InitializeControlMode(){
		if(controlMode == CONTROLS.FP){
			SetControlMode(CONTROLS.FP);

		}
		else if(controlMode == CONTROLS.TP){
			SetControlMode(CONTROLS.FP);
		}
	}

	void InitializeCharacterController(){
		controller = GetComponent<CharacterController>();
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

	void MoveCharacterController(){
		controller.Move(displacement * Time.deltaTime);
		displacement = new Vector3 (0, 0, 0);
	}

	#endregion

	#region Computations

	void SetGravityFactoredVerticalVelocity(){
		if(!controller.isGrounded){
			verticalVelocity -= gravity * Time.deltaTime;
		}
	}
	#endregion
}
