using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class JoshCoralPyramidSpawn : MonoBehaviour
{

    public MeshFilter pyramidMesh;
    public MeshFilter prismMesh;

    [Range(1, 20)] public int iterations = 5;
    [Range(1, 7)] public int flipEveryNth = 3;
    public float rotationPerSegment = 63.4f;
    public bool smallerIterations = true;
    public bool usePrism = true;
    public bool branches = true;

    public bool randomVars = true;
    public int seed;

    
    void Start()
    {
        Build();
        Debug.Log("building for josh");
    }

    public void Build()
    {
        if (randomVars)
        {
            seed = Random.Range(0, 50000000);
        }
        Random.InitState(seed);

        if (randomVars)
        {
            Randomize();
        }

        // an array to hold the meshes:
        List<CombineInstance> meshes = new List<CombineInstance>();

        // grow our mesh recursively
        transform.rotation = Quaternion.Euler(0, 0, 180);
        Grow(0, true, meshes, Vector3.down, Quaternion.identity, 1);

        // make the mesh by combining our list of meshes:
        Mesh mesh = new Mesh();
        mesh.CombineMeshes(meshes.ToArray());

        // set our mesh:
        MeshFilter filter = GetComponent<MeshFilter>();
        filter.mesh = mesh;
    }
    void Randomize()
    {
        iterations = Random.Range(4, 15);
        flipEveryNth = Random.Range(1, 7);
        rotationPerSegment = Random.Range(10, 70);
        smallerIterations = Random.value < .7f;
        usePrism = Random.value < .5f;
        if (smallerIterations)
        {
            branches = Random.value < .5f;
        }
        else branches = false;

        if (branches)
        {
            if (iterations >= 15)
            {
                iterations -= Random.Range(5, 10);
            }
        }

    }
    void Grow(int number, bool isClockwise, List<CombineInstance> meshes, Vector3 pos, Quaternion rot, float scale)
    {

        if (number >= iterations) return;

        CombineInstance inst = new CombineInstance();


        if (usePrism)
        {
            inst.mesh = prismMesh.sharedMesh;
        }
        else
        {
        inst.mesh = pyramidMesh.sharedMesh; // copy the mesh
        }


    

        inst.transform = Matrix4x4.TRS(pos, rot, new Vector3(rotationPerSegment / 53 * scale, scale, rotationPerSegment / 53 * scale));
     

        meshes.Add(inst);


        number++;
        bool shouldFlip = (number % flipEveryNth == 0);
        if (shouldFlip)
        {
            isClockwise = !isClockwise;
            if (isClockwise)
            {
                pos = inst.transform.MultiplyPoint(new Vector3(-.5f, 1, 0));
            }
            else
            {
                pos = inst.transform.MultiplyPoint(new Vector3(+.5f, 1, 0));
            }
            rot *= Quaternion.Euler(0, 0, 180);
        }
        else // if NOT flipping:
        {
            if (isClockwise)
            {
                rot *= Quaternion.Euler(0, 0, -rotationPerSegment);
            }
            else
            {
                rot *= Quaternion.Euler(0, 0, rotationPerSegment);
            }
        }
        if (smallerIterations)
        {
            scale *= .9f;
        }
            Grow(number, isClockwise, meshes, pos, rot, scale);
        if (branches)
        {
            //if next step will flip
            if (number % flipEveryNth == 0 && number != iterations-1)
            {
                if (isClockwise)
                {
                    pos = inst.transform.MultiplyPoint(new Vector3(0, 1, 0));
                }
                else
                {
                    pos = inst.transform.MultiplyPoint(new Vector3(0, 1, 0));
                }
                rot *= Quaternion.Euler(0, 0, 180);
                if (smallerIterations)
                {
                    pos += Vector3.forward * scale * scale;
                }
                else
                {
                    pos += Vector3.forward * scale;
                }
                Grow(number, isClockwise, meshes, pos, rot, scale);
            }

        }
    }
}

[CustomEditor(typeof(JoshCoralPyramidSpawn))]
public class SpawnPyramidEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("GROW!"))
        {
            JoshCoralPyramidSpawn b = (target as JoshCoralPyramidSpawn);
            b.Build();
        }
    }
}