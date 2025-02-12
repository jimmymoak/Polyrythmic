using UnityEngine;

public class BallController : MonoBehaviour
{
    public float initialY;  // The resting position of the ball
    public float gravity = 5f;  // Strength of gravity pulling the ball back
    public float jumpVelocity = 5f;  // Upward velocity when pressing space
    private float velocityY = 0f;  // Current vertical velocity
    private bool isMoving = false;  // Whether the ball is in motion

    void Start()
    {
        initialY = transform.position.y;  // Store the starting position
    }

    void Update()
    {
        // Player presses space to jump
        if (Input.GetKeyDown(KeyCode.Space) && transform.position.y <= initialY)
        {
            velocityY = jumpVelocity;  // Set upward velocity
            isMoving = true;  // Ball is in motion
        }

        if (isMoving)
        {
            // Apply velocity to move the ball
            transform.position += new Vector3(0, velocityY * Time.deltaTime, 0);

            // Apply custom gravity
            velocityY -= gravity * Time.deltaTime;

            // If ball reaches initialY or below, stop movement
            if (transform.position.y <= initialY)
            {
                transform.position = new Vector3(transform.position.x, initialY, transform.position.z);
                velocityY = 0f;
                isMoving = false;
            }
        }
    }
}
