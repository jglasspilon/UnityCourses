using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Extends the first person controller that runs on rails by adding an extra layer of smoothness
//Follows an extra invisible object that acts like a simple on rails controller
//This allows the player to perform smoother turns instead of hard turns once it reaches a node
public class FirstPersonController_OnRailsSmooth : FirstPersonController_OnRails {

    //amount of units the leader will be ahead of the follower
    public float smoothFactor;

    private FirstPersonController_OnRailsSmooth_Leader leader;

	// Use this for initialization
	protected override void Start () {
        base.Start();

        leader = new GameObject("leader").AddComponent<FirstPersonController_OnRailsSmooth_Leader>();
        leader.InitializeLeader(this);
    }
	
	// Update is called once per frame
	protected override void Update () {
		if(speedScale > 0)
        {
            leader.MoveLeader();
            transform.position = Vector3.MoveTowards(transform.position, leader.transform.position + (Vector3.up * 0.5f), speedScale * Time.deltaTime);
        }

        if(leader.targetNode < movementNodes.Length - 1)
            leader.speedScale = speedScale * 1.1f;
	}

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.GetComponent<FirstPersonController_OnRailsSmooth_Leader>() != null)
        {
            StopMovement();
        }
    }
}
