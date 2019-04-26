using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Modular Asset Coral Mesh
/// Created by Cameron Garchow to mimic Crystalline Structures
/// Based on scientific and a mix of parasitic crystal structures
/// The idea behind it is to create crystalline structures that 'spread' like a disease.
/// This can be changed to any value we want, we could take this into a game controller and randomize it
/// For this we can randomize its 
/// </summary>
public class CoralCrystalSpawner : MonoBehaviour
{
    /// <summary>
    /// Iterations : how many times the object is repeated
    /// </summary>
    [Range(2, 6)] public int iterations = 4;// Iterations of the object
    /// <summary>
    /// Angle1 : what angle an object is pointed to rotated by
    /// </summary>
    [Range(0, 45)] public int angle1 = 25; // Tendril 1's Angle of origin
    /// <summary>
    /// Angle2 : What angle2 an object is pointed to rotated by (alternate)
    /// </summary>
    [Range(0, 45)] public int angle2 = 25; // Tendril 2's angle of origin
    /// <summary>
    /// Angle3 : what angle3 an object is pointed to rotated by
    /// </summary>
    [Range(0, 45)] public int angle3 = 25; //tendril 3's angel of origin
    /// <summary>
    /// Scalar : Scale of the object, that decreases over time based on per iteration
    /// </summary>
    [Range(.1f, .85f)] public float scalar = 0.5f; //scale
    /// <summary>
    /// OBJPOS : is the object position of the spawner
    /// </summary>
    [Range(.25f, .8f)] public float objpos = 0.5f;
    /// <summary>
    /// branchScale : is the scale of each branch object / transform scale
    /// </summary>
    public Vector3 branchScale = new Vector3(.25f, 2, .25f);
    ///<summary>
    ///hueMin minimum of the hue values of colors
    /// </summary>
    [Range(.3f, .4f)] public float hueMin = .4f;
    /// <summary>
    /// hueMax, maxium of the  of the hue values of colors
    /// </summary>
    [Range(.4f, 7f)] public float hueMax = .6f;
    /// <summary>
    /// Range controls the different types of Crystal Coral, with a random range set in the update function;
    /// </summary>
    [Range(1, 5)] public float range;
    /// <summary>
    /// RandomdorRandom controls the different types of Crystal Coral, with a random range set in the update function;
    /// </summary>
    [Range(1, 4)] public float children;
    /// <summary>
    /// TrueRandomize, turns on or off the randomization function for testing / then enables it upon generation.
    /// </summary>
    public bool TrueRandomize = true;

    /// <summary>
    /// Start / Build Function
    /// Creates the object and coral mesh
    /// </summary>
    void Start()
    {
        Build();
    }

    // Update is called once per frame
    /// <summary>
    /// Update
    /// Called once a frame
    /// Once per a frame there is an update increasing and decreasing the Scalar or scale of the object.
    /// </summary>
    void Update()
    {
        if (scalar <= .25f)
        {
            scalar += .1f;
        }
        else if (scalar >= .85f) {
            scalar -= .1f;
        }
    }

