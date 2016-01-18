using UnityEngine;
using System.Collections;

public class ZoneStatus : MonoBehaviour {
	TextMesh textMesh;
	GameObject handler;
	TextIndicator textIndicator;
	// Use this for initialization
	void Start () {
		textMesh = GetComponent<TextMesh>();
		handler = transform.parent.gameObject;
		textIndicator = handler.GetComponent<TextIndicator>();
	}
	
	// Update is called once per frame
	void Update () {
		textMesh.text = textIndicator.displayMessage;
	}
}
