using UnityEngine;

public class HammerPickup : MonoBehaviour{
    public float hammerDuration = 5f;
    
    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            player.ActivateHammer(hammerDuration);
            Destroy(gameObject);
        } else {
            Debug.Log("Other = " + other.gameObject.name);
        }
    }
}
