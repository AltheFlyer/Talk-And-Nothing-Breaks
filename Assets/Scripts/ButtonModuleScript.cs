using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonModuleScript : HoverGlow
{

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        animator = GetComponentInParent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && base.mouseOver) {
            animator.Play("PressButton");
        } else if (Input.GetMouseButtonUp(0) && base.mouseOver) {
            animator.Play("ButtonUp");
        }

    }

}
