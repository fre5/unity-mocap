using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRFingerInputBasic : MonoBehaviour
{
    VRInputActions vrinput;

    public GameObject cameraObj;
   
    public bool cameraPositioner = false;

    bool toggler = false;


   

    bool righttouchpaddownstate = false;

    Animator anim;

    




    void Start()
    {

        vrinput = GetComponent<VRInputActions>();
        anim = GetComponent<Animator>();
        anim.Play("Default Take", 0, 0f);

    }

    void Update()
    {

        if (vrinput.leftButtonUp)
        {

            toggler = !toggler;
            if (toggler)
            {
                cameraPositioner = true;
            }
            else
            {
                cameraPositioner = false;
            }
        }

        // RESET CAMERA
        if (vrinput.touchpadDownRight && cameraPositioner)
        {
            if (!righttouchpaddownstate)
            {
                righttouchpaddownstate = true;
                cameraObj.GetComponent<CameraControllerMX>().ResetCamera();
            }

        }
        else
        {
            righttouchpaddownstate = false;
        }


        // MOVE CAMERA POSITION WITH VR CONTROLLER
        if (vrinput.touchpadValueRight.y > 0.5f && cameraPositioner)
        {
            cameraObj.GetComponent<CameraControllerMX>().verticalPanDown = true;
        }
        else if (vrinput.touchpadValueRight.y < -0.5f && cameraPositioner)
        {
            cameraObj.GetComponent<CameraControllerMX>().verticalPanUp = true;
        }
        else
        {
            cameraObj.GetComponent<CameraControllerMX>().verticalPanDown = false;
            cameraObj.GetComponent<CameraControllerMX>().verticalPanUp = false;

        }

        if (vrinput.touchpadValueLeft.y < -0.5f && cameraPositioner)
        {
            cameraObj.GetComponent<CameraControllerMX>().zoomIn = true;
        }
        else if (vrinput.touchpadValueLeft.y > 0.5f && cameraPositioner)
        {
            cameraObj.GetComponent<CameraControllerMX>().zoomOut = true;
        }
        else
        {
            cameraObj.GetComponent<CameraControllerMX>().zoomIn = false;
            cameraObj.GetComponent<CameraControllerMX>().zoomOut = false;
        }



            // HAND GESTURES ANIMATION
            if (vrinput.gripLeft)
            {
                anim.Play("Fist", 0, 0f);
                anim.speed = 0;

            }

            else if (vrinput.gripRight)
            {
                anim.Play("Thumb", 0, 0f);
                anim.speed = 0;
            }

            else if (vrinput.triggerLeft > 0)
            {
                anim.Play("Scissor", 0, 0f);
                anim.speed = 0;
            }

            else if (vrinput.triggerRight > 0)
            {
                anim.Play("Index", 0, 0f);
                anim.speed = 0;
            }

            else if (vrinput.triggerRight == 0 && vrinput.triggerLeft == 0 && !vrinput.gripRight && !vrinput.gripLeft)
            {
                anim.speed = 1;
            }
        




       

    }

}

/* 
 [0] - no
 [1] - no
 [2] - yes
 [3] - no
     
     */

