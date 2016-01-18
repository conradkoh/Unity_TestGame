using UnityEngine;
using System.Collections;

public class ControlZoneActivator : MonoBehaviour {
	GameObject parent;
	ControlZone controlZone;
	int occupantCount = 0;

	// Use this for initialization
	void Start () {
		parent = transform.parent.gameObject;
		controlZone = parent.GetComponent<ControlZone>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider collider){
		GameObject go = collider.gameObject;
		Player player = go.GetComponent<Player>();
		Debug.Log ("Enter");
		if(occupantCount == 0){
			controlZone.GiveControl(player);
		}
		occupantCount++;

		
	}
	
	void OnTriggerExit(){
		//controlZone.GiveControl(Color.gray);
		occupantCount--;
		Debug.Log ("Exit");
	}
}
