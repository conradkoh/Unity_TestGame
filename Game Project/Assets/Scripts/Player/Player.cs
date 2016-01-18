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
	public Color teamColor = Color.blue;
	public string name = "Grey Character";
	public LayerMask navigationLayer;
	public BaseGun gun;
	public Vector3 direction;
	private GameObject fpUI;
	private NavMeshAgent navMeshAgent;

	private enum CONTROLS{ FP, TP};
	private CONTROLS controlMode = CONTROLS.FP;
	private Vector3 displacement = new Vector3(0, 0, 0);
	//First Person Variables
	private float verticalAngle = 0;
	private float angleLimit = 70.0f;
	private float verticalDisplacement = 0;
	private float verticalVelocity = 0;
	private CharacterController controller;
	private Ray playerCrosshairRay;
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
		RaycastHit rayHit;
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
		//Mouse Movement
		float lrRotation = Input.GetAxis("Mouse X") * mouseSensitivity;
		verticalAngle -= Input.GetAxis("Mouse Y") * mouseSensitivity;

		verticalAngle = Mathf.Clamp(verticalAngle, -angleLimit, angleLimit);

		transform.Rotate(0, lrRotation, 0);
		fpCamera.transform.localRotation = Quaternion.Euler(verticalAngle, 0, 0);

		//==
		playerCrosshairRay = fpCamera.ScreenPointToRay(new Vector3(Screen.width/2, Screen.height/2, 0));
		direction = playerCrosshairRay.direction;
		Debug.DrawRay(fpCamera.transform.position, playerCrosshairRay.direction * 100000, Color.red);

		//Mouse Click
		if(Input.GetMouseButton(0)){
			gun.Fire();
		}
	}

	void HandleFPKeyboardControls(){
		float forwardDisplacement = Input.GetAxis("Vertical") * movementSpeed;
		float sidewayDisplacement = Input.GetAxis("Horizontal") * movementSpeed;
		float jumpBurst = 0;

		if(Input.GetKey(KeyCode.Space) && controller.isGrounded){

			if(controller.isGrounded){
				//Debug.Log("Captured: " + transform.position.y);
				verticalVelocity = jumpHeight;
				jumpBurst = 3.0f * jumpHeight - gravity;
			}
		}



		if(Input.GetKeyDown("y")){
			ToggleControlMode();
		}
		if(Input.GetKeyDown(KeyCode.LeftShift)){
			movementSpeed = movementSpeed * 3;
		}

		if(Input.GetKeyUp(KeyCode.LeftShift)){
			movementSpeed = movementSpeed / 3;
		}

		displacement = new Vector3(sidewayDisplacement, (verticalVelocity + jumpBurst), forwardDisplacement);
		displacement = transform.rotation * displacement;
		//Debug.Log(verticalVelocity);
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
		HandleTPMouseControls();
		HandleFPKeyboardControls();
	}

	void CameraFollowPlayer(){
		tpCamera.transform.position = new Vector3(transform.position.x + camPosition.x, transform.position.y + camPosition.y, transform.position.z + camPosition.z);
		tpCamera.transform.LookAt(transform.position);
	}

	void HandleTPMouseControls(){
		Ray ray = tpCamera.ScreenPointToRay(Input.mousePosition);
		Debug.DrawRay(tpCamera.transform.position, ray.direction * 10000);
		RaycastHit rayHit;
		if(Input.GetMouseButtonDown(1)&& Physics.Raycast(ray, out rayHit,Mathf.Infinity, navigationLayer)){
			StartCoroutine(WaitForNavMeshEnabledToMove(rayHit.point));
			navMeshAgent.SetDestination(rayHit.point);
			Debug.Log (rayHit.point);
			
		}
	}


	#endregion

	#region General Low Level Methods
	void Initialize(){
		InitializeUI();
		InitializeNavMeshAgent();
		InitializeControlMode();
		InitializeCharacterController();
		InitializeLayers();
	}

	void InitializeUI(){
		fpUI = GameObject.Find("First Person UI");
	}
	void InitializeNavMeshAgent(){
		navMeshAgent = GetComponent<NavMeshAgent>();
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
			SetUIMode(CONTROLS.TP);
		}
		else if(controlMode == CONTROLS.TP){
			SetControlMode(CONTROLS.FP);
			SetUIMode(CONTROLS.FP);
		}
	}

	void InitializeLayers(){
		navigationLayer = LayerMask.GetMask("Navigation");
	}

	void SetControlMode(CONTROLS control){
		if(control == CONTROLS.FP){
			controlMode = CONTROLS.FP;
			fpCamera.enabled = true;
			tpCamera.enabled = false;
			Cursor.visible = false;
			navMeshAgent.enabled = false;

		}
		else if(control == CONTROLS.TP){
			controlMode = CONTROLS.TP;
			tpCamera.enabled = true;
			fpCamera.enabled = false;
			Cursor.visible = true;
			StartCoroutine(WaitForGroundToEnableNavMesh());

		}
	}
	void SetUIMode(CONTROLS control){
		if(control == CONTROLS.FP){
			fpUI.SetActive(true);
		}
		else if( control == CONTROLS.TP){
			fpUI.SetActive(false);
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

	#region Coroutines
	IEnumerator WaitForGroundToEnableNavMesh(){
		while(!controller.isGrounded){
			yield return 0;
		}
		navMeshAgent.enabled = true;
		yield return 0;
	}

	IEnumerator WaitForNavMeshEnabledToMove(Vector3 position){
		
		while(!navMeshAgent.enabled == true){
			yield return 0;
		}
		navMeshAgent.SetDestination(position);
	}
	#endregion
}
