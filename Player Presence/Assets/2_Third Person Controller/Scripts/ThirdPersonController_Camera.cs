using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Controls the camera behaviours for a more sophisticated third person controller.
//This camera is independant of the character and we can set where it looks and we can control the rotation with the mouse
public class ThirdPersonController_Camera : MonoBehaviour {

    //related to mouse camera controls
    public float mouseSensitivity = 5.0f;       //sensitivity for camera movement related to mouse movement
    public float camSmoothing = 2.0f;           //controls the smoothness of the camera movement
    private Vector2 camRotation;                  //rotation value used to smoothly rotate the camera
    private Vector2 camMouseLook;                 //rotation value to control where the camera should be rotated around the character relative to the mouse movement
    private Vector2 camRotationalOffset;          //keeps track of the rotational offset created by the mouse look

    //related to the camera's positioning and orientation
    public GameObject character;                //reference to the player character
    public Vector3 positionalOffset;            //controls the camera's position relative to the character's current position
    public Vector3 lookOffset;                  //controls where the camera will look at relative to the character's position
    public float positionalSmoothing = 2.0f;
	
	// Update is called once per frame
	void Update () {
        MouseLook();
        PositionCamera();
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
        camRotation.x = Mathf.Lerp(camRotation.x, mouseDelta.x, 1f / camSmoothing);
        camRotation.y = Mathf.Lerp(camRotation.y, mouseDelta.y, camSmoothing);

        //add the cam rotation value to the rotational offset so we can use it in our positioning of the camera
        camRotationalOffset.x += camRotation.x;
        camRotationalOffset.y -= camRotation.y;

        camRotationalOffset.y = Mathf.Clamp(camRotationalOffset.y, -35, 15);

        //rotate the camera verticaly along the x axis (Vector3.rigth) by the negative y value of the mouse look 2d vector
        transform.RotateAround(character.transform.position, Vector3.up, camRotation.x);
        transform.RotateAround(character.transform.position, Vector3.right, camRotation.y);
    }

    //using the offset vector, we want the camera to always be the same distance away from the character regardless of the current camera rotation
    //similarly, we want the camera to always look at the character regardless of the current camera's rotation around the character
    private void PositionCamera()
    {
        //find the new positions relative to the character's position using the offset values
        float newPositionX = character.transform.position.x + positionalOffset.x;
        float newPositionY = character.transform.position.y + positionalOffset.y;
        float newPositionZ = character.transform.position.z + positionalOffset.z;

        //combine the floats to create a vector and modify it by our rotation from our mouse look function
        //since our camera rotational offset is only the y value and x value, we need to create a new vector with 0 value z
        Vector3 finalPosition = new Vector3(newPositionX, newPositionY, newPositionZ);
        finalPosition = RotateVectorAroundPivot(finalPosition, character.transform.position, Quaternion.Euler(new Vector3(camRotationalOffset.y, camRotationalOffset.x, 0)));


        //set this as our new position for the camera
        transform.position = Vector3.Lerp(transform.position, finalPosition, 0.1f);

        //forces the camera to the location offset from the character
        transform.LookAt(character.transform.position + lookOffset);
    }

    //some vector math to rotate a vector around a point
    //this is generally used to get the particular offset vector after our mouse rotation has taken effect
    private Vector3 RotateVectorAroundPivot(Vector3 toRotate, Vector3 pivotPoint, Quaternion angle)
    {
        return angle * (toRotate - pivotPoint) + pivotPoint;
    }
}
