using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// This script creates a mesh for a PlantKelp object using mesh construction functions.
/// Author: Kyle Lowery
/// Last Date Updated: 03/29/2019
/// </summary>
public class PlantKelpMesh : MonoBehaviour
{
    // Ints:
    /// <summary>
    /// Controls the number of iterations the Build function goes through to make the mesh.
    /// </summary>
    private int iterations = 3;
    /// <summary>
    /// Sets the minimum number of iterations this object can pass through before stopping.
    /// </summary>
    [Range(2, 20)] public int minIterations = 2;
    /// <summary>
    /// Sets the maximum number of iterations this object can pass through before stopping.
    /// </summary>
    [Range(2, 20)] public int maxIterations = 20;

    /// <summary>
    /// Stores the number of leaves attached to every segment of the mesh.
    /// </summary>
    private int numberOfLeaves = 5;
    /// <summary>
    /// Sets the minimum number of leaves that are attached to every segment.
    /// </summary>
    [Range(4, 10)] public int minLeaves = 4;
    /// <summary>
    /// Sets the maximum number of leaves that are attached to every segment.
    /// </summary>
    [Range(4, 10)] public int maxLeaves = 10;

    // Scaling Vectors:
    /// <summary>
    /// Stores the scale of the overall object.
    /// </summary>
    private float objectScaling = 2f;
    /// <summary>
    /// Sets the minimum amount for the object's scale.
    /// </summary>
    [Range(.5f, 5f)] public float minObjectScaling = .5f;
    /// <summary>
    /// Sets the maximum amount for the object's scale.
    /// </summary>
    [Range(.5f, 5f)] public float maxObjectScaling = 5f;

    /// <summary>
    /// Stores the scaling of each Stem segment in the mesh.
    /// </summary>
    public Vector3 stemScaling = new Vector3(.25f, 1, .25f);
    /// <summary>
    /// Stores the scaling of each Leaf segment in the mesh.
    /// </summary>
    public Vector3 leafScaling = new Vector3(.25f, 1, .35f);

    /// <summary>
    /// Sets the minimum value to randomly scale meshes.
    /// </summary>
    [Range(.5f, 2f)] public float minRandomScaling = .5f;
    /// <summary>
    /// Sets the maximum value to randomly scale meshes.
    /// </summary>
    [Range(.5f, 2f)] public float maxRandomScaling = 2f;

    // Angle of Instance:
    /// <summary>
    /// Controls the angle of the stem mesh in the X direction.
    /// </summary>
    [Range(0, 45f)] public float stemAngleX = 0;
    /// <summary>
    /// Controls the angle of the stem mesh in the Y direction.
    /// </summary>
    [Range(0, 45f)] public float stemAngleY = 45f;
    /// <summary>
    /// Controls the angle of the stem mesh in the Z direction.
    /// </summary>
    [Range(0, 45f)] public float stemAngleZ = 45f;

    // Offset Angle for leaves:
    /// <summary>
    /// Controls the angle of the leaf mesh in the X direction.
    /// </summary>
    public float leafOffsetAngleX = 90f;
    /// <summary>
    /// Controls the angle of the leaf mesh in the Y direction.
    /// </summary>
    public float leafOffsetAngleY = 90f;
    /// <summary>
    /// Controls the angle of the leaf mesh in the Z direction.
    /// </summary>
    public float leafOffsetAngleZ = 90f;
    /// <summary>
    /// Sets the minimum value of a random angle amount.
    /// </summary>
    [Range(0, 45f)] public float minRandomAngle = 0;
    /// <summary>
    /// Sets the maximum value of a random angle amount.
    /// </summary>
    [Range(0, 45f)] public float maxRandomAngle = 30f;

    //Hue modifiers:
    /// <summary>
    /// Sets the minimum value of the hue color.
    /// </summary>
    [Range(.1f, .9f)] public float hueMin = .1f;
    /// <summary>
    /// Sets the maximum value of the hue color.
    /// </summary>
    [Range(.1f, 1f)] public float hueMax = 1f;

    // Start is called before the first frame update
    void Start()
    {
        // Check to make sure Unity values don't break the code:
        if (minIterations > maxIterations || minIterations == maxIterations) minIterations = maxIterations - 1;
        if (minRandomAngle > maxRandomAngle || minRandomAngle == maxRandomAngle) minRandomAngle = maxRandomAngle - 1;
        if (minRandomScaling > maxRandomScaling || minRandomScaling == maxRandomScaling) minRandomScaling = maxRandomScaling - .1f;
        if (minLeaves > maxLeaves || minLeaves == maxLeaves) minLeaves = maxLeaves - 1;
        if (hueMin > hueMax || hueMin == hueMax) hueMin = hueMax - .1f;

        // Set randomized values (set values here for ones that SHOULDN'T be changed in the recursive function):
        iterations = Random.Range(minIterations, maxIterations);
        objectScaling = Random.Range(minObjectScaling, maxObjectScaling);

        // Start building the mesh:
        Build();
    }

