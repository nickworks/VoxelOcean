using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// this class makes the spheres/ fish move and roatate and use a flocking pattering to get them to stay next to each other
/// if oustside of desgnaited range they will turn around
/// </summary>
public class GobbieFlock : MonoBehaviour
{
    public float velocity; // how fast objects move
    public float maxVelocity; // the max speed they can go
    public float velocityRotation;// how fast they turn
    public float maxVelocityRotation;// max speed they can turn

    float neighbourDistance = .10f;// max distance an object can affect another object
    bool turning = false;// used if object need to turn around
                         // Start is called before the first frame update
                         /// <summary>
                         /// calls the class to set a random speend for every object
                         /// </summary>
    void Start()
    {
        RandomSpeed();
    }
    /// <summary>
    /// use all the classes to make the object move
    /// </summary>
    // Update is called once per frame
    void Update()
    {

        //Flocking();
        Swimming();
        Turning();
    }
    /// <summary>
    /// makes the object move
    /// </summary>
    public void Swimming()
    {

        transform.Translate(0, 0, velocity);

    }/// <summary>
     /// sets the random speeds of the objects
     /// </summary>
    public void RandomSpeed()
    {
        velocity = Random.Range(0.1f, 2.5f) * Time.deltaTime;
        velocityRotation = Random.Range(1.1f, 1.5f) * Time.deltaTime;
    }
    /// <summary>
    /// sets the objects to follow each other bying taking all objects withen range and putting in group and yaking  the avearge of the group
    /// when objects become to close they avoids each other
    /// </summary>
    public void Flocking()
    {
        GameObject[] fishGobbies;
        fishGobbies = FishGobies.fish;
        Vector3 vCenter = Vector3.zero;
        Vector3 vAvoid = Vector3.zero;



        float distance;
        int groupSize = 0;
        foreach (GameObject go in fishGobbies)
        {
            if (go != this.gameObject)
            {
                distance = Vector3.Distance(go.transform.position, this.transform.position);
                if (distance <= neighbourDistance)
                {
                    vCenter += go.transform.position;
                    groupSize++;
                    if (distance < 0.1f)
                    {
                        vAvoid = vAvoid + (this.transform.position - go.transform.position);

                    }
                    GobbieFlock anotherFlock = go.GetComponent<GobbieFlock>();

                }

            }
        }
        if (groupSize > 0)
        {


            Vector3 direction = (vCenter + vAvoid) - transform.position;
            if (direction != Vector3.zero)
            {


                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), velocityRotation);
            }
        }

    }
    /// <summary>
    /// makes the objects turn around when they get outside the set size for the object
    /// checks for a random number and if it is 100 object will turn
    /// </summary>
    public void Turning()
    {
        int turn = Random.Range(1, 100);
        if (Vector3.Distance(transform.position, Vector3.zero) >= FishGobies.biomeSize || turn == 100)
        {
            turning = true;
        }
        else
        {
            turning = false;

        }
        if (turning == true)
        {

            Vector3 direction = Vector3.zero - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), velocityRotation);

        }

    }
}