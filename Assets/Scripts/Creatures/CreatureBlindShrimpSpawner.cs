using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class serves to spawn in a new set of blind shrimp.  If there are too many shrimps in play, it won't spawn more.
/// </summary>
public class CreatureBlindShrimpSpawner : MonoBehaviour {

    /// <summary>
    /// A prefab reference to the shrimp to be spawned.
    /// </summary>
    public CreatureBlindShrimp prefab;
    /// <summary>
    /// How many shrimp will be spawned when this new object is spawned for the first time.
    /// </summary>
    public int shrimpCount = 10;
    /// <summary>
    /// What is the max ammount of shrimp we can spawn in?  Due to the terrain generation style, shrimp can quickly flood the screen.
    /// </summary>
    public int maxShrimp = 150;

	/// <summary>
    /// All logic is limited to the start function.  This moves this prefab 25 meters into the air, and then spawns in an appropriate number of shrimp.
    /// </summary>
	void Start () {

        transform.position = new Vector3(transform.position.x, transform.position.y + 25, transform.position.z);

        Vector3 pos = transform.position;
        for (var i = 0; i < shrimpCount; i++)
        {
            if (CreatureBlindShrimp.shrimp.Count < maxShrimp) { //don't spawn too many shrimp
                CreatureBlindShrimp shrimp = Instantiate(prefab);
                shrimp.transform.position = new Vector3(
                    pos.x + Random.Range(-5, 5),
                    pos.y + Random.Range(-1, 1),
                    pos.z + Random.Range(-5, 5));
            }
        }
	}
}
