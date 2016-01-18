using UnityEngine;
using System.Collections;
[RequireComponent(typeof(TextMesh))]
public class HealthIndicator : MonoBehaviour {
	GameObject player;
	TextMesh textMesh;
	Stats stats;
	// Use this for initialization
	void Start () {
		player = transform.parent.gameObject;
		stats = player.GetComponent<Stats>();
		textMesh = GetComponent<TextMesh>();
	}
	
	// Update is called once per frame
	void Update () {
		textMesh.text = stats.health.ToString();
	}
}