    /// <summary>
    /// Build
    /// Builds the object from the meshes.
    /// Gets a reference from the square function and uses it to build an object.
    /// Combines meshes to reduce object count
    /// Meshfilters it from reference
    /// </summary>
    public void Build()
    {
        List<CombineInstance> meshes = new List<CombineInstance>();

        Grow(meshes, iterations, Vector3.zero, Quaternion.identity, 1);
        GetComponent<Transform>().Rotate(0, Random.value * 360, 0); //randomize Y rotation


        Mesh mesh = new Mesh();
        mesh.CombineMeshes(meshes.ToArray());

        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;

    }
    /// <summary>
    /// Randomize Range
    /// Randomize the range of the objects inside of the grow function.
    /// 
    /// </summary>
    private void RandomizeRanges()
    {
        if (TrueRandomize == true)
        {
            angle1 = Random.Range(20, 45);
            angle2 = Random.Range(20, 45);
            angle3 = Random.Range(20, 45);
            scalar = Random.Range(.5f, .8f);
            objpos = Random.Range(.4f, .6f);
            range = Random.Range(1, 6);
            children = Random.Range(1, 4);
        }

        branchScale = new Vector3(Random.Range(.25f, .35f), (float)2, Random.Range(.25f, .35f));
    }
    /// <summary>
    /// Grow
    /// Grows objects from a single branch that is then randomized aned give iterations that the player can control.
    /// Objects are controlled entirely in the grow function
    /// Objects are then given randomized side postionings rotational postions and different children amounts
    /// </summary>
    /// <param name="meshes"> mesh that it takes</param>
    /// <param name="num"> number of meshes</param>
    /// <param name="pos"> postion of the object</param>
    /// <param name="rot"> rotation of the obj</param>
    /// <param name="scale"> objects scale randomized</param>
    private void Grow(List<CombineInstance> meshes, int num, Vector3 pos, Quaternion rot, float scale)
    { 

        if (num <= 0) return; // stop recursive function
            RandomizeRanges();
            CombineInstance inst = new CombineInstance();
        inst.mesh = MakeCube(num);
        inst.transform = Matrix4x4.TRS(pos, rot, branchScale * scale);

        meshes.Add(inst);

        num--;

        pos = inst.transform.MultiplyPoint(new Vector3(Random.Range(0, 1), 1, Random.Range(0, 1)));
        Vector3 sidePos = inst.transform.MultiplyPoint(new Vector3(objpos, objpos, objpos));
        Vector3 sidePos2 = inst.transform.MultiplyPoint(new Vector3(-objpos, objpos, -objpos));
        Vector3 sidePos3 = inst.transform.MultiplyPoint(new Vector3(Random.Range(.4f, 1), objpos, Random.Range(.4f, 1)));
        Vector3 sidePos4 = inst.transform.MultiplyPoint(new Vector3(Random.Range(.5f, 1), Random.Range(.5f,1), Random.Range(.5f, 1)));

        scale *= scalar;



        Quaternion rot1 = rot * Quaternion.Euler(angle3, angle1, angle2);
        Quaternion rot2 = rot * Quaternion.Euler(0, 0, 0);
        Quaternion rot3 = rot * Quaternion.Euler(-angle3, 0, -angle2); //to avoid stretching
        Quaternion rot4 = rot * Quaternion.Euler(Random.Range(30, 45), 0, Random.Range(30, 45)); //to avoid stretching
        Quaternion rot5 = rot * Quaternion.Euler(Random.Range(10, 45), 0, Random.Range(10, 45)); //to avoid stretching
        Quaternion rot6 = rot * Quaternion.Euler(-Random.Range(10, 45), 0, -Random.Range(10, 45)); //to avoid stretching

        if (range == 1) {

            Grow(meshes, num, pos, rot1, scale);
            Grow(meshes, num, pos, rot2, scale);
            Grow(meshes, num, pos, rot3, scale);
            Grow(meshes, num, pos, rot4, scale);
            if (children == 2)
            {
                Grow(meshes, num, pos, rot6, scale);
            }
        }

        if (range == 2)
        {
            Grow(meshes, num, pos, rot1, scale); //doing one for tendril like
            Grow(meshes, num, pos, rot2, scale); // doing for tendril 2
            Grow(meshes, num, sidePos, rot3, scale); // doing for tendril 2
            Grow(meshes, num, sidePos, rot4, scale); // doing for tendril 2
            if (children == 2)
            {
                Grow(meshes, num, sidePos, rot6, scale);
            }
            if (children == 1) {
            }
        }

        if (range == 3)
        {
            num--;
            Grow(meshes, num / 2, sidePos2, rot1, scale / Random.Range(1, 3)); // tendril 3
            Grow(meshes, num / 2, sidePos2, rot2, scale / 2); // tendril 4
            Grow(meshes, num / 2, sidePos3, rot3, scale / 2); // tendril 4
            Grow(meshes, num / 2, sidePos3, rot4, scale / 2); // tendril 4

            if (children == 2)
            {
                Grow(meshes, num / 2, sidePos3, rot6, scale);
            }
            if (children == 1)
            {
                Grow(meshes, num / 10, sidePos3, rot5, scale); // tendril 4
            }
        }


        if (range == 4 || range == 5)        {
            num--;
            Grow(meshes, num, sidePos3, rot1, scale / Random.Range(1, 3)); // tendril1
            Grow(meshes, num, sidePos3, rot2, scale); // tendril 2
            Grow(meshes, num, sidePos4, rot3, scale); // tendril 3
            Grow(meshes, num, sidePos4, rot4, scale / 2); // tendril 4
            if (children == 2)
            {
                Grow(meshes, num, sidePos4, rot6, scale); //tendril 5 potential
            }
            else
            {
                Grow(meshes, num / 10, sidePos4, rot5, scale); // tendril 5
            }
        }
    }


    //Cube Data
    // Makes a cube
    /// <summary>
    /// Cube 
    /// Makes a cube object
    /// </summary>
    /// <param name="num"> Number of iterations </param>
    /// <returns> A Mesh / Cube object</returns>
    private Mesh MakeCube(int num)
    {
        
        List<Color> colors = new List<Color>();
        Mesh mesh = MeshTools.MakeCube();
        Vector3[] verts = mesh.vertices;
        //Set hue min and Max

        float hue = Mathf.Lerp(hueMin, hueMax, (num / (float) iterations));
        /*
         * for each vertices located in the array color them in a hue 
        */
        for (int i = 0; i < mesh.vertexCount; i++)
        {
            float tempHue = hue + (1 / (float)iterations) * verts[i].y;

            Color color = Color.HSVToRGB(tempHue, 1, 1);

            colors.Add(color);
        }

        mesh.SetColors(colors);
        return mesh;
    }


}
/// <summary>
/// Editor for the CoralCrystal Spawner, allows building in editor for testing
/// </summary>
[CustomEditor(typeof(CoralCrystalSpawner))]
public class CoralCrystalSpawnerEditor : Editor
{

    override public void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("GROW!"))
        {
            CoralCrystalSpawner c = (target as CoralCrystalSpawner);
            c.Build();
        }

    }
}
