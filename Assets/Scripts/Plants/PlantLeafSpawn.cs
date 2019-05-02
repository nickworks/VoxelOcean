using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Spawns flower-like plants by combining two separate meshes
/// </summary>
public class PlantLeafSpawn : MonoBehaviour
{
    //references to the petal and stem meshes filters we will use later.
    public MeshFilter leafPrefab;
    public MeshFilter stemPrefab;

    //parameters to tweak
    [Tooltip("How many iterations to have.")]
    [Range(1, 15)] public int iterations = 2;
    [Tooltip("Scale of the plant.")]
    public float scale = 1;
    [Tooltip("How likely to spawn additional branches.")]
    [Range(0,100)] public float chanceForBranches = 20;
    [Tooltip("How far the leaves can be offset.")]
    [Range(0,30)] public int leafVariance = 5;

    [Tooltip("The angle the plant curves at")]
    [Range(0,20)]public int stemAngle = 5;
    [Tooltip("Average Number of leaves per branch.")]
    public int numberOfLeaves = 3;
    [Tooltip("The rate the plant shrinks each iteration.")]
    float scaleDownRate = .9f;

    //used for color smoothing
    Color colorOffset;

    [Tooltip("Check this to have random variables every spawn.")]
    public bool randomVars = true;
    [Tooltip("Uncheck this to turn off randomness or use custom seed.")]
    public bool randomSeed = true;
    [Tooltip("Current Seed")]    
    public int seed;

    /// <summary>
    /// Instantiates the build
    /// </summary>
    void Start()
    {
        Build();
          // Debug.Log("building for josh");
    }
    /// <summary>
    /// This function instantiates the iterative process
    /// creates 2 lists for mesh storage
    /// then changes the colors of 1 of the lists
    /// and composits the lists together to form into the final mesh.
    /// </summary>
    public void Build()
    {
        //this is used to make the color gradient smooth between white and yellow
        colorOffset = Color.yellow / iterations;

        //randomize seed
        if (randomSeed)
        {
            seed = Random.Range(0, 50000000);
        }
        Random.InitState(seed);

        if (randomVars){
            Randomize();
        }

        //get references to the meshes that will be used later
        List<CombineInstance> meshes = new List<CombineInstance>();
        List<CombineInstance> leaves = new List<CombineInstance>();   

        Grow(iterations, meshes, Vector3.zero, Quaternion.identity, scale, leaves,true);

        //this is test code that didn't work, but I want to try and refactor it later
        //TODO: Refactor/test
        #region
        /*
        //Vector3[] verts = meshes.vertices;
        List<Vector3[]> verts = new List<Vector3[]>();
    
        for (int i = 0; i < meshes.Count ; i++)
        {
            verts.Add(meshes[i].mesh.vertices);
        }
    
        Color[] colors = new Color[verts[1].Length];
    
        for (int i = 0; i < verts.Length; i++)
        {
            colors[i] = Color.Lerp(Color.white, Color.red, verts[i].y * 2f);
        }
        meshes.colors = colors;
        meshes.AddRange(leaves);
        */
        #endregion

        //make a new mesh and convert the meshes list to a mesh
        Mesh mesh = new Mesh();
        mesh.CombineMeshes(meshes.ToArray(),true);

        //modify the color of the mesh (stems only)
        Vector3[] verts = mesh.vertices;
        Color[] colors = new Color[verts.Length];

        for (int i = 0; i < verts.Length; i++)
        {
            //lerp between colors as you get farther into the mesh from the base
            colors[i] = Color.Lerp(Color.white, Color.yellow, verts[i].y*.5f);
        }
        mesh.colors = colors;

        //convert the mesh into a new combine instance
        CombineInstance test = new CombineInstance();
        test.transform = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, transform.localScale);
        test.mesh = mesh;

        //put the now colored stem mesh (combineinstance) onto the end of the leaves list
        leaves.Add(test);

        Mesh testMesh = new Mesh();

        //now add all the meshes into the final mesh
        testMesh.CombineMeshes(leaves.ToArray(), true);

