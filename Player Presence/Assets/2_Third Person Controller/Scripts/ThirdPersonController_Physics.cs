using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//allows the player to move in the virtual space is if looking towards the character as the main focus
//uses physics and forces to accomplish this and requires the use of a camera control script attached to the camera
//this is the more reliable method but requires more work to get right
public class ThirdPersonController_Physics : ThirdPersonController
{
    //related to character motion
    public float maxVelocity = 5.0f;            //allows us to control the maximum speed for the player movement
    private Vector2 moveSpeed;                  //keeps track of the character's actual movement speed
    public float jumpForce = 10.0f;             //controls the amplitude of the jump
    public KeyCode jumpKey;                     //allows the user to select which key we want to use for jump - not necessary but keeps things clean and modifiable

    //related to character state
    private bool isGrounded = true;             //detects if the player is grounded or not

    //related to character physics
    private Rigidbody playerRB;                 //player's physical entity in the virtual space (controls physics calculations)
    private Collider playerCol;                 //player's physical volume in the virtual space (controls object boundary and collisions)

    //These friction materials are used to reduce skating and control how quickly the player stops when no more input is read
    //If the player skates around too much or doesn't have enough power to walk up slopes, altering these physics materials will fix that
    public PhysicMaterial minFriction;          //we want minimum friction when the character is in motion (0.0)   
    public PhysicMaterial maxFriction;          //we want max friction when the player is idle (5.0)   

    public ThirdPersonController_Camera cam;    //reference to the camera controller script

    //Start is called once at the start of the scene. Used for initialization
    private void Start()
    {
        playerRB = GetComponent<Rigidbody>();
        playerCol = GetComponent<Collider>();
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
        if (moveSpeed.magnitude > 0)
        {
            playerCol.material = minFriction;
        }

        else
        {
            playerCol.material = maxFriction;
        }
    }

    //reads the player input and reacts accordingly
    private void ReadPlayerInput()
    {
        //store both axes values into floats so we can reuse them
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        //if either of the axes are not equal to zero than we want to move the player
        if (horizontalInput != 0 || verticalInput != 0)
        {
            MovePlayer();
        }

        //otherwise there is no movement input so set the move speed to zero
        else
        {
            moveSpeed = Vector2.zero;
        }

        //if the designated jump key is pressed then make the player jump
        if (Input.GetKeyDown(jumpKey))
        {
            Jump();
        }
    }

    //sends a ray from its position (bottom of capsule) downwards to see if there is something directly below
    //if the ray has a length greater than our expected threshold (0.1f) then we are not grounded
    private void CheckGround()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.1f);
    }

    //moves the player in the direction relative to the input along the horizontal and vertical input axes
    protected override void MovePlayer()
    {
        //store both axes values into floats so we can reuse them
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        //Get the movement axis vectors (x and z) and scale them byt the input and speed scale
        Vector3 xMovement = horizontalInput * moveSpeedScale * cam.transform.right;
        Vector3 zMovement = verticalInput * moveSpeedScale * NormalizeCameraForward();

        //add both movement axis vectors so we can move in both directions at once
        Vector3 MoveForce = xMovement + zMovement;

        //forces the player quickly but gradually look towards the direction it is moving
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(MoveForce), 0.5f);

        //moves the player by adding a vector force to it
        playerRB.AddForce(MoveForce);

        //limits the velocity to a maximum x and z without limiting the y velocity
        float modifiedVelocityX = Mathf.Clamp(playerRB.velocity.x, -maxVelocity, maxVelocity);
        float modifiedVelocityZ = Mathf.Clamp(playerRB.velocity.z, -maxVelocity, maxVelocity);
        Vector3 modifiedVelocity = new Vector3(modifiedVelocityX, playerRB.velocity.y, modifiedVelocityZ);
        playerRB.velocity = modifiedVelocity;

        //store the new movement in the speed so our controller knows how fast he is moving
        moveSpeed = new Vector2(xMovement.magnitude, zMovement.magnitude);
    }

    //since we want to move the player relative to the camera's forward but the camera will be angled on x-axis which will make the player move
    //vertically if this issue is not handles. This function allows us to normalize the camera's forward vector by scalling the x rotation to zero
    private Vector3 NormalizeCameraForward()
    {
        return new Vector3(cam.transform.forward.x, 0, cam.transform.forward.z);
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

    //Handles the communication to the animator controller and will set all animator parameters accordingly
    protected override void AnimatePlayer()
    {
        //////////////////////////////////////////////////////////////////////////////////////////
        //  vvvvvvvvvvvvvvvvvvvvvvv Add all animator based code here vvvvvvvvvvvvvvvvvvvvvvvvv  //



        //////////////////////////////////////////////////////////////////////////////////////////
    }
}
