using UnityEngine;

// Include the namespace required to use Unity UI
using UnityEngine.UI;

using System.Collections;
public class CueStickController2 : MonoBehaviour
{
	// Create public variables for player speed, and for the Text UI game objects
	public float speed;
	public Text playerText;
	public Text ballText;
	public Text winText;

	// Create private references to the rigidbody component on the player, and the count of pick up objects picked up so far
	private Rigidbody rb;
	private bool turn = true;
	private Renderer rend;
	private Vector3 relativePos = new Vector3 (0.0f,-2.5f,-11.0f);
	private Quaternion startRotation;
	private Vector3 startPosition;
	private string firstPlayerBalls = "None";
	private string secondPlayerBalls = "None";
	private int firstPlayerCount = 0;
	private int secondPlayerCount = 0;
	private bool ballMade = false;
    // Start is called before the first frame update
    void Start()
    {
    	startRotation = transform.rotation;
    	startPosition = transform.position;
        rb = GetComponent<Rigidbody>();
        //Fetch the Renderer from the GameObject
        rend = GetComponent<Renderer>();

        playerText.text = "Player1's Turn";
        ballText.text = "None";
        winText.text = "";
    }

    public bool noBallsMoving() {
    	bool ballMoving = false;
    	bool noBallMoving = true;
    	for (int i = 0; i < 7; i++) {
    		ballMoving = GameObject.Find("Solids" + i.ToString()).GetComponent<BallController>().isMoving();
    		if (ballMoving) {
    			noBallMoving = false;
    		}
    		ballMoving = GameObject.Find("Stripes" + i.ToString()).GetComponent<BallController>().isMoving();
    		if (ballMoving) {
    			noBallMoving = false;
    		}
    	}
    	ballMoving = GameObject.Find("8 Ball").GetComponent<BallController>().isMoving();
    	if (ballMoving) {
    		noBallMoving = false;
    	}
    	ballMoving = GameObject.Find("Cue Ball").GetComponent<PlayerController>().isMoving();
    	if (ballMoving) {
    		noBallMoving = false;
    	}
    	return noBallMoving;
    }

    private IEnumerator resetGame() {
    	yield return new WaitForSeconds(10);
    	transform.position = startPosition;
    	transform.rotation = startRotation;
    	ballMade = false;
    	GameObject.Find("Cue Ball").GetComponent<PlayerController>().resetBall();
    	GameObject.Find("8 Ball").GetComponent<BallController>().resetBall();
    	for (int i = 0; i < 7; i++) {
    		GameObject.Find("Solids" + i.ToString()).GetComponent<BallController>().resetBall();
    		GameObject.Find("Stripes" + i.ToString()).GetComponent<BallController>().resetBall();
    	}
    	GameObject.Find("Main Camera").GetComponent<CameraController>().resetCamera();
    	turn = true;
    	firstPlayerBalls = "None";
    	secondPlayerBalls = "None";
    	secondPlayerCount = 0;
    	firstPlayerCount = 0;
    	playerText.text = "Player1's Turn";
        ballText.text = "None";
        winText.text = "";
    }

    public void madeBall(string tag) {
    	if (tag == "8 Ball") {
    		if (turn) {
    			if (firstPlayerCount == 7) {
    				winText.text = "Player1Wins!";
    			}
    			else {
    				winText.text = "Player2Wins!";
    			}
    		}
    		if (!turn) {
    			if (secondPlayerCount == 7) {
    				winText.text = "Player2Wins!";
    			}
    			else {
    				winText.text = "Player1Wins!";
    			}
    		}
    		StartCoroutine(resetGame());
    	}
    	else if (!ballMade) {
    		if (turn) {
    			firstPlayerBalls = tag;
    			if (tag == "Stripes") {
    				secondPlayerBalls = "Solids";
    			}
    			else {
    				secondPlayerBalls = "Stripes";
    			}
    			firstPlayerCount += 1;
    		}
    		else {
    			if (tag == "Stripes") {
    				firstPlayerBalls = "Solids";
    			}
    			else {
    				firstPlayerBalls = "Stripes";
    			}
    			secondPlayerBalls = tag;
    			secondPlayerCount += 1;
    		}
    		ballMade = true;
    		turn = !turn;
    	}
    	else {
    		if (turn && tag == firstPlayerBalls) {
    			firstPlayerCount += 1;
    			turn = false;
    		}
    		else if (!turn && tag == secondPlayerBalls) {
    			secondPlayerCount += 1;
    			turn = true;
    		}
    		else if (!turn && tag == firstPlayerBalls) {
    			firstPlayerCount += 1;
    		}
    		else {
    			secondPlayerCount += 1;
    		}
    	}
    }

    public void turnChange() {
    	turn = !turn;
    	transform.rotation = startRotation;
	    Vector3 cueBallPos = GameObject.Find("Cue Ball").transform.position;
	    rb.MovePosition(cueBallPos - relativePos);
    	rend.enabled = true;
    	if (turn == true) {
    		playerText.text = "Player1's Turn";
    		ballText.text = firstPlayerBalls;
    	}
    	else {
    		playerText.text = "Player2's Turn";
    		ballText.text = secondPlayerBalls;
    	}
    }

    // Update is called once per frame
    void FixedUpdate()
    {
    	var cueBall = GameObject.Find("Cue Ball");
		if (noBallsMoving()) {
			float moveHorizontal = Input.GetAxis ("Horizontal");
			transform.RotateAround(cueBall.transform.position, Vector3.up, moveHorizontal);
		}
    }
}
