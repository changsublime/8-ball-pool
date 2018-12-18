using UnityEngine;
using System.Collections;
using System;

public class CameraController : MonoBehaviour {

	// store a public reference to the Player game object, so we can refer to it's Transform
	public GameObject player;
	public GameObject cueStick;

	// Store a Vector3 offset from the player (a distance to place the camera from the player at all times)
	private Vector3 offsetBall;
	private Vector3 offsetCue;
	private Rigidbody rb;
	private Quaternion startRotation;

	// At the start of the game..
	void Start ()
	{
		rb = GetComponent<Rigidbody>();
		startRotation = transform.rotation;
		// Create an offset by subtracting the Camera's position from the player's position
		cueStick = GameObject.Find("Cue Stick");
		offsetCue = transform.position - cueStick.transform.position;
		offsetBall = transform.position - player.transform.position;
	}

	public void turnChange() {
		transform.rotation = startRotation;
	    rb.MovePosition(player.transform.position + offsetBall);
    }

	// After the standard 'Update()' loop runs, and just before each frame is rendered..
	void FixedUpdate ()
	{
		// Set the position of the Camera (the game object this script is attached to)
		// to the player's position, plus the offset amount
		if (player.GetComponent<Rigidbody>().velocity == new Vector3(0f,0f,0f)) {
			var cueBall = GameObject.Find("Cue Ball");
			float moveHorizontal = Input.GetAxis ("Horizontal");
			transform.RotateAround(cueBall.transform.position, Vector3.up, moveHorizontal);
		}
		else {
			transform.rotation = startRotation;
			rb.MovePosition(player.transform.position + offsetBall);
		}
	}
}