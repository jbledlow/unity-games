using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {

	// Reference to player gameObject
	GameObject player;
	public float speed = 100f;


	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		if (gameObject.tag == "Bullet") {
			gameObject.transform.position = player.transform.position;
			gameObject.transform.rotation = player.transform.rotation;
		}
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 direction = Vector2.up;
		Move (direction);
	}

	void Move(Vector2 direction) {
		Vector2 pos = gameObject.transform.position;
		pos += direction * speed * Time.deltaTime;
		gameObject.transform.position = pos;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag.Equals("Enemy")) {
			other.GetComponent<EnemyScript>().Dead();
			DestroyObject (gameObject);
		}
		if (other.tag.Equals ("Shield")) {
			other.GetComponentInParent<EnemyScript> ().HitShield ();
			DestroyObject (gameObject);
		}
	}
}
