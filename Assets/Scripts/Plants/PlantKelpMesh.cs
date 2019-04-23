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
    [Range(2, 20)] public int iterations = 3;
    /// <summary>
    /// Stores the number of leaves attached to every segment of the mesh.
    /// </summary>
    [Range(2, 10)] public int numberOfLeaves = 5;

    // Scaling Vectors:
    /// <summary>
    /// Stores the scaling of each Stem segment in the mesh.
    /// </summary>
    public Vector3 stemScaling = new Vector3(.25f, 1, .25f);
    /// <summary>
    /// Stores the scaling of each Leaf segment in the mesh.
    /// </summary>
    public Vector3 leafScaling = new Vector3(.25f, 1, .35f);

    // Angle of Instance:
    /// <summary>
    /// Controls the angle of the stem mesh in the X direction.
    /// </summary>
    [Range(0, 45f)] public float angleX = 0;
    /// <summary>
    /// Controls the angle of the stem mesh in the Y direction.
    /// </summary>
    [Range(0, 45f)] public float angleY = 45f;
    /// <summary>
    /// Controls the angle of the stem mesh in the Z direction.
    /// </summary>
    [Range(0, 45f)] public float angleZ = 45f;

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

    // Modifiers for a randomized range of value:
    /// <summary>
    /// Sets the minimum value of a random number.
    /// </summary>
    [Range(0, 45f)] public float minRandom = 0;
    /// <summary>
    /// Sets the maximum value of a random number.
    /// </summary>
    [Range(0, 45f)] public float maxRandom = 30f;

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
        if (minRandom > maxRandom || minRandom == maxRandom) minRandom = maxRandom - 1;
        if (hueMin > hueMax || hueMin == hueMax) hueMin = hueMax - .1f;

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

        // Add Stem Mesh:
        CombineInstance stem = new CombineInstance();
        stem.mesh = MeshTools.MakePentagonalCylinder();
        AddColorToVertices(stem.mesh, num);
        stem.transform = Matrix4x4.TRS(pos, rot, stemScaling);
        meshes.Add(stem);

        // Add Leaves Mesh:
        for (int i = 1; i <= numberOfLeaves; i++)
        {
            CombineInstance leaves = new CombineInstance();
            leaves.mesh = MeshTools.MakeCube();
            AddColorToVertices(leaves.mesh, num);
            float rotAmount = 360 / (numberOfLeaves * i);
            Quaternion leafRot = rot * Quaternion.Euler(leafOffsetAngleX, leafOffsetAngleY * rotAmount, leafOffsetAngleZ);
            leaves.transform = Matrix4x4.TRS(pos, leafRot, leafScaling);
            meshes.Add(leaves);
        }

        // Count down number of passes:
        num--;

        // Get the position of the end of this section:
        pos = stem.transform.MultiplyPoint(new Vector3(0, 1, 0));

        //Generate random angles based on set angle values:
        float randX = angleX + Random.Range(minRandom, maxRandom);
        float randY = angleY + Random.Range(minRandom, maxRandom);
        float randZ = angleZ + Random.Range(minRandom, maxRandom);

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
