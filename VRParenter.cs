using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRParenter : MonoBehaviour
{
    public GameObject headref;      //attach to a head reference 
    public GameObject leftHandref;  //attach to a left hand reference
    public GameObject rightHandref; //attach to a right hand reference
    public Vector3 head;            //head transform controller
    public Vector3 leftHand;        //left hand transform controller
    public Vector3 rightHand;       //right hand transform controller



    private CharacterController charControl;

    private CharacterControllerVR charControlVR;

    private void Start()
    {
        charControl = GetComponent<CharacterController>();
        charControlVR = GetComponent<CharacterControllerVR>();
    }


    private void Update()
    {
        headref.transform.position = head;
        leftHandref.transform.position = leftHand;
        rightHandref.transform.position = rightHand;

        


        if(Input.GetKeyUp(KeyCode.I))       // I keypress will toggle control 
        {
            charControl.enabled = !charControl.enabled;
            charControlVR.enabled = !charControlVR.enabled;
        }
    }
}


