using UnityEngine;
using System.Collections;

public class Box : MonoBehaviour, IInteraction {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Interact(){
		Renderer render = GetComponent<Renderer>();
		render.material.color = Color.green;
		Debug.Log("Green");
	}
}
