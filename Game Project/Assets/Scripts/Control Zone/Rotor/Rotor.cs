using UnityEngine;
using System.Collections;

public class Rotor: MonoBehaviour {
	private GameObject parent;
	public enum DIRECTION {CLOCKWISE, COUNTERCLOCKWISE};
	public float speed = 10;
	public Color color = Color.gray;
	public DIRECTION direction = DIRECTION.CLOCKWISE;
	int rotationAxis = 1;
	// Use this for initialization
	void Start () {
		parent = transform.parent.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		ControlZone zone = parent.GetComponent<ControlZone>();
		color = zone.color;

		if(direction == DIRECTION.CLOCKWISE){
			rotationAxis = 1;
		}
		else{
			rotationAxis = -1;
		}
		//transform.RotateAround(new Vector3(0,0,0), new Vector3(0,1,0), 0.1f * speed);
		transform.RotateAround(transform.position, new Vector3(0,rotationAxis,0), 0.1f * speed);
	}
}
