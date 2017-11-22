using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//allows the player to move in the virtual space as if looking through the eyes of the character
public class FirstPersonController_Physics : MonoBehaviour {

    //related to character motion
    public float speedScale = 5.0f;             //scales the character's movement speed
    public float maxVelocity = 5.0f;            //allows us to control the maximum speed for the player movement
    private Vector2 moveSpeed;                  //keeps track of the character's actual movement speed
    public float jumpForce = 10.0f;             //controls the amplitude of the jump - not always necessary
    public KeyCode jumpKey;                     //allows the user to select which key we want to use for jump - not necessary but keeps things clean and modifiable

    //related to character state
    private bool isGrounded = true;             //detects if the player is grounded or not

    //related to character physics
    private Rigidbody playerRB;                 //player's physical entity in the virtual space (controls physics calculations)
    private Collider playerCol;                 //player's physical volume in the virtual space (controls object boundary and collisions)

    //related to mouse camera controls
    public float mouseSensitivity = 5.0f;       //sensitivity for camera movement related to mouse movement
    public float camSmoothing = 2.0f;           //controls the smoothness of the camera movement
    private Vector2 camSmoothRotation;          //rotation 2d vector used to smoothly rotate the camera
    private Vector2 camMouseLook;               //2d vector to control where the camera should look at relative to the mouse movement
    private Camera cam;

    //These friction materials are used to reduce skating and control how quickly the player stops when no more input is read
    public PhysicMaterial minFriction;          //we want minimum friction when the character is in motion (0.0)   
    public PhysicMaterial maxFriction;          //we want max friction when the player is idle (5.0)               
    

    //Start is called once at the start of the scene. Used for initialization
    private void Start()
    {
        playerRB = GetComponent<Rigidbody>();
        playerCol = GetComponent<Collider>();
        cam = Camera.main;
    }

    //FixedUpdate is called every frame regardless of frame rate (best when doing physics related functionality)
    private void FixedUpdate()
    {
        HandleFriction();
        CheckGround();
        ReadPlayerInput();
    }

    //controls how the friction should be handled depending on what the player is doing (only necessary if using physcis material)
    //if the player is in motion than we want zero friction, otherwise we want max friction
    private void HandleFriction()
    {
        //since our move speed is a vector, we can check to see if either of the values are greater than zero by looking at the magnitude.
        //if the magnitude of the vector is zero, then both values are equal to zero so there is no movement
        if(moveSpeed.magnitude > 0)
        {
            playerCol.material = minFriction;
        }

        else
        {
            playerCol.material = maxFriction;
        }
    }

    //sends a ray from its position (bottom of capsule) downwards to see if there is something directly below
    //if the ray has a length greater than our expected threshold (0.5) then we are not grounded
    private void CheckGround()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.5f);
        Debug.Log(isGrounded);
    }

    //reads the player input and reacts accordingly
    private void ReadPlayerInput()
    {
        //store both axes values into floats so we can reuse them without recreating extra variables
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        //if either of the axes are not equal to zero than we want to move the player
        if(horizontalInput != 0 || verticalInput != 0)
        {
            MovePlayerUsingForces(horizontalInput, verticalInput);
        }

        //otherwise we are not moving so set the move speed to zero
        else
        {
            moveSpeed = Vector2.zero;
        }

        //if the designated jump key is pressed then make the player jump
        if (Input.GetKeyDown(jumpKey))
        {
            Jump();
        }

        MouseLook();
    }

    //moves the player relative to the input form the axes using the rigidbody function AddForce
    //this is the most reliable method for moving physical objects in a 3D game when we need to take gravity and collisions into account
    //however, a physics material is generally needed in order to control the slowdown
    private void MovePlayerUsingForces(float horizontalAxis, float verticalAxis)
    {
        //Get the movement axis vectors (x and z) and scale them byt the input and speed scale
        Vector3 xMovement = horizontalAxis * speedScale * transform.right;
        Vector3 zMovement = verticalAxis * speedScale * transform.forward;

        //add both movement axis vectors so we can move in both directions at once
        Vector3 MoveForce = xMovement + zMovement;

        //moves the player by adding a vector force to it and use the Vector3 function ClampMagnitude to limit the velocity
        playerRB.AddForce(MoveForce);
        playerRB.velocity = Vector3.ClampMagnitude(playerRB.velocity, maxVelocity);
        
        //store the new movement in the speed so our controller knows how fast he is moving
        moveSpeed = new Vector2(xMovement.magnitude, zMovement.magnitude);
    }

    //adds an impulse force upwards with the strength of our jump force if the player is grounded
    //an impulse force is a single burst to the rigidbody's acceleration so the forces is only added once
    private void Jump()
    {
        if (isGrounded)
        {
            playerRB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    //uses the mouse movement to rotate the camera
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
