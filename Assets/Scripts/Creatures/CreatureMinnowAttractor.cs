using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class acts as an attractor to minnows, controlling their movement.
/// </summary>
public class CreatureMinnowAttractor : MonoBehaviour
{
    /// <summary>
    /// adds itsself to the list of minnow attractors
    /// </summary>
    void Start()
    {
        CreatureMinnow.attracts.Add(this); 
    }

    /// <summary>
    /// Remove yourself from the minnow's list of attractors on destruction.
    /// </summary>
    private void OnDestroy()
    {
        CreatureMinnow.attracts.Remove(this); 
    }
}
