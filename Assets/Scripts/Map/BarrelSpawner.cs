using System.Collections;
using UnityEngine;

public class BarrelSpawner : MonoBehaviour {
    public GameObject barrelObj;
    public float barrelTimeout = 3.0f;

    void Start() {
        StartCoroutine(WaitTimeout());
    }

    IEnumerator WaitTimeout() {
        while (true) {
            Instantiate(barrelObj, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(barrelTimeout);
        }
    }
}
