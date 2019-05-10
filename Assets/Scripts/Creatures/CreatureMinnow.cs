//heavily based off Chris's blind shrimp script so I can quickly get to the shark and the interaction between the minnows and the shark
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class controls the behavior of minnow type creatures.
/// </summary>
public class CreatureMinnow : MonoBehaviour
{
    /// <summary>
    /// velocity of the minnow;
    /// </summary>
    Vector3 velocity;
    /// <summary>
    /// acceleration of the minnow;
    /// </summary>
    Vector3 acceleration;
    /// <summary>
    /// The steering velocity of this shrimp;
    /// </summary>
    Vector3 steer;
    /// <summary>
    /// How heavily the shrimp will favor the behavior of swimming in the same direction as their closest neighbors.
    /// </summary>
    public float alignWeight = 1;
    /// <summary>
    /// How heavily the shrimp will favor swimming towards their neighbors.
    /// </summary>
    public float cohesionWeight = 1;
    /// <summary>
    /// How heavily the shrimp will favor swimming apart from one another to avoid collisions.
    /// </summary>
    public float seperateWeight = 1;
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
    /// A simple ticker value which incriments every frame, used in conjunction with % to limit the use of certain actions.
    /// </summary>
    int ticker = 0;
    /// <summary>
    /// The cenger of the school of minnows.
    /// </summary>
    Vector3 flockCenter;
    /// <summary>
    /// The direction needed to move in to target the center of the school;
    /// </summary>
    Vector3 directionToCongregate;
    /// <summary>
    /// list of all the minnows;
    /// </summary>
    public static List<CreatureMinnow> minnows = new List<CreatureMinnow>();
    /// <summary>
    /// list of all the minnow attractors;
    /// </summary>
    public static List<CreatureMinnowAttractor> attracts = new List<CreatureMinnowAttractor>();
    /// <summary>
    /// list of all the minnow repulsors;
    /// </summary>
    public static List<CreatureMinnowRepulsor> repulses = new List<CreatureMinnowRepulsor>();


    /// <summary>
    /// sets vel, accel, and speed. Adds self to list of minnows;
    /// </summary>
    void Start()
    {
        velocity = new Vector3(Random.Range(-50, 50), Random.Range(-50, 50), Random.Range(-50, 50));

        acceleration = new Vector3();

        speed *= Random.Range(.75f, 1.2f); 

        minnows.Add(this); 
    }

    /// <summary>
    /// finds a desired direction and moves towards it;
    /// </summary>
    void Update()
    {
        ticker++; 

        acceleration = Vector3.zero;

        pickDirection();
        Move(); 
    }
    /// <summary>
    /// finds a desired direction using a flock algorithm, align, separate, congregate, attractors, and repulsors;
    /// </summary>
    void pickDirection()
    {
        //logic to stop this function from doing stuff every frame
        if (minnows.IndexOf(this) % 2 == 0 && ticker % 2 == 0 ||
            minnows.IndexOf(this) % 2 != 0 && ticker % 2 != 0) return;

        directionToAlign = Vector3.zero;
        directionToCongregate = Vector3.zero;
        directionToSeperate = Vector3.zero;
        flockCenter = Vector3.zero;
        float runningTotal = 0;
        float shortestDist = 1000000;

        foreach (CreatureMinnow b in minnows)
        {
            float dist = Vector3.Distance(transform.position, b.transform.position);
            if (dist == 0) continue;
            if (dist < closeDist)
            {
                if (dist < shortestDist) shortestDist = dist;
                target = transform.position - b.transform.position;
                directionToSeperate += target.normalized / (dist * dist);
                // get direction towards center of flock, weighted by distance
                flockCenter += b.transform.position;
                // align direction with neighbors, maybe weighted?
                directionToAlign += b.velocity;

                runningTotal++;
            }
        }
        if(runningTotal > 0)
        {
            directionToAlign /= runningTotal;
            flockCenter /= runningTotal;

            //calc direction to congregate
            directionToCongregate = flockCenter - transform.position;
            float distToCenter = Vector3.Distance(flockCenter, transform.position);

            directionToAlign = directionToAlign.normalized * speed;
            directionToSeperate = directionToSeperate.normalized * speed;
            directionToCongregate = directionToCongregate.normalized * speed;


            //align
            steer = (directionToAlign - velocity).normalized;

            AddForce(steer * turnForce * alignWeight * (10 / shortestDist));

            //seperate
            steer = (directionToSeperate - velocity).normalized;

            AddForce(steer * turnForce * seperateWeight * (10 / shortestDist));

            //congregate
            steer = (directionToCongregate - velocity).normalized;

            AddForce(steer * turnForce * cohesionWeight * (distToCenter / 50)); //50
        }

        //handle attractors
        shortestDist = 100000000;
        desire = Vector3.zero;
        foreach (CreatureMinnowAttractor b in attracts)
        {
            float dist = Vector3.Distance(transform.position, b.transform.position);
            if (dist < shortestDist) shortestDist = dist;
            target = b.transform.position - transform.position;
            desire += target.normalized * (dist/100);
            
        }
        AddForce(desire * attractStrength);



        //handle repulsors
        float range = 5;
        desire = Vector3.zero;

        foreach(CreatureMinnowRepulsor b in repulses)
        {
            float dist = Vector3.Distance(transform.position, b.transform.position);
            if (dist < range)
            {
                //finds the direction towards the repulsor and goes the other way
                target = transform.position - b.transform.position;
                desire += target.normalized * (1.5f / dist);
            }
            
        }
        AddForce(desire * repulsStrength * turnForce);

    }
    /// <summary>
    /// moves the shark;
    /// </summary>
    void Move()
    {
        velocity += acceleration * Time.deltaTime;
        if (velocity.sqrMagnitude > speed * speed) velocity = velocity.normalized * speed;
        transform.position = transform.position + velocity * Time.deltaTime;

        transform.LookAt(transform.position + velocity);
    }
    /// <summary>
    /// shortcut to add force to acceleration;
    /// </summary>
    void AddForce(Vector3 force)
    {
        acceleration += force; 
    }

}
