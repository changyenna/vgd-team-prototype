using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 7f;
    private Rigidbody rb;
    private bool isGrounded;
    private GameObject nearbyPickup;
    private Renderer playerRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Get Rigidbody
        playerRenderer = GetComponent<Renderer>(); // Get player material
    }

    void Update()
    {
        // Basic movement
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(moveX * speed, rb.linearVelocity.y, moveZ * speed);
        rb.linearVelocity = move;

        // Jumping
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }

        // Picking up pajamas (Press "F")
        if (nearbyPickup != null && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Picked up " + nearbyPickup.name + "!");

            // Get the Pajamas material
            Renderer pajamasRenderer = nearbyPickup.GetComponent<Renderer>();
            if (pajamasRenderer != null)
            {
                playerRenderer.material = pajamasRenderer.material; // Copy the PajamasMaterial to the Player
            }

            Destroy(nearbyPickup); // Remove pajamas after pickup
            nearbyPickup = null;
        }
    }

    // Detect Ground & Lava
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Platform"))
        {
            isGrounded = true;
        }

        if (collision.gameObject.CompareTag("Lava"))
        {
            RespawnPlayer();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Platform"))
        {
            isGrounded = false;
        }
    }

    // Detect nearby pickup objects
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickup"))
        {
            Debug.Log("Near " + other.gameObject.name + ". Press 'F' to pick up.");
            nearbyPickup = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pickup"))
        {
            Debug.Log("Left " + other.gameObject.name);
            nearbyPickup = null;
        }
    }

    // Respawn the Player
    void RespawnPlayer()
    {
        transform.position = new Vector3(-4, 1.5f, -4); // Reset to starting position
        rb.linearVelocity = Vector3.zero; // Reset velocity so the player doesn't keep moving after respawning
        Debug.Log("Player respawned");
    }
}
