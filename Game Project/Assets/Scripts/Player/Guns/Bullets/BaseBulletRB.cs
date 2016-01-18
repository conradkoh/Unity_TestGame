using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]

public class BaseBulletRB : MonoBehaviour {
	Rigidbody bullet;
	public float damage = 0;
	public float speed = 1.0f;
	public Vector3 direction;
	int variability = 1;
	// Use this for initialization
	void Start () {
		bullet = GetComponent<Rigidbody>();
		//damage = transform.parent.gameObject.GetComponent<BaseGun>().bulletDmg;
		Accelerate();
	}
	
	// Update is called once per frame
	void Update () {
		//Accelerate();
	}
	void RotationalVariability(){
		variability = (variability) % 5 + 1;
	}
	void Accelerate(){
		bullet.AddForce(Vector3.Normalize(direction) * speed, ForceMode.Impulse);
//		Debug.Log("direction: " + direction);
//		bullet.Move(Vector3.Normalize(direction) * speed);
		//transform.position = (transform.position + direction) * 0.2f * variability;
	}
	
	void OnCollisionEnter(Collision damagedObjectCollider){
		GameObject damagedObject = damagedObjectCollider.gameObject;
		Stats dmgObjStat = damagedObject.GetComponent<Stats>();
		dmgObjStat.TakeDamage(damage);
		Debug.Log("give: " + damage);
		Destroy(gameObject);
	}
	
	
}
