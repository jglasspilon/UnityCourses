using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//allows the player to move in the virtual space as if looking towards the character as the main focus
//uses simple transform manipulation to accomplish this and let's Unity's hierarchy system control the camera movement
//the camera should be a child of the controller so when it rotates the camera follows along (no camera control script required)
//this is the simplest method but is not always reliable in a 3d space 
public class ThirdPersonController_Simple : MonoBehaviour {

    //related to character motion
    public float moveSpeedScale = 5.0f;             //scales the character's movement speed
    public float turnSpeedScale = 50.0f;            //scales the character's turning speed

    private Animator playerAnimator;                //reference to the character's animator (there should be only a single animator component for the entire character)

    //Use this for initialization
    private void Start()
    {
        playerAnimator = GetComponentInChildren<Animator>();
    }

    //Update is called once per frame
    private void Update()
    {
        MovePlayer();
        TurnPlayer();
    }

    //moves the player forward or backwards relative to the player input
    private void MovePlayer()
    {
        //store the vertical axis value so we can reuse it
        float verticalInput = Input.GetAxis("Vertical");

        //scales the axis input value by our speed scale and the frame rate
        float zMovement = verticalInput * moveSpeedScale * Time.deltaTime;

        //moves the player by the Vector amount
        transform.Translate(0, 0, zMovement);
    }

    //rotates the player along the y axis (rotates left-right) relative to the player input
    private void TurnPlayer()
    {
        //store the horizontal axis value so we can reuse it
        float horiztonalInput = Input.GetAxis("Horizontal");

        //scales the axis input value by our speed scale and the frame rate
        float yRotation = horiztonalInput * turnSpeedScale * Time.deltaTime;

        //rotates the player by the yRotation amount
        transform.Rotate(0, yRotation, 0);
    }
}
