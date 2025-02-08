using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public float moveSpeed = 4.2f;
    public float climbSpeed = 0.6f;
    public float jumpForce = 6f;
    public Transform groundCheckTransform;
    public float groundCheckRadius = .2f;
    public LayerMask groundLayer;
    private Rigidbody2D rb;
    public bool isGrounded;
    private bool isNearLadder = false;
    private bool isClimbing = false;
    private GameObject currLadder;
    private float moveInput;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update(){
        isGrounded = Physics2D.OverlapCircle(groundCheckTransform.position, groundCheckRadius, groundLayer);
        if (Input.GetButtonDown("Jump") && isGrounded) {
            rb.linearVelocity = new Vector2(rb.linearVelocityX / 10000, jumpForce);
        }

        if (isNearLadder && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))) {
            isClimbing = true;
            Debug.Log("Near Ladder");
            StartClimbing();
        }

        if (isClimbing && (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))) {
            isClimbing = false;
            Debug.Log("Off Ladder");
            StopClimbing();
        }
    }

    private void StartClimbing() {
        rb.gravityScale = 0;
        transform.position = new Vector2(currLadder.transform.position.x, transform.position.y);
        Debug.Log("Started Climbing");
    }

    private void StopClimbing() {
        rb.gravityScale = 1;
        Debug.Log("Stop Climbing");
    }
    private void FixedUpdate() {
        moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        if (isClimbing) {
            float verticalInput = Input.GetAxis("Vertical");
            transform.Translate(Vector2.up * verticalInput * climbSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Ladder")) {
            isNearLadder = true;
            currLadder = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Ladder")) {
            isNearLadder = false;
            currLadder = null;
            isClimbing = false;
            StopClimbing();
        }
    }

    // Desenha um circulo no groundcheck, para debug
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheckTransform.position, groundCheckRadius);
    }
}
