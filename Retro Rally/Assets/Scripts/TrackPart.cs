using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackPart : MonoBehaviour
{
    public TrackNames name;
    public enum TrackNames
    {
        forward,
        left,
        right
    };
}
