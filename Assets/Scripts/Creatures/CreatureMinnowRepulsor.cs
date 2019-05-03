using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureMinnowRepulsor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CreatureMinnow.repulses.Add(this); 
    }

    // Update is called once per frame
    private void OnDestroy()
    {
        CreatureMinnow.repulses.Remove(this);
    }
}
