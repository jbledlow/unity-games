using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

	public float speed = 6f;

	Vector3 movement;
	Animator anim;
	Rigidbody playerRigidbody;
	int floorMask; //LayerMasks are stored as an integer
	float camRayLength = 100f;


	void Awake() {
		playerRigidbody = GetComponent<Rigidbody> ();
		floorMask = LayerMask.GetMask ("Floor");
		anim = GetComponent <Animator> ();
	}

	void FixedUpdate() {
		float h = Input.GetAxisRaw("Horizontal");
		float v = Input.GetAxisRaw("Vertical");

		Move (h, v);

		Turning ();

		Animating (h, v);

	}

	void Move(float h, float v) {
		movement.Set (h, 0f, v);

		movement = movement.normalized * speed * Time.deltaTime;

		playerRigidbody.MovePosition (transform.position + movement);
	}
    
	void Turning() {
		//Create a ray pointing from the camera to the mouse
		Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

		//Store what the ray hits
		RaycastHit floorHit;

		if (Physics.Raycast (camRay, out floorHit, camRayLength, floorMask)) {
			Vector3 playerToPoint = floorHit.point - transform.position;

			playerToPoint.y = 0f;

			Quaternion newRotation = Quaternion.LookRotation (playerToPoint);

			playerRigidbody.MoveRotation (newRotation);
		}

	}

	void Animating(float h, float v) {
		bool walking = h != 0 || v != 0;
		anim.SetBool ("IsWalking", walking);
	}
}
