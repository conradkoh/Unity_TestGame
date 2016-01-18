using UnityEngine;
using System.Collections;

public class Stats: MonoBehaviour {
	public float health = 1000;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void TakeDamage(float damage){
		Debug.Log(damage);
		health -= damage;
		if(health <= 0){
			health = 0;
			Destroy (gameObject);
		}
	}
}
