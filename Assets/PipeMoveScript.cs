using UnityEngine;

public class PipeMoveScript : MonoBehaviour
{
    public  float moveSpeed = 5f; // Speed at which the pipe moves
    public float deadZone = -45f; // Position at which the pipe is considered out of bounds

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;

        if (transform.position.x < deadZone)
        {
            Debug.Log("Pipe deleted!");
            Destroy(gameObject); // Destroy the pipe if it goes out of bounds
        }
    }
}
