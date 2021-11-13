using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class VRInput20FaceController : MonoBehaviour
{
    public SteamVR_Action_Vector2 touchpadAction;
    public SteamVR_Action_Boolean touchpadButton;
    CharacterVoice characterVoice;
    GamepadInput input;
    /*
    xLeftAnalog, yLeftAnalog, thumbLeftAnalog, xRightAnalog, 
    yRightAnalog, thumbRightAnalog, triggers, submit, cancel, 
    Abutton, Bbutton, Xbutton, Ybutton;
    */

    [HideInInspector]
    public float sadEyebrows, lowerEyebrows, raiseEyebrows, angryEyebrows, lookLeft, lookRight, lookUp, lookDown, blinkLeft, blinkRight, eyesSmall, mouthAAH, mouthEEH, mouthOOO, mouthPout, mouthSmile, mouthOpen, blinkLeftArc, blinkRightArc = 0.0f;
    public bool sadface, angryface, droopyface, sleepyface, happyface, surpriseface = false;
    public static bool blinking = false;
    Vector2 touchpadValueRight, touchpadValueLeft;
    bool touchpadButtonLeft, touchpadButtonRight;
    public bool state = false;
    
    [Tooltip("Default : 3000f")]
    public float micSensitivity = 3000f;
    
    private SkinnedMeshRenderer skinMeshRenderer;
    alphaBlender alphablend;

    private void Start()
    {
        input = GetComponent<GamepadInput>();
        skinMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        characterVoice = GetComponent<CharacterVoice>();

        // START BLINKING
        InvokeRepeating("Blink", 2.0f, 5f);

        if(GetComponent<alphaBlender>() != null)
        {
            alphablend = GetComponent<alphaBlender>();
        }
    }
    
    void Eyebrows()
    {
        skinMeshRenderer.SetBlendShapeWeight(0, sadEyebrows);
        skinMeshRenderer.SetBlendShapeWeight(1, lowerEyebrows);
        skinMeshRenderer.SetBlendShapeWeight(2, raiseEyebrows);
        skinMeshRenderer.SetBlendShapeWeight(3, angryEyebrows);
    }

    void Eyes()
    {
        skinMeshRenderer.SetBlendShapeWeight(4, lookLeft);
        skinMeshRenderer.SetBlendShapeWeight(5, lookRight);
        skinMeshRenderer.SetBlendShapeWeight(6, lookUp);
        skinMeshRenderer.SetBlendShapeWeight(7, lookDown);
        skinMeshRenderer.SetBlendShapeWeight(8, blinkLeft);
        skinMeshRenderer.SetBlendShapeWeight(9, blinkRight);
        skinMeshRenderer.SetBlendShapeWeight(10, blinkLeftArc);
        skinMeshRenderer.SetBlendShapeWeight(11, blinkRightArc);
        skinMeshRenderer.SetBlendShapeWeight(12, eyesSmall);

    }

    void Mouth()
    {
        skinMeshRenderer.SetBlendShapeWeight(13, mouthAAH);
        skinMeshRenderer.SetBlendShapeWeight(14, mouthEEH);
        skinMeshRenderer.SetBlendShapeWeight(15, mouthOOO);
        skinMeshRenderer.SetBlendShapeWeight(16, mouthPout);
        skinMeshRenderer.SetBlendShapeWeight(17, mouthSmile);
        skinMeshRenderer.SetBlendShapeWeight(18, mouthOpen);
    }

    void Blink()
    {
        if (state == false && blinkLeftArc != 85f && blinkLeft != 100f && blinkRight != 100f && blinkLeftArc != 100f && blinkRightArc != 100f )
        {
            StartCoroutine(Blinking(true,true));
        }
    }

    void BlinkRight()
    {
        if (state == false && blinkLeftArc != 85f && blinkLeft != 100f && blinkRight != 100f)
        {
            StartCoroutine(Blinking(false, true));
        }
    }

    void BlinkLeft()
    {
        if (state == false && blinkLeftArc != 85f && blinkLeft != 100f && blinkRight != 100f)
        {
            StartCoroutine(Blinking(true, false));
        }
    }

    void LeftBlinkOpen()
    {
        blinkLeft -= 4000f * Time.deltaTime;
    }

    void RightBlinkOpen()
    {
        blinkRight -= 4000f * Time.deltaTime;
    }

    void LeftBlinkClose()
    {
        blinkLeft += 4000f * Time.deltaTime;
    }   

    void RightBlinkClose()
    {
        blinkRight += 4000f * Time.deltaTime;
    }
    
    IEnumerator Blinking(bool left, bool right)
    {
        blinking = true;

        if (left == true)
        {
            InvokeRepeating("LeftBlinkClose", 0.0f, 0.05f);
        }
        if (right == true)
        {
            InvokeRepeating("RightBlinkClose", 0.0f, 0.05f);
        }
        yield return new WaitForSeconds(0.2f);

        if (left == true)
        {
            InvokeRepeating("LeftBlinkOpen", 0.0f, 0.05f);
        }
        if (right == true)
        {
            InvokeRepeating("RightBlinkOpen", 0.0f, 0.05f);
        }
        
        blinking = false;
    }

    void FixedUpdate()
    {
        Eyebrows();
        Eyes();
        Mouth();

        touchpadValueLeft = touchpadAction.GetAxis(SteamVR_Input_Sources.LeftHand);
        touchpadValueRight = touchpadAction.GetAxis(SteamVR_Input_Sources.RightHand);
        touchpadButtonRight = touchpadButton.GetState(SteamVR_Input_Sources.RightHand);
        touchpadButtonLeft = touchpadButton.GetState(SteamVR_Input_Sources.LeftHand);

        //EYES
        lookLeft = Mathf.Clamp(input.xLeftAnalog, 0f, 0.7f) * 100f;
        lookRight = Mathf.Clamp(input.xLeftAnalog, -0.7f, 0f) * -100f;
        lookUp = Mathf.Clamp(input.yLeftAnalog, 0f, 0.8f) * 100f;
        lookDown = Mathf.Clamp(input.yLeftAnalog, -0.8f, 0f) * -100f;

        //Happy 
        if (touchpadValueLeft.x > 0.5f && touchpadValueLeft.y < 0.5f && touchpadValueLeft.y > -0.5f && blinking != true && state == false)
        {
            blinkLeftArc = 100f;
            blinkRightArc = 100f;
            sadEyebrows = 50f;

            state = true;
        }
        //Sleepy
        else if (touchpadValueLeft.x < -0.5f && touchpadValueLeft.y < 0.5f && touchpadValueLeft.y > -0.5f && blinking != true && state == false)
        {
            blinkLeft = 30f;
            blinkRight = 30f;
            lowerEyebrows = 20f;

            state = true;
        }
        //Sad
        else if (touchpadValueLeft.y > 0.5f && touchpadValueLeft.x < 0.5f && touchpadValueLeft.x > -0.5f && blinking != true && state == false)
        {
            sadEyebrows = 100f;
            blinkLeft = 40f;
            blinkRight = 40f;

            state = true;
        }
        //Surprise
        else if (touchpadValueLeft.y < -0.5f && touchpadValueLeft.x < 0.5f && touchpadValueLeft.x > -0.5f && blinking != true && state == false)
        {
            sadEyebrows = 25f;
            eyesSmall = 50f;

            state = true;
        }
        //Reset
        else if (touchpadValueLeft.x < 0.5f && touchpadValueLeft.x > -0.5f && touchpadValueLeft.y < 0.5f && touchpadValueLeft.y > -0.5f && blinking != true)
        {
            blinkLeft = 0f;
            blinkRight = 0f;
            sadEyebrows = 0f;
            blinkLeftArc = 0f;
            blinkRightArc = 0f;
            lowerEyebrows = 0f;
            angryEyebrows = 0f;
            eyesSmall = 0f;
            raiseEyebrows = 0f;
            state = false;
        }

        //Looking Eyes
        lookLeft = Mathf.Clamp(-touchpadValueRight.x, 0f, 0.7f) * 100f;
        lookRight = Mathf.Clamp(-touchpadValueRight.x, -0.7f, 0f) * -100f;
        lookUp = Mathf.Clamp(touchpadValueRight.y, 0f, 0.8f) * 100f;
        lookDown = Mathf.Clamp(touchpadValueRight.y, -0.8f, 0f) * -100f;

        mouthAAH = characterVoice.clipLoudness * micSensitivity;
        mouthOpen = mouthAAH * 0.1f;

        if (mouthOpen < 1f)
        {
            mouthOpen = 0f;
        }

        if (mouthAAH < 10f)
        {
            mouthAAH = 0f;
        }

        if (mouthAAH > 100f)
        {
            mouthAAH = 100f;
        }

        if (mouthOpen < 0.2f && mouthAAH < 0.2f && mouthPout < 100f)
        {
            mouthSmile = 100f;
        }
        else
        {
            mouthSmile = 0f;
        }

        // Blink Auto Reset
        if (blinkLeft >= 100f)
        {
            CancelInvoke("LeftBlinkClose");

            blinkLeft = 100f;

        }
        if (blinkLeft <= 15f)
        {
            CancelInvoke("LeftBlinkOpen");

            blinkLeft = 15f;

        }
        if (blinkRight >= 100f)
        {
            CancelInvoke("RightBlinkClose");
            blinkRight = 100f;
        }
        
        if (blinkRight <= 15f)
        {
            CancelInvoke("RightBlinkOpen");
            blinkRight = 15f;
        }
        
        // Wink
        if (touchpadButtonRight && !touchpadButtonLeft)
        {
            blinkRightArc = 100f;
        }

        //Blush
        if (touchpadButtonLeft && !touchpadButtonRight)
        {              
            if (alphablend.blendAmount < 1f && GetComponent<alphaBlender>())            
            {
                alphablend.blendAmount += 0.001f * Time.time;
            }

        }
        else if (!touchpadButtonLeft && GetComponent<alphaBlender>())
        {
            if (alphablend.blendAmount > 0f)
            {
                alphablend.blendAmount -= 0.001f * Time.time;
            }
        }
    }
}





