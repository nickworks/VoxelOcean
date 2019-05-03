using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class serves as an attractor for the shrimp.  This is to keep them reasonably contained within the reef instead of swimming off in a random direction forever.
/// </summary>
public class CreatureBlindShrimpAttractor : MonoBehaviour {
    
	/// <summary>
    /// Add yourself to the shrimps list of attractors.
    /// </summary>
	void Start () {
        CreatureBlindShrimp.attracts.Add(this);
	}
    /// <summary>
    /// Remove yourself from the shrimp's list of attractors on destruction.
    /// </summary>
    private void OnDestroy()
    {
        CreatureBlindShrimp.attracts.Remove(this);
    }
}
