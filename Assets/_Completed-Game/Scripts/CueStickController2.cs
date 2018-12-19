using UnityEngine;

// Include the namespace required to use Unity UI
using UnityEngine.UI;

using System.Collections;
using System.Collections.Generic;
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
	private Vector3 relativeToBall;
	private Quaternion startRotation;
	private Vector3 startPosition;
	private string firstPlayerBalls = "None";
	private string secondPlayerBalls = "None";
	private int firstPlayerCount = 0;
	private int secondPlayerCount = 0;
	private bool ballMade = false;
    private bool firstPlayer = false;
    private bool firstBall = false;
    private GameObject cueBall;
    private List<BallController> balls; 
    private PlayerController cueBallScript;
    // Start is called before the first frame update
    void Start()
    {
        balls = new List<BallController>();
    	startRotation = transform.rotation;
    	startPosition = transform.position;
        cueBall = GameObject.Find("Cue Ball");
        cueBallScript = cueBall.GetComponent<PlayerController>();
        relativeToBall = cueBall.transform.InverseTransformPoint(transform.position);
        rb = GetComponent<Rigidbody>();
        //Fetch the Renderer from the GameObject
        rend = GetComponent<Renderer>();
        cueBall = GameObject.Find("Cue Ball");
        playerText.text = "Player1's Turn";
        ballText.text = "None";
        winText.text = "";
        for (int i = 0; i < 7; i++) {
            balls.Add(GameObject.Find("Solids" + i.ToString()).GetComponent<BallController>());
            balls.Add(GameObject.Find("Stripes" + i.ToString()).GetComponent<BallController>());
        }
        balls.Add(GameObject.Find("8 Ball").GetComponent<BallController>());
    }

    public bool noBallsMoving() {
    	bool ballMoving = false;
    	bool noBallMoving = true;
    	for (int i = 0; i < 15; i++) {
    		ballMoving = balls[i].isMoving();
    		if (ballMoving) {
    			noBallMoving = false;
    		}
    	}
        ballMoving = cueBallScript.isMoving();
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
        firstBall = false;
    	for (int i = 0; i < 15; i++) {
    		balls[i].resetBall();
    	}
        cueBallScript.resetBall();
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
    		firstPlayer = turn;
            firstBall = true;
            turn = !turn;
    	}
    	else {
    		if (turn && tag == firstPlayerBalls) {
    			firstPlayerCount += 1;
                if (!firstBall) {
                    turn = false;
                }
                else {
                    turn = !firstPlayer;
                }
    		}
    		else if (!turn && tag == secondPlayerBalls) {
    			secondPlayerCount += 1;
                if (!firstBall) {
                    turn = true;
                }
                else {
                    turn = !firstPlayer;
                }
    		}
    		else if (!turn && tag == firstPlayerBalls) {
    			firstPlayerCount += 1;
                if (firstBall) {
                    turn = !firstPlayer;
                }
    		}
    		else {
    			secondPlayerCount += 1;
                if (firstBall) {
                    turn = !firstPlayer;
                }
    		}
    	}
    }

    public void turnChange() {
        firstBall = false;
    	turn = !turn;
    	transform.rotation = startRotation;
	    transform.position = cueBall.transform.TransformPoint(relativeToBall);
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

    public void followBall(Vector3 move) {
        transform.position = transform.position + move;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
		if (noBallsMoving()) {
			float moveHorizontal = Input.GetAxis ("Horizontal");
			transform.RotateAround(cueBall.transform.position, Vector3.up, -moveHorizontal);
		}
    }
}
