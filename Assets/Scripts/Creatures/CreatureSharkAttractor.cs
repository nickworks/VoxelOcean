using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureSharkAttractor : MonoBehaviour
{
    /// <summary>
    /// var to reference the prefab shark with;
    /// </summary>
    public CreatureShark prefab;
    /// <summary>
    /// number of sharks this can spawn;
    /// </summary>
    public int sharkCount = 1;
    /// <summary>
    /// number of sharks that can exist concurrently;
    /// </summary>
    public int maxSharks = 1;
    /// <summary>
    /// controls update speed;
    /// </summary>
    int ticker = 0;

    /// <summary>
    /// spawns a shark, adds self to the shark attractor list;
    /// </summary>
    void Start()
    {
        
        Vector3 pos = transform.position;
        for (int i = 0; i < sharkCount; i++)
        {
            if (CreatureShark.sharks.Count < maxSharks)
            {
                CreatureShark shark = Instantiate(prefab);
                shark.transform.position = new Vector3(
                    pos.x,
                    pos.y,
                    pos.z);

            }
        }

        CreatureShark.attracts.Add(this);

    }

    /// <summary>
    /// update every other tick, finds location that is in center of minnows within a range and sets location to there, if no minnows in range destroys self;
    /// </summary>
    void Update()
    {
        if (ticker == 1)
        {
            //get all minnows within a certain distance
            float runningTotal = 0;
            float range = 10;
            Vector3 target = Vector3.zero;
            foreach (CreatureMinnow b in CreatureMinnow.minnows)
            {
                float dist = Vector3.Distance(transform.position, b.transform.position);
                if (dist < range)
                {
                    target += b.transform.position;
                    runningTotal++;
                }
            }
            if (runningTotal == 0)
            {
                CreatureShark.attracts.Remove(this);
                Destroy(this);
            }
            else if (runningTotal > 0)
            {
                target = target / runningTotal;
                transform.position = target;
            }
            ticker = 0;
        }
        ticker++; 
    }
    /// <summary>
    /// removes self from shark attractor list on destruction;
    /// </summary>
    private void OnDestroy()
    {
        CreatureShark.attracts.Remove(this);
    }
}
