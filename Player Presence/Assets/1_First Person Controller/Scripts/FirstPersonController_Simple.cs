using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController_Simple : MonoBehaviour {

    //related to character motion
    public float speedScale = 5.0f;             //scales the character's movement speed

    //related to mouse camera controls
    public float mouseSensitivity = 5.0f;       //sensitivity for camera movement related to mouse movement
    public float camSmoothing = 2.0f;           //controls the smoothness of the camera movement
    private Vector2 camSmoothRotation;          //rotation 2d vector used to smoothly rotate the camera
    private Vector2 camMouseLook;               //2d vector to control where the camera should look at relative to the mouse movement
    private Camera cam;

    // Use this for initialization
    void Start () {
        cam = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
        MovePlayer();
        MouseLook();
	}

    //moves the player relative to the input from the movement axes using the transform function Translate
    //this is the simplest method for moving an object in the virtual space but the least reliable when using physics (best when used in 2D games)
    private void MovePlayer()
    {
        //store both axes values into floats so we can reuse them without recreating extra variables
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        //scales the axis input value by our speed scale and the frame rate
        float xMovement = horizontalInput * speedScale * Time.deltaTime;
        float zMovement = verticalInput * speedScale * Time.deltaTime;

        //moves the player by the Vector amount
        transform.Translate(xMovement, 0, zMovement);
    }

    //uses the mouse movement to rotate the camera by determining the distance the mouse has moved, smoothing it
    //out and adding this scalled distance to the camera or character's rotation
    private void MouseLook()
    {
        //create a 2d vector using the mouse movement axes
        Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        //scale the vector by the sensitivity and smoothing
        mouseDelta *= mouseSensitivity * camSmoothing;

        //interpolates the next value of the cam's rotation based on the movement of the mouse
        //will gradually increase the value of camSmoothRotation towards mouseDelta at a rate of 1/camSmoothing
        camSmoothRotation.x = Mathf.Lerp(camSmoothRotation.x, mouseDelta.x, 1f / camSmoothing);
        camSmoothRotation.y = Mathf.Lerp(camSmoothRotation.y, mouseDelta.y, 1f / camSmoothing);

        //adds the new rotation values to the camera's look vector
        camMouseLook += camSmoothRotation;

        //rotate the camera verticaly along the x axis (Vector3.rigth) by the negative y value of the mouse look 2d vector
        //and rotate the player horizontaly along its upward axis (transform.up) by the x value of the mouse look 2d vector
        cam.transform.localRotation = Quaternion.AngleAxis(-camMouseLook.y, Vector3.right);
        transform.localRotation = Quaternion.AngleAxis(camMouseLook.x, transform.up);
    }
}
