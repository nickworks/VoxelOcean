using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This flocking boid style shrimp will serve to populate the reef with some interesting life.
/// </summary>
public class CreatureBlindShrimp : MonoBehaviour
{
    /// <summary>
    /// The degree of ability that the shrimp have to turn towards their target.  Higher number = faster turning.
    /// </summary>
    public float turnForce = 25;
    /// <summary>
    /// The basic speed at which the sprimp move.
    /// </summary>
    public float speed = 5;
    /// <summary>
    /// How close the shrimp are allowed to swim to one another before they push away from each other.
    /// </summary>
    public float closeDist = 20;
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
    /// A simple ticker value which incriments every frame, used in conjunction with % to limit the use of certain actions.
    /// </summary>
    int ticker = 0;
    /// <summary>
    /// The velocity of this shrimp;
    /// </summary>
    Vector3 veloc;
    /// <summary>
    /// The acceleration of this shrimp;
    /// </summary>
    Vector3 accel;
    /// <summary>
    /// The steering velocity of this shrimp;
    /// </summary>
    Vector3 steer;
    /// <summary>
    /// The target position that this shrimp wants to head towards.
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
    /// The desired direction to move in general
    /// </summary>
    Vector3 desire;
    /// <summary>
    /// The cenger of the school of shrimp.
    /// </summary>
    Vector3 flockCenter;
    /// <summary>
    /// The direction needed to move in to target the center of the school;
    /// </summary>
    Vector3 directionToCongregate;
    /// <summary>
    /// A self contained list of all shrimp.  Distinct from the PropertiesAnimal dictionary because shrimp should only school with themselves, not other fish.
    /// </summary>
    public static List<CreatureBlindShrimp> shrimp = new List<CreatureBlindShrimp>();
    /// <summary>
    /// A list of all attractors exerting influence over these shrimp.
    /// </summary>
    public static List<CreatureBlindShrimpAttractor> attracts = new List<CreatureBlindShrimpAttractor>();

    /// <summary>
    /// Sets up the shrimp with a random starting velocity, a random speed, and adds them to the shrimps array.
    /// </summary>
    void Start()
    {
        veloc = new Vector3(Random.Range(-100, 100), 0, Random.Range(-100, 100));
        //transform.position = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));
        accel = new Vector3();

        speed *= Random.Range(.75f, 1.25f);

        shrimp.Add(this);
    }

    /// <summary>
    /// The update loop serves only to call other more specialized functions to determin actual movement.
    /// </summary>
    void Update()
    {
        ticker++;
        accel = Vector3.zero;

        handleBrain();
        handleMove();
    }
    /// <summary>
    /// This function handles the decision making of the shrimp.  This is where they determine what direction they will attempt to move in.
    /// </summary>
    void handleBrain()
    {
        if (shrimp.IndexOf(this) % 2 == 0 && ticker % 2 == 0 ||
            shrimp.IndexOf(this) % 2 != 0 && ticker % 2 != 0) return;

        directionToAlign = Vector3.zero;
        directionToCongregate = Vector3.zero;
        directionToSeperate = Vector3.zero;
        flockCenter = Vector3.zero;
        float runningTotal = 0;
        float shortestDist = 1000000;
        //0> loop
        foreach (CreatureBlindShrimp b in shrimp)
        {
            float dist = Vector3.Distance(transform.position, b.transform.position);
            if (dist == 0) continue;
            if (dist < closeDist)
            {
                if (dist < shortestDist) shortestDist = dist;
                //TODO:
                // get direction away from every near boid, inversely weighted by proximity
                target = transform.position - b.transform.position;
                directionToSeperate += target.normalized / (dist * dist);
                // get direction towards center of flock, weighted by distance
                flockCenter += b.transform.position;
                // align direction with neighbors, maybe weighted?
                directionToAlign += b.veloc;

                runningTotal++;
            }
        }
        if (runningTotal > 0)
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
            steer = (directionToAlign - veloc).normalized;

            AddForce(steer * turnForce * alignWeight * (10 / shortestDist));

            //seperate
            steer = (directionToSeperate - veloc).normalized;

            AddForce(steer * turnForce * seperateWeight * (10 / shortestDist));

            //congregate
            steer = (directionToCongregate - veloc).normalized;

            AddForce(steer * turnForce * cohesionWeight * (distToCenter / 50));
        }

        //handle attractors
        shortestDist = 100000000;
        desire = Vector3.zero;
        foreach (CreatureBlindShrimpAttractor b in attracts)
        {
            float dist = Vector3.Distance(transform.position, b.transform.position);
            if (dist < shortestDist) shortestDist = dist;
            //TODO:
            // get direction away from every near boid, inversely weighted by proximity
            target = b.transform.position - transform.position;
            desire += target.normalized * (dist);
        }
        if (attracts.Count > 0)
        {
            desire = desire.normalized * speed;
            steer = (desire - veloc).normalized;

            AddForce(steer * attractStrength * (shortestDist / 50));
        }

        //handle repulsors
        shortestDist = 100000000;
        desire = Vector3.zero;
        
        if (attracts.Count > 0)
        {
            //desire = desire.normalized * speed;
            //steer = (desire - veloc).normalized;

            //AddForce(steer * repulsStrength * (50 / shortestDist));
        }
    }
    /// <summary>
    /// This function is where the shrimp will actually impliment their movement, as determined by the heading determined in handleBrain().
    /// </summary>
    void handleMove()
    {
        veloc += accel * Time.deltaTime;
        if (veloc.sqrMagnitude > speed * speed) veloc = veloc.normalized * speed;
        transform.position = transform.position + veloc * Time.deltaTime;

        transform.LookAt(transform.position + veloc);
    }
    /// <summary>
    /// A simple function to apply force to propell the shrimp as they move.
    /// </summary>
    /// <param name="force"></param>
    void AddForce(Vector3 force)
    {
        accel += force;
    }
}

