using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureMinnowRepulsor : MonoBehaviour
{
    /// <summary>
    /// Adds itsself to the list of minnow repulsors
    /// </summary>
    void Start()
    {
        CreatureMinnow.repulses.Add(this); 
    }

    /// <summary>
    /// Removes itself from the list of minnow repulsors on destruction
    /// </summary>
    private void OnDestroy()
    {
        CreatureMinnow.repulses.Remove(this);
    }
}
