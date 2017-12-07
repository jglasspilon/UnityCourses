using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAndClick_CameraSector : MonoBehaviour {

    //sector's camera identity (where the camera should be located if the player is within the sector)
    public Transform sectorIdentity;

    //reference to the camera's controller script
    private PointAndClickController_Camera cam;

	//Start is called once on start up. Used for Initialization
	private void Start () {

        //get the camera controller script from the main camera in the scene
        cam = Camera.main.transform.GetComponent<PointAndClickController_Camera>();
	}

    //Called once as soon as this object enters within another collider
    private void OnTriggerEnter(Collider collision)
    {
        //if the object that has just collided with us has a point and click controller script then trigger the camera move
        if(collision.transform.GetComponent<PointAndClickController>() != null)
        {
            cam.MoveCameraToNewSector(sectorIdentity);
        }
    }
}
