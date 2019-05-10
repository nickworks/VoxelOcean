using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class CoralSupreme : MonoBehaviour
{
    /// <summary>
    /// The number of iterations that the recursive Grow() function will iterate
    /// </summary>
    [Range(2, 8)] public int iterations = 6;
    /// <summary>
    /// A value of up to 30 degrees off of 120 degrees (360/3 for 3 branches) for the branches to grow from the center
    /// </summary>
    [Range(0, 30)] public int randomDir1Offset = 0;
    /// <summary>
    /// A value of up to 15 degrees off of 45 degrees from horizontal for the branches to grow upward.
    /// </summary>
    [Range(0, 15)] public int randomDir2Offset = 0;
    /// <summary>
    /// The chance of a coral branch spawning from the center of the previous branch in the exact same direction rather than splitting into 3
    /// </summary>
    [Range(0, 100)]public int Spawn4thBranchChance = 0;
    /// <summary>
    /// The chance of a 4th branch spawning along with the other 3 branches. The 4th branch spawns directly out of the other branch.
    /// </summary>
    [Range(0, 50)] public int Mut1Chance = 0;

    /// <summary>
    /// Boolean value to override the OldColor and YoungColor variables and spawn the coral with 2 randomly decided values.
    /// </summary>
    public bool RandomizeColors;

    /// <summary>
    /// The color to spawn nearest the center of the coral
    /// </summary>
    public Color OldColor;

    /// <summary>
    /// The color to spawn furthest from the center of the coral.
    /// </summary>
    public Color YoungColor;

    /// <summary>
    /// The X Y and Z scaling for the rectangular portions of the coral.
    /// </summary>
    public Vector3 branchScaling = new Vector3(.25f, 1, .25f);


    /// <summary>
    /// The random object used for determining randomness
    /// </summary>
    private System.Random random = new System.Random();

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {

        Build();
        
    }

    /// <summary>
    /// Build function sets up and begins the recursive function Grow() 
    /// </summary>
    public void Build()
    {
        if (RandomizeColors)
        {
            YoungColor = new Color((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble(), 1);
            OldColor = new Color((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble(), 1);
        }

        List<CombineInstance> meshes = new List<CombineInstance>();
        Grow(iterations, meshes, Vector3.zero, Quaternion.identity, 1, iterations);

        Mesh mesh = new Mesh();
        mesh.CombineMeshes(meshes.ToArray());
        
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;
        
    }

    /// <summary>
    /// The Recursive Grow Function
    /// Each recursion spawns more branches.
    /// </summary>
    /// <param name="num">The number of iterations left in the recursive function. If less than or = to 0, returns.</param>
    /// <param name="meshes">A list of CombineInstance objects which is expanded each iteration of the function.</param>
    /// <param name="pos">The position of the transform of the previous iteration. If the first iteration, is the transform of the GameObject.</param>
    /// <param name="rot">The rotation of the transform of the previous iteration. If the first iteration, is the transform of the GameObject.</param>
    /// <param name="scale">The Scale of the transform of the previous iteration. If the first iteration, is the transform of the GameObject.</param>
    /// <param name="maxNum">The Initial number of iterations. Should not change.</param>
    private void Grow(int num, List<CombineInstance> meshes, Vector3 pos, Quaternion rot, float scale, int maxNum)
    {
        if (num <= 0) return;

        

        CombineInstance inst = new CombineInstance();
        //inst.mesh = MakeCube(num, maxNum);
        inst.mesh = MeshTools.MakeCube();
        Color[] topAndBottomColors = GetColors(num, maxNum);
        List<Color> vertexColors = new List<Color>();
        foreach (var vert in inst.mesh.vertices)
        {
            if (vert.y == 1)
                vertexColors.Add(topAndBottomColors[1]);
            else
                vertexColors.Add(topAndBottomColors[0]);
        }
        inst.mesh.colors = vertexColors.ToArray();

        inst.transform = Matrix4x4.TRS(pos, rot, branchScaling * scale);

        meshes.Add(inst);
        num--;
        //TODO: modify pos, rot, scale

        pos = inst.transform.MultiplyPoint(new Vector3(0, 1, 0));
        Quaternion rot1 = rot * Quaternion.Euler(60 + (random.Next(-randomDir2Offset, randomDir2Offset)), 240 + (random.Next(-randomDir1Offset, randomDir1Offset)), 0);
        Quaternion rot2 = rot * Quaternion.Euler(60 + (random.Next(-randomDir2Offset, randomDir2Offset)), 0 + (random.Next(-randomDir1Offset, randomDir1Offset)), 0);
        Quaternion rot3 = rot * Quaternion.Euler(60 + (random.Next(-randomDir2Offset, randomDir2Offset)), 120 + (random.Next(-randomDir1Offset, randomDir1Offset)), 0);
        Quaternion rot0 = rot * Quaternion.Euler(0, 0, 0);
        int roll = random.Next(0, 99);

        if (Mut1Chance > roll)
        {

            Quaternion multRot = Quaternion.Lerp(rot, Quaternion.Euler(Vector3.up), .2f);
            Grow(num + 1, meshes, pos, multRot, scale * .1f * (random.Next(6, 8)), maxNum);
        }
        else
        {
            Grow(num, meshes, pos, rot1, scale * .1f * (random.Next(6, 8)), maxNum);
            Grow(num, meshes, pos, rot2, scale * .1f * (random.Next(6, 8)), maxNum);
            Grow(num, meshes, pos, rot3, scale * .1f * (random.Next(6, 8)), maxNum);


            if (Spawn4thBranchChance > roll)
            {
                Grow(num, meshes, pos, rot0, scale, maxNum);

            }
        }
        
    }

    /// <summary>
    /// Creates the rectangular meshes for the branches.
    /// </summary>
    /// <param name="num">The current iteration in the calling Grow() function.</param>
    /// <param name="maxNum">The initial iteration in the calling Grow() function.</param>
    /// <returns></returns>
    //private Mesh MakeCube(int num, int maxNum)
    //{
    //    List<Vector3> verts = new List<Vector3>();
    //    List<Vector2> uvs = new List<Vector2>();
    //    List<Vector3> normals = new List<Vector3>();
    //    List<int> tris = new List<int>();

    //    List<Color> vertexColors = new List<Color>();

    //    Color[] topAndBottomColors = GetColors(num, maxNum);
    //    //Front (I hate you for making me tediously making me type this)
    //    verts.Add(new Vector3(-0.5f, 0, -0.5f));
    //    vertexColors.Add(topAndBottomColors[1]);
    //    verts.Add(new Vector3(-0.5f, 1, -0.5f));
    //    vertexColors.Add(topAndBottomColors[0]);
    //    verts.Add(new Vector3(+0.5f, 1, -0.5f));
    //    vertexColors.Add(topAndBottomColors[0]);
    //    verts.Add(new Vector3(+0.5f, 0, -0.5f));
    //    vertexColors.Add(topAndBottomColors[1]);
    //    normals.Add(new Vector3(0, 0, -1));
    //    normals.Add(new Vector3(0, 0, -1));
    //    normals.Add(new Vector3(0, 0, -1));
    //    normals.Add(new Vector3(0, 0, -1));
    //    uvs.Add(new Vector2(0, 0));
    //    uvs.Add(new Vector2(0, 1));
    //    uvs.Add(new Vector2(1, 1));
    //    uvs.Add(new Vector2(1, 0));
    //    tris.Add(0);
    //    tris.Add(1);
    //    tris.Add(2);
    //    tris.Add(2);
    //    tris.Add(3);
    //    tris.Add(0);

        

    //    //Back (I hate you for making me tediously making me type this)
    //    verts.Add(new Vector3(-0.5f, 0, +0.5f));
    //    vertexColors.Add(topAndBottomColors[1]);
    //    verts.Add(new Vector3(+0.5f, 0, +0.5f));
    //    vertexColors.Add(topAndBottomColors[1]);
    //    verts.Add(new Vector3(+0.5f, 1, +0.5f));
    //    vertexColors.Add(topAndBottomColors[0]);
    //    verts.Add(new Vector3(-0.5f, 1, +0.5f));
    //    vertexColors.Add(topAndBottomColors[0]);
    //    normals.Add(new Vector3(0, 0, +1));
    //    normals.Add(new Vector3(0, 0, +1));
    //    normals.Add(new Vector3(0, 0, +1));
    //    normals.Add(new Vector3(0, 0, +1));
    //    uvs.Add(new Vector2(0, 0));
    //    uvs.Add(new Vector2(0, 1));
    //    uvs.Add(new Vector2(1, 1));
    //    uvs.Add(new Vector2(1, 0));
    //    tris.Add(4);
    //    tris.Add(5);
    //    tris.Add(6);
    //    tris.Add(6);
    //    tris.Add(7);
    //    tris.Add(4);

    //    //Left (I hate you for making me tediously making me type this)
    //    verts.Add(new Vector3(-0.5f, 0, -0.5f));
    //    vertexColors.Add(topAndBottomColors[1]);
    //    verts.Add(new Vector3(-0.5f, 0, +0.5f));
    //    vertexColors.Add(topAndBottomColors[1]);
    //    verts.Add(new Vector3(-0.5f, 1, +0.5f));
    //    vertexColors.Add(topAndBottomColors[0]);
    //    verts.Add(new Vector3(-0.5f, 1, -0.5f));
    //    vertexColors.Add(topAndBottomColors[0]);
    //    normals.Add(new Vector3(-1, 0, 0));
    //    normals.Add(new Vector3(-1, 0, 0));
    //    normals.Add(new Vector3(-1, 0, 0));
    //    normals.Add(new Vector3(-1, 0, 0));
    //    uvs.Add(new Vector2(0, 0));
    //    uvs.Add(new Vector2(0, 1));
    //    uvs.Add(new Vector2(1, 1));
    //    uvs.Add(new Vector2(1, 0));
    //    tris.Add(8);
    //    tris.Add(9);
    //    tris.Add(10);
    //    tris.Add(10);
    //    tris.Add(11);
    //    tris.Add(8);

    //    //Right (I hate you for making me tediously making me type this)
    //    verts.Add(new Vector3(+0.5f, 0, -0.5f));
    //    vertexColors.Add(topAndBottomColors[1]);
    //    verts.Add(new Vector3(+0.5f, 1, -0.5f));
    //    vertexColors.Add(topAndBottomColors[0]);
    //    verts.Add(new Vector3(+0.5f, 1, +0.5f));
    //    vertexColors.Add(topAndBottomColors[0]);
    //    verts.Add(new Vector3(+0.5f, 0, +0.5f));
    //    vertexColors.Add(topAndBottomColors[1]);
    //    normals.Add(new Vector3(+1, 0, 0));
    //    normals.Add(new Vector3(+1, 0, 0));
    //    normals.Add(new Vector3(+1, 0, 0));
    //    normals.Add(new Vector3(+1, 0, 0));
    //    uvs.Add(new Vector2(0, 0));
    //    uvs.Add(new Vector2(0, 1));
    //    uvs.Add(new Vector2(1, 1));
    //    uvs.Add(new Vector2(1, 0));
    //    tris.Add(12);
    //    tris.Add(13);
    //    tris.Add(14);
    //    tris.Add(14);
    //    tris.Add(15);
    //    tris.Add(12);

    //    //Top (I hate you for making me tediously making me type this)
    //    verts.Add(new Vector3(-0.5f, 1, -0.5f));
    //    vertexColors.Add(topAndBottomColors[0]);
    //    verts.Add(new Vector3(-0.5f, 1, +0.5f));
    //    vertexColors.Add(topAndBottomColors[0]);
    //    verts.Add(new Vector3(+0.5f, 1, +0.5f));
    //    vertexColors.Add(topAndBottomColors[0]);
    //    verts.Add(new Vector3(+0.5f, 1, -0.5f));
    //    vertexColors.Add(topAndBottomColors[0]);
    //    normals.Add(new Vector3(0, +1, 0));
    //    normals.Add(new Vector3(0, +1, 0));
    //    normals.Add(new Vector3(0, +1, 0));
    //    normals.Add(new Vector3(0, +1, 0));
    //    uvs.Add(new Vector2(0, 0));
    //    uvs.Add(new Vector2(0, 1));
    //    uvs.Add(new Vector2(1, 1));
    //    uvs.Add(new Vector2(1, 0));
    //    tris.Add(16);
    //    tris.Add(17);
    //    tris.Add(18);
    //    tris.Add(18);
    //    tris.Add(19);
    //    tris.Add(16);

        

        


    //    //Bottom (I hate you for making me tediously making me type this)
    //    verts.Add(new Vector3(-0.5f, 0, -0.5f));
    //    vertexColors.Add(topAndBottomColors[1]);
    //    verts.Add(new Vector3(+0.5f, 0, -0.5f));
    //    vertexColors.Add(topAndBottomColors[1]);
    //    verts.Add(new Vector3(+0.5f, 0, +0.5f));
    //    vertexColors.Add(topAndBottomColors[1]);
    //    verts.Add(new Vector3(-0.5f, 0, +0.5f));
    //    vertexColors.Add(topAndBottomColors[1]);
    //    normals.Add(new Vector3(0, -1, 0));
    //    normals.Add(new Vector3(0, -1, 0));
    //    normals.Add(new Vector3(0, -1, 0));
    //    normals.Add(new Vector3(0, -1, 0));
    //    uvs.Add(new Vector2(0, 0));
    //    uvs.Add(new Vector2(0, 1));
    //    uvs.Add(new Vector2(1, 1));
    //    uvs.Add(new Vector2(1, 0));
    //    tris.Add(20);
    //    tris.Add(21);
    //    tris.Add(22);
    //    tris.Add(22);
    //    tris.Add(23);
    //    tris.Add(20);

    //    Mesh mesh = new Mesh();
    //    mesh.SetVertices(verts);
    //    mesh.SetUVs(0, uvs);
    //    mesh.SetNormals(normals);
    //    mesh.SetTriangles(tris, 0);
    //    mesh.colors = vertexColors.ToArray();
    //    return mesh;



    //}

    /// <summary>
    /// Gets the colors for the vertices for the current generation. The num and maxNum arguments are used to find proportions and to Lerp the colors.
    /// </summary>
    /// <param name="num">The current iteration</param>
    /// <param name="maxNum">The maximum iteration.</param>
    /// <returns>A Color array of size 2, containing the colors which the current generation will gradiant between. </returns>
    private Color[] GetColors(int num, int maxNum)
    {

        float ratio1 = (num - 1f) / maxNum;
        float ratio2 = (float)num / maxNum;

        

        Color color1 = Color.Lerp(YoungColor, OldColor, ratio1);
        Color color2 = Color.Lerp(YoungColor, OldColor, ratio2);
        return new Color[2] { color1, color2 };
    }

    

}
/// <summary>
/// Adds the GROW button to the inspector, and stuff.
/// </summary>
[CustomEditor(typeof(CoralSupreme))]
public class CoralSupremeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        
        base.OnInspectorGUI();

        if (GUILayout.Button("GROW!"))
        {
            CoralSupreme cm =  (target as CoralSupreme);
                    
            cm.Build();
        }
        
    }
}

