using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
//TODO: Comment Class
// TO DO : Change name of class and script to prevent scripting
public class CoralMesh : MonoBehaviour
{
    //TO DO Give comments to variables
    [Range(2, 8)] public int iterations = 6;
    [Range(0, 30)] public int randomDir1Offset = 0;
    [Range(0, 15)] public int randomDir2Offset = 0;
    [Range(0, 100)]public int Spawn4thBranchChance = 0;
    [Range(0, 50)] public int Mut1Chance = 0;

    public Color OldColor;

    public Color YoungColor;

    //private List<Color> vertexColors = new List<Color>();


    public Vector3 branchScaling = new Vector3(.25f, 1, .25f);

    private System.Random random = new System.Random();

    // Start is called before the first frame update
    void Start()
    {

        Build();
        
    }

    // Update is called once per frame
    public void Build()
    {
        //vertexColors = new List<Color>();
        List<CombineInstance> meshes = new List<CombineInstance>();
        Grow(iterations, meshes, Vector3.zero, Quaternion.identity, 1, iterations);

        Mesh mesh = new Mesh();
        mesh.CombineMeshes(meshes.ToArray());
        
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        //mesh = meshFilter.mesh;
        //mesh.colors = vertexColors.ToArray();
        
        
    }
    //TODO: Name and Comment this section
    // TO DO CHANGE NAME
    private void Grow(int num, List<CombineInstance> meshes, Vector3 pos, Quaternion rot, float scale, int maxNum)
    {
        if (num <= 0) return;


        CombineInstance inst = new CombineInstance();
        inst.mesh = MakeCube(num, maxNum);
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
    //TO DO COMMENT this CODE
    private Mesh MakeCube(int num, int maxNum)
    {
        List<Vector3> verts = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<Vector3> normals = new List<Vector3>();
        List<int> tris = new List<int>();

        List<Color> vertexColors = new List<Color>();

        Color[] topAndBottomColors = GetColors(num, maxNum);
        //Front (I hate you for making me tediously making me type this)
        verts.Add(new Vector3(-0.5f, 0, -0.5f));
        vertexColors.Add(topAndBottomColors[1]);
        verts.Add(new Vector3(-0.5f, 1, -0.5f));
        vertexColors.Add(topAndBottomColors[0]);
        verts.Add(new Vector3(+0.5f, 1, -0.5f));
        vertexColors.Add(topAndBottomColors[0]);
        verts.Add(new Vector3(+0.5f, 0, -0.5f));
        vertexColors.Add(topAndBottomColors[1]);
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

        

        //Back (I hate you for making me tediously making me type this)
        verts.Add(new Vector3(-0.5f, 0, +0.5f));
        vertexColors.Add(topAndBottomColors[1]);
        verts.Add(new Vector3(+0.5f, 0, +0.5f));
        vertexColors.Add(topAndBottomColors[1]);
        verts.Add(new Vector3(+0.5f, 1, +0.5f));
        vertexColors.Add(topAndBottomColors[0]);
        verts.Add(new Vector3(-0.5f, 1, +0.5f));
        vertexColors.Add(topAndBottomColors[0]);
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

        //Left (I hate you for making me tediously making me type this)
        verts.Add(new Vector3(-0.5f, 0, -0.5f));
        vertexColors.Add(topAndBottomColors[1]);
        verts.Add(new Vector3(-0.5f, 0, +0.5f));
        vertexColors.Add(topAndBottomColors[1]);
        verts.Add(new Vector3(-0.5f, 1, +0.5f));
        vertexColors.Add(topAndBottomColors[0]);
        verts.Add(new Vector3(-0.5f, 1, -0.5f));
        vertexColors.Add(topAndBottomColors[0]);
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

        //Right (I hate you for making me tediously making me type this)
        verts.Add(new Vector3(+0.5f, 0, -0.5f));
        vertexColors.Add(topAndBottomColors[1]);
        verts.Add(new Vector3(+0.5f, 1, -0.5f));
        vertexColors.Add(topAndBottomColors[0]);
        verts.Add(new Vector3(+0.5f, 1, +0.5f));
        vertexColors.Add(topAndBottomColors[0]);
        verts.Add(new Vector3(+0.5f, 0, +0.5f));
        vertexColors.Add(topAndBottomColors[1]);
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

        //Top (I hate you for making me tediously making me type this)
        verts.Add(new Vector3(-0.5f, 1, -0.5f));
        vertexColors.Add(topAndBottomColors[0]);
        verts.Add(new Vector3(-0.5f, 1, +0.5f));
        vertexColors.Add(topAndBottomColors[0]);
        verts.Add(new Vector3(+0.5f, 1, +0.5f));
        vertexColors.Add(topAndBottomColors[0]);
        verts.Add(new Vector3(+0.5f, 1, -0.5f));
        vertexColors.Add(topAndBottomColors[0]);
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

        

        


        //Bottom (I hate you for making me tediously making me type this)
        verts.Add(new Vector3(-0.5f, 0, -0.5f));
        vertexColors.Add(topAndBottomColors[1]);
        verts.Add(new Vector3(+0.5f, 0, -0.5f));
        vertexColors.Add(topAndBottomColors[1]);
        verts.Add(new Vector3(+0.5f, 0, +0.5f));
        vertexColors.Add(topAndBottomColors[1]);
        verts.Add(new Vector3(-0.5f, 0, +0.5f));
        vertexColors.Add(topAndBottomColors[1]);
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

        Mesh mesh = new Mesh();
        mesh.SetVertices(verts);
        mesh.SetUVs(0, uvs);
        mesh.SetNormals(normals);
        mesh.SetTriangles(tris, 0);
        mesh.colors = vertexColors.ToArray();
        return mesh;



    }
    //TO DO COMMENT COLORS
    private Color[] GetColors(int num, int maxNum)
    {

        float ratio1 = (num - 1f) / maxNum;
        float ratio2 = (float)num / maxNum;

        Color color1 = Color.Lerp(YoungColor, OldColor, ratio1);
        Color color2 = Color.Lerp(YoungColor, OldColor, ratio2);
        return new Color[2] { color1, color2 };
    }

    

}

//TODO: Comment this section
[CustomEditor(typeof(CoralMesh))]
public class CoralMeshEditor : Editor
{
    public override void OnInspectorGUI()
    {
        
        base.OnInspectorGUI();

        if (GUILayout.Button("GROW!"))
        {
            CoralMesh cm =  (target as CoralMesh);
                    
            cm.Build();
        }
        
    }
}

