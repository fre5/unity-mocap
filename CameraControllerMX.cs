using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Enables control of a Camera using a Standard Two Axis Game Controller and Keyboard Shortcuts. 

    //v.0.2.8
    //Switched background web green to composite friendly green

    //v.0.2.7
    //Added rotatearound character feature, fixed the orthographic camera fustrum error. Code optimization.

    //v.0.2.6
    //Added solid green screen background

    //v.0.2.5
    //Added fine adjuster for Vertical and Zoom camera positioning using keyboard UpArrow, DownArrow, and Right Control key combination.

    //v.0.2.4
    //Added Reset camera feature that adjusts camera position based on character's head position.

    //v.0.2.3
    //Added Toggler for Perspective and Orthographic Camera mode.

[RequireComponent(typeof(Rigidbody))]
public class CameraControllerMX : MonoBehaviour {

    [Header("Current Version v.0.2.7", order = 0)]


    //TOGGLE VARIABLES
    private float toggleVertical, toggleLook, toggleSpeed;

    //LEFT CONTROLLER
    private float xLeftAnalog, yLeftAnalog, thumbLeftAnalog;

    //RIGHT CONTROLLER
    private float xRightAnalog, yRightAnalog, thumbRightAnalog;

    //BUMPERS
    private bool leftBumpers, rightBumpers;

    //TRIGGERS
    private float triggers;

    //GAMEPAD SELECT AND START
    private bool submit, cancel;

    //SPEED PARAMETERS
    private float translationSpeed;
    private float rotationSpeed;

    [Header("Default = 0.25, 5")]
    public float tSpeedOrtho;
    public float rSpeedOrtho;

    [Header("Default = 10, 10")]
    public float tSpeedPers;
    public float rSpeedPers; 

    //RIGIDBODY OF THE CAMERA
    private Rigidbody rb;

    //INITIAL STATE VARIABLES
    private Vector3 initCameraPos;
    private float initOrthographicSize;

    //SELECTOR
    private Camera cam;

    //CHARACTER HEAD POSITION
    public Transform headPosition;

    //EXTERNAL SCRIPT VARIABLE CONTROL
    [HideInInspector]
    public bool verticalPanUp, verticalPanDown, zoomIn, zoomOut = false;

    //GREEN BACKGROUND
    public bool green = false;
    private bool white = false;

    public bool resolution4k;

    public bool audioCue = true;


    //SET BUILD SCREEN ORIENTATION
    public bool portraitMode = false;

    public Slider sliderPT;
    public Slider sliderPR;
    public Slider sliderOT;
    public Slider sliderOR;
    public Slider verticalTranslation;



    //THE BIG OL' START
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = GetComponent<Camera>();

        // SET INITIAL STATE OF CAMERA POSITION AND CAMERA MODE
        initCameraPos = transform.position;
        initOrthographicSize = cam.orthographicSize;

        if (cam.orthographic == true)
        {
            translationSpeed = tSpeedOrtho;
            rotationSpeed = rSpeedOrtho;
        }
        
        if (cam.orthographic == false)
        {
            
            translationSpeed = tSpeedPers;
            rotationSpeed = rSpeedPers;
        }

