using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CoralPyramidSpawn : MonoBehaviour
{

    //the meshes we're going to use for generation
    public Mesh pyramidMesh;
    public Mesh prismMesh;

    //parameters to tweak
    [Tooltip("How many iterations to have.")]
    [Range(1, 20)] public int iterations = 5;
    [Tooltip("How often the mesh will reverse direction.")]
    [Range(1, 7)] public int flipEveryNth = 3;
    [Tooltip("How far each segment will rotate.")]
    public float rotationPerSegment = 63.4f;
    [Tooltip("Whether to make each subsequent segment smaller.")]
    public bool smallerIterations = true;
    [Tooltip("Leave this on to use Prism mesh, else it will use Pyramid.")]
    public bool usePrism = true;
    [Tooltip("Whether additional branches can spawn or not.")]
    public bool branches = true;

    [Tooltip("Uncheck this to turn off randomness or use custom seed.")]
    public bool randomVars = true;
    [Tooltip("Current Seed.")]
    public int seed;

    /// <summary>
    /// Instantiates the build
    /// </summary>
    void Start()
    {
        Build();
     //   Debug.Log("building for josh");
    }

    /// <summary>
    /// Thsi function initiates the iterative building process.
    /// Creates a list for mesh storage
    /// and generates the final mesh from the list.
    /// </summary>
    public void Build()
    {
        //randomize seed
        if (randomVars)
        {
            seed = Random.Range(0, 50000000);
        }
        Random.InitState(seed);

        //use random variables if boolean is set
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
    /// <summary>
    /// This function randomizes the base parameters to permit various spawning patterns
    /// </summary>
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
    /// <summary>
    /// This is where the core of the iterative spawning
    /// </summary>
    /// <param name="number">recursive avlue for how many pieces left to grow.</param>
    /// <param name="isClockwise">bool for current orientation.</param>
    /// <param name="meshes">Reverence to the list of meshes</param>
    /// <param name="pos">the position to instantiate the current segment at</param>
    /// <param name="rot">the rotation to instantiate the current segment at</param>
    /// <param name="scale">the scale to instantiate the current segment at</param>
    void Grow(int number, bool isClockwise, List<CombineInstance> meshes, Vector3 pos, Quaternion rot, float scale)
    {
        //TODO: pick up from here
        if (number >= iterations) return;

        CombineInstance inst = new CombineInstance();


        if (usePrism)
        {
            inst.mesh = prismMesh; //use prism mesh
        }
        else
        {
        inst.mesh = pyramidMesh; // use pyramid mesh
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
/// <summary>
/// This provides the grow button for testing in editor
/// </summary>
[CustomEditor(typeof(CoralPyramidSpawn))]
public class SpawnPyramidEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("GROW!"))
        {
            CoralPyramidSpawn b = (target as CoralPyramidSpawn);
            b.Build();
        }
    }
}