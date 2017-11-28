using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController_Camera : MonoBehaviour {

    //related to mouse camera controls
    public float mouseSensitivity = 5.0f;       //sensitivity for camera movement related to mouse movement
    public float camSmoothing = 2.0f;           //controls the smoothness of the camera movement
    private Vector2 camSmoothRotation;          //rotation 2d vector used to smoothly rotate the camera
    private Vector2 camMouseLook;               //2d vector to control where the camera should look at relative to the mouse movement
    private FirstPersonController player;       //reference to the player object in the scene

    private void Start()
    {
        player = transform.parent.GetComponent<FirstPersonController>();
    }

    // Update is called once per frame
    void Update () {
        MouseLook();
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

        //limit the max vertical rotation between -90 degrees and 90 degrees so we can't flip the camera
        camMouseLook.y = Mathf.Clamp(camMouseLook.y, -90, 90);

        //rotate the camera verticaly along the x axis (Vector3.rigth) by the negative y value of the mouse look 2d vector
        //and rotate the player horizontaly along the y axis (Vector3.up) by the x value of the mouse look 2d vector
        transform.localRotation = Quaternion.AngleAxis(-camMouseLook.y, Vector3.right);
        player.transform.localRotation = Quaternion.AngleAxis(camMouseLook.x, Vector3.up);
    }
}
