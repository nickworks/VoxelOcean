using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class MeshStitcher : MonoBehaviour
{
    public GameObject parent;
    List<MeshFilter> meshesToStitch = new List<MeshFilter>();
    List<CombineInstance> combos = new List<CombineInstance>();
    public MeshFilter output;
    private Mesh generatedMesh;
    private static int meshIndex = 1;

    // Use this for initialization
    public void Stitch()
    {
        combos.Clear();
        meshesToStitch.Clear();

        foreach (MeshFilter mf in parent.GetComponentsInChildren<MeshFilter>())
        {
            CombineInstance combo = new CombineInstance();
            combo.mesh = mf.sharedMesh;
            combo.transform = Matrix4x4.TRS(mf.transform.position, mf.transform.rotation, mf.transform.lossyScale);

            combos.Add(combo);
        }

        Mesh mesh = new Mesh();
        mesh.CombineMeshes(combos.ToArray(), true);

        MeshFilter newOutput = Instantiate(output, transform);
        newOutput.mesh = mesh;

        generatedMesh = mesh;
        meshIndex++;

        //AssetDatabase.CreateAsset(mesh, "Assets/");
        //AssetDatabase.SaveAssets();
    }

    public void SaveMesh()
    {
        string path = EditorUtility.SaveFilePanel("Save Mesh as New", "Assets/", "newMesh" + meshIndex, "asset");
        if (string.IsNullOrEmpty(path) || generatedMesh == null) return;

        path = FileUtil.GetProjectRelativePath(path);

        MeshUtility.Optimize(generatedMesh);

        AssetDatabase.CreateAsset(generatedMesh, path);
        AssetDatabase.SaveAssets();
    }
    private void RemoveDuplicates(/*Mesh complexMesh, bool printDebug = false*/)
    {
        List<Vector3> verts = new List<Vector3>();
        List<int> tris = new List<int>();
        List<Vector3> norms = new List<Vector3>();
        List<Color> cols = new List<Color>();

        generatedMesh.GetVertices(verts);
        generatedMesh.GetTriangles(tris, 0);
        generatedMesh.GetNormals(norms);
        generatedMesh.GetColors(cols);

        int count1 = verts.Count;
        //Debug.Log(count1);
        //return;

        for (int i = 0; i < verts.Count; i++)
        {
            // find duplicates:
            for (int j = 0; j < verts.Count; j++)
            {
                if (i == j) continue;
                if (j >= verts.Count) break;
                if (i >= verts.Count) break;
                if (Vector3.Distance(verts[i], verts[j]) <= .5) // if a duplicate vert:
                {
                    verts.RemoveAt(j); // remove it
                    norms[i] = (norms[i] + norms[j]) / 2;
                    norms.RemoveAt(j);
                    cols.RemoveAt(j);
                    for (int k = 0; k < tris.Count; k++)
                    {
                        if (tris[k] == j) tris[k] = i;
                        if (tris[k] > j) tris[k] = tris[k] - 1;
                    }
                }
            }
        }
        int count2 = verts.Count;

        //if (printDebug) print($"{count1} reduced to {count2}");

        Mesh mesh = new Mesh();
        mesh.SetVertices(verts);
        mesh.SetTriangles(tris, 0);
        mesh.SetNormals(norms);
        mesh.SetColors(cols);
        
        MeshFilter newOutput = Instantiate(output, transform);
        newOutput.mesh = mesh;
        //return mesh;

    } // RemoveDuplicates()

    [CustomEditor(typeof(MeshStitcher))]
    class VoxelUniverseEditor : Editor
    {
        override public void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Stitch Meshes"))
            {
                (target as MeshStitcher).Stitch();
            }
            if (GUILayout.Button("Clean Mesh"))
            {
                (target as MeshStitcher).RemoveDuplicates();
            }
            if (GUILayout.Button("Save Mesh"))
            {
                (target as MeshStitcher).SaveMesh();
            }

        }
    }
}

