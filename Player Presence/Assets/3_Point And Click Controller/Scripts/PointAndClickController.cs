using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//Base class for all point and click controllers
public abstract class PointAndClickController : MonoBehaviour {

    //abstract function that will control player movement to a point on a navmesh
    public abstract void MoveToPoint(BaseEventData data);
}
