using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Modular Asset Coral Mesh
/// Created by Cameron Garchow to mimic Crystalline Flower Structures
/// Based on scientific and a mix of parasitic crystal structures
/// Mimicing in a Crystal Biome a naturalistic Flower\
/// With an emissive mapping and others this object 
/// Is meant to glow and spread the crystalline disease
/// The idea behind it is to create crystalline structures that 'spread' like a disease.
/// This can be changed to any value we want, we could take this into a game controller and randomize it
/// For this we can randomize its scale
/// </summary>
public class CoralCrystalFlowerSpawn : MonoBehaviour
{

    /// <summary>
    /// Iterations : how many times the object is repeated
    /// </summary>
    [Range(2, 6)] public int iterations = 4;
    /// <summary>
    /// Angle1 : what angle an object is pointed to rotated by
    /// </summary>
    [Range(0, 50)] public int angle1 = 45;
    /// <summary>
    /// Angle2 : What angle2 an object is pointed to rotated by (alternate)
    /// </summary>
    [Range(0, 50)] public int angle2 = 45;
    /// <summary>
    /// what angle3 an object is pointed to rotated by
    /// </summary>
    [Range(0, 50)] public int angle3 = 45;
    /// <summary>
    /// Scale of the object, that decreases over time based on per iteration
    /// </summary>
    [Range(0, 1)] public float scalar = 0.5f;
    /// <summary>
    /// is the object position of the spawner
    /// </summary>
    [Range(.25f, .8f)] public float objpos = 0.5f;
    /// <summary>
    /// is the scale of each branch object / transform scale
    /// </summary>
    public Vector3 branchScale = new Vector3(.25f, 2, .25f);
    ///<summary>
    ///hueMin minimum of the hue values of colors
    /// </summary>
    [Range(.1f, .7f)] public float hueMin = .5f;
    /// <summary>
    /// hueMax, maxium of the  of the hue values of colors
    /// </summary>
    [Range(.4f, 4f)] public float hueMax = 2.22f;
    /// <summary>
    /// Range controls the different types of Crystal Coral, with a random range set in the update function;
    /// </summary>
    [Range(1, 5)] public float range;
    /// <summary>
    /// RandomdorRandom controls the different types of Crystal Coral, with a random range set in the update function;
    /// </summary>
    [Range(1, 4)] public float children;

    /// <summary>
    /// Start / Build Function
    /// Creates the object and coral mesh
    /// Created in Firstframe
    /// </summary>
    void Start()
    {
        Build();
    }
    
    /// <summary>
    /// Update
    /// Called once a frame
    /// Once per a frame there is an update increasing and decreasing the Scalar or scale of the object.
    /// Does nothing currently
    /// TODO: Will add in functionality for glowing and parasitic abilities
    /// </summary>
    void Update()
    {
        
    }