        if (portraitMode) //  ONLY USE FOR BUILDING A PORTRAIT ORIENTED BUILD
        {
            
            if(resolution4k) { Screen.SetResolution(2160, 3840, true); }
            else { Screen.SetResolution(1080, 1920, true); }

        }
        else
        {
            if (resolution4k) { Screen.SetResolution(3840, 2160, true); }
            else { Screen.SetResolution(1920, 1080, true); }
        }
    }

    //LIVE INPUT OF GAMEPAD
    void Gamepad()
    {
        //LEFT ANALOG INPUT
        xLeftAnalog = Input.GetAxis("LeftAnalogHorizontal") ;
        yLeftAnalog = -Input.GetAxis("LeftAnalogVertical") ;
        thumbLeftAnalog = Input.GetAxis("ThumbstickLeft");

        //RIGHT ANALOG INPUT
        xRightAnalog = Input.GetAxis("RightAnalogHorizontal");
        yRightAnalog = Input.GetAxis("RightAnalogVertical");
        thumbRightAnalog = Input.GetAxis("ThumbstickRight");

        leftBumpers = Input.GetButton("LeftBumper");
        rightBumpers = Input.GetButton("RightBumper");

        triggers = Input.GetAxis("Triggers") * translationSpeed;

        submit = Input.GetButtonUp("Submit");
        cancel = Input.GetButtonUp("Cancel");
    }


    public void ResetCamera() //resets camera position to character's head
    {
        transform.position = headPosition.position;
        transform.eulerAngles = new Vector3(0f, headPosition.eulerAngles.y + 180f, 0f);
        cam.orthographicSize = initOrthographicSize;
    }

    void Update() {

        tSpeedPers = sliderPT.value;
        rSpeedPers = sliderPR.value;
        tSpeedOrtho = sliderOT.value;
        rSpeedOrtho = sliderOR.value;
        
        Gamepad();

        // RESET CAMERA BASED ON CHARACTER HEAD POSITION
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetCamera();
        }

        // PERSPECTIVE CAMERA SELECTOR
        if(Input.GetKeyDown(KeyCode.P))
        {
            cam.nearClipPlane = 0.01f;
            cam.orthographic = false;

            sliderPT.gameObject.SetActive(true);
            sliderPR.gameObject.SetActive(true);

            sliderOT.gameObject.SetActive(false);
            sliderOR.gameObject.SetActive(false);
        }

        // ORTHOGRAPHIC CAMERA SELECTOR
        if (Input.GetKeyDown(KeyCode.O))
        {
            cam.nearClipPlane = -50f;
            cam.orthographic = true;

            sliderOT.gameObject.SetActive(true);
            sliderOR.gameObject.SetActive(true);

            sliderPT.gameObject.SetActive(false);
            sliderPR.gameObject.SetActive(false);
        }

        if (cam.orthographic == true)
        {
            translationSpeed = tSpeedOrtho;
            rotationSpeed = rSpeedOrtho;
        }

        if (cam.orthographic == false)
        {
            translationSpeed = tSpeedPers;
            rotationSpeed = rSpeedPers;
        }

        if (thumbLeftAnalog > 0f && thumbRightAnalog > 0f)
        {
            ResetCamera();
        }

        // ORTOGRAPHIC CAMERA CONTROL
        if (cam.orthographicSize >= 0.1f)
        {
            cam.orthographicSize += yLeftAnalog * 0.001f * tSpeedOrtho;
        }
        
        if (cam.orthographicSize < 0.1f)
        {
            cam.orthographicSize = 0.1f;
        }

        // TRIGGER CONTROL
        if(triggers > 0 || triggers < 0)
        {
            toggleVertical = triggers * verticalTranslation.value  * Time.deltaTime;
        }
        else
        {
            toggleVertical = 0f;
        }

        //CAMERA ROTATION

        //YAW
        if (xRightAnalog > 0.5f || xRightAnalog < -0.5f)
        {
            //transform.eulerAngles += new Vector3(0, xRightAnalog * rotationSpeed, 0f) * Time.deltaTime; // FREE ROTATION

            transform.RotateAround(headPosition.position, Vector3.up, xRightAnalog * rotationSpeed * Time.deltaTime);
        }
        else
        {
            transform.eulerAngles += new Vector3(0f, 0f, 0f) * Time.deltaTime;
        }

         //PITCH
        if (yRightAnalog > 0.5f || yRightAnalog < -0.5f)
        {
            //transform.eulerAngles += new Vector3(-yRightAnalog * rotationSpeed, 0f, 0f) * Time.deltaTime;
            transform.RotateAround(headPosition.position, Vector3.right, yRightAnalog * rotationSpeed * Time.deltaTime);
        }
        else
        {
            transform.eulerAngles += new Vector3(0f, 0f, 0f) * Time.deltaTime;
        }


        //ROLL
        if (leftBumpers)
        {
            transform.RotateAround(headPosition.position, Vector3.forward, -rotationSpeed * Time.deltaTime);
        }
        
        if (rightBumpers)
        {
            transform.RotateAround(headPosition.position, Vector3.forward, rotationSpeed * Time.deltaTime);
        }

        //CAMERA TRANSLATION
        if (xLeftAnalog > 0.5f || xLeftAnalog < -0.5f || triggers != 0)
        {
            Vector3 moveDirection = new Vector3(xLeftAnalog * translationSpeed, toggleVertical, 0f) * 0.01f * Time.deltaTime;
            moveDirection = transform.TransformDirection(moveDirection);

            rb.MovePosition(transform.position + moveDirection);
        }

        if ((yLeftAnalog > 0.5f || yLeftAnalog < -0.5f || triggers != 0) && cam.orthographicSize > 0.1f)
        {
            Vector3 moveDirection = new Vector3(0f, toggleVertical, -yLeftAnalog * translationSpeed) * Time.deltaTime;
            moveDirection = transform.TransformDirection(moveDirection);

            rb.MovePosition(transform.position + moveDirection);
        }
        else
        {
            Vector3 moveDirection = Vector3.zero;
        }

        // Simple Keyboard Shortcut Camera Control

        ///Vertical Pan
        if (verticalPanUp || (Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.RightControl)))
        {
            Vector3 moveDirection = new Vector3(0f, 1f, 0f) * Time.deltaTime;
            moveDirection = transform.TransformDirection(moveDirection);

            rb.MovePosition(transform.position + moveDirection);
        }
        else if (verticalPanDown || (Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.RightControl)))
        {
            Vector3 moveDirection = new Vector3(0f, -1f, 0f) * Time.deltaTime;
            moveDirection = transform.TransformDirection(moveDirection);

            rb.MovePosition(transform.position + moveDirection);
        }
       
        ///Zoom
        if (zoomOut || (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.RightControl) && cam.orthographic == true))
        {
            cam.orthographicSize += 0.01f;
        }
        else if (zoomIn || (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.RightControl) && cam.orthographic == true))
        {
            cam.orthographicSize -= 0.01f;
        }

        if (zoomOut || (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.RightControl) && cam.orthographic == false))
        {
            Vector3 moveDirection = new Vector3(0f, 0f, 1f) * Time.deltaTime;
            moveDirection = transform.TransformDirection(moveDirection);

            rb.MovePosition(transform.position + moveDirection);
        }
        else if (zoomIn || (Input.GetKey(KeyCode.DownArrow) && Input.GetKey(KeyCode.RightControl) && cam.orthographic == false))
        {
            Vector3 moveDirection = new Vector3(0f, 0f, -1f) * Time.deltaTime;
            moveDirection = transform.TransformDirection(moveDirection);

            rb.MovePosition(transform.position + moveDirection);
        }

        // Green Screen toggler
        IEnumerator WaitToColor()
        {
            cam.backgroundColor = Color.black;
            yield return new WaitForSeconds(0.1f);

            if (green)
            {
                Color newCol;
                ColorUtility.TryParseHtmlString("#00b140", out newCol);
                cam.backgroundColor = newCol;
            }
            else
            {
                cam.backgroundColor = new Color(1.0f, 1.0f, 1.0f);
            }
        }

        // Rotate background color green -> white -> black -> white
        if (Input.GetKeyUp(KeyCode.Space))
        {
            green = !green;
            if (green)
            {
                Color newCol;
                ColorUtility.TryParseHtmlString("#00b140", out newCol);
                cam.backgroundColor = newCol;
            }
            else
            {
                if (!white)
                {
                    cam.backgroundColor = new Color(1.0f, 1.0f, 1.0f); // white
                    white = !white;
                }
                else if(white)
                {
                    
                    cam.backgroundColor = new Color(0f, 0f, 0f); // black
                    white = !white;
                }  
            }
        }

        //AUDIO CUE - flash the screen black for start audio sync
        if (Input.GetKey(KeyCode.S))
        {
            if (audioCue)
                StartCoroutine(WaitToColor());
        }
    }
}
