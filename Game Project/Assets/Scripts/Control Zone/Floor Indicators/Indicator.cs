using UnityEngine;
using System.Collections;

public class Indicator : MonoBehaviour {
	GameObject parent;
	Renderer renderer;
	FloorIndicatorController controller;
	// Use this for initialization
	void Start () {
		parent = transform.parent.gameObject;
		renderer = GetComponent<Renderer>();
		controller = parent.GetComponent<FloorIndicatorController>();
	}
	
	// Update is called once per frame
	void Update () {
		renderer.material.color = controller.color;
	}
}
