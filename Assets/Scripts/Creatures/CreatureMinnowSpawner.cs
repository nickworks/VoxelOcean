using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is responsible for the generation of new minnows.
/// </summary>
public class CreatureMinnowSpawner : MonoBehaviour
{
    /// <summary>
    /// var to reference the prefab minnow with;
    /// </summary>
    public CreatureMinnow prefabMinnow;
    /// <summary>
    /// var to reference the prefab shark attractor with;
    /// </summary>
    public CreatureSharkAttractor prefabShark;
    /// <summary>
    /// var that sets the maximum amount of minnows that this spawner will spawn;
    /// </summary>
    public int minnowsCount = 15;
    /// <summary>
    /// var that sets the maximum amount of minnows that can be exist ata given time;
    /// </summary>
    public int maxMinnows = 150;
    /// <summary>
    /// keeps track of if this spawner has spawned minnows
    /// </summary>
    bool spawnedMinnows = false;


    /// <summary>
    /// Sets position above ground, spawns as many minnows as determined by the count and max values, spawns a shark attractor if any minnows were spawned
    /// </summary>
    void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + 20, transform.position.z);

        Vector3 pos = transform.position;
        for (int i = 0; i < minnowsCount; i++)
        {
            if(CreatureMinnow.minnows.Count < maxMinnows)
            {
                CreatureMinnow minnow = Instantiate(prefabMinnow);
                minnow.transform.position = new Vector3(
                    pos.x + Random.Range(-5, 5),
                    pos.y + Random.Range(-1, 1),
                    pos.z + Random.Range(-5, 5));
                spawnedMinnows = true;
            }
        }

        if (spawnedMinnows)
        {
            CreatureSharkAttractor shark = Instantiate(prefabShark);
            shark.transform.position = transform.position; 
        }




    }


}
