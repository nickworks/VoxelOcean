using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Creature Crystal Fish
/// This Fish can also be a spinner
/// Has Acceleration / Speed for each. Is attached to a single object on spawn and its target is a coral
/// Has a Detection Distance that detects if the object is nearby. Could also be the player.
/// This was created from several different and references to ACtionScript code from 255
/// The Object seeks the position of the target position and minuses it from its own position
/// From there we set the rotation to the current POS of the object
/// From there we get the rotation and set it to 'spin' around an object
/// Created by Cameron Garchow 
/// Refs to Nick's 255 Code
/// </summary>
public class CreatureCrystalFish : MonoBehaviour
{
    /// <summary>
    /// Speed in FLOAT that is capped at 5 meters per a second
    /// </summary>
    float acceleration = 5f;
    /// <summary>
    /// Is the target at which it seeks an object like the player or other game objects
    /// </summary>
    [SerializeField]Transform target;
    /// <summary>
    /// Is the distance of detection for raycasting
    /// </summary>
    public float detectionDist = 20f;
    /// <summary>
    /// RotationalDamp is the rotation YAW 
    /// </summary>
    public float rotationalDamp = .5f;
    /// <summary>
    /// Offset for the raycasting 
    /// </summary>
    public float rayCastOffset = 2.5f;

    /// <summary>
    /// Update
    /// Calls SEek, Turn and Move in order to function
    /// </summary>
    void Update()
    {
        seek();
        Turn();
        Move();
    }
    /// <summary>
    /// Turn
    /// Turn toward the target position of the object
    /// </summary>
    void Turn()
    {
        Vector3 pos = target.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(pos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationalDamp * Time.deltaTime);
    }
    /// <summary>
    /// Move
    /// Moves forward based on acceleration and delta time
    /// </summary>
    void Move()
    {
        transform.position += transform.forward * acceleration * Time.deltaTime;

    }
    /// <summary>
    /// Seeks the target
    /// LEFT, RIGHT, UP, DOWN
    /// </summary>
    /// RaycastHit Hit draws a line toward a target
    /// LEFT is the directional LEFT of the object if it collides with something (must have a Collider)
    /// RIGHT is the directional RIGHT of the object if it collides with something (must have a Collider)
    /// UP is the directional UP of the object if it collides with something (must have a Collider)
    /// DOWN is the directional DOWN of the object if it collides with something (must have a Collider)
    /// Detects if it has hit something and moves towards the object if collided on left, right, up, and down.
    /// If any is true object turns
    void seek()
    {
        RaycastHit hit;
        Vector3 offsetRay = Vector3.zero;

        Vector3 left = transform.position - transform.right * rayCastOffset;
        Vector3 right = transform.position + transform.right * rayCastOffset;
        Vector3 up = transform.position + transform.up * rayCastOffset;
        Vector3 down = transform.position - transform.up * rayCastOffset;

        if (Physics.Raycast(left, transform.forward, out hit, detectionDist))
        {
            offsetRay += Vector3.right;
        }

        else if (Physics.Raycast(right, transform.forward, out hit, detectionDist))
        {
            offsetRay -= Vector3.right;
        }
        if (Physics.Raycast(up, transform.forward, out hit, detectionDist))
        {
            offsetRay -= Vector3.up;
        }

        else if (Physics.Raycast(down, transform.forward, out hit, detectionDist))
        {
            offsetRay += Vector3.up;
        }

        if (offsetRay != Vector3.zero)
        {
            transform.Rotate(offsetRay * 5f * Time.deltaTime);
        }
        else
        {
            Turn();
        }
    }
}
