using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CoralTree : MonoBehaviour
{
    [Range(2, 8)] public int iterations = 3;

    public Vector3 branchScale = new Vector3(.25f, 1, .25f);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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
        if (num <= 0) return;//stop

        CombineInstance inst = new CombineInstance();
        inst.mesh = MakeCube(num);
        inst.transform = Matrix4x4.TRS(pos, rot, branchScale * scale);

        meshes.Add(inst);

        num--;

        pos = inst.transform.MultiplyPoint(new Vector3(0, 1, 0));
        Vector3 sidePos = inst.transform.MultiplyPoint(new Vector3(.5f, .5f, 0));

        int ran = Random.Range(0, 4);

        if (ran == 0)
        {
            Quaternion rot1 = rot * Quaternion.Euler(0, Random.Range(30, 90), Random.Range(0, 45));

            Grow(num, meshes, sidePos, rot1, scale);
            print("0");
        }

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
            print("1");
        }

        if (ran == 2)
        {
            Quaternion rot1 = rot * Quaternion.Euler(0, Random.Range(0, 90), Random.Range(0, 45));
            Quaternion rot2 = rot * Quaternion.Euler(0, Random.Range(0, -90), Random.Range(0, -45));
            Quaternion rot3 = rot * Quaternion.Euler(Random.Range(0, 45), Random.Range(30, 90), 0);

            Grow(num, meshes, pos, rot1, scale);
            Grow(num, meshes, pos, rot2, scale);
            Grow(num, meshes, pos, rot3, scale);
            print("2");
        }

        if (ran == 3)
        {
            Quaternion rot1 = rot * Quaternion.Euler(0, Random.Range(0, 90), Random.Range(0, 45));
            Quaternion rot2 = rot * Quaternion.Euler(0, Random.Range(0, -90), Random.Range(0, -45));
            
            Grow(num, meshes, pos, rot1, scale);
            Grow(num, meshes, pos, rot2, scale);
            print("3");
        }

        scale *= scale;
    }

    private Mesh MakeCube(int num)
    {
        List<Vector3> verts = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<Vector3> normals = new List<Vector3>();
        List<Color> colors = new List<Color>();
        List<int> tris = new List<int>();

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

        float hueMin = Random.Range(.4f, .7f);
        float hueMax = Random.Range(.7f, 1f);

        float hue = Mathf.Lerp(hueMin, hueMax, (num / (float)iterations));

        foreach (Vector3 pos in verts)
        {
            float tempHue = hue; //+ (1 / (float)iterations) * pos.y;

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

[CustomEditor(typeof(CoralTree))]
public class CoralMeshEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("GROW!"))
        {
            CoralTree b = (target as CoralTree);
            b.Build();
        }
    }
}
