using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpCamera: MonoBehaviour
{

    private float moveSpeed = 0.2f;
    private float scrollSpeed = 35f;

    void Start() 
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update() 
    {
        if (Input.GetMouseButton(1))
        {
            float h = Input.GetAxis("Mouse X");
            float v = Input.GetAxis("Mouse Y");
            transform.Translate(h * moveSpeed, v * moveSpeed, 0);
        } else {
            float h = Input.GetAxis("Mouse X");
            float v = Input.GetAxis("Mouse Y");
            transform.Rotate(-v, h, 0, Space.Self);
            Quaternion q = transform.rotation;
            transform.rotation = Quaternion.Euler(q.eulerAngles.x, q.eulerAngles.y, 0);
        }


        if (Input.GetAxis("Mouse ScrollWheel") != 0) {
            transform.position += scrollSpeed * new Vector3(0, -Input.GetAxis("Mouse ScrollWheel"), 0) * Time.deltaTime;
        }

        if (Input.GetKey("r")) {
            transform.position = new Vector3(0, 5.75f, 0);
            transform.rotation = Quaternion.Euler(90, 0, 0);
        }
    }   
}