        //assign final mesh
        MeshFilter filter = GetComponent<MeshFilter>();
        filter.mesh = testMesh;
    }
    /// <summary>
    /// This function randomizes the base parameters to permit various spawning patterns
    /// </summary>
    void Randomize()
    {
        //bell curve for more in the middle
        iterations = Random.Range(2,4) + Random.Range(2,4);
        scale = Random.Range(.5f, 7);
        chanceForBranches = Random.Range(5, 20) + Random.Range(5, 20) + (int) scale/2;
        numberOfLeaves = Random.Range(3, 5);       
        leafVariance = Random.Range(10, 25);
        stemAngle = Random.Range(1, 5) + Random.Range(1, 5);
    }
    /// <summary>
    /// used to grow the core of the plant recursively
    /// </summary>
    /// <param name="number">recursive value for how many pieces left to grow.</param>
    /// <param name="meshes">the list of meshes we add new segments to.</param>
    /// <param name="pos">the position to instantiate the current segment at.</param>
    /// <param name="rot">the rotation to instantiate the current segment at.</param>
    /// <param name="scale">the scale to instantiate the current segment at.</param>
    /// <param name="leaves">reference to the list of leaves, used to make sure it's not lost to cleanup.</param>
    /// <param name="sprout">if the current segment is the first one in a new branch, we won't spawn leaves because they clip and it looks bad.</param>
    void Grow(int number, List<CombineInstance> meshes, Vector3 pos, Quaternion rot, float scale, List<CombineInstance> leaves, bool sprout)
    {
        //how to break out of the recursive functoin
        if (number <= 0) return;

        CombineInstance inst = new CombineInstance();

       //grab stem mesh
        inst.mesh = stemPrefab.sharedMesh;        
        inst.transform = Matrix4x4.TRS(pos, rot, Vector3.one * scale);
        

        meshes.Add(inst);

        //growing leaves
        //float angleOffset = 360 / numberOfLeaves;        
        if (numberOfLeaves > 0 && !sprout)
        {
            int iterationLeaves = numberOfLeaves+ Random.Range(-1, 1);            

          for (int i = 1; i <= iterationLeaves ; i++)
          {
            Quaternion newrot = rot * Quaternion.Euler(Random.Range(20, 30), 360 / i + Random.Range(-leafVariance, leafVariance), Random.Range(20, 30));
                 GrowLeaf(leaves, pos,newrot,scale);
            }
        }

        //set rotation and next position for next iteration
        rot *= Quaternion.Euler(stemAngle+Random.value, 0, stemAngle+Random.value);
        //pos = inst.transform.MultiplyPoint(new Vector3(0, .985f*scale, 0));
        pos = inst.transform.MultiplyPoint(new Vector3(0, 1, 0));
        //scale down number and scale
        number--;
        scale *= scaleDownRate;

        
        //grow another stem
        Grow(number, meshes, pos, rot, scale,leaves,false);

        //try to grow another branch
        if(Random.Range(0,100) < chanceForBranches)
        {
            pos = inst.transform.MultiplyPoint(Vector3.up * Random.Range(0, 1));
            rot = Quaternion.Euler(rot.eulerAngles - new Vector3(0, Random.Range(rot.eulerAngles.y-300,rot.eulerAngles.y-30), 0));
            number = Random.Range(2, number);
            Grow(number, meshes, pos, rot, scale, leaves,true);
        }
    }

   /// <summary>
   /// This function instantiates a leaf
   /// </summary>
   /// <param name="leaves">Reference to the list of leaves this leaf will be added to</param>
   /// <param name="pos">Position to instantiate the leaf at</param>
   /// <param name="rot">Rotation to instantiate the leaf at</param>
   /// <param name="scale">Scale to instantiate the leaf at</param>
    void GrowLeaf(List<CombineInstance> leaves, Vector3 pos, Quaternion rot, float scale)
    {
        CombineInstance inst = new CombineInstance();
        //grab petal mesh
        inst.mesh = leafPrefab.sharedMesh;
        pos += rot * Vector3.forward*-.4f*scale;
        //pos += Vector3.up * Random.Range(0,scale);
        inst.transform = Matrix4x4.TRS(pos, rot, Vector3.one* scale);

        //colors the leaf along it's length
        Vector3[] verts = inst.mesh.vertices;
        Color[] colors = new Color[verts.Length];
        for (int i = 0; i < verts.Length ; i++)
        {
            colors[i] = Color.Lerp(Color.blue, new Color(255,0,255), verts[i].y);
        }
        inst.mesh.colors = colors;
        leaves.Add(inst);
    }
}

/// <summary>
/// The grow button to test this code in the editor
/// </summary>
[CustomEditor(typeof(PlantLeafSpawn))]
public class SpawnFlowerCoralEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("GROW!"))
        {
            PlantLeafSpawn b = (target as PlantLeafSpawn);
            
            b.Build();
        }
    }
}
