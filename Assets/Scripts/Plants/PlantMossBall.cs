using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Spawns Coral type Fan 
/// </summary>
public class PlantMossBall : MonoBehaviour
{
    /// <summary>
    /// The number of iterations spawned from the original
    /// </summary>
    public int iterations = 6;
    public bool sideFan = false;
    /// <summary>
    /// How the scaling of the branches are for both the skeleton set and fan set
    /// </summary>
    public Vector3 branchScaling = new Vector3(.03f, .2f, .1f);
    public Vector3 pos2 = new Vector3(-2, 0, 2);
    public Vector3 pos3 = new Vector3(2, 0, 2);
    public Vector3 pos4 = new Vector3(-1, 0, 1);
    public Vector3 pos5 = new Vector3(1, 0, -1);
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
        //how many iterations there will be to create variety.
        List<CombineInstance> meshes = new List<CombineInstance>();
        //starting branches that spawn in 
        Grow(iterations, meshes, Vector3.zero, Quaternion.identity, 1);
        Grow(iterations, meshes, Vector3.zero, Quaternion.identity, 1);

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
        if (num <= 0) return; //stop recursive function
        num--;
        //adds meshes to the coral list that have been generated
        CombineInstance inst = new CombineInstance();

        inst.mesh = MakeCube(num);
        inst.transform = Matrix4x4.TRS(pos, rot, branchScaling);


        meshes.Add(inst);


        //where the branches will be positioned when spawning in
        pos = inst.transform.MultiplyPoint(new Vector3(0, .5f, 0));

        //rotation of the branches spawned
        Quaternion rot1 = rot * Quaternion.Euler(Random.Range(30, 60), Random.Range(30, 60), Random.Range(30, 60));
        Quaternion rot2 = rot * Quaternion.Euler(Random.Range(-30, -60), 0, Random.Range(-60, -30));
        Quaternion rot4 = rot * Quaternion.Euler(Random.Range(-90, 90), Random.Range(-90, 90), Random.Range(-90, 90));
        Quaternion rot5 = rot * Quaternion.Euler(Random.Range(-40, 40), 0, Random.Range(-40, 40));

        //how each iteration of a branch will be generated
        scale *= .8f;

