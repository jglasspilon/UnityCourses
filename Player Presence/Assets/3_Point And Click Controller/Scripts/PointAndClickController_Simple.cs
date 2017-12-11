using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;

//Simple point and click controller that uses the navmesh Unity system
//will be able to move to a point on a nav mesh or to the closest point on the navmesh to the clicked point
public class PointAndClickController_Simple : PointAndClickController
{
    //related to character navigation
    private NavMeshAgent agent;                 //reference to the player's navmesh agent 
    private Vector3 targetPosition;             //tells the navmesh agent where to move to along the navmesh

    //Start is called once upon start up. Used for initialization
    private void Start()
    {
        //get the reference to our navmesh agent and set the target position to the player's current location
        //this prevent the player from moving on start up and prevents any null-ref errors
        agent = GetComponent<NavMeshAgent>();
        targetPosition = transform.position;
    }

    //Update is called once every frame
    private void Update()
    {
        PolishMovement();
    }

    /// <summary>
    /// Moves the player to a point on the navmesh
    /// </summary>
    /// <param name="data">mouse event data holding information about the user pressing the mouse button</param>
    public override void MoveToPoint(BaseEventData data)
    {
        //since data is a base event data, it has generic information so we need to cast it into a more specific event data that we can use
        //we can cast it into the more specific Pointer event data which holds all of our mouse click data such as which button was press and where
        //in the world the mouse pointer was location 
        PointerEventData pData = (PointerEventData)data;

        //set the target position to the position (in world space) that the mouse was pointing to
        targetPosition = pData.pointerCurrentRaycast.worldPosition;

        //set the navmesh agent's destination to our target position and start it
        agent.SetDestination(targetPosition);
        agent.isStopped = false;
    }

    //Polishes the movement so the player turns and moves more naturally 
    private void PolishMovement()
    {
        agent.MoveToPoint(targetPosition);
    }
}
