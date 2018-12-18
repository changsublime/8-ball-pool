using UnityEngine;

// Include the namespace required to use Unity UI
using UnityEngine.UI;

using System.Collections;
using System;

public class PlayerController : MonoBehaviour {
	
	// Create public variables for player speed, and for the Text UI game objects
	public float speed;

	// Create private references to the rigidbody component on the player, and the count of pick up objects picked up so far
	private Rigidbody rb;
	private bool moving = false;
	private bool turnChanged = false;
	private float hitTime = 0f;
	private float startTime = 0f;
	private float endTime = 0f;
	private bool spacePressed = false; 
	private Vector3 startPosition;
	private bool scratched =  false;
	// At the start of the game..
	void Start ()
	{
		// Assign the Rigidbody component to our private rb variable
		rb = GetComponent<Rigidbody>();

		startPosition = transform.position;

	}

	public bool isMoving() {
		return moving;
	}

	public void resetBall() {
		transform.position = startPosition;
		rb.AddForce(Vector3.zero);
		rb.velocity = Vector3.zero;
		scratched = false;
	}

	void Update() {
		var cueStick = GameObject.Find("Cue Stick");
		var cueStickScript = cueStick.GetComponent<CueStickController2>();
		if (Input.GetKeyDown("space") && !moving && turnChanged && !spacePressed) {
			startTime = Time.frameCount;
			spacePressed = true;
		}
		if (Input.GetKeyUp("space") && !moving && turnChanged && spacePressed) {
			endTime = Time.frameCount;
			hitTime = Mathf.Min(endTime - startTime,120);
			Vector3 direction = transform.position - cueStick.transform.position;
			direction = new Vector3(direction.x, 0f,  direction.z);
			direction = direction / direction.magnitude;
			Vector3 force = direction * hitTime * speed;
			rb.AddForce(direction * hitTime * speed, ForceMode.Impulse);
			spacePressed = false;
		}
	}

	// Each physics step..
	void FixedUpdate ()
	{
		// Set some local float variables equal to the value of our Horizontal and Vertical Inputs
		var cueStick = GameObject.Find("Cue Stick");
		var cueStickScript = cueStick.GetComponent<CueStickController2>();
		var camera = GameObject.Find("Main Camera");
		var cameraScript = camera.GetComponent<CameraController>();
		if (scratched) {
			transform.position = startPosition;
			rb.AddForce(Vector3.zero);
			rb.velocity = Vector3.zero;
			scratched = false;
		}
		// Create a Vector3 variable, and assign X and Z to feature our horizontal and vertical float variables above
		//Vector3 movement = new Vector3 (0.0f, 0.0f, moveVertical);

		// Add a physical force to our Player rigidbody using our 'movement' Vector3 above, 
		// multiplying it by 'speed' - our public player speed that appears in the inspector
		//rb.AddForce (movement * speed);
		if (rb.velocity.magnitude < .3 && !(rb.velocity == new Vector3(0f,0f,0f))) {
			rb.velocity = new Vector3(0f,0f,0f);
			moving = false;
		}
		if (GameObject.Find("Cue Stick").GetComponent<CueStickController2>().noBallsMoving() && !turnChanged) {
			cueStickScript.turnChange();
			cameraScript.turnChange();
			turnChanged = true;
		}
		if (rb.velocity != new Vector3(0f,0f,0f)) {
			moving = true;
			turnChanged = false;
			cueStick.GetComponent<Renderer>().enabled = false;
		}
	}

	// When this game object intersects a collider with 'is trigger' checked, 
	// store a reference to that collider in a variable named 'other'..
	void OnTriggerEnter(Collider other) 
	{
		// ..and if the game object we intersect has the tag 'Pick Up' assigned to it..
		if (other.gameObject.CompareTag("Scoring Plane"))
		{
			// Make the other game object (the pick up) inactive, to make it disappear
			scratched = true;
		}
	}
}