    /// <summary>
    /// Build
    /// Builds the object from the meshes.
    /// Gets a reference from the square function and uses it to build an object.
    /// Combines meshes to reduce object count
    /// Meshfilters it from reference
    /// </summary>
    public void Build ()
    {
        List<CombineInstance> meshes = new List<CombineInstance>();

        Grow(meshes, iterations, Vector3.zero, Quaternion.identity, 1);
        GetComponent<Transform>().Rotate(0, Random.value * 360, 0);
        Mesh mesh = new Mesh();
        mesh.CombineMeshes(meshes.ToArray());

        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;

    }
    /// <summary>
    /// Grow
    /// Grows objects from a single branch that is then randomized aned give iterations that the player can control.
    /// Objects are controlled entirely in the grow function
    /// </summary>
    /// <param name="meshes"> mesh that it takes</param>
    /// <param name="num"> number of meshes</param>
    /// <param name="pos"> postion of the object</param>
    /// <param name="rot"> rotation of the obj</param>
    /// <param name="scale"> objects scale randomized</param>
    private void Grow(List<CombineInstance> meshes, int num, Vector3 pos, Quaternion rot, float scale)
    {
        if (num <= 0) return; // stop recursive function
        RandomizeValues();

        CombineInstance inst = new CombineInstance();
        inst.mesh = MakeCylinder(num);
        //inst.transform =
        inst.transform = Matrix4x4.TRS(pos, rot, branchScale * scale);

        meshes.Add(inst);

        num--;

        pos = inst.transform.MultiplyPoint(new Vector3(0, 1, 0));
        Vector3 sidePos = inst.transform.MultiplyPoint(new Vector3(objpos, objpos, objpos));
        Vector3 sidePos2 = inst.transform.MultiplyPoint(new Vector3(-objpos, objpos, -objpos));
        Quaternion rot1 = rot * Quaternion.Euler(angle1, 90 + angle2, angle3);
        Quaternion rot2 = rot * Quaternion.Euler(-angle1, 90 - angle2, -angle3);
        Quaternion rot3 = rot * Quaternion.Euler(angle1, 45 + angle3, angle3);
        Quaternion rot4 = rot * Quaternion.Euler(angle3, 45 - angle3, angle2);
        Quaternion rot5 = rot * Quaternion.Euler(-angle3, Random.Range(30, 40) + angle2, -angle2);
        scale *= scalar;

        if(range == 1) {
            Grow(meshes, num, pos, rot1, scale); //doing one for tendril like
            Grow(meshes, num, pos, rot2, scale); // doing for tendril 2
            Grow(meshes, num, pos, rot3, scale); // tendril 3
            if (children >= 2)
            {
                Grow(meshes, num, sidePos, rot4, scale); // tendril 4
                if (children == 3)
                {
                    Grow(meshes, num, sidePos, rot5, scale); // tendril 5
                }
            }
        }
        if (range == 2)
        {
            Grow(meshes, num / 2, sidePos, rot1, scale); //doing one for tendril like
            Grow(meshes, num, pos, rot2, scale); // doing for tendril 2
            Grow(meshes, num / 2, pos, rot3, scale); // tendril 3
            if (children >= 2)
            {
                Grow(meshes, num, sidePos2, rot4, scale); // tendril 4
                if (children == 3)
                {
                    Grow(meshes, num / 4, sidePos2, rot5, scale); // tendril 5
                }
            }
        }
        if (range == 3)
        {
            Grow(meshes, num, sidePos2, rot1, scale); //doing one for tendril like
            Grow(meshes, num, sidePos2, rot2, scale); // doing for tendril 2
            Grow(meshes, num, sidePos2, rot3, scale); // tendril 3
        
            if (children >= 2)
            {
                Grow(meshes, num, sidePos, rot4, scale); // tendril 4
                if (children == 3)
                {
                    Grow(meshes, num, sidePos, rot5, scale); // tendril 5
                }

            }
        }
    }
    /// <summary>
    /// Randomize Values
    /// Randomizes the objects within the grown objects.
    /// </summary>
    private void RandomizeValues()
    {
        angle1 = Random.Range(5, 30);
        angle2 = Random.Range(5, 40);
        angle3 = Random.Range(5, 40);
        scalar = Random.Range(.5f, 1);
        range = Random.Range(1, 4);
        children = Random.Range(1, 4);
        branchScale = new Vector3(Random.Range(.25f, .4f), Random.Range(2,4), Random.Range(.25f, .4f));
    }
    //Cube Data
    // Makes a cube
    /// <summary>
    /// Makes a cube with data taken from X,Y,Z coords
    /// </summary>
    /// <returns></returns>
    private Mesh MakeCylinder(int num)
    {
     
        List<Color> colors = new List<Color>();
       
       

        //Set hue min and Max

        float hue = Mathf.Lerp(hueMin, hueMax, (num / (float)iterations));
        Mesh mesh = MeshTools.MakePentagonalCylinder();
        Vector3[] verts = mesh.vertices;

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