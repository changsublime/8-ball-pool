using UnityEngine;

// Include the namespace required to use Unity UI
using UnityEngine.UI;

using System.Collections;
using System;

public class PlayerController : MonoBehaviour {
	
	// Create public variables for player speed, and for the Text UI game objects
	public float speed;
	public Text countText;
	public Text winText;

	// Create private references to the rigidbody component on the player, and the count of pick up objects picked up so far
	private Rigidbody rb;
	private int count;
	private bool moving = false;
	private bool turnChanged = false;
	private float hitTime = 0f;
	private float startTime = 0f;
	private float endTime = 0f;
	private bool spacePressed = false; 
	// At the start of the game..
	void Start ()
	{
		// Assign the Rigidbody component to our private rb variable
		rb = GetComponent<Rigidbody>();

		// Set the count to zero 
		count = 0;

		// Run the SetCountText function to update the UI (see below)
		SetCountText ();

		// Set the text property of our Win Text UI to an empty string, making the 'You Win' (game over message) blank
		winText.text = "";
	}

	public bool isMoving() {
		return moving;
	}

	void Update() {
		var cueStick = GameObject.Find("Cue Stick");
		var cueStickScript = cueStick.GetComponent<CueStickController2>();
		if (Input.GetKeyDown("space") && !moving && turnChanged && !spacePressed) {
			startTime = Time.time;
			spacePressed = true;
		}
		if (Input.GetKeyUp("space") && !moving && turnChanged && spacePressed) {
			endTime = Time.time;
			hitTime = endTime - startTime;
			Vector3 direction = transform.position - cueStick.transform.position;
			direction = new Vector3(direction.x, 0f,  direction.z);
			direction = direction / direction.magnitude;
			rb.AddForce(direction * hitTime * speed);
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
		// Create a Vector3 variable, and assign X and Z to feature our horizontal and vertical float variables above
		//Vector3 movement = new Vector3 (0.0f, 0.0f, moveVertical);

		// Add a physical force to our Player rigidbody using our 'movement' Vector3 above, 
		// multiplying it by 'speed' - our public player speed that appears in the inspector
		//rb.AddForce (movement * speed);
		if (rb.velocity.magnitude < .1 && !(rb.velocity == new Vector3(0f,0f,0f))) {
			rb.velocity = new Vector3(0f,0f,0f);
			moving = false;
		}
		if (!moving && !turnChanged) {
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
		if (other.gameObject.CompareTag ("Pick Up"))
		{
			// Make the other game object (the pick up) inactive, to make it disappear
			other.gameObject.SetActive (false);

			// Add one to the score variable 'count'
			count = count + 1;

			// Run the 'SetCountText()' function (see below)
			SetCountText ();
		}
	}

	// Create a standalone function that can update the 'countText' UI and check if the required amount to win has been achieved
	void SetCountText()
	{
		// Update the text field of our 'countText' variable
		countText.text = "Count: " + count.ToString ();

		// Check if our 'count' is equal to or exceeded 12
		if (count >= 12) 
		{
			// Set the text value of our 'winText'
			winText.text = "You Win!";
		}
	}
}