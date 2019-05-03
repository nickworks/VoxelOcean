using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaDragonMovement : MonoBehaviour
{
    [Tooltip("Maximum movement speed")]
    public float maxSpeed = 5;
    public float currentSpeed;
    [Tooltip("Speed of Rotation")]
    public float rotationspeed = 1;
    [Tooltip("Maximum horizontal distance creature can go from it's home")]
    public int horizontalRange = 30;
    [Tooltip("Maximum vertical distance creature can go from it's home")]
    public int verticalRange = 10;

    Vector3 velocity = Vector3.zero;
    //where the creature spawns, it will always stay near this area
    Vector3 home = new Vector3();
    //where the creature is currently aiming for
    Vector3 targetLocation = new Vector3();

    // Start is called before the first frame update
    void Start()
    {
        home = this.transform.position;
        PickTargetLocation();
    }

    // Update is called once per frame
    void Update()
    {
        Steering();
    }
    /// <summary>
    /// handles movement for the creature
    /// </summary>
    void Steering()
    {
        Vector3 acceleration = new Vector3();

        //target direction / distance
        Vector3 targetDirection = targetLocation - transform.position;
        //if creature found it's target, pick new target
        if(CheckRange(targetDirection, 1))
        {
            PickTargetLocation();
        }
        //slow down as it approaches it's target
        if (CheckRange(targetDirection, 5))
        {
            currentSpeed *= .9f;
        }

        //get direction vector
        targetDirection.Normalize();
        targetDirection *= maxSpeed;

        //lerp rotation from current to target
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetDirection), Time.deltaTime * rotationspeed);

        targetDirection = targetDirection - velocity;
        targetDirection = Vector3.ClampMagnitude(targetDirection, maxSpeed);

        acceleration += targetDirection;

        velocity += acceleration;
        //add in some forward movement just to make sure it doesn't spin in place, also makes it feel more realistic
        velocity += transform.forward;

        //clamp movement to speed
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        //apply movement
        transform.position += velocity;
    }
    /// <summary>
    /// checks if the creature is within a given range of the target vector3
    /// </summary>
    /// <param name="targetVector">the location we are comparing our position to</param>
    /// <param name="distance">how far away we are checking</param>
    /// <returns>returns true if the vector's x, y or z parameter is within the given range</returns>
    bool CheckRange(Vector3 targetVector, float distance)
    {
        if (targetVector.x > -distance && targetVector.x < distance &&
        targetVector.y > -2 && targetVector.y < 2 &&
        targetVector.z > -2 && targetVector.z < 2)
        {
            return true;
        }
        return false;
    }

    void PickTargetLocation()
    {
        int randX = Random.Range(-horizontalRange, horizontalRange);
        int randZ = Random.Range(-horizontalRange, horizontalRange);
        int randY = Random.Range(0, verticalRange);


        targetLocation.Set(home.x + randX, home.y + randY, home.z + randZ);
        currentSpeed = maxSpeed;
    }
}
