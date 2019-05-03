using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureSharkAttractor : MonoBehaviour
{

    public CreatureShark prefab;

    public int sharkCount = 1;

    public int maxSharks = 1;

    int ticker = 0; 

    // Start is called before the first frame update
    void Start()
    {
        //spawn 1 shark, ever
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

    // Update is called once per frame
    void Update()
    {
        if (ticker == 1)
        {
            //get all minnows within a certain distance
            float runningTotal = 0;
            float range = 10000;
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

    private void OnDestroy()
    {
        CreatureShark.attracts.Remove(this);
    }
}
