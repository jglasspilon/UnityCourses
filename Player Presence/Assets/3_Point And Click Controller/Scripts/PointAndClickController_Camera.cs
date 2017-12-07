using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Controls the camera's behaviour
//Camera is static and will teleport from one sector to another when triggered
public class PointAndClickController_Camera : MonoBehaviour {

    /// <summary>
    /// When called moves the camera to the new identity's location and matches it's rotation
    /// </summary>
    /// <param name="newIdentity">new transform identity for the camera to match</param>
    public void MoveCameraToNewSector(Transform newIdentity)
    {
        transform.position = newIdentity.position;
        transform.rotation = newIdentity.rotation;
    }
}
