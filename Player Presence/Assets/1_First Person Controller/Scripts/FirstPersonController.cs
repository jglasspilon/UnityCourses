using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//allows the player to move in the virtual space as if looking through the eyes of the character
public class FirstPersonController : MonoBehaviour {

    //related to character motion
    public float moveSpeed = 5.0f;              //controls the character's movement speed
    public float jumpForce = 10.0f;             //controls the amplitude of the jump - not always necessary

    //related to character state
    private bool isGrounded = true;             //detects if the player is grounded or not

    //related to character physics
    private Rigidbody playerRB;                 //player's physical entity in the virtual space

    //=====================================================================================================//
    public PhysicMaterial zeroFriction;         //we want zero friction when the character is in motion    //
    public PhysicMaterial maxFriction;          //we want max friction when the player is idle             //
    //========================================= ---------------------------------------------------------  //
    //========================================= | Not necessary for simple projects but as soon as another //
    //========================================= | rigidbody with movement is introduced in the scene this  //
    //========================================= | prevents the player from being pushed around by them     //
    //=====================================================================================================//

    private void Start()
    {
        playerRB = GetComponent<Rigidbody>();
    }
}
