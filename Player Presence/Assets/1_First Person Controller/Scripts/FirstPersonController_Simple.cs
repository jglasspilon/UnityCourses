using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController_Simple : MonoBehaviour {

    public float speedScale = 5.0f;
    private Vector2 moveSpeed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //moves the player relative to the input from the movement axes using the transform function Translate
    //this is the simplest method for moving an object in the virtual space but the least reliable when using physics (best when used in 2D games)
    private void MovePlayer(float horizontalAxis, float verticalAxis)
    {
        //scales the axis input value by our speed scale and the frame rate
        float xMovement = horizontalAxis * speedScale * Time.deltaTime;
        float zMovement = verticalAxis * speedScale * Time.deltaTime;

        //moves the player by the Vector amount
        transform.Translate(xMovement, 0, zMovement);

        //store the new movement in the speed so our controller knows how fast he is moving
        moveSpeed = new Vector2(xMovement, zMovement);
    }
}
