using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    float lookYaw;
    float lookPitch;

    bool flipVertical = true;
    bool flipHorizontal = false;

    Vector3 velocity = Vector3.zero;

    public float maxSpeed = 5; 
    public float acceleration = 10f; 

    private VoxelUniverse universeReference;
    private int renderDist;
    private int renderDistVert;
    private int resPerChunk;
    private Vector3 counterMax;
    private Vector3 counterMin;

    private Vector3 pos; 

    private void Start()
    {
        universeReference = GameObject.Find("VoxelUniverse").GetComponent<VoxelUniverse>();
        renderDist = universeReference.renderDistance;
        renderDistVert = universeReference.renderDistanceVertical;
        resPerChunk = universeReference.resPerChunk;

        counterMax = new Vector3(resPerChunk / 2.0f, resPerChunk / 2.0f, resPerChunk / 2.0f);
        counterMin = -counterMax;

        pos = GetComponent<Transform>().position; 
            

        //print(counterMin.x + " " + counterMin.y + " " + counterMin.z); 
    }

    // Update is called once per frame
    void Update()
    {
        Look();
        Move();

        
        pos = GetComponent<Transform>().position;

        if(pos.x > counterMax.x)
        {
            universeReference.CreateMoreChunks(1); 
            counterMax.x += resPerChunk;
            counterMin.x += resPerChunk; 
        }
        else if(pos.x < counterMin.x)
        {
            universeReference.CreateMoreChunks(2);
            counterMax.x -= resPerChunk;
            counterMin.x -= resPerChunk;
        }

        if (pos.z > counterMax.z)
        {
            universeReference.CreateMoreChunks(3);
            counterMax.z += resPerChunk;
            counterMin.z += resPerChunk;
        }
        else if (pos.z < counterMin.z)
        {
            universeReference.CreateMoreChunks(4);
            counterMax.z -= resPerChunk;
            counterMin.z -= resPerChunk;
        }

        if (pos.y > counterMax.y)
        {
            universeReference.CreateMoreChunks(5);
            counterMax.y += resPerChunk;
            counterMin.y += resPerChunk;
        }
        else if (pos.y < counterMin.y)
        {
            universeReference.CreateMoreChunks(6);
            counterMax.y -= resPerChunk;
            counterMin.y -= resPerChunk;
        }

        





    }

    private void Move()
    {
        float f = Input.GetAxis("MoveForward");
        float r = Input.GetAxis("MoveRight");
        float u = Input.GetAxis("MoveUp");

        Vector3 a = new Vector3(f, r, u) * Time.deltaTime * this.acceleration;

        velocity += a.x * transform.forward;
        velocity += a.y * transform.right;
        velocity += a.z * transform.up;

        Drag();

        transform.position += velocity * Time.deltaTime;
    }
    private void Slow(ref float speed)
    {
        if (speed > 0)
        {
            speed -= Time.deltaTime * acceleration;
            if (speed < 0) speed = 0;
        }
        if (speed < 0)
        {
            speed += Time.deltaTime * acceleration;
            if (speed > 0) speed = 0;
        }
    }
    private void Drag()
    {
        float densityOfFluid = 1;
        float friction = 1f;

        float k = (densityOfFluid * friction)/2;
        Vector3 force = -k * velocity.sqrMagnitude * velocity.normalized;
        
        float mass = 1;
        velocity += (force / mass) * Time.deltaTime;
    }
    private void Look()
    {
        float lookX = Input.GetAxis("Look X") * (flipHorizontal ? -1 : 1);
        float lookY = Input.GetAxis("Look Y") * (flipVertical ? -1 : 1);

        lookYaw += lookX * Time.deltaTime * 100;
        lookPitch += lookY * Time.deltaTime * 100;

        lookPitch = Mathf.Clamp(lookPitch, -80, 80);

        transform.eulerAngles = new Vector3(lookPitch, lookYaw, 0);
    }
}
