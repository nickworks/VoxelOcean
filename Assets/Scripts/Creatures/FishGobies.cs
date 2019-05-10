using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// instantes all of the fish
/// </summary>
public class FishGobies : MonoBehaviour
{
    public GameObject FishGobbiesPrefab;// crates the gameobject
    static int numGobbies = 25;// sets the number of objects created
    public static int biomeSize = 5; // the range of how far they can go
    public int spawnRadius = 5;
    // public static Vector3 goalPos = Vector3.zero; // makes postion of object availlble
    public static GameObject[] fish = new GameObject[numGobbies];//the array of objects so they can see each other
    // Start is called before the first frame update
    /// <summary>
    /// spawns objects and sets the target location
    /// </summary>
    void Start()
    {

        SpawnGobbies();
       
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    /// <summary>
    /// instantiate the objects so they will spawn in a random location based of the spawn radious in the x,y,z planes
    /// </summary>
    public void SpawnGobbies()
    {


        for (int i = 0; i < numGobbies; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-spawnRadius, spawnRadius), Random.Range(-spawnRadius, spawnRadius), Random.Range(-spawnRadius, spawnRadius));
            fish[i] = (GameObject)Instantiate(FishGobbiesPrefab);
        }



    }
}