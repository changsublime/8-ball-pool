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
	private bool turnChanged = true;
	private float hitTime = 0f;
	private float startTime = 0f;
	private float endTime = 0f;
	private bool spacePressed = false; 
	private Vector3 startPosition;
	private bool scratched = false;
	private bool movable = true;
	private Quaternion startRotation;
	private CameraController camera;
	private float lightIntensity = 0f;
    private Light lt;
    private CueStickController2 cueStickScript;
    private GameObject cueStick;
    private CameraController cameraScript;
    private Renderer cueStickRenderer;
    private Vector3 rightMove;
    private Vector3 leftMove;
	// At the start of the game..
	void Start ()
	{
		// Assign the Rigidbody component to our private rb variable
		rightMove = new Vector3(-.05f,0f,0f);
		leftMove = new Vector3(.05f,0f,0f);
		rb = GetComponent<Rigidbody>();
		lt = GetComponent<Light>();
		startPosition = transform.position;
		startRotation = transform.rotation;
		camera = GameObject.Find("Main Camera").GetComponent<CameraController>();
		cueStick = GameObject.Find("Cue Stick");
		cueStickScript = cueStick.GetComponent<CueStickController2>();
		cameraScript = camera.GetComponent<CameraController>();
		cueStickRenderer = cueStick.GetComponent<Renderer>();
	}

	public bool isMoving() {
		return moving;
	}

	public void resetBall() {
		transform.position = startPosition;
		transform.rotation = startRotation;
		rb.AddForce(Vector3.zero);
		rb.velocity = Vector3.zero;
		scratched = false;
		movable = true;
	}

	void Update() {
		if (tag != "8 Ball") {
 			if (camera.isDark()) {
 				lightIntensity = Mathf.Lerp(lightIntensity,5f,.01f);
 				lt.intensity = lightIntensity;
 			}
 			else {
 				lightIntensity = Mathf.Lerp(lightIntensity,0f,.01f);
 				lt.intensity = lightIntensity;
 			}
		}
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
		if (movable) {
			if (Input.GetKey("j")) {
				if ((transform.position + leftMove).x < 9 && (transform.position + leftMove).x > -9) {
					transform.position = transform.position + leftMove;
					cueStickScript.followBall(leftMove);
				}
			}
			if (Input.GetKey("l")) {
				if ((transform.position + rightMove).x < 9 && (transform.position + rightMove).x > -9) {
					transform.position = transform.position + rightMove;
					cueStickScript.followBall(rightMove);
				}
			}
		}
	}

	// Each physics step..
	void FixedUpdate ()
	{
		// Set some local float variables equal to the value of our Horizontal and Vertical Inputs
		if (scratched && cueStickScript.noBallsMoving()) {
			transform.position = startPosition;
			rb.AddForce(Vector3.zero);
			rb.velocity = Vector3.zero;
			transform.rotation = startRotation;
			turnChanged = false;
			moving = false;
			scratched = false;
			movable = true;
			cueStickScript.turnChange();
			cameraScript.turnChange();
			turnChanged = true;
		}
		// Create a Vector3 variable, and assign X and Z to feature our horizontal and vertical float variables above
		//Vector3 movement = new Vector3 (0.0f, 0.0f, moveVertical);

		// Add a physical force to our Player rigidbody using our 'movement' Vector3 above, 
		// multiplying it by 'speed' - our public player speed that appears in the inspector
		//rb.AddForce (movement * speed);
		if (rb.velocity.magnitude < .3 && !(rb.velocity == Vector3.zero)) {
			rb.velocity = Vector3.zero;
			transform.rotation = startRotation;
			rb.angularVelocity = Vector3.zero;
			moving = false;
		}
		if (rb.velocity == Vector3.zero) {
			transform.rotation = startRotation;
		}
		if (cueStickScript.noBallsMoving() && !turnChanged && !scratched) {
			cueStickScript.turnChange();
			cameraScript.turnChange();
			turnChanged = true;
		}
		if (rb.velocity != Vector3.zero && !moving) {
			moving = true;
			turnChanged = false;
			cueStickRenderer.enabled = false;
			movable = false;
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