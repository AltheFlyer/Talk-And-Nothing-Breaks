using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpCamera: MonoBehaviour
{

    void Start() 
    {
    }

    void Update() 
    {
        if (Input.GetMouseButton(1))
        {
            float h = Input.GetAxis("Mouse X");
            float v = Input.GetAxis("Mouse Y");
            transform.Translate(h, v, 0);
        }
    }   
}