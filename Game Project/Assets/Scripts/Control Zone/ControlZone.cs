using UnityEngine;
using System.Collections;

public class ControlZone : MonoBehaviour{
	public Color color = Color.grey;
	public string displayMessage = "Control Zone";
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

	public void GiveControl(Player player){
		color = player.teamColor;
		displayMessage = player.name;
	}
}
