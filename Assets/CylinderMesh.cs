using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderMesh : MonoBehaviour
{
    public Vector3 objectScaling = new Vector3(1, 1, 1);

    // Start is called before the first frame update
    void Start()
    {
        List<CombineInstance> meshes = new List<CombineInstance>();
        Build(meshes, Vector3.zero, Quaternion.identity);

        Mesh mesh = new Mesh();
        mesh.CombineMeshes(meshes.ToArray());

        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;
    }

    void Build(List<CombineInstance> meshes, Vector3 pos, Quaternion rot)
    {
        CombineInstance cylinder = new CombineInstance();
        cylinder.mesh = MeshTools.MakePolygonalCylinder(4);
        cylinder.transform = Matrix4x4.TRS(pos, rot, objectScaling);

        meshes.Add(cylinder);
    }
}
