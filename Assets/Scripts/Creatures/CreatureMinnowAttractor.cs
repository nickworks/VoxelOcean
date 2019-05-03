using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureMinnowAttractor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CreatureMinnow.attracts.Add(this); 
    }

    /// <summary>
    /// Remove yourself from the shrimp's list of attractors on destruction.
    /// </summary>
    private void OnDestroy()
    {
        CreatureMinnow.attracts.Remove(this); 
    }
}
