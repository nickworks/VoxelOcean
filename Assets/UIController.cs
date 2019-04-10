using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class UIController : MonoBehaviour
{
    public GameObject titleText;
    public GameObject pressAnyKeyText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       if(Input.GetMouseButtonDown(0))
        {
            print("here");
            //RaycastHit hit;
            // if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 10000) )
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, 100))
            {

                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward), Color.red);
                Debug.Log("GOT A HIT");
            }//End of if Raycast condition
        }//End of Input condition
    }
}
