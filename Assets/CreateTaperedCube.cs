using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateTaperedCube : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Quaternion rot = Quaternion.identity;
        Vector3 scale = new Vector3(1, 1, 1);

        List<CombineInstance> meshes = new List<CombineInstance>();

        CombineInstance cube = new CombineInstance();
        cube.mesh = MeshTools.MakeCube();
        cube.transform = Matrix4x4.TRS(pos, rot, scale);

        meshes.Add(cube);

        Mesh mesh = new Mesh();
        mesh.CombineMeshes(meshes.ToArray());

        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
