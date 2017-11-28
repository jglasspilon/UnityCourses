using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FirstPersonController : MonoBehaviour {

    //related to character motion
    public float speedScale = 5.0f;             //scales the character's movement speed

    //abstract function that will control the movement of the player
    protected abstract void MovePlayer();
}
