using System.Collections;
using UnityEngine;

public class PlatformBehaviour : MonoBehaviour {
    public GameObject playerObj;
    private PlatformEffector2D effector2D;

    void Start() {
        effector2D = GetComponent<PlatformEffector2D>();
    }

    void Update() {
        if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow)) {
            StartCoroutine(RestartRotOffset());
        }

        if (playerObj.GetComponent<PlayerMovement>().isGrounded && (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))) {
            effector2D.rotationalOffset = 180f;
        }
    }

    IEnumerator RestartRotOffset() {
        yield return new WaitForSeconds(0.45f);
        effector2D.rotationalOffset = 0f;
    }
}