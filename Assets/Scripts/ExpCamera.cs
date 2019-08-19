using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpCamera: MonoBehaviour
{

    private float moveSpeed = 5f;
    private float scrollSpeed = 35f;

    public float lowerBound;
    public float upperBound;

    void Start() 
    {
        //Cursor.lockState = CursorLockMode.Locked;
    }

    void Update() 
    {
        /* 
        if (Input.GetMouseButton(1))
        {
            float h = Input.GetAxis("Mouse X");
            float v = Input.GetAxis("Mouse Y");
            transform.Translate(h * moveSpeed * 0.04f, v * moveSpeed * 0.04f, 0);
        }
        */
        /*
         else {
            float h = Input.GetAxis("Mouse X");
            float v = Input.GetAxis("Mouse Y");
            transform.Rotate(-v, h, 0, Space.Self);
            Quaternion q = transform.rotation;
            if (q.eulerAngles.x > 80f) {
                q = Quaternion.Euler(80, q.eulerAngles.y, 0);
            } else if (q.eulerAngles.x < -80f) {
                q = Quaternion.Euler(-80, q.eulerAngles.y, 0);
            }
            transform.rotation = Quaternion.Euler(q.eulerAngles.x, q.eulerAngles.y, 0);
        }
        */

        if (Input.GetAxis("Mouse ScrollWheel") != 0) {
            transform.Translate(0, 0, scrollSpeed * Input.GetAxis("Mouse ScrollWheel") *Time.deltaTime);
            if (transform.position.y < lowerBound) {
                transform.position = new Vector3(transform.position.x, lowerBound, transform.position.z);
            } else if (transform.position.y > upperBound) {
                transform.position = new Vector3(transform.position.x, upperBound, transform.position.z);
            }
        }

        if (Input.GetKey(KeyCode.W)) {
            transform.Translate(0, moveSpeed * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.A)) {
            transform.Translate(-moveSpeed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.S)) {
            transform.Translate(0, -moveSpeed * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.D)) {
            transform.Translate(moveSpeed * Time.deltaTime, 0, 0);
        }
    }   
}