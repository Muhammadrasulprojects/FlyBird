using UnityEngine;

public class BirdScript : MonoBehaviour
{
    public Rigidbody2D myRigidbody;
    public float flapForce;
    public LogicScript logicScript; // Reference to the LogicScript component
    public bool birdIsAlive = true; // Variable to track if the bird is alive

    public float lowerLimitY = -30f; // if bird falls below this Y → game over

    private Vector3 startPosition;
    private Quaternion startRotation;
    private Collider2D col2d;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        logicScript = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
        startPosition = transform.position;
        startRotation = transform.rotation;
        col2d = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (birdIsAlive && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0) || Input.touchCount > 0))
        {
            myRigidbody.linearVelocity = Vector2.up * flapForce;
        }

        if (birdIsAlive && transform.position.y < lowerLimitY)
        {
            logicScript.gameOver();
            birdIsAlive = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!birdIsAlive) return;
        logicScript.gameOver(); // Call the gameOver method from LogicScript when a collision occurs
        birdIsAlive = false; // Set birdIsAlive to false to indicate the bird is no longer alive
    }

    public void resetBird()
    {
        transform.position = startPosition; // Reset the bird's position to the starting position
        transform.rotation = startRotation; // Reset the bird's rotation to the starting rotation

        myRigidbody.linearVelocity = Vector2.zero; // Reset the bird's velocity to zero
        myRigidbody.angularVelocity = 0f; // Reset the bird's angular velocity to zero

        if(col2d != null) col2d.enabled = true;

        birdIsAlive = true; // Set birdIsAlive to true to indicate the bird is alive again
    }
}
