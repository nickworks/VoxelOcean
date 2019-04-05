using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// This script is used to create a mesh for a sea star within VoxelOcean.
/// Author: Kyle Lowery
/// Last Date Updated: 04/04/2019
/// </summary>
public class CreatureSeaStarMesh : MonoBehaviour
{

    // Ints:
    /// <summary>
    /// Controls the number of iterations the Build function goes through to make the mesh.
    /// </summary>
    [Range(2, 20)] public int iterations = 3;

    public int numberOfPoints = 5;

    // Scaling Vectors:
    public Vector3 pointScale = new Vector3(1, 1, 1);

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
        //Check to make sure Unity values don't break the program.
        if (hueMin > hueMax || hueMin == hueMax) hueMin = hueMax - .1f;

        Build();
    }

    public void Build()
    {
        List<CombineInstance> meshes = new List<CombineInstance>();
        Grow(iterations, meshes, Vector3.zero, Quaternion.identity);

        Mesh mesh = new Mesh();
        mesh.CombineMeshes(meshes.ToArray());

        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;
    }

    void Grow(int num, List<CombineInstance> meshes, Vector3 pos, Quaternion rot)
    {
        if (num <= 0) return;

        //If Grow is on first iteration:
        if (num == iterations)
        {
            //Make points of sea star:
            for (int i = 1; i <= numberOfPoints; i++)
            {
                CombineInstance point = new CombineInstance();
                point.mesh = MakeCube(num);
                float rotAmount = 360 / (numberOfPoints * i);
                Quaternion pointRot = rot * Quaternion.Euler(0, rotAmount, 0);
                point.transform = Matrix4x4.TRS(pos, pointRot, pointScale);
                meshes.Add(point);
            }
        }
        //Use this code for every other iteration:
        else
        {
            //Make points of sea star:
            for (int i = 1; i <= numberOfPoints; i++)
            {
                CombineInstance point = new CombineInstance();
                point.mesh = MakeCube(num);
                float rotAmount = 360 / (numberOfPoints * i);
                //Draw vector from rotation:

                //Build out point in that direction:

                //Scale down Mesh based on number of iterations:

                Quaternion pointRot = rot * Quaternion.Euler(0, rotAmount, 0);
                point.transform = Matrix4x4.TRS(pos, pointRot, pointScale);
                meshes.Add(point);
            }
        }

        num--;
        Grow(num, meshes, pos, rot);
    }

    /// <summary>
    /// Makes a mesh for a 1m-sized cube.
    /// </summary>
    /// <returns>Mesh data for a 1m-sized cube.</returns>
    private Mesh MakeCube(int num)
    {
        List<Vector3> verts = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<Vector3> normals = new List<Vector3>();
        List<int> tris = new List<int>();
        List<Color> colors = new List<Color>();

        //FRONT
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

        //BACK
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

        //LEFT
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

        //RIGHT
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

        //TOP
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

        //BOTTOM
        verts.Add(new Vector3(-0.5f, 0, -0.5f));
        verts.Add(new Vector3(+0.5f, 0, -0.5f));
        verts.Add(new Vector3(+0.5f, 0, +0.5f));
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

        float hue = Mathf.Lerp(hueMin, hueMax, (num / (float)iterations));

        foreach (Vector3 pos in verts)
        {
            float tempHue = hue;
            Color tempColor = Color.HSVToRGB(tempHue, 1, 1);
            colors.Add(tempColor);
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

[CustomEditor(typeof(CreatureSeaStarMesh))]
public class CreatureSeaStarMeshEditor : Editor
{
    /// <summary>
    /// Runs the base OnInspectorGUI command and adds a "Grow" button to the inspector in the GUI.
    /// </summary>
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("GROW!"))                              //if the "Grow" button is pressed...
        {
            CreatureSeaStarMesh s = (target as CreatureSeaStarMesh);                //create a new PlantKelpMesh on the script target.
            s.Build();                                                  //Call the new PlantKelpMesh's Build function.
        }
    }
}

