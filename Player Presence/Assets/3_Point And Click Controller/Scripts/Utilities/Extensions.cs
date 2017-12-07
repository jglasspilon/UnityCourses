using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

//Extension methods add functonality to the component type intended (the first parameter type using the "this" keyword)
public static class PointAndClickExtensions
{
    /// <summary>
    /// Controls the nav mesh agent movement to a point on the nav mesh by splitting it into 3 states: Stopping, Slowing and Moving
    /// </summary>
    /// <param name="agent">Nav mesh agent to move</param>
    /// <param name="destinationPosition">New destination to set for agent to move towards</param>
    /// <param name="playerAnimator">Animator to update with new speed values</param>
    public static void MoveToPoint(this NavMeshAgent agent, Vector3 destinationPosition, Animator playerAnimator = null)
    {
        //return out of the function without doing anything if the agent is currently calculating a path
        if (agent.pathPending)
        {
            return;
        }

        //get the speed from the agent (this calculates the desired velocity based on the destination point) 
        float speed = agent.desiredVelocity.magnitude;

        //if the agent is within the modified stopping distance (a fraction of the agent's stopping distance value), stop the agent
        if (agent.remainingDistance <= agent.stoppingDistance * UtilitiesData.PointAndClickExtensionsData.STOP_DISTANCE_PROPORTION)
        {
            Stopping(out speed, agent, destinationPosition);
        }

        //otherwise if the agent is within the agent's natural stopping distance, start slowing the agent
        else if (agent.remainingDistance <= agent.stoppingDistance)
        {
            Slowing(out speed, agent, destinationPosition);
        }

        //otherwise if the speed is greater than the turn speed threshold, move the agent towards the target
        //the threshold check prevents the player from making sharp, unnatural turns at low speeds
        else if (speed > UtilitiesData.PointAndClickExtensionsData.TURN_SPEED_THRESHOLD)
        {
            Moving(agent);
        }

        //if the animator exists, set the animator's speed parameter to the speed value.
        if (playerAnimator != null)
        {
            playerAnimator.SetFloat("Speed", speed);
        }
    }

    /// <summary>
    /// Controls the nav mesh agent movement to an object splitting it into 3 states: Stopping, Slowing and Moving
    /// </summary>
    /// <param name="agent">Nav mesh agent to move</param>
    /// <param name="destination">New transform to set for agent to move towards</param>
    /// <param name="playerAnimator">Animator to update with new speed values</param>    
    public static void MoveToObject(this NavMeshAgent agent, Transform destination, Animator playerAnimator = null)
    {
        //return out of the function without doing anything if the agent is currently calculating a path
        if (agent.pathPending)
        {
            return;
        }

        //get the speed from the agent (this calculates the desired velocity based on the destination point
        float speed = agent.desiredVelocity.magnitude;

        //if the agent is within the modified stopping distance (a fraction of the agent's stopping distance value), stop the agent
        if (agent.remainingDistance <= agent.stoppingDistance * UtilitiesData.PointAndClickExtensionsData.STOP_DISTANCE_PROPORTION)
        {
            Stopping(out speed, agent, destination.position);
        }

        //otherwise if the agent is within the agent's natural stopping distance, start slowing the agent
        else if (agent.remainingDistance <= agent.stoppingDistance)
        {
            Slowing(out speed, agent, destination);
        }

        //otherwise if the speed is greater than the turn speed threshold, move the agent towards the target
        //the threshold check prevents the player from making sharp, unnatural turns at low speeds
        else if (speed > UtilitiesData.PointAndClickExtensionsData.TURN_SPEED_THRESHOLD)
        {
            Moving(agent);
        }

        //if the animator exists, set the animator's speed parameter to the speed value.
        if (playerAnimator != null)
        {
            playerAnimator.SetFloat("Speed", speed);
        }
    }

    /// <summary>
    /// Returns true if the agent has reached its destination
    /// </summary>
    /// <param name="agent"></param>
    /// <returns>if the agent has arrived</returns>
    public static bool HasArrived(this NavMeshAgent agent)
    {
        //return false if the agent is currently calculating its path
        if (agent.pathPending)
        {
            return false;  
        }

        return (agent.remainingDistance <= (agent.stoppingDistance * UtilitiesData.PointAndClickExtensionsData.STOP_DISTANCE_PROPORTION));
    }

    //stops the agent and moves it into the proper position
    private static void Stopping(out float speed, NavMeshAgent agent, Vector3 destinationPosition)
    {
        agent.isStopped = true;
        if((destinationPosition - agent.destination).magnitude < UtilitiesData.PointAndClickExtensionsData.STOP_DISTANCE_PROPORTION)
        agent.transform.position = destinationPosition + new Vector3(0, agent.baseOffset, 0);
        speed = 0f;
    }

    //stops the agent but maintains it's previous speed in order to manually slow it down towards the target position
    private static void Slowing(out float speed, NavMeshAgent agent, Vector3 destinationPosition)
    {
        agent.isStopped = true;

        //position calculations
        float proportionalDistance = 1f - agent.remainingDistance / agent.stoppingDistance;
        agent.transform.position = Vector3.MoveTowards(agent.transform.position, destinationPosition, UtilitiesData.PointAndClickExtensionsData.SLOWING_SPEED * Time.deltaTime);

        //gradual slow down
        speed = Mathf.Lerp(UtilitiesData.PointAndClickExtensionsData.SLOWING_SPEED, 0f, proportionalDistance);
    }

    //stops the agent but maintains it's previous speed in order to manually slow it down towards the target object and graudually rotate it to the desired rotation
    private static void Slowing(out float speed, NavMeshAgent agent, Transform destination)
    {
        agent.isStopped = true;

        //position calculations
        float proportionalDistance = 1f - agent.remainingDistance / agent.stoppingDistance;
        agent.transform.position = Vector3.MoveTowards(agent.transform.position, destination.position, UtilitiesData.PointAndClickExtensionsData.SLOWING_SPEED * Time.deltaTime);

        //rotation calculations
        Quaternion targetRotation = destination.rotation;
        agent.transform.rotation = Quaternion.Lerp(agent.transform.rotation, targetRotation, proportionalDistance);

        //gradual slow down
        speed = Mathf.Lerp(UtilitiesData.PointAndClickExtensionsData.SLOWING_SPEED, 0f, proportionalDistance);
    }

    //gradually rotates the agent towards the position it is walking towards
    private static void Moving(NavMeshAgent agent)
    {
        Quaternion targetRotation = Quaternion.LookRotation(agent.desiredVelocity);
        agent.transform.rotation = Quaternion.Lerp(agent.transform.rotation, targetRotation, UtilitiesData.PointAndClickExtensionsData.TURN_SMOOTHING * Time.deltaTime);
    }
}

