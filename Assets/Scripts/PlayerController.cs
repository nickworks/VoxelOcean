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

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Look();
        Move();

    }

    private void Move()
    {
        float f = Input.GetAxis("Vertical");
        float r = Input.GetAxis("Horizontal");

        float u = Input.GetAxis("Jump");

        Vector3 move = Vector3.zero;
        move += f * transform.forward;
        move += r * transform.right;
        move += u * Vector3.up;


        if (move.sqrMagnitude < .5f)
        {
            velocity -= velocity.normalized * Time.deltaTime * 10;
        }
        else
        {
            velocity += move * Time.deltaTime * 5;
        }
        if (velocity.sqrMagnitude > maxSpeed * maxSpeed) velocity = velocity.normalized * maxSpeed;
        transform.position += velocity * Time.deltaTime;
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
