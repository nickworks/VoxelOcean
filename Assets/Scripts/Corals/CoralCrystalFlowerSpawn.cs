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
    [Range(2, 10)] public int iterations = 3;
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
        inst.mesh = MakeCube(num);
        //inst.transform =
        inst.transform = Matrix4x4.TRS(pos, rot, branchScale * scale);

        meshes.Add(inst);

        num--;

        pos = inst.transform.MultiplyPoint(new Vector3(0, 1, 0));
        Vector3 sidePos = inst.transform.MultiplyPoint(new Vector3(objpos, objpos, objpos));
        Quaternion rot1 = rot * Quaternion.Euler(angle1, angle2, angle3);
        Quaternion rot2 = rot * Quaternion.Euler(-angle1, -angle2, -angle3);
        scale *= scalar;

        Grow(meshes, num, pos, rot1, scale); //doing one for tendril like
        Grow(meshes, num, sidePos, rot2, scale); // doing for tendril 2
        Grow(meshes, num, pos, rot2, scale); // tendril 3
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
    }

    //Cube Data
    // Makes a cube
    /// <summary>
    /// Makes a cube with data taken from X,Y,Z coords
    /// </summary>
    /// <returns></returns>
    private Mesh MakeCube(int num)
    {
     
        List<Color> colors = new List<Color>();
       
       

        //Set hue min and Max
        float hueMin = .6f;
        float hueMax = Random.Range(.7f, 1);

        float hue = Mathf.Lerp(hueMin, hueMax, (num / (float)iterations));
        Mesh mesh = MeshTools.MakeCube();
        for (int i = 0; i < mesh.vertexCount; i++)
        {
            float tempHue = hue;

            Color color = Color.HSVToRGB(tempHue, 1, 1);

            colors.Add(color);
        }


    
        mesh.SetColors(colors);

        return mesh;
    }


}
/// <summary>
/// Editor
/// Editor for the CoralCrystalFlowerSpawn, allows building in editor for testing
/// </summary>
[CustomEditor(typeof(CoralCrystalFlowerSpawn))]
public class CoralCrystalFlowerSpawnEditor : Editor
{

    override public void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("GROW!"))
        {
            CoralCrystalFlowerSpawn c = (target as CoralCrystalFlowerSpawn);
            c.Build();
        }

    }
}
