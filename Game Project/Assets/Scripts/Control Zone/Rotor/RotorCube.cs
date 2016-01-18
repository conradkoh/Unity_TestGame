using UnityEngine;
using System.Collections;

public class RotorCube : MonoBehaviour {
	public Color color = Color.grey;
	private Renderer renderer;
	private GameObject parent;
	// Use this for initialization
	void Start () {
		renderer = GetComponent<Renderer>();
		parent = transform.parent.gameObject;
		//renderer.material.color = color;
	}
	
	// Update is called once per frame
	void Update () {
		Rotor handler = parent.GetComponent<Rotor>();
		renderer.material.color = handler.color;
	}
}
