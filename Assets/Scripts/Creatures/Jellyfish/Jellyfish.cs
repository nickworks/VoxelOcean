using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jellyfish : MonoBehaviour
{
    /// <summary>
    /// The base speed that the Jellyfish move
    /// </summary>
    public float speed = 1;

    /// <summary>
    /// The velocity of the jellyfish
    /// </summary>
    Vector3 velocity;
    /// <summary>
    /// the acceleration of the jellyfish
    /// </summary>
    Vector3 accel;

    /// <summary>
    /// Defines a random starting velocity of the jellyfish, and a random speed multiplier
    /// </summary>
    void Start()
    {
        velocity = new Vector3(Random.Range(.25f,.5f), 0, Random.Range(.25f,.5f));

        accel = new Vector3();
        speed *= Random.Range(.25f, .50f);
    }

    /// <summary>
    /// Determines movement
    /// </summary>
    void Update()
    {
        velocity += accel * Time.deltaTime;

        transform.position = transform.position + velocity * Time.deltaTime;
    }

    /// <summary>
    /// Applies force to the jellyfish
    /// </summary>
    /// <param name="force"></param>
    void AddForce(Vector3 force)
    {
        accel += force;
    }
}
