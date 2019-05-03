using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureSeaUrchin : MonoBehaviour
{
    /// <summary>
    /// The number of iterations spawned from the original
    /// </summary>
    public int iterations;
    public int iteratoinsFan;
    public bool sideFan = false;
    /// <summary>
    /// How the scaling of the branches are for both the skeleton set and fan set
    /// </summary>
    public Vector3 branchScaling = new Vector3(.5f, 2f, .5f);
    public Vector3 baseScaling = new Vector3(.5f, 1f, .5f);
    void Start()
    {
        //initalizes the spawn of the coral
        Build();
    }
    /// <summary>
    /// Combines the meshes generated into one to get rid of the number
    /// of meshes in the coral reef
    /// </summary>
    public void Build()
    {
        iterations = Random.Range(4, 6);
        iteratoinsFan = iterations + 1;
        List<CombineInstance> meshes = new List<CombineInstance>();
        //What makes the spindles in the sea urchin
        Grow(iterations, meshes, Vector3.zero, Quaternion.Euler(Random.Range(-5, 5), 0, Random.Range(-25, 25)), 1);
        Grow(iterations, meshes, Vector3.zero, Quaternion.Euler(Random.Range(-5, 5), 0, Random.Range(-40, 40)), 1);
        Grow(iterations, meshes, Vector3.zero, Quaternion.Euler(Random.Range(-5,5), 0, Random.Range(-75, 75)), 1);
        Grow(iterations, meshes, Vector3.zero, Quaternion.Euler(Random.Range(-5, 5), 0, Random.Range(-90, 90)), 1);
        Grow(iterations, meshes, Vector3.zero, Quaternion.Euler(Random.Range(-30, 35), 0, Random.Range(-25, 25)), 1);
        Grow(iterations, meshes, Vector3.zero, Quaternion.Euler(Random.Range(-30, 35), 0, Random.Range(-40, 40)), 1);
        Grow(iterations, meshes, Vector3.zero, Quaternion.Euler(Random.Range(-30, 35), 0, Random.Range(-75, 75)), 1);
        Grow(iterations, meshes, Vector3.zero, Quaternion.Euler(Random.Range(-30, 35), 0, Random.Range(-90, 90)), 1);
        Grow(iterations, meshes, Vector3.zero, Quaternion.Euler(Random.Range(-75, 75), 0, Random.Range(-25, 25)), 1);
        Grow(iterations, meshes, Vector3.zero, Quaternion.Euler(Random.Range(-75, 75), 0, Random.Range(-40, 40)), 1);
        Grow(iterations, meshes, Vector3.zero, Quaternion.Euler(Random.Range(-75, 75), 0, Random.Range(-75, 75)), 1);
        Grow(iterations, meshes, Vector3.zero, Quaternion.Euler(Random.Range(-75, 75), 0, Random.Range(-90, 90)), 1);
        Grow(iterations, meshes, Vector3.zero, Quaternion.Euler(Random.Range(-90,90), 0, Random.Range(-25, 25)), 1);
        Grow(iterations, meshes, Vector3.zero, Quaternion.Euler(Random.Range(-90, 90), 0, Random.Range(-40, 40)), 1);
        Grow(iterations, meshes, Vector3.zero, Quaternion.Euler(Random.Range(-90, 90), 0, Random.Range(-75, 75)), 1);
        Grow(iterations, meshes, Vector3.zero, Quaternion.Euler(Random.Range(-90, 90), 0, Random.Range(-90, 90)), 1);
        Grow(iterations, meshes, Vector3.zero, Quaternion.Euler(Random.Range(-5, 10), Random.Range(0, 180), 0), 1);
        Grow(iterations, meshes, Vector3.zero, Quaternion.Euler(Random.Range(-20, 25), Random.Range(0, 180), 0), 1);
        Grow(iterations, meshes, Vector3.zero, Quaternion.Euler(Random.Range(-30, 30), Random.Range(0, 180), 0), 1);
        Grow(iterations, meshes, Vector3.zero, Quaternion.Euler(Random.Range(-90, 90), Random.Range(0, 180), 0), 1);
        //creates the main body part of the urchin
        Grow2(iteratoinsFan, meshes, Vector3.zero, Quaternion.identity, 1);
        //creates and combines the meshes created by the grow comands
        Mesh mesh = new Mesh();
        mesh.CombineMeshes(meshes.ToArray());

        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;
    }
    /// <summary>
    /// Where the grow function generates the new branches 
    /// </summary>
    /// <param name="num">the number of iterations</param>
    /// <param name="meshes">list of the meshes we combined</param>
    /// <param name="pos">position of our meshes</param>
    /// <param name="rot">rotation of our meshes</param>
    /// <param name="scale">scale of our meshes</param>
    private void Grow(int num, List<CombineInstance> meshes, Vector3 pos, Quaternion rot, float scale)
    {
        //to help randomize put in this random number generator if its above 8 it will end it early so each spindle has a different amount of things.

        if (num <= 0) return; //stop recursive function
        num--;
        //adds meshes to the coral list that have been generated
        CombineInstance inst = new CombineInstance();

        inst.mesh = MakeCube(num);
        inst.transform = Matrix4x4.TRS(pos, rot, branchScaling * scale);

        meshes.Add(inst);
        //sets the scale of the urchins spindles.
        scale *= .7f;
        //sets a new position for the new spindle that will be spawning in
        pos = inst.transform.MultiplyPoint(new Vector3(0, 1f, 0));

        Grow(num, meshes, pos, rot, scale);
    }
    private void Grow2(int num, List<CombineInstance> meshes, Vector3 pos, Quaternion rot, float scale)
    {
       
        if (num <= 0) return; //stop recursive function
        num--;
        //adds meshes to the coral list that have been generated
        CombineInstance urchinBase = new CombineInstance();

        urchinBase.mesh = MakeCube1();
        urchinBase.transform = Matrix4x4.TRS(pos, rot, baseScaling);


        meshes.Add(urchinBase);

    }
    /// <summary>
    /// creates the mesh of the cube that is generated
    /// </summary>
    private Mesh MakeCube(int num)
    {

        Mesh mesh = MeshTools.MakeTaperCube(.2f);
        List<Color> colors = new List<Color>();

        for (int i = 0; i < mesh.vertexCount; i++)
        {

            Color color = Color.HSVToRGB(Random.Range(0, 2), 1, Random.Range(0, 2));

            colors.Add(color);
        }      

        mesh.SetColors(colors);
        return mesh;

    }
    private Mesh MakeCube1()
    {

        List<Color> colors = new List<Color>();
        Mesh mesh = MeshTools.MakeSmoothCube();

        for (int i = 0; i < mesh.vertexCount; i++)
        {


            Color color = Color.HSVToRGB(0, Random.Range(0, 2), 0);

            colors.Add(color);
        }

        mesh.SetColors(colors);
        return mesh;

    }
}
