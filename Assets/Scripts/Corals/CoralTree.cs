using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Spawns random Coral Trees
/// </summary>
public class CoralTree : MonoBehaviour
{
    /// <summary>
    /// how many iterations per branch
    /// </summary>
    [Range(2, 8)] public int iterations = 6;

    /// <summary>
    /// the scale of each branch
    /// </summary>
    public Vector3 branchScale = new Vector3(.25f, 1, .25f);

    void Start()
    {
        Build();
    }

    /// <summary>
    /// combines everything into one mesh
    /// </summary>
    public void Build()
    {
        List<CombineInstance> meshes = new List<CombineInstance>();

        Grow(iterations, meshes, Vector3.zero, Quaternion.identity, 1);

        //combines the meshes
        Mesh mesh = new Mesh();
        mesh.CombineMeshes(meshes.ToArray());

        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;

    }//end Build

    /// <summary>
    /// where the recursion happens
    /// </summary>
    /// <param name="num">the number of iterations</param>
    /// <param name="meshes">the list of the meshes to be combined</param>
    /// <param name="pos">the position of the meshes</param>
    /// <param name="rot">the rotstion of the meshes</param>
    /// <param name="scale">the scale of the meshes</param>
    private void Grow(int num, List<CombineInstance> meshes, Vector3 pos, Quaternion rot, float scale)
    {
        if (num <= 0) return;//stop

        CombineInstance inst = new CombineInstance();
        inst.mesh = MakeCube();
        inst.transform = Matrix4x4.TRS(pos, rot, branchScale * scale);

        meshes.Add(inst);

        num--;

        pos = inst.transform.MultiplyPoint(new Vector3(0, 1, 0));
        Vector3 sidePos = inst.transform.MultiplyPoint(new Vector3(.5f, .5f, 0));

        //decide the amount of branches based on the number picked
        int ran = Random.Range(0, 4);

        //one branch
        if (ran == 0)
        {
            Quaternion rot1 = rot * Quaternion.Euler(0, Random.Range(30, 90), Random.Range(0, 45));

            Grow(num, meshes, sidePos, rot1, scale);
        }

        //four branches
        if (ran == 1)
        {
            Quaternion rot1 = rot * Quaternion.Euler(0, Random.Range(30, 90), Random.Range(0, 45));
            Quaternion rot2 = rot * Quaternion.Euler(0, Random.Range(-30, -90), Random.Range(0, -45));
            Quaternion rot3 = rot * Quaternion.Euler(Random.Range(0, 45), Random.Range(30, 90), 0);
            Quaternion rot4 = rot * Quaternion.Euler(Random.Range(0, -45), Random.Range(-30, -90), 0);

            Grow(num, meshes, pos, rot1, scale);
            Grow(num, meshes, pos, rot2, scale);
            Grow(num, meshes, pos, rot3, scale);
            Grow(num, meshes, pos, rot4, scale);
        }

        //three branches
        if (ran == 2)
        {
            Quaternion rot1 = rot * Quaternion.Euler(0, Random.Range(0, 90), Random.Range(0, 45));
            Quaternion rot2 = rot * Quaternion.Euler(0, Random.Range(0, -90), Random.Range(0, -45));
            Quaternion rot3 = rot * Quaternion.Euler(Random.Range(0, 45), Random.Range(30, 90), 0);

            Grow(num, meshes, pos, rot1, scale);
            Grow(num, meshes, pos, rot2, scale);
            Grow(num, meshes, pos, rot3, scale);
        }

        //two branches
        if (ran == 3)
        {
            Quaternion rot1 = rot * Quaternion.Euler(0, Random.Range(0, 90), Random.Range(0, 45));
            Quaternion rot2 = rot * Quaternion.Euler(0, Random.Range(0, -90), Random.Range(0, -45));

            Grow(num, meshes, pos, rot1, scale);
            Grow(num, meshes, pos, rot2, scale);
        }

        scale *= scale;

    }//end Grow

    /// <summary>
    /// Creates the mesh
    /// </summary>
    /// <returns></returns>
    private Mesh MakeCube()
    {
      
        List<Color> colors = new List<Color>();

        //random vertex colors
        float hueMin = Random.Range(.4f, .7f);
        float hueMax = Random.Range(.7f, 1f);

        //blends between the min and max by way of the number of iterations
        float hue = Mathf.Lerp(hueMin, hueMax, (float)iterations);
        Mesh mesh = MeshTools.MakeCube();
        //applies the colors to each vertex
        for (int i = 0; i < mesh.vertexCount; i++)
        {
            Color color = Color.HSVToRGB(hue, 1, 1);

            colors.Add(color);
        }

       
        mesh.SetColors(colors);
        return mesh;

    }//end MakeCube

}//end CoralTree

