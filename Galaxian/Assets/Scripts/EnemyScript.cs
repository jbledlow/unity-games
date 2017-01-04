using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyScript : MonoBehaviour {

	public float minTimeBetweenShots = 3f;
	public ParticleSystem explosion;
	public AudioClip explosionSoundClip;
	public ParticleSystem shieldExplosion;
	Vector3 waypointOne;
	Vector3 waypointTwo;
	public float speed = 5f;
	public float travelDistance = 5f;
	public GameObject bullet;
	public int points = 10;
	public bool hasShield = false;
	public bool targeting = false;
	List<Vector3> targets = new List<Vector3> ();
	public List<GameObject> powerUps;
	[Range(0f,1f)]
	public float powerUpChance = .25f;

	//float direction = -1f;
	Rigidbody2D rbody;
	Vector3 target;
	int targetInt = 0;
	float timeSinceLastShot = 0f;
	GameObject shield;
	AudioSource aSource;
	GameObject player;



	// Use this for initialization
	void Start () {
		waypointOne = new Vector3 (transform.position.x + travelDistance, transform.position.y, transform.position.z);
		waypointTwo = new Vector3 (transform.position.x - travelDistance, transform.position.y, transform.position.z);
		rbody = gameObject.GetComponent<Rigidbody2D> ();
		targets.Add (waypointOne);
		targets.Add (waypointTwo);
		target = targets [targetInt];

		// Set reference to player
		player = GameObject.FindGameObjectWithTag("Player");

		//Set reference to shield object
		shield = gameObject.transform.FindChild("Shield").gameObject;

		if (hasShield) {
			shield.SetActive (true);
		}

		//Set reference to audio source
		aSource = gameObject.GetComponent<AudioSource>();
		Manager.current.AddEnemy ();
	}

	void Update() {
		timeSinceLastShot += Time.deltaTime;
		if (timeSinceLastShot > minTimeBetweenShots & Random.Range(0f,1f) > .98f) {
			Vector3 direction = (gameObject.transform.position - player.transform.position).normalized;
			Quaternion rotation = Quaternion.LookRotation (direction);
			Instantiate (bullet, gameObject.transform.position,Quaternion.identity,transform);
			timeSinceLastShot = 0f;
			//gameObject.GetComponent<AudioSource> ().Play ();
		}
	}

	// Update is called once per frame
	void FixedUpdate () {

		//Vector2 newPos = rbody.position + new Vector2(direction*speed*Time.deltaTime,0f);
		transform.position = Vector3.MoveTowards (transform.position, target, speed * Time.deltaTime);
		if (transform.position == target) {
			targetInt++;
			if (targetInt > targets.Count -1) {
				targetInt = 0;
			}
			target = targets[targetInt];
		}
	}

	public void Dead () {
		explosion.transform.position = gameObject.transform.position;
		Instantiate (explosion);
		DestroyObject (gameObject);
		Manager.current.AddPoint (points);
		Manager.current.PlayExplosion ();
		Manager.current.RemoveEnemy ();
		if (Random.Range (0f, 1f) > 0.75f) {
			Instantiate (powerUps [Random.Range (0, powerUps.Count)],new Vector3(transform.position.x,transform.position.y,transform.position.z),Quaternion.identity);
		}
	}

	public void HitShield () {
		shieldExplosion.transform.position = gameObject.transform.position;
		Instantiate (shieldExplosion);
		DestroyObject (shield);
	}
}
