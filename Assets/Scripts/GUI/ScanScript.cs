using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// this script handles scanning when the player presses the scan button
/// </summary>
public class ScanScript : MonoBehaviour
{
    //references to the background and text for the scan gui
    public Image scanBackgroundImage;
    public Text scanText;

    //reference to the main camera for raycasting
    Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        ///turn off the gui elements at start
        scanBackgroundImage.enabled = false;
        scanText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if player presses e (scan button)
        if (Input.GetKeyDown(KeyCode.E))
        {     
            //if the scan box is already up, we turn it off instead
            if(scanBackgroundImage.enabled == false)
            {

            //fire raycast from center of the camera/screen
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
            RaycastHit hit;

            //if the raycast hit something (with a mesh renderer)
            if (Physics.Raycast(ray, out hit))
            {
                //reference to the target
                GameObject target = hit.transform.gameObject;

                //everything properties - all objects will have this
                PropertiesBase properties = target.GetComponent<PropertiesBase>();

                if (properties == null)
                {
                    scanBackgroundImage.enabled = false;
                    scanText.enabled = false;
                    return;
                }

                //turn on the gui
                scanBackgroundImage.enabled = true;
                scanText.enabled = true;

                //master string all of the information will populate
                string output = "";

                //the string is populated with different information based on what properties it has
                string creator = properties.creator;
                string species = properties.species;
                Mobility mobility = properties.mobility;
                Defense defense = properties.defense;
                Size size = properties.size;
                List<Special> special = properties.special;

                output = $"Creator: {creator}\n" +
                            $"Species: {species}\n" +
                            $"Mobility: {mobility}\n" +
                            $"Defence: {defense}\n" +
                            $"Size: {size}\n";

                foreach (Special s in special)
                {
                    output += s.ToString() + "\n";
                }

                PropertiesNonAnimal nonAnimalProperties = target.GetComponent<PropertiesNonAnimal>();
                PropertiesAnimal animalProperties = target.GetComponent<PropertiesAnimal>();
                if (nonAnimalProperties != null)
                {
                    //nonanimal properties            
                    Tangibility tangibility = nonAnimalProperties.tangibility;
                    output += $"Tangibility: {tangibility.ToString()} \n";
                }
                else if (animalProperties != null)
                {
                    //animal properties
                    Diet diet = animalProperties.diet;
                    Activity activity = animalProperties.activity;
                    Strength strength = animalProperties.strength;
                    Speed speed = animalProperties.speed;

                    output += $"Diet: {diet}\n" +
                              $"Activity: {activity}\n" +
                              $"Strength: {strength}\n" +
                              $"Speed: {speed}\n";
                }

                //assign the string to the text output
                scanText.text = output;
                }
                else
                {
                    scanBackgroundImage.enabled = false;
                    scanText.enabled = false;
                }
            }
            else
            {
                //didn't hit anything or want to stop scanning - turn the gui off                
                scanBackgroundImage.enabled = false;
                scanText.enabled = false;
               // print("I'm looking at nothing!");
            }
        }
        //if player presses escape, also turn the gui off
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            scanBackgroundImage.enabled = false;
            scanText.enabled = false;
        }
    }
}