        //branches that spawn 
        Grow(num, meshes, pos, rot1, scale);
        Grow(num, meshes, pos, rot4, scale);
        Grow(num, meshes, pos, rot5, scale);

    }

    /// <summary>
    /// creates the mesh of the cube that is generated
    /// </summary>
    private Mesh MakeCube(int num)
    {
        List<Vector3> verts = new List<Vector3>();
        List<Vector3> uvs = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();
        List<Color> colors = new List<Color>();
        List<int> tris = new List<int>();

        //Front
        verts.Add(new Vector3(-0.5f, 0, -0.5f));
        verts.Add(new Vector3(-0.5f, 1, -0.5f));
        verts.Add(new Vector3(+0.5f, 1, -0.5f));
        verts.Add(new Vector3(+0.5f, 0, -0.5f));
        normals.Add(new Vector3(0, 0, -1));
        normals.Add(new Vector3(0, 0, -1));
        normals.Add(new Vector3(0, 0, -1));
        normals.Add(new Vector3(0, 0, -1));
        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 1));
        uvs.Add(new Vector2(1, 0));
        tris.Add(0);
        tris.Add(1);
        tris.Add(2);
        tris.Add(2);
        tris.Add(3);
        tris.Add(0);

        //Back
        verts.Add(new Vector3(-0.5f, 0, +0.5f));
        verts.Add(new Vector3(+0.5f, 0, +0.5f));
        verts.Add(new Vector3(+0.5f, 1, +0.5f));
        verts.Add(new Vector3(-0.5f, 1, +0.5f));
        normals.Add(new Vector3(0, 0, +1));
        normals.Add(new Vector3(0, 0, +1));
        normals.Add(new Vector3(0, 0, +1));
        normals.Add(new Vector3(0, 0, +1));
        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 1));
        uvs.Add(new Vector2(1, 0));
        tris.Add(4);
        tris.Add(5);
        tris.Add(6);
        tris.Add(6);
        tris.Add(7);
        tris.Add(4);

        //Left
        verts.Add(new Vector3(-0.5f, 0, -0.5f));
        verts.Add(new Vector3(-0.5f, 0, +0.5f));
        verts.Add(new Vector3(-0.5f, 1, +0.5f));
        verts.Add(new Vector3(-0.5f, 1, -0.5f));
        normals.Add(new Vector3(-1, 0, 0));
        normals.Add(new Vector3(-1, 0, 0));
        normals.Add(new Vector3(-1, 0, 0));
        normals.Add(new Vector3(-1, 0, 0));
        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 1));
        uvs.Add(new Vector2(1, 0));
        tris.Add(8);
        tris.Add(9);
        tris.Add(10);
        tris.Add(10);
        tris.Add(11);
        tris.Add(8);

        //Right
        verts.Add(new Vector3(+0.5f, 0, -0.5f));
        verts.Add(new Vector3(+0.5f, 1, -0.5f));
        verts.Add(new Vector3(+0.5f, 1, +0.5f));
        verts.Add(new Vector3(+0.5f, 0, +0.5f));
        normals.Add(new Vector3(+1, 0, 0));
        normals.Add(new Vector3(+1, 0, 0));
        normals.Add(new Vector3(+1, 0, 0));
        normals.Add(new Vector3(+1, 0, 0));
        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 1));
        uvs.Add(new Vector2(1, 0));
        tris.Add(12);
        tris.Add(13);
        tris.Add(14);
        tris.Add(14);
        tris.Add(15);
        tris.Add(12);

        //Top
        verts.Add(new Vector3(-0.5f, 1, -0.5f));
        verts.Add(new Vector3(-0.5f, 1, +0.5f));
        verts.Add(new Vector3(+0.5f, 1, +0.5f));
        verts.Add(new Vector3(+0.5f, 1, -0.5f));
        normals.Add(new Vector3(0, +1, 0));
        normals.Add(new Vector3(0, +1, 0));
        normals.Add(new Vector3(0, +1, 0));
        normals.Add(new Vector3(0, +1, 0));
        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 1));
        uvs.Add(new Vector2(1, 0));
        tris.Add(16);
        tris.Add(17);
        tris.Add(18);
        tris.Add(18);
        tris.Add(19);
        tris.Add(16);

        //Bottom
        verts.Add(new Vector3(-0.5f, 0, -0.5f));
        verts.Add(new Vector3(+0.5f, 0, -0.5f));
        verts.Add(new Vector3(+0.5f, 1, +0.5f));
        verts.Add(new Vector3(-0.5f, 0, +0.5f));
        normals.Add(new Vector3(0, -1, 0));
        normals.Add(new Vector3(0, -1, 0));
        normals.Add(new Vector3(0, -1, 0));
        normals.Add(new Vector3(0, -1, 0));
        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 1));
        uvs.Add(new Vector2(1, 0));
        tris.Add(20);
        tris.Add(21);
        tris.Add(22);
        tris.Add(22);
        tris.Add(23);
        tris.Add(20);



        //Set hue min and Max
        float hueMin = .3f;
        float hueMax = Random.Range(.3f, 1);

        float hue = Mathf.Lerp(hueMin, hueMax, (num / (float)iterations));
        //creating it so hues start out at random then turn to green
        foreach (Vector3 pos in verts)
        {
            float tempHue = hue;

            Color color = Color.HSVToRGB(tempHue, 1, 1);

            colors.Add(color);
        }

        Mesh mesh = new Mesh();
        mesh.SetVertices(verts);
        mesh.SetUVs(0, uvs);
        mesh.SetNormals(normals);
        mesh.SetTriangles(tris, 0);
        mesh.SetColors(colors);
        return mesh;

    }
}