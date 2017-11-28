using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Base class for all third person controllers. 
public abstract class ThirdPersonController : MonoBehaviour {

    //related to characgter motion
    public float moveSpeedScale = 5.0f;             //controls the character's movement speed

    //related to animations
    protected Animator playerAnimator;              //reference to the character's animator (there should be only a single animator component for the entire character)

    //abstract fucntion that will control the movement of the player
    protected abstract void MovePlayer();

    //abstract function that will control all of the character's animations
    protected abstract void AnimatePlayer();
}
