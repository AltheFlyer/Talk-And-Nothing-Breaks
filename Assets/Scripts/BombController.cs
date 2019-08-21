using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BombController : MonoBehaviour {

    private IEnumerator flipCoroutine;

    void Update() {
        if (Input.GetKey(KeyCode.Q)) {
            transform.Rotate(0, 0, 90 * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.E)) {
            transform.Rotate(0, 0, -90 * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.F) && flipCoroutine == null) {
            flipCoroutine = Flip(0);
            StartCoroutine(flipCoroutine);
        }

        if (Input.GetMouseButton(1)) {
            float h = Input.GetAxis("Mouse X");
            float v = Input.GetAxis("Mouse Y");
            transform.Rotate(3 * v, 0, -3 * h, Space.Self);
            Quaternion q = transform.rotation;
            transform.rotation = Quaternion.Euler(q.eulerAngles.x, 0, q.eulerAngles.z);
        }
    }

    IEnumerator Flip(float theta) {
        float increment;
        while (theta < 180) {
            increment = Time.deltaTime * 270;
            theta += increment;

            //Force an exact 180 degree turn
            if (theta > 180) {
                increment -= (theta - 180);
            }

            transform.Rotate(0, 0, increment);

            //Wait until next frame
            yield return null;
        }
        StopCoroutine(flipCoroutine);
        flipCoroutine = null;
    }
}