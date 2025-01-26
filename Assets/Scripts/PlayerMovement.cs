using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public float moveSpeed = 4.2f;
    public float jumpForce = 6f;
    public Transform groundCheckTransform;
    public float groundCheckRadius = .2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;
    private float moveInput;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update(){
        isGrounded = Physics2D.OverlapCircle(groundCheckTransform.position, groundCheckRadius, groundLayer);
        if ((Input.GetButtonDown("Jump")) && isGrounded) {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    private void FixedUpdate() {
        moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    // Desenha um circulo no groundcheck, para debug
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheckTransform.position, groundCheckRadius);
    }
}
