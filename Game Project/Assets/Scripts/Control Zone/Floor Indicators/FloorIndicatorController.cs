using UnityEngine;
using System.Collections;

public class FloorIndicatorController: MonoBehaviour {
	public Color color = Color.grey;
	GameObject parent;
	ControlZone zone;
	// Use this for initialization
	void Start () {
		parent = transform.parent.gameObject;
		zone = parent.GetComponent<ControlZone>();
	}
	
	// Update is called once per frame
	void Update () {
		color = zone.color;
	}
}
