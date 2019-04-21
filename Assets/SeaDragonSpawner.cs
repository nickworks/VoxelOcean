using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SeaDragonSpawner : MonoBehaviour
{
    public int numberOfSegments = 5;
    public float segmentScale = 1;
    public float bodyWidth = .5f;
    [Space]

    public int stemSize = 5;
    public float chanceForMoreStems = 50;

    [Tooltip("Uncheck this to turn off randomness or use custom seed.")]
    public bool randomSeed = true;
    [Tooltip("Current Seed")]
    public int seed;


    // Start is called before the first frame update
    void Start()
    {
        Build();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Build()
    {
        //randomize seed
        if (randomSeed)
        {
            seed = Random.Range(0, 50000000);
        }
        Random.InitState(seed);

        List<CombineInstance> meshes = new List<CombineInstance>();
        List<CombineInstance> stems = new List<CombineInstance>();

        GrowBody(true, numberOfSegments, meshes, Vector3.zero, Quaternion.Euler(90, 0, 0), segmentScale, false, stems);
        Mesh mesh = new Mesh();

        mesh.CombineMeshes(meshes.ToArray());
        //convert mesh into new combine instance
        CombineInstance test = new CombineInstance();
        test.transform = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, transform.localScale);
        test.mesh = mesh;

        stems.Add(test);

        Mesh testMesh = new Mesh();

        testMesh.CombineMeshes(stems.ToArray());

        //TODO: figure out how to remove duplicate verticies
        //MeshTools.RemoveDuplicates(mesh);

        MeshFilter filter = GetComponent<MeshFilter>();
        filter.mesh = testMesh;

    }

    void GrowBody(bool firstSegment, int number, List<CombineInstance> meshes, Vector3 pos, Quaternion rot, float scale, bool switchDirection, List<CombineInstance> stems)
    {
        if (number <= 0) return;

        CombineInstance inst = new CombineInstance();

        inst.mesh = MeshTools.MakePentagonalCylinder();
        inst.transform = Matrix4x4.TRS(pos, rot, new Vector3(bodyWidth, scale, bodyWidth));

        meshes.Add(inst);


        //scale

        //rotation
        //rot *= Quaternion.Euler(0, 90, 0);
        Vector3 stemRotation = new Vector3();
        if (switchDirection)
        {
            rot = Quaternion.Euler(90, 0, 0);
            if (!firstSegment) stemRotation.Set(45, 0, 0);
        }
        else
        {
            rot = Quaternion.Euler(180, 0, 0);
            if (!firstSegment) stemRotation.Set(-135, 0, 0);
        }
        if (!firstSegment)
        {

            if (Random.Range(0, 100) > chanceForMoreStems)
            {
                stemRotation = new Vector3(stemRotation.x, stemRotation.y, -45);
                GrowStem(stemSize, stems, pos, Quaternion.Euler(stemRotation), scale);
                stemRotation = new Vector3(stemRotation.x, stemRotation.y, 45);
            }
            GrowStem(stemSize, stems, pos, Quaternion.Euler(stemRotation), scale);
        }

        switchDirection = !switchDirection;

        pos = inst.transform.MultiplyPoint(new Vector3(0, 1, 0));

        //de increment iterations
        number--;

        //grow another segment
        GrowBody(false, number, meshes, pos, rot, scale, switchDirection, stems);
    }
    void GrowStem(int number, List<CombineInstance> meshes, Vector3 pos, Quaternion rot, float scale)
    {
        CombineInstance inst = new CombineInstance();

        inst.mesh = MeshTools.MakePentagonalCylinder();
        inst.transform = Matrix4x4.TRS(pos, rot, new Vector3(.2f, number, .2f));

        meshes.Add(inst);

    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(SeaDragonSpawner))]
public class SeaDragonSpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("ARISE!!"))
        {
            SeaDragonSpawner s = (target as SeaDragonSpawner);

            s.Build();
        }
    }
}
#endif