using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureMinnowSpawner : MonoBehaviour
{

    public CreatureMinnow prefab;

    public int minnowsCount = 15;

    public int maxMinnows = 150; 


    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + 15, transform.position.z);

        Vector3 pos = transform.position;
        for (int i = 0; i < minnowsCount; i++)
        {
            if(CreatureMinnow.minnows.Count < maxMinnows)
            {
                CreatureMinnow minnow = Instantiate(prefab);
                minnow.transform.position = new Vector3(
                    pos.x + Random.Range(-5, 5),
                    pos.y + Random.Range(-1, 1),
                    pos.z + Random.Range(-5, 5));

            }
        }






    }


}
