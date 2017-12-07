using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//various constant data values for use throughout the project
public class UtilitiesData
{
    //data related to the custom movement extensions
    public static class PointAndClickExtensionsData
    {
        public const float TURN_SMOOTHING = 15f;
        public const float SPEED_DAMP_TIME = 0.1f;
        public const float SLOWING_SPEED = 0.175f;
        public const float TURN_SPEED_THRESHOLD = 0.5f;
        public const float STOP_DISTANCE_PROPORTION = 0.1f;
    }
}