    /// <summary>
    /// Builds the Mesh for PlantKelp out of coded-in meshes.
    /// </summary>
    public void Build()
    {
        List<CombineInstance> meshes = new List<CombineInstance>();
        Grow(iterations, meshes, Vector3.zero, Quaternion.identity);

        Mesh mesh = new Mesh();
        mesh.CombineMeshes(meshes.ToArray());

        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;
    }
    /// <summary>
    /// Pieces together a mesh based on a set position, rotation, and iterations.
    /// </summary>
    /// <param name="num">The number of iterations that have passed already.</param>
    /// <param name="meshes">Stores all the meshes this function constructs.</param>
    /// <param name="pos">The position of the endPoint of the previous stem segment/spawner location.</param>
    /// <param name="rot">The rotation of the previous stem segment/spawner location</param>
    private void Grow(int num, List<CombineInstance> meshes, Vector3 pos, Quaternion rot)
    {
        if (num <= 0) return;   //stop recursive function

        //Adjust randomness for scale values:
        Vector3 tempStemScaling = (stemScaling * Random.Range(minRandomScaling, maxRandomScaling)) * objectScaling;
        Vector3 tempLeafScaling = (leafScaling * Random.Range(minRandomScaling, maxRandomScaling)) * objectScaling;

        //Adjust randomness for leaves:
        numberOfLeaves = Random.Range(minLeaves, maxLeaves);

        // Add Stem Mesh:
        CombineInstance stem = new CombineInstance();
        stem.mesh = MeshTools.MakePentagonalCylinder();
        AddColorToVertices(stem.mesh, num);
        stem.transform = Matrix4x4.TRS(pos, rot, tempStemScaling);
        meshes.Add(stem);

        // Add Leaves Mesh:
        for (int i = 1; i <= numberOfLeaves; i++)
        {
            CombineInstance leaves = new CombineInstance();
            leaves.mesh = MeshTools.MakeCube();
            AddColorToVertices(leaves.mesh, num);
            float rotAmount = 360 / (numberOfLeaves * i);
            Quaternion leafRot = rot * Quaternion.Euler(leafOffsetAngleX, leafOffsetAngleY * rotAmount, leafOffsetAngleZ);
            leaves.transform = Matrix4x4.TRS(pos, leafRot, tempLeafScaling);
            meshes.Add(leaves);
        }

        // Count down number of passes:
        num--;

        // Get the position of the end of this section:
        pos = stem.transform.MultiplyPoint(new Vector3(0, 1, 0));

        //Generate random angles based on set angle values:
        float randX = stemAngleX + Random.Range(minRandomAngle, maxRandomAngle);
        float randY = stemAngleY + Random.Range(minRandomAngle, maxRandomAngle);
        float randZ = stemAngleZ + Random.Range(minRandomAngle, maxRandomAngle);

        //Create a Quaternion to change the local transform to the world Vector3.up:
        Quaternion rot1 = Quaternion.FromToRotation(transform.up, Vector3.up);
        //Create a Quaternion using the random angles from earlier:
        Quaternion rot2 = Quaternion.Euler(randX, randY, randZ);

        //Use a slerp to create a Quaternion between rot1 and rot2, leaning more towards randomness in later segments:
        Quaternion finalRot = Quaternion.Slerp(rot1, rot2, (num / (float)iterations));

        //Grow another segment:
        Grow(num, meshes, pos, finalRot);
    }
    /// <summary>
    /// Adds color to vertices based on the number of iterations that have passed making the object.
    /// </summary>
    /// <param name="mesh">The mesh you wish to add vertex colors to.</param>
    /// <param name="num">The current number of iterations that have passed.</param>
    private void AddColorToVertices(Mesh mesh, int num)
    {
        List<Color> colors = new List<Color>();
        float hue = Mathf.Lerp(hueMin, hueMax, (num / (float)iterations));

        foreach (Vector3 pos in mesh.vertices)
        {
            float tempHue = hue;
            Color tempColor = Color.HSVToRGB(tempHue, 1, 1);
            colors.Add(tempColor);
        }

        mesh.SetColors(colors);
    }
}
