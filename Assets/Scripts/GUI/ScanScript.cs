using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScanScript : MonoBehaviour
{
    public Image img;
    public Text text;
    Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        img.enabled = false;
        text.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {

            Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                img.enabled = true;
                text.enabled = true;
                print("I'm looking at " + hit.transform.name);

                GameObject target = hit.transform.gameObject;

                string output;

                //everything properties
                PropertiesBase properties = target.GetComponent<PropertiesBase>();

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


                // hit.transform.GetComponent<Data>().ReturnData(out name, out maker, out info);
                //
                // text.text = $"Name: {name}\n" +
                //     $"Maker: {maker}\n" +
                //     $"Info: {info}";
                text.text = output;
            }
            else
            {
                img.enabled = false;
                text.enabled = false;
                print("I'm looking at nothing!");
            }
        }
    }
}
