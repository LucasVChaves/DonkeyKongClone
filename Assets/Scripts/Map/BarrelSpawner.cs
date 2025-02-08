using System.Collections;
using UnityEngine;

public class BarrelSpawner : MonoBehaviour {
    public GameObject barrelObj;
    public float barrelTimeout = 3.0f;

    void Update() {
        StartCoroutine(WaitTimeout());
        Instantiate(barrelObj, transform.position, Quaternion.identity);
    }

    IEnumerator WaitTimeout() {
        yield return new WaitForSeconds(barrelTimeout);
    }
}
