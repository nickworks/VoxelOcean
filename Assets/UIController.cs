using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class UIController : MonoBehaviour
{
    public GameObject titleText;
    public GameObject pressAnyKeyText;

    public Transform newTitleTextTransform;
    public Transform playTextTransform;
    public Transform creditsTextTransform;
    public Transform optionsTextTransform;
    public Transform quitTextTransform;

    public GameObject playText;
    public GameObject creditsText;
    public GameObject optionsText;
    public GameObject quitText;

    private bool anyKeyPressed;

    

    private bool transitionToSecondaryMenu = false;

    private float startTime = 0;
   
    public float transitionSpeed;
    private float length;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        length = Vector3.Distance(titleText.transform.position, newTitleTextTransform.position);
    }

    // Update is called once per frame
    void Update()
    {
       if(Input.GetMouseButtonDown(0))
        {
            
            
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit, 100))
            {
                if(hit.transform == pressAnyKeyText.transform)
                {
                    print("we are going to alter main menu now");
                    //We call the alter menu function
                    AlterMenu();
                }
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward), Color.red);
               
            }//End of if Raycast condition


            //USED TO MOVE PAUSE MENU UP
            if(transitionToSecondaryMenu)
            {
                float distance = (Time.time - startTime) * transitionSpeed;

               // float journey = distance / length;
                titleText.transform.position = Vector3.Lerp(titleText.transform.position, newTitleTextTransform.position, Time.deltaTime);
               
                    CreateSecondaryMenu();
                
            }//end of transitiontosecondmenu condition

        }//End of Input condition
    }


    void AlterMenu()
    {
        //Destroy(pressAnyKeyText);
        pressAnyKeyText.SetActive(false);
        transitionToSecondaryMenu = true;
    }

    void CreateSecondaryMenu()
    {
        playText.SetActive(true);
    }
}
