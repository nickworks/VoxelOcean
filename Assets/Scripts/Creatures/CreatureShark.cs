using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureShark : MonoBehaviour
{
    Vector3 velocity;

    Vector3 acceleration;


    /// <summary>
    /// The steering velocity of this shrimp;
    /// </summary>
    Vector3 steer;
    /// <summary>
    /// How strongly the shrimp will favor moving towards an attractor.
    /// </summary>
    public float attractStrength = 1;
    /// <summary>
    /// how strongly the shrimp will favor moving away from a repulsor.
    /// </summary>
    public float repulsStrength = 1;
    /// <summary>
    /// The degree of ability that the shrimp have to turn towards their target.  Higher number = faster turning.
    /// </summary>
    public float turnForce = 25;
    /// <summary>
    /// The basic speed at which the sprimp move.
    /// </summary>
    public float speed = 5;
    /// <summary>
    /// The desired direction to move in general
    /// </summary>
    Vector3 desire;
    /// <summary>
    /// How close the shrimp are allowed to swim to one another before they push away from each other.
    /// </summary>
    public float closeDist = 20;
    /// <summary>
    /// The target position that this minnow wants to head towards.
    /// </summary>
    Vector3 target;
    /// <summary>
    /// The desired direction to move in to align with neighbors.
    /// </summary>
    Vector3 directionToAlign;
    /// <summary>
    /// the desired direction to move in seperate from neighbors.
    /// </summary>
    Vector3 directionToSeperate;
    /// <summary>
    /// The cenger of the school of minnows.
    /// </summary>
    Vector3 flockCenter;
    /// <summary>
    /// The direction needed to move in to target the center of the school;
    /// </summary>
    Vector3 directionToCongregate;
    /// <summary>
    /// A simple ticker value which incriments every frame, used in conjunction with % to limit the use of certain actions.
    /// </summary>
    int ticker = 0;

    public static List<CreatureSharkAttractor> attracts = new List<CreatureSharkAttractor>();

    public static List<CreatureShark> sharks = new List<CreatureShark>(); 

    // Start is called before the first frame update
    void Start()
    {
        velocity = new Vector3(Random.Range(-50, 50), Random.Range(-50, 50), Random.Range(-50, 50));

        acceleration = new Vector3();

        speed *= Random.Range(.75f, 1.5f);

        sharks.Add(this); 


       // CreatureMinnow.minnows to access a list of the minnows

    }

    // Update is called once per frame
    void Update()
    {
        if (ticker == 1)
        {
            acceleration = Vector3.zero;

            pickDirection();
            Move();
            ticker = 0;
        }
        ticker++; 
    }

    void pickDirection()
    {

        directionToAlign = Vector3.zero;
        directionToCongregate = Vector3.zero;
        directionToSeperate = Vector3.zero;
        flockCenter = Vector3.zero;
        float shortestDist = 1000000;


        //handle attractors
        shortestDist = 100000000;
        desire = Vector3.zero;
        foreach (CreatureSharkAttractor b in attracts)
        {
            float dist = Vector3.Distance(transform.position, b.transform.position);
            if (dist < shortestDist) shortestDist = dist;
            //TODO:
            // get direction away from every near boid, inversely weighted by proximity
            target = b.transform.position - transform.position;
            desire += target.normalized * (dist / 5);

        }
        AddForce(desire);

        if (CreatureMinnow.minnows != null)
        {
            for (int i = 0; i < CreatureMinnow.minnows.Count; i++)//  CreatureMinnow m in CreatureMinnow.minnows)
            {
                CreatureMinnow m = CreatureMinnow.minnows[i];
                float dist = Vector3.Distance(transform.position, m.transform.position);
                if (dist <= transform.localScale.x / 2)
                {
                    CreatureMinnow.minnows.Remove(m);

                    Destroy(m.gameObject);
                }
            }
        }


    }

    void Move()
    {
        velocity += acceleration * Time.deltaTime;
        if (velocity.sqrMagnitude > speed * speed) velocity = velocity.normalized * speed;
        transform.position = transform.position + velocity * Time.deltaTime;

        transform.LookAt(transform.position + velocity);
    }

    void AddForce(Vector3 force)
    {
        acceleration += force;
    }
}
