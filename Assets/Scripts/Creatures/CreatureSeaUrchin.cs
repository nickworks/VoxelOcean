using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureSeaUrchin : MonoBehaviour
{
    /// <summary>
    /// How the scaling of the branches are for both the urchin body and spines
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

        List<CombineInstance> meshes = new List<CombineInstance>();
        //What makes the spindles in the sea urchin
        for (int i = 0; i < 30; i++) {
            GrowSpines( meshes);
        }
     
        //creates the main body part of the urchin
        Grow2(meshes);
        //creates and combines the meshes created by the grow comands
        Mesh mesh = new Mesh();
        mesh.CombineMeshes(meshes.ToArray());

        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;
    }
    /// <summary>
    /// Where the grow function generates the new branches 
    /// </summary>
    /// <param name="meshes">list of the meshes we combined</param>
    private void GrowSpines(List<CombineInstance> meshes)
    {

        //adds meshes to the coral list that have been generated
        CombineInstance inst = new CombineInstance();
        inst.mesh = MakeCube();
        Vector3 dir = Random.insideUnitSphere;
        if(dir.z> 0)
        {
            dir.z *= -1;
        }
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, dir);
        inst.transform = Matrix4x4.TRS(Vector3.one, rot, new Vector3(.1f,.1f,5));
        meshes.Add(inst);
    }
    private void Grow2( List<CombineInstance> meshes)
    {
       
        //adds meshes to the coral list that have been generated
        CombineInstance urchinBase = new CombineInstance();
        urchinBase.mesh = MakeCube1();
        urchinBase.transform = Matrix4x4.TRS(new Vector3(.9f, .5f, 1f),Quaternion.identity, new Vector3(.8f, .8f, .8f));


        meshes.Add(urchinBase);

    }
    /// <summary>
    /// creates the mesh of the cube that is generated
    /// </summary>
    private Mesh MakeCube()
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
