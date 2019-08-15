using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonModuleScript : HoverGlow
{

    Animator animator;

    float startX;
    float startY;

    Vector3 pos;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        pos = GetComponent<Transform>().position;
        startY = pos.y;
    }

    // Update is called once per frame
    void Update()
    {
        bool a = false;
        if (Input.GetMouseButton(0) && mouseOver) {
            print(pos);
            if (pos.y > startY - 0.139974f) {
                pos.y -= 0.1f * Time.deltaTime;
            }
            print(pos);
            a = true;
        } else {
            if (pos.y < startY) {
                pos.y += 0.1f * Time.deltaTime;
            }
        }
        transform.position = pos;
    }

}
