using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BarrelBehaviour : MonoBehaviour {
    public float speed = 3.5f;
    public float ladderDist = 2f;
    public float ladderDuration = 0.5f;
    // Chance de descer uma escada se encontrar
    public float ladderChance = 0.25f;
    private Rigidbody2D rb;
    private bool isDescending = false;
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(speed, rb.linearVelocityY);
    }

    void Update() {
        if (!isDescending) {
            rb.linearVelocity = new Vector2(speed, rb.linearVelocityY);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("LadderTop") && Random.value <= 0.25f && !isDescending) {
            Vector2 targetPos = new Vector2(transform.position.x, transform.position.y - ladderDist);
            StartCoroutine(DescendLadder(targetPos));
        }

        if (other.CompareTag("Wall")) {
            speed = -speed;
            rb.linearVelocity = new Vector2(speed, rb.linearVelocityY);
        }

        if (other.CompareTag("FireBarrel")) Destroy(gameObject);

        // Reinicia o jogo
        if (other.CompareTag("Player")) {
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            if (!player.hasHammer) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            else Destroy(gameObject);
        }
    }

    // Movimenta pra baixo, desce a escada, depois continua rolando
    IEnumerator DescendLadder(Vector2 targetPos) {
        //Debug.Log("DESCENDO!");
        isDescending = true;
        
        rb.bodyType = RigidbodyType2D.Kinematic;
        Vector2 startPos = transform.position;
        
        float elapsed = 0f;
        while(elapsed < ladderDuration) {
            transform.position = Vector2.Lerp(startPos, targetPos, elapsed / ladderDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPos;

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.linearVelocity = new Vector2(-speed, rb.linearVelocity.y);
        isDescending = false;
    }
}
