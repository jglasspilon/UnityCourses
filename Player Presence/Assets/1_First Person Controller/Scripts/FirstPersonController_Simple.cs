using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//allows the player to move in the virtual space as if looking through the eyes of the character
//uses simple transform manipulatation to accomplish this. Easiest method but not always reliable in a 3d space
public class FirstPersonController_Simple : FirstPersonController {
	
	//Update is called once per frame
	void Update () {
        MovePlayer();
	}

    //moves the player relative to the input from the movement axes using the transform function Translate
    //this is the simplest method for moving an object in the virtual space but the least reliable when using physics (best when used in 2D games)
    protected override void MovePlayer()
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
}
