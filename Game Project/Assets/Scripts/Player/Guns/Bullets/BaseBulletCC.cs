using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]

public class BaseBulletCC : MonoBehaviour {
	CharacterController bullet;
	public float damage = 1.0f;
	public float speed = 10.0f;
	public Vector3 direction;
	int variability = 1;
	// Use this for initialization
	void Start () {
		bullet = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
		//Accelerate();
	}
	void RotationalVariability(){
		variability = (variability) % 5 + 1;
	}
	void Accelerate(){
		Debug.Log("direction: " + direction);
		bullet.Move(Vector3.Normalize(direction) * speed);
		//transform.position = (transform.position + direction) * 0.2f * variability;
	}

	void OnCollisionEnter(Collision damagedObjectCollider){
		GameObject damagedObject = damagedObjectCollider.gameObject;
		Stats dmgObjStat = damagedObject.GetComponent<Stats>();
		dmgObjStat.TakeDamage(damage);
		Destroy(this);
	}


}
