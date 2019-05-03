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

    public Mesh leafMesh;

    [Tooltip("Uncheck this to turn off randomness or use custom seed.")]
    public bool randomSeed = true;
    [Tooltip("Current Seed")]
    public int seed;


    // Start is called before the first frame update
    void Start()
    {
        Build();
        Debug.Log("did a thing");
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

        float offset = numberOfSegments / 4 * segmentScale;
       //   Vector3 pos = new Vector3(0, offset, offset);
        Vector3 pos = Vector3.zero;

        GrowBody(true, numberOfSegments, meshes, pos, Quaternion.Euler(-90, 0, 0), segmentScale, false, stems);
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
 ;
        Vector3 stemRotation = new Vector3();

        if (switchDirection)
        {//top side
            rot = Quaternion.Euler(-90, 0, 0);
            if (!firstSegment) stemRotation.Set(-45, 0, 0);
        }
        else
        {//bottom side
            rot = Quaternion.Euler(-180, 0, 0);
            if (!firstSegment) stemRotation.Set(135, 0, 0);
        }
        if (!firstSegment)
        {
            int numberOfBranches = 1;
            if (Random.Range(0, 100) < chanceForMoreStems)
            {
                numberOfBranches += 1;

                stemRotation = new Vector3(stemRotation.x, stemRotation.y, -45);
                GrowStem(stemSize, numberOfBranches, stems, pos, Quaternion.Euler(stemRotation), scale, switchDirection);
                stemRotation = new Vector3(stemRotation.x, stemRotation.y, 45);
            }
            GrowStem(stemSize,numberOfBranches, stems, pos, Quaternion.Euler(stemRotation), scale, switchDirection);
        }

        switchDirection = !switchDirection;

        pos = inst.transform.MultiplyPoint(new Vector3(0, 1, 0));

        //de increment iterations
        number--;

        //grow another segment
        GrowBody(false, number, meshes, pos, rot, scale, switchDirection, stems);
    }
    void GrowStem(int number, int numberOfStems, List<CombineInstance> meshes, Vector3 pos, Quaternion rot, float scale, bool top)
    {
        CombineInstance inst = new CombineInstance();

        inst.mesh = MeshTools.MakePentagonalCylinder();
        inst.transform = Matrix4x4.TRS(pos, rot, new Vector3(.2f, number, .2f));

        meshes.Add(inst);

        //rot *= Quaternion.Euler(1, 1, rot.z - 90);

        //rot = Quaternion.Euler(225, 0, 0);
        //if(numberOfStems == 2) rot *= Quaternion.Euler(1, 0, 45);

        rot *= Quaternion.Euler(0, 180, 90);

        if (!top)
        {
            rot *= Quaternion.Euler(180, 0, 0);
        }
       
        
        float x = .1f;
        while (x < 1)
        {
            if(x * 2 < 1){
            x = Random.Range(2*x, 1);

            }
            else{
                break;
            }
            pos = inst.transform.MultiplyPoint(Vector3.up * x);
            scale = 1 + (1 - x) * 3;

            GrowLeaf(1, meshes, pos, rot, scale);
        }

    }
    void GrowLeaf(int numberOfLeaves, List<CombineInstance> meshes, Vector3 pos, Quaternion rot, float scale)
    {
        CombineInstance inst = new CombineInstance();
        inst.mesh = leafMesh;

        inst.transform = Matrix4x4.TRS(pos, rot, new Vector3(scale, scale, scale));

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