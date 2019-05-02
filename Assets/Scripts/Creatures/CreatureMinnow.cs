//heavily based off Chris's blind shrimp script so I can quickly get to the shark and the interaction between the minnows and the shark
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureMinnow : MonoBehaviour
{

    Vector3 velocity;

    Vector3 acceleration;

    /// <summary>
    /// A simple ticker value which incriments every frame, used in conjunction with % to limit the use of certain actions.
    /// </summary>
    int ticker = 0;

    public static List<CreatureMinnow> minnows = new List<CreatureMinnow>(); 

    // Start is called before the first frame update
    void Start()
    {
        velocity = new Vector3(Random.Range(-50, 50), Random.Range(-50, 50), Random.Range(-50, 50));

        acceleration = new Vector3();

        minnows.Add(this); 
    }

    // Update is called once per frame
    void Update()
    {
        ticker++; 
        acceleration = Vector3.zero;

        pickDirection();
        Move(); 
    }

    void pickDirection()
    {
        if (minnows.IndexOf(this) % 2 == 0 && ticker % 2 == 0 ||
            minnows.IndexOf(this) % 2 != 0 && ticker % 2 != 0) return;








    }

    void Move()
    {

    }

}
