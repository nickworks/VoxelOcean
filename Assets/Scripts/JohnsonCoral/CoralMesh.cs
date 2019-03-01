using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CoralMesh : MonoBehaviour
{


    [Range(2, 6)] public int iterations = 3;
    [Range(1, 4)] public int branches = 5;
    //[Range(0f, 1f)] public float scaling = .75f;
    //public bool randomScale = false;

    public float xScaling = .15f;
    public float yScaling = 1f;
    public float zScaling = .15f;




    void Start()
    {
        Vector3 branchScaling = new Vector3(xScaling, yScaling, zScaling);
        List<CombineInstance> meshes = new List<CombineInstance>();



        Grow(iterations, meshes, Vector3.zero, Quaternion.identity, branchScaling);


        Mesh mesh = new Mesh();
        mesh.CombineMeshes(meshes.ToArray());

        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;
    }


    public void Build()
    {
        Vector3 branchScaling = new Vector3(xScaling, yScaling, zScaling);
        List<CombineInstance> meshes = new List<CombineInstance>();



        Grow(iterations, meshes, Vector3.zero, Quaternion.identity, branchScaling);


        Mesh mesh = new Mesh();
        mesh.CombineMeshes(meshes.ToArray());

        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;
    }

    private void Grow(int num, List<CombineInstance> meshes, Vector3 pos, Quaternion rot, Vector3 scale)
    {

        bool isTop = false;
        bool isLeft = false;
        bool isRight = false;
        bool isFront = false;
        bool isBack = false;


        if (num <= 0) return; // stop recursive function


        CombineInstance inst = new CombineInstance();
        inst.mesh = MakeCube(num);
        inst.transform = Matrix4x4.TRS(pos, rot, scale);



        meshes.Add(inst);


        num--;
        //TODO: modifiy pos,rot,scale



        scale.x *= .8f;
        scale.z *= .8f;
        scale.y *= .65f;



        pos = inst.transform.MultiplyPoint(new Vector3(0, .9f, 0));

        //face positions
        Vector3 sidePosition03 = inst.transform.MultiplyPoint(new Vector3(0, Random.Range(0.5f, .8f), 0));
        Vector3 sidePosition04 = inst.transform.MultiplyPoint(new Vector3(0, Random.Range(0.5f, .8f), 0));
        Vector3 sidePosition07 = inst.transform.MultiplyPoint(new Vector3(0, Random.Range(0.5f, .8f), 0));
        Vector3 sidePosition08 = inst.transform.MultiplyPoint(new Vector3(0, Random.Range(0.5f, .8f), 0));

        //top rotations
        Quaternion rot1 = rot * Quaternion.Euler(Random.Range(-15, 15), Random.Range(-15, 15), Random.Range(-15, 15));


        //face rotations
        Quaternion rot5 = rot * Quaternion.Euler(0, 0, Random.Range(45, 95));
        Quaternion rot6 = rot * Quaternion.Euler(0, 0, Random.Range(-45, -95));
        Quaternion rot9 = rot * Quaternion.Euler(Random.Range(75, 95), 0, 0);
        Quaternion rot10 = rot * Quaternion.Euler(Random.Range(-75, -95), 0, 0);


        int randomPicker = Random.Range(1, 6);


        //int randomPicker = 3;

        for (int i = 0; i < branches; i++)
        {
            if (randomPicker == 1 && isFront == false)
            {
                Grow(num, meshes, sidePosition03, rot5, scale);
                isFront = true;
            }

            else if (randomPicker == 2 && isBack == false)
            {
                Grow(num, meshes, sidePosition04, rot6, scale);
                isBack = true;
            }

            else if (randomPicker == 3 && isRight == false)
            {
                Grow(num, meshes, sidePosition07, rot9, scale);
                isRight = true;
            }

            else if (randomPicker == 4 && isLeft == false)
            {
                Grow(num, meshes, sidePosition08, rot10, scale);
                isLeft = true;
            }

            else if (randomPicker == 5 && isTop == false)
            {
                Grow(num, meshes, pos, rot1, scale);
                isTop = true;
            }
            else
            {
                randomPicker = Random.Range(1, 6);
                i--;
            }


        }




    }


    private Mesh MakeCube(int num)
    {

        List<Vector3> verts = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
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

        float hueMin = .94f;
        float hueMax = .99f;

        float hue = Mathf.Lerp(hueMax, hueMin, (num / (float)iterations));

        foreach (Vector3 pos in verts)
        {
            float tempHue = hue;// + (1/(float)iterations) * pos.y;

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

[CustomEditor(typeof(CoralMesh))]
public class CoralMeshEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("GROW!"))
        {

            CoralMesh b = (target as CoralMesh);
            b.Build();

        }

    }


}