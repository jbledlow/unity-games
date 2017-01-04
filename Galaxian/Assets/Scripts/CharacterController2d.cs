using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CharacterController2d : MonoBehaviour {


	// Initialize public variables
	[Range(1f,10f)]
	public float speed = 3f;
	public int startHealth = 100;
	public GameObject bullet;
	public ParticleSystem explosion;
	public int startLives = 3;
	public GameObject[] UIlives;

	// Private Variables
	Rigidbody2D rbody;
	Animator animator;
	AudioSource source;
	public static int lives;
	float newTime = 3f;
	bool newSpawned = false;
	float spawnTime = 0;
	[HideInInspector]
	public static string equippedGun;
	float autoDelay = 0.1f;
	float timeSinceLastFire = 0f;


	// Use this for initialization
	void Start () {
		rbody = gameObject.GetComponent<Rigidbody2D> ();
		animator = gameObject.GetComponent<Animator> ();
		source = gameObject.GetComponent<AudioSource> ();
		if (SceneManager.GetActiveScene()==SceneManager.GetSceneByName("Level 1")) {
			lives = startLives;
		}
		if (equippedGun == null) {
			equippedGun = "semi"; 
		}

		updateLifeIndicator ();
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 movement = new Vector2 (Input.GetAxisRaw("Horizontal"),0);
		Move (movement);


		switch (equippedGun) {
		case("semi"):
			if (Input.GetButtonDown ("Fire1")) {
				Instantiate (bullet);
				source.Play ();
			}
			break;
		case("auto"):
			if (Input.GetButton("Fire1")) {
				if (timeSinceLastFire >= autoDelay) {
					Instantiate (bullet);
					source.Play ();
					timeSinceLastFire = 0f;
				} else {
					timeSinceLastFire += Time.deltaTime;
				}
			}
			break;
		}
		// Shoot bullet if space is pressed


		if (newSpawned == true) {
			if (spawnTime >= newTime) {
				newSpawned = false;
				animator.SetBool ("isNew", false);
			} else {
				spawnTime += Time.deltaTime;
			}
		}

	}

	// Move is called during every update 
	void Move(Vector2 movement) {
		// Check to see if movement is left or right
		if (movement == Vector2.left | movement == Vector2.right) {
			// Flip the scale of the sprite and start moving animation
			transform.localScale = new Vector3 (movement.x*-1f, 1f, 1f);
			animator.SetBool ("isMoving", true);
		} else {
			animator.SetBool ("isMoving", false);
		}
		Vector2 pos = transform.position;
		pos += movement * speed * Time.deltaTime;
		transform.position = pos;
	}

	public void Death() {
		if (newSpawned == false) {
			explosion.transform.position = gameObject.transform.position;
			Instantiate (explosion);
			lives--;

			if (lives == 0) {
				DestroyObject (gameObject);
				Manager.current.GameOver ();
			} else {
				respawnPlayer ();
				updateLifeIndicator();
			}
		}
	}

	public void respawnPlayer() {
		animator.SetBool ("isNew", true);
		newSpawned = true;
		spawnTime = 0f;
	}

	public void updateLifeIndicator () {
		for (int i = 0; i < UIlives.Length; i++) {
			if (i < lives - 1) {
				UIlives [i].SetActive (true);
			} else {
				UIlives [i].SetActive (false);
			}
		}
	}
}
