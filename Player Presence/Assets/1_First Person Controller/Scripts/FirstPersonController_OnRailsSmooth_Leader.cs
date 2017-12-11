using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Component of the smooth on rails first person controller
//acts as a moving target that the player will follow in order to produce smoother turns
public class FirstPersonController_OnRailsSmooth_Leader : MonoBehaviour{

    public float speedScale;
    private Transform[] movementNodes;
    public int targetNode = 1;

    //Initializes the object so it matches the values of the follower 
    public void InitializeLeader(FirstPersonController_OnRailsSmooth follower)
    {
        transform.position = follower.transform.position + (follower.transform.forward * follower.smoothFactor);
        movementNodes = follower.movementNodes;
        speedScale = follower.speedScale;

        gameObject.AddComponent<Rigidbody>().freezeRotation = true;
        gameObject.AddComponent<CapsuleCollider>().center = new Vector3(0, 0.99f, 0);
    }

    public void MoveLeader()
    {
        //if the player has reached the target node...
        if (Vector3.Distance(transform.position, movementNodes[targetNode].position) <= 0.5f)
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
                speedScale = 0;
                return;
            }
        }

        //otherwise the player has yet to reach the target node so if the player is moving move it towards the target node
        else
        {
            if (speedScale > 0)
            {
                transform.position = Vector3.MoveTowards(transform.position, movementNodes[targetNode].position, speedScale * Time.deltaTime);
            }
        }
    }
}
