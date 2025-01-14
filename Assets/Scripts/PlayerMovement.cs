// TODO: Se o player segurar o movimento na direcao
// de uma parede ele trava nela
// talvez implementar atrito??

using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public float moveSpeed = 3.5f;
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

    private void Update() {
        moveInput = Input.GetAxis("Horizontal");
        
        isGrounded = Physics2D.OverlapCircle(groundCheckTransform.position, groundCheckRadius, groundLayer);
        if (Input.GetButtonDown("Jump") && isGrounded) {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        if (isGrounded) {
            Debug.Log("ground");
        }
    }

    private void FixedUpdate() {
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    // Desenha um circulo no groundcheck, para debug
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheckTransform.position, groundCheckRadius);
    }
}
