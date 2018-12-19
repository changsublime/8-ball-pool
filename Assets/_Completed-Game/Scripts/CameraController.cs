using UnityEngine;
using System.Collections;
using System;

public class CameraController : MonoBehaviour {

	// store a public reference to the Player game object, so we can refer to it's Transform
	public GameObject player;
	public GameObject cueStick;
	public GameObject TargetPosition;

	// Store a Vector3 offset from the player (a distance to place the camera from the player at all times)
	private Vector3 offsetBall;
	private Vector3 offsetCue;
	private Rigidbody rb;
	private Quaternion startRotation;
	private Vector3 startPosition;
	private Vector3	rbtRelativeToCue;
	private CueStickController2 cueStickScript;

	private bool cueView = true;

	private float animationSpeed = 0.01f;
	private bool dark = false;
	private Color startColor;
	private Color darkColor;
	private Quaternion birdsEye;
	private Vector3 birdsEyePos; 


	// At the start of the game..
	void Start ()
	{
		birdsEye = Quaternion.Euler(90f,90f,0f);
		birdsEyePos = new Vector3(0f,25f,0f);
		darkColor = new Color(20/255f,20/255f,20/255f);
		rb = GetComponent<Rigidbody>();
		startRotation = transform.rotation;
		startPosition = transform.position;
		startColor = RenderSettings.ambientSkyColor;
		// Create an offset by subtracting the Camera's position from the player's position	
		cueStick = GameObject.Find("Cue Stick");
		cueStickScript = cueStick.GetComponent<CueStickController2>();
		rbtRelativeToCue = cueStick.transform.InverseTransformPoint(transform.position);
		offsetCue = transform.position - cueStick.transform.position;
		offsetBall = transform.position - player.transform.position;
	}

	public void turnChange() {
		transform.rotation = startRotation;
	    rb.MovePosition(player.transform.position + offsetBall);
    }

    public void resetCamera() {
    	transform.rotation = startRotation;
    	transform.position = startPosition;
    }

    public bool isDark() {
    	return dark;
    }

    void Update() {
    	if (Input.GetKeyDown("b")) {
    		dark = !dark;
    	}
    	if (dark) {
    		RenderSettings.ambientSkyColor = Color.Lerp(RenderSettings.ambientSkyColor,darkColor,.01f);
    	}
    	else {
    		RenderSettings.ambientSkyColor = Color.Lerp(RenderSettings.ambientSkyColor,startColor,.01f);
    	}
    }

	// After the standard 'Update()' loop runs, and just before each frame is rendered..
	void FixedUpdate ()
	{
		// Set the position of the Camera (the game object this script is attached to)
		// to the player's position, plus the offset amount
		if (cueStickScript.noBallsMoving()) {
			if (cueView == false){
				transform.rotation = Quaternion.Lerp(transform.rotation, birdsEye, animationSpeed);
				transform.position = Vector3.Lerp(transform.position, birdsEyePos, animationSpeed);
			} else {
				transform.position = Vector3.Lerp(transform.position, cueStick.transform.TransformPoint(rbtRelativeToCue), .2f);
				transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(player.transform.position - transform.position, Vector3.up) , .2f);
			}
			if (Input.GetKeyDown(KeyCode.LeftShift)){
         		cueView = !cueView;
     		}
		}
		else {
			cueView = true;
			transform.rotation = Quaternion.Lerp(transform.rotation, birdsEye, animationSpeed);
			transform.position = Vector3.Lerp(transform.position, birdsEyePos, animationSpeed);
		}
	}
}