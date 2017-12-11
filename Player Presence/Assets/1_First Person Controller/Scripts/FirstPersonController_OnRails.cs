using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//First person controller that puts the player movement our of the player's hands.
//Will move the player along a predetermined path using an array of movement nodes (Transforms)
public class FirstPersonController_OnRails : FirstPersonController
{
    //related to the on rail path
    public Transform[] movementNodes;                               //holds a reference to each transform that the player is going to move towards
    protected int targetNode = 1;                                     //index reference to the target node within the movement nodes array

    //related to any active coroutines to prevent double calling
    protected Coroutine speedChangeRoutine = null;                    //keeps a reference of the active coroutine that is currently affecting the speed
    

    //Start is called once on start up. Used for Initialization
    protected virtual void Start()
    {
        //Move the player to the first node and make it look towards the next only if there is more than 1 node
        if (movementNodes.Length > 1)
        {
            transform.position = movementNodes[0].position;
            transform.LookAt(movementNodes[1]);
        }

        //otherewise there are not enough nodes for the player to move so send a warning message
        else
        {
            Debug.LogError("there is not enough nodes for the player to move. Make sure there are at least 2 nodes in the movement node array");
        }
    }

    //Update is called once every frame
    protected virtual void Update()
    {
        if(movementNodes.Length > 0)
            MovePlayer();
    }

    //moves the forward along the movement nodes path from one node to the next. 
    //Movement stops once the player reaches the final node
    protected override void MovePlayer()
    {
        //if the player has reached the target node...
        if(Vector3.Distance(transform.position, movementNodes[targetNode].position) <= 0.1f)
        {
            //if the target node is not the last then move on to the next node
            if (targetNode < movementNodes.Length - 1)
            {
                targetNode++;
            }

            //otherwise this is the last node so send a warning and stop the player movement
            else
            {
                Debug.LogWarning("Player has reached the end of the nodes path");
                StopMovement();
                return;
            }
        }

        //otherwise the player has yet to reach the target node so if the player is moving move it towards the target node
        else
        {
            if(speedScale > 0)
            {
                transform.position = Vector3.MoveTowards(transform.position, movementNodes[targetNode].position, speedScale * Time.deltaTime);
            }
        }
    }

    /// <summary>
    /// Speeds up or slows down the player until it hits the target speed at a rate of acceleration
    /// </summary>
    /// <param name="targetSpeed">the target speed we want the player to reach</param>
    /// <param name="acceleration">the rate at which we want the speed to change - 1 = instant speed change</param>
    public void CallChangeSpeed(float targetSpeed, float acceleration)
    {
        //if a couroutine is already started then stop that coroutine so we can start a new one
        if(speedChangeRoutine != null)
        {
            StopCoroutine(speedChangeRoutine);
        }

        //set the new coroutine to change the speed of the player and start it
        speedChangeRoutine = StartCoroutine(ChangeSpeed(targetSpeed, acceleration));
    }

    //linearly interpolates the move speed towards the target speed at a rate of acceleration
    private IEnumerator ChangeSpeed(float targetSpeed, float acceleration)
    {
        //keeps track of how far along our linear interpolation we are (0 at starting point, 1 at target point)
        float counter = 0.0f;

        //ensures that our acceleration is between 0.01 and 1
        //if the acceleration was 0 or less, then we would never reach our target speed
        //if the acceleration was greater than 1 then we sould surpass our target speed
        //if the acceleration is equal to 1 then the speed change is instantanious
        acceleration = Mathf.Clamp(acceleration, 0.01f, 1.0f);

        //loops as long as the move speed is not approximately equal to the target speed
        while(!Mathf.Approximately(speedScale, targetSpeed))
        {
            //Linearly interpolate from movespeed to target speed based on how far along we are on the counter
            speedScale = Mathf.Lerp(speedScale, targetSpeed, counter);

            //increase the counter by acceleration
            counter += acceleration * Time.deltaTime;

            //yield until the next frame
            yield return null;
        }
    }

    /// <summary>
    /// Completely stops the player movement 
    /// </summary>
    public void StopMovement()
    {
        //if there is a current coroutine running to alter the speed, stop it and remove it from reference
        if(speedChangeRoutine != null)
        {
            StopCoroutine(speedChangeRoutine);
            speedChangeRoutine = null;
        }

        //set the speed to 0
        speedScale = 0;
    }
}
