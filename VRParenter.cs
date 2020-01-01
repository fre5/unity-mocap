using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRParenter : MonoBehaviour
{
    public GameObject headref;
    public GameObject leftHandref;
    public GameObject rightHandref;
    public Vector3 head;
    public Vector3 leftHand;
    public Vector3 rightHand;



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

        


        if(Input.GetKeyUp(KeyCode.I))
        {
            charControl.enabled = !charControl.enabled;
            charControlVR.enabled = !charControlVR.enabled;
        }
    }
}


