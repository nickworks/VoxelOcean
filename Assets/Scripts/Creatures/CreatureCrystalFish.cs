using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureCrystalFish : MonoBehaviour
{

    Vector3 acceleration;
    Vector3 pos;
    Vector3 velocity;
    Vector3 target;
    Vector3 offset;
    Vector3 desired;
    float offsetRotSp;
    float r = 3.0f;
    float MAX_FORCE = .05f;
    float MAX_SPEED = 3f;
    public static List<CreatureCrystalFish> glowfish = new List<CreatureCrystalFish>();
    // Start is called before the first frame update
    void Start()
    {
        acceleration = new Vector3(0, 0, 0);
        velocity = new Vector3(Random.Range(-1,1), Random.Range(-1, 1), Random.Range(-1, 1));
        pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);

    }

    // Update is called once per frame
    void Update()
    {
       // flock();
        velocity += acceleration;
     /*   if (velocity <= MAX_SPEED)
        {

        }
       */ acceleration *= 0;
    }

    void seek(Vector3 target)
    {
     //   desired = Vector3(target.position.x, target.position.y, target.position.z);
    }
}
