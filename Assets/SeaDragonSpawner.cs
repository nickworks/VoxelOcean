using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SeaDragonSpawner : MonoBehaviour
{
    [Tooltip("How many segments to grow")]
    public int numberOfSegments = 5;
    [Tooltip("How big is each segment")]
    public float segmentScale = 1;
    [Tooltip("Width of the body segments")]
    public float bodyWidth = 2f;
    [Tooltip("Scale of the creature")]
    public float scale = 1;

    [Space]

    [Tooltip("How big should the stems be?")]
    public int stemSize = 5;
    [Tooltip("How likely to grow a second stem")]
    public float chanceForMoreStems = 50;

    
    [Tooltip("Reference to SeaDragonLeaf Mesh")]
    public Mesh leafMesh;

    [Tooltip("Uncheck this to turn off randomness or use custom seed")]
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
    /// <summary>
    /// function that initiates generation of the body of the creature
    /// then combines the meshes into one
    /// </summary>
    public void Build()
    {        
        //randomize seed
        if (randomSeed)
        {
            seed = Random.Range(0, 50000000);
        }
        Random.InitState(seed);
        Randomize();

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
        // TODO: Apply colors of chunk
        stems.Add(test);

        Mesh testMesh = new Mesh();

        testMesh.CombineMeshes(stems.ToArray());

        //TODO: figure out how to remove duplicate verticies
        //MeshTools.RemoveDuplicates(mesh);

        MeshFilter filter = GetComponent<MeshFilter>();
        filter.mesh = testMesh;

    }
    /// <summary>
    /// randomize parameters for the creatuer
    /// </summary>
    void Randomize()
    {
        numberOfSegments = Random.Range(4, 8);
        segmentScale = Random.Range(.25f, 4)+2f;
        stemSize = Random.Range(3, 8);
        chanceForMoreStems = Random.Range(30, 80);
        scale = Random.Range(.2f, .7f);
    }
    /// <summary>
    /// Primary recursive function used to grow segments of the body, and attached stems
    /// </summary>
    /// <param name="firstSegment">special modifier for the 'head' or first segment of the body</param>
    /// <param name="number">number of segments to grow, counts down to 0</param>
    /// <param name="meshes">reference to list of combine instances</param>
    /// <param name="pos">where to spawn the segment</param>
    /// <param name="rot">rotation of the segment</param>
    /// <param name="bodyScale">how much to scale each segment</param>
    /// <param name="switchDirection">used to change direction the segment grows in</param>
    /// <param name="stems">reference to list of combine instances for stems and leaves</param>
    void GrowBody(bool firstSegment, int number, List<CombineInstance> meshes, Vector3 pos, Quaternion rot, float bodyScale, bool switchDirection, List<CombineInstance> stems)
    {
        if (number <= 0) return;

        CombineInstance inst = new CombineInstance();

        inst.mesh = MeshTools.MakeCylinder(5);
        inst.transform = Matrix4x4.TRS(pos, rot, new Vector3(bodyWidth, bodyScale, bodyWidth)*scale);

        meshes.Add(inst);


        //scale

        //rotation
 ;
        Vector3 stemRotation = new Vector3();

        //calculate which way to grow the next segment
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
        //don't spawn stems from the first segment
        if (!firstSegment)
        {

            //random chance for 2 stems
            if (Random.Range(0, 100) < chanceForMoreStems)
            {
                stemRotation = new Vector3(stemRotation.x, stemRotation.y, -45);
                GrowStem(stemSize, stems, pos, Quaternion.Euler(stemRotation), switchDirection);
                stemRotation = new Vector3(stemRotation.x, stemRotation.y, 45);
            }
            GrowStem(stemSize, stems, pos, Quaternion.Euler(stemRotation), switchDirection);
        }

        switchDirection = !switchDirection;

        pos = inst.transform.MultiplyPoint(new Vector3(0, 1, 0));

        //de increment iterations
        number--;

        //grow another segment
        GrowBody(false, number, meshes, pos, rot, bodyScale, switchDirection, stems);
    }
    /// <summary>
    /// creates a stem, growing from a position
    /// </summary>
    /// <param name="number">how long should the stem be</param>
    /// <param name="meshes">reference to list of meshes</param>
    /// <param name="pos">where to start the stem</param>
    /// <param name="rot">rotation of the stem</param>
    /// <param name="top">whether or not the stem is on top of the creature, used to calculate leaf direction</param>
    void GrowStem(int number, List<CombineInstance> meshes, Vector3 pos, Quaternion rot, bool top)
    {
        CombineInstance inst = new CombineInstance();

        inst.mesh = MeshTools.MakePentagonalCylinder();
        inst.transform = Matrix4x4.TRS(pos, rot, new Vector3(.2f, number, .2f));

        meshes.Add(inst);

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
           float scale = 1 + (1 - x) * 3;

            GrowLeaf(1, meshes, pos, rot, scale);
        }

    }
    /// <summary>
    /// creates a leaf at the target location
    /// </summary>
    /// <param name="numberOfLeaves">how many leaves to grow</param>
    /// <param name="meshes">reference to combine instance list</param>
    /// <param name="pos">where to spawn the leaf</param>
    /// <param name="rot">rotation of the leaf</param>
    /// <param name="leafScale">how big is the leaf</param>
    void GrowLeaf(int numberOfLeaves, List<CombineInstance> meshes, Vector3 pos, Quaternion rot, float leafScale)
    {
        CombineInstance inst = new CombineInstance();
        inst.mesh = leafMesh;

        inst.transform = Matrix4x4.TRS(pos, rot, Vector3.one * leafScale*scale);

        meshes.Add(inst);        
    }
}
/// <summary>
/// Custom button for in editor testing
/// </summary>
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