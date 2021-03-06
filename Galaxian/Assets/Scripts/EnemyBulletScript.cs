using UnityEngine;
using System.Collections;

public class EnemyBulletScript : MonoBehaviour {

	// Reference to player gameObject
	GameObject player;
	public float speed = 100f;

	Vector2 direction;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		if (gameObject.GetComponentInParent<EnemyScript>().targeting == true) {
			direction = (gameObject.transform.position - player.transform.position).normalized;
		} else {
			direction = Vector2.up;
		}
		transform.parent = null;
	}
	
	// Update is called once per frame
	void Update () {
		Move (direction);
	}

	void Move(Vector2 direction) {
		Vector2 pos = gameObject.transform.position;
		pos += direction * speed * Time.deltaTime;
		gameObject.transform.position = pos;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag.Equals("Player")) {
			other.GetComponent<CharacterController2d>().Death();
			DestroyObject (gameObject);
		}
	}
}
