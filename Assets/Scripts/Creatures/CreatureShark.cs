using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class controlls the behavior of shark type creatures.
/// </summary>
public class CreatureShark : MonoBehaviour
{
    /// <summary>
    /// The velocity of the shark;
    /// </summary>
    Vector3 velocity;
    /// <summary>
    /// The acceleration of the shark;
    /// </summary>
    Vector3 acceleration;
    /// <summary>
    /// How strongly the shark will favor moving towards an attractor.
    /// </summary>
    public float attractStrength = 1;
    /// <summary>
    /// The degree of ability that the shark have to turn towards their target.  Higher number = faster turning.
    /// </summary>
    public float turnForce = 25;
    /// <summary>
    /// The basic speed at which the shark move.
    /// </summary>
    public float speed = 5;
    /// <summary>
    /// The desired direction to move in general
    /// </summary>
    Vector3 desire;
    /// <summary>
    /// The target position that this shark wants to head towards.
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

    /// <summary>
    /// The blood which drips when a minnow is eaten.
    /// </summary>
    ParticleSystem blood;

    /// <summary>
    /// A list of all shark attractors.
    /// </summary>
    public static List<CreatureSharkAttractor> attracts = new List<CreatureSharkAttractor>();

    /// <summary>
    /// A list of all shark type creatures.
    /// </summary>
    public static List<CreatureShark> sharks = new List<CreatureShark>();

    /// <summary>
    /// sets up the blood particle effect, vel, accel, speed, and adds itself to the public list of sharks;
    /// </summary>
    void Start()
    {
        blood = GetComponentInChildren<ParticleSystem>(); 

        velocity = new Vector3(Random.Range(-50, 50), Random.Range(-50, 50), Random.Range(-50, 50));

        acceleration = new Vector3();

        speed *= Random.Range(.75f, 1.5f);

        sharks.Add(this); 

    }

    /// <summary>
    /// An update that uses ticker to run the sharks logic every other tick;
    /// </summary>
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

    /// <summary>
    /// Takes the information from the shark attractors and finds a force that points to that. Force is stronger the farther from the attractor;
    /// </summary>
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
            target = b.transform.position - transform.position;
            desire += target.normalized * (dist / 5);

        }
        AddForce(desire * turnForce);

        //handle eating minnows
        if (CreatureMinnow.minnows != null)
        {
            for (int i = 0; i < CreatureMinnow.minnows.Count; i++)//  CreatureMinnow m in CreatureMinnow.minnows)
            {
                CreatureMinnow m = CreatureMinnow.minnows[i];
                float dist = Vector3.Distance(transform.position, m.transform.position);
                if (dist <= transform.localScale.x / 1.5f)
                {
                    blood.Play(); 

                    CreatureMinnow.minnows.Remove(m);

                    Destroy(m.gameObject);


                }
            }
        }


    }
    /// <summary>
    /// handles the movement of the shark;
    /// </summary>
    void Move()
    {
        velocity += acceleration * Time.deltaTime;
        if (velocity.sqrMagnitude > speed * speed) velocity = velocity.normalized * speed;
        transform.position = transform.position + velocity * Time.deltaTime;

        transform.LookAt(transform.position + velocity);
    }
    /// <summary>
    /// Shortcut to add force to the acceleration;
    /// </summary>
    void AddForce(Vector3 force)
    {
        acceleration += force;
    }
}
