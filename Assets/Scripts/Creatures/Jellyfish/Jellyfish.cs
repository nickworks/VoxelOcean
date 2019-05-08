using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jellyfish : MonoBehaviour
{
    public float speed = 1;

    Vector3 velocity;
    Vector3 accel;

    // Start is called before the first frame update
    void Start()
    {
        velocity = new Vector3(Random.Range(.25f,.5f), 0, Random.Range(.25f,.5f));

        accel = new Vector3();
        speed *= Random.Range(.25f, .50f);
    }

    // Update is called once per frame
    void Update()
    {
        velocity += accel * Time.deltaTime;

        transform.position = transform.position + velocity * Time.deltaTime;
    }

    void AddForce(Vector3 force)
    {
        accel += force;
    }
}
