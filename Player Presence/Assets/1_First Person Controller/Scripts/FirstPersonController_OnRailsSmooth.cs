using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Extends the first person controller that runs on rails by adding an extra layer of smoothness
//Follows an extra invisible object that acts like a simple on rails controller
//This allows the player to perform smoother turns instead of hard turns once it reaches a node
public class FirstPersonController_OnRailsSmooth : FirstPersonController_OnRails {

    //amount of units the leader will be ahead of the follower
    public float smoothFactor;

    //reference to the leader that the player will follow
    private FirstPersonController_OnRailsSmooth_Leader leader;

	//Use this for initialization
	protected override void Start () {
        base.Start();

        //creates the leader and initializes it
        leader = new GameObject("leader").AddComponent<FirstPersonController_OnRailsSmooth_Leader>();
        leader.InitializeLeader(this);
    }
	
	//Update is called once per frame
	protected override void Update () {

        //if the player is moving then move the leader move the player towards it
		if(speedScale > 0)
        {
            leader.MoveLeader();
            transform.position = Vector3.MoveTowards(transform.position, leader.transform.position + (Vector3.up * 0.5f), speedScale * Time.deltaTime);
        }

        //if we are not moving towards the last node then make the speed of the leader slightly higher than the player
        if(leader.targetNode < movementNodes.Length - 1)
            leader.speedScale = speedScale * 1.1f;
	}

    //Called on any collision detected
    private void OnCollisionEnter(Collision collision)
    {
        //if the collided object has a leader script attached then stop the movement
        if(collision.transform.GetComponent<FirstPersonController_OnRailsSmooth_Leader>() != null)
        {
            StopMovement();
        }
    }
}
