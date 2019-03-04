using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CoralPurpleFan : MonoBehaviour
{
    [Range(2, 8)] public int iterations = 3;
    public Vector3 branchScaling = new Vector3(.25f, 1, .25f);
    public Vector3 baseScaling = new Vector3(.25f, 1, .25f);
    void Start()
    {
        
    }

    public void Build()
    {
        List<CombineInstance> meshes = new List<CombineInstance>();
        Grow(iterations, meshes, Vector3.zero, Quaternion.identity, 1);

        Mesh mesh = new Mesh();
        mesh.CombineMeshes(meshes.ToArray());

        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;
    }
    private void Grow(int num, List<CombineInstance> meshes, Vector3 pos, Quaternion rot, float scale)
    {
        if (num <= 0) return; //stop recursive function

        
        CombineInstance inst = new CombineInstance();
        CombineInstance coralBase = new CombineInstance();

        inst.mesh = MakeCube();
        inst.transform = Matrix4x4.TRS(pos, rot, branchScaling * scale);

        coralBase.mesh = MakeCube();
        coralBase.transform = Matrix4x4.TRS(pos, rot, baseScaling * scale);


        meshes.Add(coralBase);
      
        meshes.Add(inst);
       

        num--;

        pos = inst.transform.MultiplyPoint(new Vector3(0, 1, 0));
        Vector3 posLeft = inst.transform.MultiplyPoint(new Vector3(1, 0, 0));
        Vector3 posRight = inst.transform.MultiplyPoint(new Vector3(-1, 0, 0));


        Quaternion rot1 = rot * Quaternion.Euler(0, 0, Random.Range(0, 75));
        Quaternion rot2 = rot * Quaternion.Euler(0, 0, Random.Range(0, -75));

        Quaternion rotRight = rot * Quaternion.Euler(0, 0, Random.Range(0, 30));
        Quaternion rotLeft = rot * Quaternion.Euler(0, 0, Random.Range(0, -30));

        scale *= .8f;
       
        Grow(num, meshes, posLeft, rotLeft, scale);
        Grow(num, meshes, posRight, rotRight, scale);
        Grow(num, meshes, pos, rot1, scale);
        Grow(num, meshes, pos, rot2, scale);
       // Grow(num, meshes, pos, rot2, scale);
       // Grow(num, meshes, pos, rot1, scale);
        
        
        
        pos = coralBase.transform.MultiplyPoint(new Vector3(0, 1, 0));
        Grow(num, meshes, pos, rot, scale);
        Grow(num, meshes, pos, rot, scale);

        
    }
    private Mesh MakeCube()
    {
        List<Vector3> verts = new List<Vector3>();
        List<Vector3> uvs = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();
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

        Mesh mesh = new Mesh();
        mesh.SetVertices(verts);
        mesh.SetUVs(0, uvs);
        mesh.SetNormals(normals);
        mesh.SetTriangles(tris, 0);
        return mesh;

    }
}
[CustomEditor(typeof(CoralPurpleFan))]
public class CoralPurpleFanEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("GROW!"))
        {
            CoralPurpleFan b = (target as CoralPurpleFan);
            b.Build();
        }
    }
}
