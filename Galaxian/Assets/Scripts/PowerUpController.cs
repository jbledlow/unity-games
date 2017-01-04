using UnityEngine;
using System.Collections;

public class PowerUpController : MonoBehaviour {
	// Public Variables
	public float speed = 3f;
	public enum PowerUpType{life,auto,other};
	public PowerUpType powerUpType;

	// Private Variables
	Vector3 direction;


	// Use this for initialization
	void Start () {
		direction = new Vector3 (0f, -1f, 0f);
	}
	
	// Update is called once per frame
	void Update () {
		doMove (direction);
	}

	public void doMove(Vector3 directions) {
		transform.position += direction * speed * Time.deltaTime;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag.Equals ("Player")) {
			switch (powerUpType) {
			case(PowerUpType.auto):
				CharacterController2d.equippedGun = "auto";
				DestroyObject (gameObject);
				break;
			case(PowerUpType.life):
				CharacterController2d.lives++;
				other.GetComponent<CharacterController2d> ().updateLifeIndicator ();
				DestroyObject (gameObject);
				break;
			}
		}
	}
}
