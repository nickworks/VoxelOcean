using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonLivingTridentMesh : MonoBehaviour
{

    float hueMin = .2f;
    float hueMax = .8f;

    int iterations = 1;
    // Start is called before the first frame update
    void Start()
    {
        BuildMesh();
    }

    void BuildMesh()
    {
        List<CombineInstance> meshes = new List<CombineInstance>();
        ConstructTrident(meshes, Vector3.zero, Quaternion.identity);

        Mesh mesh = new Mesh();
        mesh.CombineMeshes(meshes.ToArray());

        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;
        
    }

    void ConstructTrident(List<CombineInstance> meshes, Vector3 pos, Quaternion rot)
    {
        //create polearm
        CombineInstance polearm = new CombineInstance();
        polearm.mesh = MeshTools.MakeCylinder(5);
        AddColorToVertices(polearm.mesh, 1);
        Vector3 polearmScale = new Vector3(.3f, 5f, .3f);
        polearm.transform = Matrix4x4.TRS(pos, rot, polearmScale);
        meshes.Add(polearm);

        //create trident head
        Vector3 headPos = pos;
        headPos.y += polearmScale.y;

        for (int i = 0; i < 3; i++) {
            Quaternion headRot = rot * Quaternion.Euler( 0, 0 , -90f) * Quaternion.Euler(0, 0, (i == 0) ? 0 : (90 * i));

            CombineInstance fork = new CombineInstance();
            fork.mesh = MeshTools.MakeCube();
            AddColorToVertices(fork.mesh, 1);
            Vector3 forkScale = new Vector3(.3f, 1f, .3f);
            fork.transform = Matrix4x4.TRS(headPos, headRot, forkScale);

            CombineInstance forkPoint = new CombineInstance();
            forkPoint.mesh = MeshTools.MakeTaperCube(.3f);
            Vector3 forkPos = fork.transform.MultiplyPoint(new Vector3(0, 1, 0));
            Quaternion upQuaternion = Quaternion.Euler(-90, 1, 0);
            Vector3 forkPointScale = new Vector3(.3f, .3f, 1f);
            forkPoint.transform = Matrix4x4.TRS(forkPos, upQuaternion, forkPointScale);
            meshes.Add(fork);
            meshes.Add(forkPoint);
        }

    }

    /// <summary>
    /// Adds color to vertices based on the number of iterations that have passed making the object.
    /// </summary>
    /// <param name="mesh">The mesh you wish to add vertex colors to.</param>
    /// <param name="num">The current number of iterations that have passed.</param>
    private void AddColorToVertices(Mesh mesh, int num)
    {
        List<Color> colors = new List<Color>();
        float hue = Mathf.Lerp(hueMin, hueMax, (num / (float)iterations));

        foreach (Vector3 pos in mesh.vertices)
        {
            float tempHue = hue;
            Color tempColor = Color.HSVToRGB(tempHue, 1, 1);
            colors.Add(tempColor);
        }

        mesh.SetColors(colors);
    }
}
