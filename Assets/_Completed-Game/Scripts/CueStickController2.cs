using UnityEngine;

// Include the namespace required to use Unity UI
using UnityEngine.UI;

using System.Collections;
public class CueStickController2 : MonoBehaviour
{
	// Create public variables for player speed, and for the Text UI game objects
	public float speed;
	public Text countText;
	public Text winText;

	// Create private references to the rigidbody component on the player, and the count of pick up objects picked up so far
	private Rigidbody rb;
	private bool turn = true;
	private Renderer rend;
	private Vector3 relativePos = new Vector3 (0.0f,-2.5f,-11.0f);
	private Quaternion startRotation;
    // Start is called before the first frame update
    void Start()
    {
    	startRotation = transform.rotation;
        rb = GetComponent<Rigidbody>();
        //Fetch the Renderer from the GameObject
        rend = GetComponent<Renderer>();

        //Set the main Color of the Material to green
        rend.material.shader = Shader.Find("_Color");
        rend.material.SetColor("_Color", Color.blue);

        rend.material.shader = Shader.Find("Specular");
        rend.material.SetColor("_SpecColor", Color.red);

    }

    public void turnChange() {
    	turn = !turn;
    	transform.rotation = startRotation;
	    Vector3 cueBallPos = GameObject.Find("Cue Ball").transform.position;
	    rb.MovePosition(cueBallPos - relativePos);
    	rend.enabled = true;
    	if (turn == true) {
    		rend.material.shader = Shader.Find("_Color");
        	rend.material.SetColor("_Color", Color.blue);
        	rend.material.shader = Shader.Find("Specular");
        	rend.material.SetColor("_SpecColor", Color.red);
    	}
    	else {
    		rend.material.shader = Shader.Find("_Color");
        	rend.material.SetColor("_Color", Color.green);
        	rend.material.shader = Shader.Find("Specular");
        	rend.material.SetColor("_SpecColor", Color.red);
    	}
    }

    // Update is called once per frame
    void FixedUpdate()
    {
    	var cueBall = GameObject.Find("Cue Ball");
		var cueBallScript = cueBall.GetComponent<PlayerController>();
		if (!cueBallScript.isMoving()) {
			float moveHorizontal = Input.GetAxis ("Horizontal");
			transform.RotateAround(cueBall.transform.position, Vector3.up, moveHorizontal);
		}
    }
}
