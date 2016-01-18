using UnityEngine;
using System.Collections;

public class TextIndicator : MonoBehaviour {
	public string displayMessage;
	GameObject zone;
	ControlZone controlZone;
	// Use this for initialization
	void Start () {
		zone = transform.parent.gameObject;
		controlZone = zone.GetComponent<ControlZone>();
	}
	
	// Update is called once per frame
	void Update () {
		displayMessage = controlZone.displayMessage;
	}
}
