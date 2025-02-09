using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 7f;
    private Rigidbody rb;
    private bool isGrounded;
    private GameObject nearbyPickup;
    private Renderer playerRenderer;
    // public TextMeshProUGUI pickupText;
    public TextMeshProUGUI sleepinessTimerText;
    private float sleepinessTime = 60f;
    private bool isGameOver = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerRenderer = GetComponent<Renderer>();

        // pickupText = GameObject.Find("PickupMessage").GetComponent<TextMeshProUGUI>();
        sleepinessTimerText = GameObject.Find("SleepinessTimer").GetComponent<TextMeshProUGUI>();

        // pickupText.gameObject.SetActive(false); 
    }

    void Update()
    {
        if (!isGameOver)
        {
            sleepinessTime -= Time.deltaTime;
            sleepinessTimerText.text = "Time Left: " + Mathf.Ceil(sleepinessTime) + "s";

            if (sleepinessTime <= 0)
            {
                isGameOver = true;
                sleepinessTimerText.text = "You fell asleep!";
                Debug.Log("Game Over - Player fell asleep!");
            }
        }

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(moveX * speed, rb.linearVelocity.y, moveZ * speed);
        rb.linearVelocity = move;

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }

        if (nearbyPickup != null && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Picked up " + nearbyPickup.name + "!");

            Renderer pajamasRenderer = nearbyPickup.GetComponent<Renderer>();
            if (pajamasRenderer != null)
            {
                playerRenderer.material = pajamasRenderer.material;
            }

            Destroy(nearbyPickup);
            nearbyPickup = null;
            // pickupText.gameObject.SetActive(false); 
        }
    }
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
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickup"))
        {
            Debug.Log("Near " + other.gameObject.name + ". Press 'F' to pick up.");
            nearbyPickup = other.gameObject;
            // pickupText.gameObject.SetActive(true); 
        }

        if (other.CompareTag("Bed"))
        {
            Debug.Log("Player got into bed. Game Complete!");
            sleepinessTimerText.text = "You went to bed on time!";
            isGameOver = true;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pickup"))
        {
            Debug.Log("Left " + other.gameObject.name);
            nearbyPickup = null;
            // pickupText.gameObject.SetActive(false); // Hide UI text
        }
    }

    // Respawn the Player
    void RespawnPlayer()
    {
        Debug.Log("Respawning Player...");
        transform.position = new Vector3(-4, 1.5f, -4);
        rb.linearVelocity = Vector3.zero;
    }
}
