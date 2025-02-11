using System;
using System.Collections;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour {
    [Header("Player")]
    public float moveSpeed = 4.2f;
    public float climbSpeed = 0.6f;
    public float jumpForce = 6f;
    public Transform groundCheckTransform;
    public float groundCheckRadius = .2f;
    public LayerMask groundLayer;
    [Header("Martelo")]
    public bool hasHammer = false;
    public float hammerTimer = 0f;
    public GameObject hammerObj;
    private Rigidbody2D rb;
    public bool isGrounded;
    private bool isNearLadder = false;
    private bool isClimbing = false;
    private GameObject currLadder;
    private float moveInput;
    private Animator animator;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        hammerObj.SetActive(false);
    }

    private void Update(){
        isGrounded = Physics2D.OverlapCircle(groundCheckTransform.position, groundCheckRadius, groundLayer);
        if (Input.GetButtonDown("Jump") && isGrounded) {
            rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpForce);
        }

        if (!hasHammer && isNearLadder && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))) {
            isClimbing = true;
            StartClimbing();
        }

        if (isClimbing && (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))) {
            isClimbing = false;
            StopClimbing();
        }

        if (hasHammer) {
            hammerTimer -= Time.deltaTime;
            if (hammerTimer <= 0) DeactivateHammer();
        }
    }

    private void StartClimbing() {
        animator.SetBool("isClimbing", true);
        rb.gravityScale = 0;
        transform.position = new Vector2(currLadder.transform.position.x, transform.position.y);
        //Debug.Log("Started Climbing");
    }

    private void StopClimbing() {
        rb.gravityScale = 1;
        animator.SetBool("isClimbing", false);
        //Debug.Log("Stop Climbing");
    }
    private void FixedUpdate() {
        moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        animator.SetFloat("xVelocity", rb.linearVelocityX);

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

        if (other.CompareTag("Barrel")) {
            if (hasHammer) Destroy(other.gameObject);
            else StartCoroutine(Die());
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

    public void ActivateHammer(float hammerDuration) {
        hasHammer = true;
        hammerObj.SetActive(true);
        hammerTimer = hammerDuration;
        //Debug.Log("MARTELO");
    }

    private void DeactivateHammer() {
        hasHammer = false;
        hammerObj.SetActive(false);
        //Debug.Log("SEM MARTELO");
    }

    IEnumerator Die() {
        animator.SetBool("isDead", true);
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Desenha um circulo no groundcheck, para debug
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheckTransform.position, groundCheckRadius);
    }
}
