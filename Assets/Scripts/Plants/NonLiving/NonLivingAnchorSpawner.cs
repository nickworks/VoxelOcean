using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A control center for the generation of anchors.
/// It generates and colors a complex anchor mesh, which it then instantiates and returns a reference to.
/// Call Generate() to get a anchor.
/// </summary>
public class NonLivingAnchorSpawner : MonoBehaviour
{
    /// <summary>
    /// A reference to the VertexColor material, which will be applied to the mesh upon generation.
    /// </summary>
    public Material mat;

    /// <summary>
    /// The mesh filter which will be used for the output mesh upon generation.
    /// </summary>
    public MeshFilter filter;

    /// <summary>
    /// The mesh renderer which will be used for the output mesh upon generation.
    /// </summary>
    public MeshRenderer meshrenderer;

    /// <summary>
    /// The base for how many iterations of recursive generation should be used.
    /// </summary>
    [Range(5, 15)]public int minBaseIterations = 5;

    /// <summary>
    /// The base for how many iterations of recursive generation should be used.
    /// </summary>
    [Range(5, 15)]public int maxBaseIterations = 15;

    /// <summary>
    /// The starting scale of the first rock, which other rocks/anchors scale down for.
    /// </summary>
    public float baseScale = 5;

    /// <summary>
    /// The internal length of how many local units long every anchor should be.
    /// </summary>
    public float ropeLength = 2;

    /// <summary>
    /// The random seed to be used in generation.
    /// </summary>
    public int seed = 0;

    /// <summary>
    /// A reference to a prefab of the rock, used for easy access to the rock mesh.
    /// </summary>
    public MeshFilter anchorPrefab;

    /// <summary>
    /// A reference to a prefab of the anchor body rope, used for easy access to the rock mesh.
    /// </summary>
    public MeshFilter ropePrefab;

    /// <summary>
    /// The hue of yellow used for the middle of anchor bodies.
    /// </summary>
    Color softYellow = new Color(.8f, .4f, .15f);

    /// <summary>
    /// All of the generated meshes which will be combined into a single final mesh.
    /// </summary>
    private List<CombineInstance> meshes = new List<CombineInstance>();

    /// <summary>
    /// Function which runs once on instantiation.  Kicks off the generation of the mesh.
    /// </summary>
    private void Start()
    {
        Generate();
    }

    /// <summary>
    /// The starting point for actual generation which initilizes the recursive loop.
    /// </summary>
    /// <returns>The resulting game object to be placed in the scene.</returns>
    public void Generate()
    {
        meshes.Clear();
        seed = Random.Range(0, 255);
        Random.InitState(seed);

        SpawnAnchor(Random.Range(minBaseIterations, maxBaseIterations), Vector3.zero, Quaternion.identity, baseScale);

        Mesh mesh = new Mesh();
        mesh.CombineMeshes(meshes.ToArray(), true);

        filter.mesh = mesh;
        meshrenderer.material = mat;

        transform.localScale *= .25f;
    }
    /// <summary>
    /// Generate a anchor.
    /// </summary>
    /// <param name="iter">The iteration of the loop, used as an escape condition.</param>
    /// <param name="pos">The position to spawn this mesh at.</param>
    /// <param name="rot">The rotation to spawn this mesh at.</param>
    /// <param name="scale">The scale to spawn this mesh at.</param>
    void SpawnAnchor(int iter, Vector3 pos, Quaternion rot, float scale)
    {
        iter--;

        //Make the rock mesh.
        CombineInstance anchor = new CombineInstance();
        anchor.mesh = anchorPrefab.sharedMesh;
        anchor.transform = Matrix4x4.TRS(pos, rot, Vector3.one * scale);

        Vector3[] verts = anchor.mesh.vertices;
        Color[] colors = new Color[verts.Length];

        //Set up the rock's colors, yellow on top and black on the bottom.
        for (int i = 0; i < verts.Length; i++)
        {
            colors[i] = Color.Lerp(Color.black, softYellow, verts[i].y / 100);
            colors[i].a = 0;
        }
        anchor.mesh.colors = colors;

        meshes.Add(anchor);

        pos = anchor.transform.MultiplyPoint(new Vector3(0, 90, 0));
        SpawnRope(iter, pos, rot, scale, iter);
    }
    /// <summary>
    /// Spawn a segment of rope.
    /// </summary>
    /// <param name="iter">The iteration of the loop, used as an escape condition.</param>
    /// <param name="pos">The position to spawn this mesh at.</param>
    /// <param name="rot">The rotation to spawn this mesh at.</param>
    /// <param name="scale">The scale to spawn this mesh at.</param>
    /// <param name="baseIter">The original iteration when the base of this anchor was first made.</param>
    void SpawnRope(int iter, Vector3 pos, Quaternion rot, float scale, int baseIter)
    {

        iter--;

        //make the rope mesh
        CombineInstance rope = new CombineInstance();
        rope.mesh = Instantiate(ropePrefab.sharedMesh);
        rope.transform = Matrix4x4.TRS(pos, rot, new Vector3(scale, scale * ropeLength, scale));

        //make the color for the anchor
        //nested lerps make it fade from grey, to yellow, to white in a three color gradient
        Vector3[] verts = rope.mesh.vertices;
        Color[] colors = new Color[verts.Length];

        float lerpByIter = 1 - ((float)iter + 1) / (float)baseIter;
        float plerpByIter = 1 - ((float)iter + 2) / (float)baseIter;

        Color startColor1 = Color.Lerp(Color.grey, softYellow, lerpByIter);
        Color startColor2 = Color.Lerp(softYellow, softYellow, lerpByIter);
        Color startFinalColor = Color.Lerp(startColor1, startColor2, lerpByIter);

        Color endColor1 = Color.Lerp(Color.grey, softYellow, plerpByIter);
        Color endColor2 = Color.Lerp(softYellow, softYellow, plerpByIter);
        Color endFinalColor = Color.Lerp(endColor1, endColor2, plerpByIter);
        

        for (int i = 0; i < verts.Length; i++)
        {
            colors[i] = Color.Lerp(endFinalColor, startFinalColor, verts[i].y);
            colors[i].a = (baseIter - (iter - verts[i].y / 2)) / baseIter;
        }
        rope.mesh.colors = colors;

        meshes.Add(rope);


        //recursively spawn additional rope segments, and cap the body off with a head if ready.

        if (iter >= 0)
        {
            rot.eulerAngles = new Vector3(
                rot.eulerAngles.x + Random.Range(-5, 5),
                rot.eulerAngles.y + Random.Range(-5, 5),
                rot.eulerAngles.z + Random.Range(-5, 5));

            
            rot.eulerAngles = new Vector3(
                rot.eulerAngles.x + Random.Range(-5, 5),
                rot.eulerAngles.y + Random.Range(-5, 5),
                rot.eulerAngles.z + Random.Range(-5, 5));
            pos = rope.transform.MultiplyPoint(new Vector3(0, 2, 0));
            SpawnRope(iter, pos, rot, scale, baseIter);
            
        }
    }
}
