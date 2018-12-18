using UnityEngine;

// Include the namespace required to use Unity UI
using UnityEngine.UI;

using System.Collections;
public class CueStickController : MonoBehaviour
{
	// Create public variables for player speed, and for the Text UI game objects
	public float speed;
	public Text countText;
	public Text winText;

	// Create private references to the rigidbody component on the player, and the count of pick up objects picked up so far
	private Rigidbody rb;
	private bool turn = true; 
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void TurnChange() {
    	turn = !turn;
    }
    // Update is called once per frame
    void Update()
    {
        var cueBallPos = GameObject.Find("Cue Ball").transform.position;
        rb.MovePosition(cueBallPos);
    }
}
