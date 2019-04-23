using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A control center for the generation of drifting kelp.
/// It generates and colors a complex drifting kelp mesh, which it then instantiates and returns a reference to.
/// Call Generate() to get a drifting kelp.
/// </summary>
public class PlantDriftingKelp : MonoBehaviour
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
    public int baseIterations = 14;

    /// <summary>
    /// The starting scale of the first mush, which other mush/stalks scale down for.
    /// </summary>
    public float baseScale = 5;

    /// <summary>
    /// The rate at which rocks scale down.  Each iteration is multiplied by this float.
    /// </summary>
    public float scaleRate = .9f;

    /// <summary>
    /// The size which a new stalk should be in comparison to its source mush.  drifting kelps are a fixed scale and do not scale up or down.
    /// </summary>
    public float tubeWormScale = .25f;

    /// <summary>
    /// The chance that a new stalk will spawn in place of a rock every iteration.
    /// </summary>
    public float tubeWormSpawnRate = .25f;

    /// <summary>
    /// What percent of the way through iterations the rock generation needs to be before it can start making drifting kelps.
    /// </summary>
    public float tubeWormSpawnDelay = .5f;

    /// <summary>
    /// The odds that a drifting kelp will break off into a double-headed stalk.
    /// </summary>
    public float tubeWormBranchChance = .05f;

    /// <summary>
    /// The internal length of how many local units long every drifting kelp should be.
    /// </summary>
    public float tubeWormSegmentLength = 2;

    /// <summary>
    /// The minimum possible ammount of leaves.
    /// </summary>
    public float leafSpawnMin = 2;

    /// <summary>
    /// The maximum possible ammount of leaves.
    /// </summary>
    public float leafSpawnMax = 10;

    /// <summary>
    /// The variability of leaf sizes.
    /// </summary>
    public float leafSizeVar = 3;

    /// <summary>
    /// How much extra length should be added to the drifting kelp on top of the base iteration count recieved from the parent rock.
    /// </summary>
    public int wormBonusIter = 0;
    /// <summary>
    /// The random seed to be used in generation.
    /// </summary>
    public int seed = 0;

    /// <summary>
    /// A reference to a prefab of the mush at the base of the floating kelp, used for easy access to the mush mesh.
    /// </summary>
    public MeshFilter mushPrefab;

    /// <summary>
    /// A reference to a prefab of the kelp leaf, used for easy access to the leaf mesh.
    /// </summary>
    public MeshFilter leafPrefab;

    /// <summary>
    /// A reference to a prefab of the kelp stalk, used for easy access to the stalk mesh.
    /// </summary>
    public MeshFilter stalkPrefab;

    /// <summary>
    /// The hue of yellow used for the middle of drifting kelp bodies.
    /// </summary>
    Color softYellow = new Color(.5f, .5f, .1f);

    /// <summary>
    /// The hue of yellow used for the middle of drifting kelp bodies.
    /// </summary>
    Color softGreen = new Color(.25f, .5f, .1f);

    /// <summary>
    /// The hue of yellow used for the middle of drifting kelp bodies.
    /// </summary>
    Color darkGreen = new Color(.05f, .1f, .01f);

    /// <summary>
    /// The starting position of this drifting kelp.  it will always slowly drift back towards where it started.
    /// </summary>
    private Vector3 startPos = new Vector3();

    /// <summary>
    /// The velocity this drifting kelp.
    /// </summary>
    private Vector3 veloc = new Vector3();

    /// <summary>
    /// The starting speed of the kelp.
    /// </summary>
    public float driftSpeed = 10;

    /// <summary>
    /// The max speed of the kelp.
    /// </summary>
    public float maxSpeed = 1;

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


        startPos = new Vector3(transform.position.x, transform.position.y + 10, transform.position.z);
        transform.position = new Vector3(
            transform.position.x + Random.Range(-5, 5), 
            transform.position.y + 10 + Random.Range(-.5f, .5f), 
            transform.position.z + Random.Range(-5, 5));

        veloc = new Vector3(
            transform.position.x + Random.Range(-.05f, .05f), 
            transform.position.y + 10 + Random.Range(-.01f, .01f), 
            transform.position.z + Random.Range(-.05f, .05f));
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

        SpawnMush(baseIterations, Vector3.zero, Quaternion.identity, baseScale);

        Mesh mesh = new Mesh();
        mesh.CombineMeshes(meshes.ToArray(), true);

        filter.mesh = mesh;
        meshrenderer.material = mat;

        transform.localScale = new Vector3(transform.localScale.x * .05f, transform.localScale.y * -.05f, transform.localScale.z * .05f);
    }
    /// <summary>
    /// Generate a chunk of mush, which will then either recursively create more mush or create a coral stalk.
    /// </summary>
    /// <param name="iter">The iteration of the loop, used as an escape condition.</param>
    /// <param name="pos">The position to spawn this mesh at.</param>
    /// <param name="rot">The rotation to spawn this mesh at.</param>
    /// <param name="scale">The scale to spawn this mesh at.</param>
    void SpawnMush(int iter, Vector3 pos, Quaternion rot, float scale)
    {
        iter--;

        //Make the rock mesh.
        CombineInstance mush = new CombineInstance();
        mush.mesh = mushPrefab.sharedMesh;
        mush.transform = Matrix4x4.TRS(pos, rot, Vector3.one * scale);

        Vector3[] verts = mush.mesh.vertices;
        Color[] colors = new Color[verts.Length];

        //Set up the rock's colors, yellow on top and black on the bottom.
        for (int i = 0; i < verts.Length; i++)
        {
            colors[i] = Color.Lerp(darkGreen, darkGreen, verts[i].y * verts[i].y * verts[i].y);
        }
        mush.mesh.colors = colors;

        meshes.Add(mush);

        scale *= scaleRate;

        //recursively call for more mush to be made, or for a stalk to be made.
        //update pos, rot, and scale for the next model before calling for them.
        if (iter >= 0)
        {
            for (int i = 0; i < Random.Range(1, 3); i++)
            {
                rot.eulerAngles = new Vector3(
                    rot.eulerAngles.x + Random.Range(-5, 5),
                    rot.eulerAngles.y + Random.Range(-180, 180),
                    rot.eulerAngles.z + Random.Range(-5, 5));
                int rand = (int)Random.Range(0, 6);

                if (iter > (float)baseIterations * tubeWormSpawnDelay || Random.Range(0, 1f) > tubeWormSpawnRate)
                {
                    switch (rand)//make rocks
                    {
                        case 0:
                            pos = mush.transform.MultiplyPoint(new Vector3(0, 1, 0));
                            SpawnMush(iter, pos, rot, scale);
                            break;
                        case 1:
                            pos = mush.transform.MultiplyPoint(new Vector3(1, 0, 0));
                            SpawnMush(iter, pos, rot, scale);
                            break;
                        case 2:
                            pos = mush.transform.MultiplyPoint(new Vector3(-1, 0, 0));
                            SpawnMush(iter, pos, rot, scale);
                            break;
                        case 3:
                            pos = mush.transform.MultiplyPoint(new Vector3(0, 0, 1));
                            SpawnMush(iter, pos, rot, scale);
                            break;
                        case 4:
                            pos = mush.transform.MultiplyPoint(new Vector3(0, 0, -1));
                            SpawnMush(iter, pos, rot, scale);
                            break;
                        case 5:
                            pos = mush.transform.MultiplyPoint(new Vector3(0, -1, 0));
                            SpawnMush(iter, pos, rot, scale);
                            break;
                    }
                }
                else
                {
                    pos = mush.transform.MultiplyPoint(new Vector3(0, 1, 0));
                    SpawnStalk(iter + wormBonusIter, pos, rot, scale * tubeWormScale, iter + wormBonusIter);

                    /*switch (rand)//make drifting kelps
                    {
                        case 0:
                            pos = mush.transform.MultiplyPoint(new Vector3(0, 1, 0));
                            SpawnStalk(iter + wormBonusIter, pos, rot, scale * tubeWormScale, iter + wormBonusIter);
                            break;
                        case 1:
                            pos = mush.transform.MultiplyPoint(new Vector3(1, 0, 0));
                            SpawnStalk(iter + wormBonusIter, pos, rot, scale * tubeWormScale, iter + wormBonusIter);
                            break;
                        case 2:
                            pos = mush.transform.MultiplyPoint(new Vector3(-1, 0, 0));
                            SpawnStalk(iter + wormBonusIter, pos, rot, scale * tubeWormScale, iter + wormBonusIter);
                            break;
                        case 3:
                            pos = mush.transform.MultiplyPoint(new Vector3(0, 0, 1));
                            SpawnStalk(iter + wormBonusIter, pos, rot, scale * tubeWormScale, iter + wormBonusIter);
                            break;
                        case 4:
                            pos = mush.transform.MultiplyPoint(new Vector3(0, 0, -1));
                            SpawnStalk(iter + wormBonusIter, pos, rot, scale * tubeWormScale, iter + wormBonusIter);
                            break;
                    }*/
                }

            }
        }
    }
    /// <summary>
    /// Spawn a segment of the body of the kelp.
    /// </summary>
    /// <param name="iter">The iteration of the loop, used as an escape condition.</param>
    /// <param name="pos">The position to spawn this mesh at.</param>
    /// <param name="rot">The rotation to spawn this mesh at.</param>
    /// <param name="scale">The scale to spawn this mesh at.</param>
    /// <param name="baseIter">The original iteration when the base of this stalk was first made.</param>
    /// <param name="previousWorm">The mesh of the stalk segment that spawned this one.</param>
    void SpawnStalk(int iter, Vector3 pos, Quaternion rot, float scale, int baseIter, Mesh previousWorm = null)
    {

        iter--;

        //make the tube mesh
        CombineInstance stalk = new CombineInstance();
        stalk.mesh = Instantiate(stalkPrefab.sharedMesh);
        stalk.transform = Matrix4x4.TRS(pos, rot, new Vector3(scale, scale * tubeWormSegmentLength, scale));

        //make the color for the drifting kelp
        //nested lerps make it fade from grey, to yellow, to white in a three color gradient
        Vector3[] verts = stalk.mesh.vertices;
        Color[] colors = new Color[verts.Length];

        float lerpByIter = 1 - ((float)iter + 1) / (float)baseIter;
        float plerpByIter = 1 - ((float)iter + 2) / (float)baseIter;

        Color startColor1 = Color.Lerp(darkGreen, softYellow, lerpByIter);
        Color startColor2 = Color.Lerp(softYellow, darkGreen, lerpByIter);
        Color startFinalColor = Color.Lerp(startColor1, startColor2, lerpByIter);

        Color endColor1 = Color.Lerp(darkGreen, softYellow, plerpByIter);
        Color endColor2 = Color.Lerp(softYellow, darkGreen, plerpByIter);
        Color endFinalColor = Color.Lerp(endColor1, endColor2, plerpByIter);

        for (int i = 0; i < verts.Length; i++)
        {
            colors[i] = Color.Lerp(endFinalColor, startFinalColor, verts[i].y);

            if (previousWorm != null)
            {
                if (i % 4 == 0)
                {
                    stalk.mesh.vertices[i].x = 0;//reviousWorm.vertices[i % 32 + 3].x;
                    stalk.mesh.vertices[i].y = 0;//reviousWorm.vertices[i % 32 + 3].y;
                    stalk.mesh.vertices[i].z = 0;//previousWorm.vertices[i % 32 + 3].z;
                }
                else if (i % 2 == 0)
                {
                    stalk.mesh.vertices[i].x = 0;//reviousWorm.vertices[i - 1].x;
                    stalk.mesh.vertices[i].y = 0;//reviousWorm.vertices[i - 1].y;
                    stalk.mesh.vertices[i].z = 0;//previousWorm.vertices[i - 1].z;
                }
                stalk.mesh.vertices[i].x = 0;
                stalk.mesh.vertices[i].y = 0;
                stalk.mesh.vertices[i].z = 0;
            }
        }
        stalk.mesh.colors = colors;

        meshes.Add(stalk);


        //recursively spawn additional tube segments, and cap the body off with a head if ready.

        if (iter >= 0)
        {
            rot.eulerAngles = new Vector3(
                rot.eulerAngles.x + Random.Range(-5, 5),
                rot.eulerAngles.y + Random.Range(-5, 5),
                rot.eulerAngles.z + Random.Range(-5, 5));

            if (Random.Range(0, 1f) > tubeWormBranchChance)//spawn more segments
            {
                rot.eulerAngles = new Vector3(
                    rot.eulerAngles.x + Random.Range(-5, 5),
                    rot.eulerAngles.y + Random.Range(-5, 5),
                    rot.eulerAngles.z + Random.Range(-5, 5));
                pos = stalk.transform.MultiplyPoint(new Vector3(0, 2, 0));
                SpawnStalk(iter, pos, rot, scale, baseIter, stalk.mesh);
            }
            else
            {
                pos = stalk.transform.MultiplyPoint(new Vector3(0, 2, 0));
                SpawnMush(baseIter, pos, rot, scale * 2);

                /*rot.eulerAngles = new Vector3(
                    rot.eulerAngles.x + Random.Range(-15, 15),
                    rot.eulerAngles.y + Random.Range(-5, 5),
                    rot.eulerAngles.z + Random.Range(-15, 15));
                pos = stalk.transform.MultiplyPoint(new Vector3(0, 2, 0));
                SpawnStalk(iter, pos, rot, scale, baseIter, stalk.mesh);

                rot.eulerAngles = new Vector3(
                    rot.eulerAngles.x + Random.Range(-15, 15),
                    rot.eulerAngles.y + Random.Range(-5, 5),
                    rot.eulerAngles.z + Random.Range(-15, 15));
                SpawnStalk(iter, pos, rot, scale, baseIter, stalk.mesh);*/
            }


        }
        else //spawn a head
        {
            //pos = stalk.transform.MultiplyPoint(new Vector3(0, 2, 0));
            //SpawnMush(iter, pos, rot, scale * 2);
        }

        for(int i = 0; i < Random.Range(leafSpawnMin, leafSpawnMax); i++)
        {
            pos = stalk.transform.MultiplyPoint(new Vector3(0, Random.Range(-1, 1), 0));
            rot.eulerAngles.Set(0, Random.Range(0,360), 0);
            SpawnLeaf(iter, pos, rot, scale * Random.Range(2, leafSizeVar) * ((Random.Range(0,1f) > .5f)? 1:-1));
        }
    }
    /// <summary>
    /// Spawn in a pre-made leaf piece for the kelp.
    /// </summary>
    /// <param name="iter">The iteration of the loop, used as an escape condition.</param>
    /// <param name="pos">The position to spawn this mesh at.</param>
    /// <param name="rot">The rotation to spawn this mesh at.</param>
    /// <param name="scale">The scale to spawn this mesh at.</param>
    void SpawnLeaf(int iter, Vector3 pos, Quaternion rot, float scale)
    {
        //make the mesh of the leaf head
        CombineInstance leaf = new CombineInstance();
        leaf.mesh = leafPrefab.sharedMesh;
        leaf.transform = Matrix4x4.TRS(pos, rot, Vector3.one * scale);

        //color the leaf head, gradiating from white on the bottom to a read top.
        Vector3[] verts = leaf.mesh.vertices;
        Color[] colors = new Color[verts.Length];

        for (int i = 0; i < verts.Length; i++)
        {
            colors[i] = Color.Lerp(darkGreen, softGreen, verts[i].x);
        }
        leaf.mesh.colors = colors;

        meshes.Add(leaf);
    }

    /// <summary>
    /// Update loop handles basic movement to simulate the kelp as drifting through the ocean.
    /// </summary>
    private void Update()
    {
        Vector3 dir = (startPos - transform.position).normalized;

        veloc += (dir * Time.deltaTime * driftSpeed);// / Vector3.Distance(startPos, transform.position);

        transform.position += veloc * Time.deltaTime;

        if (veloc.sqrMagnitude > maxSpeed * maxSpeed)
        {
            veloc.Normalize();
            veloc *= maxSpeed;
        }
    }
}
