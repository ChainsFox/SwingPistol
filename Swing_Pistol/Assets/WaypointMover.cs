using UnityEngine;

/// <summary>
/// Moves an object back and forth smoothly between two Vector3 points relative to its starting position.
/// It also detects collisions with objects tagged "Bullet" and cycles the object's color between gray and red.
/// </summary>
public class WaypointMover : MonoBehaviour
{
    [Header("Relative Waypoints")]
    [Tooltip("The local offset from the object's starting position for Point A. Use Vector3.zero to keep Point A at the starting position.")]
    public Vector3 pointStartOffset = Vector3.zero;

    [Tooltip("The local offset from the object's starting position for Point B. This defines the end of the patrol path.")]
    public Vector3 pointEndOffset = new Vector3(5, 0, 0); // Default to 5 units right

    [Header("Movement Settings")]
    [Tooltip("The speed at which the object moves. Higher value means faster oscillation.")]
    public float speed = 1.0f;

    // Stores the position the object had when the scene started.
    private Vector3 initialPosition;
    // Stores a reference to the Renderer component to allow color changes.
    private Renderer objectRenderer;

    // State variable to track the current color (true if currently gray).
    private bool isGray = false;

    void Start()
    {
        // Capture the position the object starts at in the scene.
        initialPosition = transform.position;

        // Get the Renderer component once at the start for color changes.
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer == null)
        {
            Debug.LogWarning("WaypointMover on " + gameObject.name + " is missing a Renderer component, so color changes will not work.");
        }
    }

    void Update()
    {
        // 1. Define the absolute target positions by adding the offsets to the initial starting position
        Vector3 targetA = initialPosition + pointStartOffset;
        Vector3 targetB = initialPosition + pointEndOffset;

        // 2. Calculate the 't' value for Lerp
        // Mathf.PingPong(Time.time * speed, 1f) creates a value that smoothly
        // oscillates between 0 and 1 over time (0 -> 1 -> 0 -> 1...)
        float t = Mathf.PingPong(Time.time * speed, 1f);

        // 3. Perform the movement
        transform.position = Vector3.Lerp(targetA, targetB, t);
    }

    /// <summary>
    /// Checks for collision with an object tagged "Bullet" and cycles the object's color 
    /// between gray and red on successive hits.
    /// </summary>
    private void OnCollisionEnter(Collision objectWeHit)
    {
        // Check if the object we hit has the tag "Bullet"
        if (objectWeHit.gameObject.CompareTag("Bullet"))
        {
            // Check if we have a Renderer component to modify
            if (objectRenderer != null)
            {
                if (isGray)
                {
                    // If the object is currently gray, change it back to red.
                    objectRenderer.material.color = Color.red;
                    isGray = false;
                }
                else
                {
                    // If the object is not gray, change it to gray.
                    objectRenderer.material.color = Color.gray;
                    isGray = true;
                }
            }
        }
    }
}

