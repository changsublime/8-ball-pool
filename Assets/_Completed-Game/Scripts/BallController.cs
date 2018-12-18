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
    void Start()
    {
    	rb = GetComponent<Rigidbody>();
        startPosition = transform.position;
    }

    public void resetBall() {
    	transform.position = startPosition;
    	rb.AddForce(Vector3.zero);
		rb.velocity = Vector3.zero;
		restart = true;
		GetComponent<Renderer>().enabled = true;    
	}

	public bool isMoving() {
		if (rb.velocity == Vector3.zero) {
			return false;
		}
		else {
			return true;
		}
	}

    void OnTriggerEnter(Collider other) 
	{
		// ..and if the game object we intersect has the tag 'Pick Up' assigned to it..
		if (other.gameObject.CompareTag("Scoring Plane"))
		{
			Console.WriteLine("Collision");
			var cueStick = GameObject.Find("Cue Stick");
			var cueStickScript = cueStick.GetComponent<CueStickController2>();
			// Make the other game object (the pick up) inactive, to make it disappear
			//gameObject.SetActive(false);
			GetComponent<Renderer>().enabled = false;
			cueStickScript.madeBall(tag);
		}
	}
    // Update is called once per frame
    void FixedUpdate()
    {
        if (rb.velocity.magnitude < .1 && !(rb.velocity == new Vector3(0f,0f,0f))) {
			rb.velocity = new Vector3(0f,0f,0f);
		}
		if (restart) {
			rb.AddForce(Vector3.zero);
			rb.velocity = Vector3.zero;
			restart = false;
		}
    }
}
