using UnityEngine;
using System.Collections;

public class BoxTrigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(){
		Debug.Log ("Enter");
		GameObject parent = transform.parent.gameObject;
		IInteraction interaction = parent.GetComponent<IInteraction>();
		interaction.Interact();

	}

	void OnTriggerExit(){
		Debug.Log ("Exit");
	}

}
