using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BallController : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 startPosition;
    private bool restart = false;
    private Rigidbody rb;
    private float lightIntensity = 0f;
    private Light lt;
    private CameraController camera;
    private GameObject cueStick;
    private CueStickController2 cueStickScript;
    void Start()
    {
    	cueStick = GameObject.Find("Cue Stick");
		cueStickScript = cueStick.GetComponent<CueStickController2>();
    	camera = GameObject.Find("Main Camera").GetComponent<CameraController>();
    	rb = GetComponent<Rigidbody>();
        startPosition = transform.position;
        lt = GetComponent<Light>();
    }

    public void resetBall() {
    	transform.position = startPosition;
    	rb.AddForce(Vector3.zero);
		rb.velocity = Vector3.zero;
		restart = true;   
	}

	public bool isMoving() {
		if (rb.velocity == Vector3.zero) {
			return false;
		}
		else {
			return true;
		}
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
	}

    void OnTriggerEnter(Collider other) 
	{
		// ..and if the game object we intersect has the tag 'Pick Up' assigned to it..
		if (other.gameObject.CompareTag("Scoring Plane"))
		{
			cueStickScript.madeBall(tag);
		}
	}
    // Update is called once per frame
    void FixedUpdate()
    {
        if (rb.velocity.magnitude < .1 && !(rb.velocity == Vector3.zero)) {
			rb.velocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;
		}
		if (restart) {
			rb.AddForce(Vector3.zero);
			rb.velocity = Vector3.zero;
			restart = false;
		}
    }
}
