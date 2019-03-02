using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A control center for the generation of tube worms.
/// It generates and colors a complex tube worm mesh, which it then instantiates and returns a reference to.
/// Call Generate() to get a tube worm.
/// </summary>
public class Coral_TubeWormSpawner : MonoBehaviour
{
    /// <summary>
    /// A reference to the VertexColor material, which will be applied to the mesh upon generation.
    /// </summary>
    public Material mat;

    /// <summary>
    /// The base for how many iterations of recursive generation should be used.
    /// </summary>
    public int baseIterations = 14;

    /// <summary>
    /// The starting scale of the first rock, which other rocks/worms scale down for.
    /// </summary>
    public float baseScale = 5;

    /// <summary>
    /// The rate at which rocks scale down.  Each iteration is multiplied by this float.
    /// </summary>
    public float scaleRate = .9f;

    /// <summary>
    /// The size which a new tubworm should be in comparison to its source rock.  Tube worms are a fixed scale and do not scale up or down.
    /// </summary>
    public float tubeWormScale = .25f;

    /// <summary>
    /// The chance that a new tubworm will spawn in place of a rock every iteration.
    /// </summary>
    public float tubeWormSpawnRate = .25f;

    /// <summary>
    /// What percent of the way through iterations the rock generation needs to be before it can start making tube worms.
    /// </summary>
    public float tubeWormSpawnDelay = .5f;

    /// <summary>
    /// The odds that a tube worm will break off into a double-headed worm.  Base is 5% every segment.
    /// </summary>
    public float tubeWormBranchChance = .05f;

    /// <summary>
    /// The internal length of how many local units long every tube worm should be.
    /// </summary>
    public float tubeWormSegmentLength = 2;

    /// <summary>
    /// How much extra length should be added to the tube worm on top of the base iteration count recieved from the parent rock.
    /// </summary>
    public int wormBonusIter = 0;
    /// <summary>
    /// The random seed to be used in generation.
    /// </summary>
    public int seed = 0;

    /// <summary>
    /// A reference to a prefab of the rock, used for easy access to the rock mesh.
    /// </summary>
    public MeshFilter rockPrefab;

    /// <summary>
    /// A reference to a prefab of the worm head, used for easy access to the rock mesh.
    /// </summary>
    public MeshFilter wormPrefab;

    /// <summary>
    /// A reference to a prefab of the worm body tube, used for easy access to the rock mesh.
    /// </summary>
    public MeshFilter tubePrefab;

    /// <summary>
    /// The hue of yellow used for the middle of tube worm bodies.
    /// </summary>
    Color softYellow = new Color(.9f, .9f, .75f);

    /// <summary>
    /// All of the generated meshes which will be combined into a single final mesh.
    /// </summary>
    private List<CombineInstance> meshes = new List<CombineInstance>();
    /// <summary>
    /// The starting point for actual generation which initilizes the recursive loop.
    /// </summary>
    /// <returns>The resulting game object to be placed in the scene.</returns>
    public GameObject Generate()
    {
        meshes.Clear();
        seed = Random.Range(0, 255);
        Random.InitState(seed);

        SpawnRock(baseIterations, Vector3.zero, Quaternion.identity, baseScale);

        Mesh mesh = new Mesh();
        mesh.CombineMeshes(meshes.ToArray(), true);

        GameObject ob = new GameObject();
        ob.AddComponent<MeshFilter>().mesh = mesh;
        ob.AddComponent<MeshRenderer>().material = mat;

        return ob;
    }
    /// <summary>
    /// Generate a rock, which will then either recursively create more rocks or create a tube worm.
    /// </summary>
    /// <param name="iter">The iteration of the loop, used as an escape condition.</param>
    /// <param name="pos">The position to spawn this mesh at.</param>
    /// <param name="rot">The rotation to spawn this mesh at.</param>
    /// <param name="scale">The scale to spawn this mesh at.</param>
    void SpawnRock(int iter, Vector3 pos, Quaternion rot, float scale)
    {
        iter--;

        //Make the rock mesh.
        CombineInstance rock = new CombineInstance();
        rock.mesh = rockPrefab.sharedMesh;
        rock.transform = Matrix4x4.TRS(pos, rot, Vector3.one * scale);

        Vector3[] verts = rock.mesh.vertices;
        Color[] colors = new Color[verts.Length];

        //Set up the rock's colors, yellow on top and black on the bottom.
        for (int i = 0; i < verts.Length; i++)
        {
            colors[i] = Color.Lerp(Color.black, softYellow, verts[i].y * verts[i].y * verts[i].y);
        }
        rock.mesh.colors = colors;

        meshes.Add(rock);

        scale *= scaleRate;

        //recursively call for more rocks to be made, or for a tubeworm to be made.
        //update pos, rot, and scale for the next model before calling for them.
        if (iter >= 0)
        {
            for (int i = 0; i < Random.Range(1, 3); i++)
            {
                rot.eulerAngles = new Vector3(
                    rot.eulerAngles.x + Random.Range(-5, 5),
                    rot.eulerAngles.y + Random.Range(-180, 180),
                    rot.eulerAngles.z + Random.Range(-5, 5));
                int rand = (int)Random.Range(0, 5);

                if (iter > (float)baseIterations * tubeWormSpawnDelay || Random.Range(0, 1f) > tubeWormSpawnRate)
                {
                    switch (rand)//make rocks
                    {
                        case 0:
                            pos = rock.transform.MultiplyPoint(new Vector3(0, 1, 0));
                            SpawnRock(iter, pos, rot, scale);
                            break;
                        case 1:
                            pos = rock.transform.MultiplyPoint(new Vector3(1, 0, 0));
                            SpawnRock(iter, pos, rot, scale);
                            break;
                        case 2:
                            pos = rock.transform.MultiplyPoint(new Vector3(-1, 0, 0));
                            SpawnRock(iter, pos, rot, scale);
                            break;
                        case 3:
                            pos = rock.transform.MultiplyPoint(new Vector3(0, 0, 1));
                            SpawnRock(iter, pos, rot, scale);
                            break;
                        case 4:
                            pos = rock.transform.MultiplyPoint(new Vector3(0, 0, -1));
                            SpawnRock(iter, pos, rot, scale);
                            break;
                    }
                }
                else
                {
                    switch (rand)//make tube worms
                    {
                        case 0:
                            pos = rock.transform.MultiplyPoint(new Vector3(0, 1, 0));
                            SpawnTube(iter + wormBonusIter, pos, rot, scale * tubeWormScale, iter + wormBonusIter);
                            break;
                        case 1:
                            pos = rock.transform.MultiplyPoint(new Vector3(1, 0, 0));
                            SpawnTube(iter + wormBonusIter, pos, rot, scale * tubeWormScale, iter + wormBonusIter);
                            break;
                        case 2:
                            pos = rock.transform.MultiplyPoint(new Vector3(-1, 0, 0));
                            SpawnTube(iter + wormBonusIter, pos, rot, scale * tubeWormScale, iter + wormBonusIter);
                            break;
                        case 3:
                            pos = rock.transform.MultiplyPoint(new Vector3(0, 0, 1));
                            SpawnTube(iter + wormBonusIter, pos, rot, scale * tubeWormScale, iter + wormBonusIter);
                            break;
                        case 4:
                            pos = rock.transform.MultiplyPoint(new Vector3(0, 0, -1));
                            SpawnTube(iter + wormBonusIter, pos, rot, scale * tubeWormScale, iter + wormBonusIter);
                            break;
                    }
                }

            }
        }
    }
    /// <summary>
    /// Spawn a segment of the body of the tubeworm.  TODO: replace this with branch branch revolution.
    /// </summary>
    /// <param name="iter">The iteration of the loop, used as an escape condition.</param>
    /// <param name="pos">The position to spawn this mesh at.</param>
    /// <param name="rot">The rotation to spawn this mesh at.</param>
    /// <param name="scale">The scale to spawn this mesh at.</param>
    /// <param name="baseIter">The original iteration when the base of this worm was first made.</param>
    /// <param name="previousWorm">The mesh of the worm segment that spawned this one.</param>
    void SpawnTube(int iter, Vector3 pos, Quaternion rot, float scale, int baseIter, Mesh previousWorm = null)
    {

        iter--;

        //make the tube mesh
        CombineInstance tube = new CombineInstance();
        tube.mesh = Instantiate(tubePrefab.sharedMesh);
        tube.transform = Matrix4x4.TRS(pos, rot, new Vector3(scale, scale * tubeWormSegmentLength, scale));

        //make the color for the tube worm
        //nested lerps make it fade from grey, to yellow, to white in a three color gradient
        Vector3[] verts = tube.mesh.vertices;
        Color[] colors = new Color[verts.Length];

        float lerpByIter = 1 - ((float)iter + 1) / (float)baseIter;
        float plerpByIter = 1 - ((float)iter + 2) / (float)baseIter;

        Color startColor1 = Color.Lerp(Color.grey, softYellow, lerpByIter);
        Color startColor2 = Color.Lerp(softYellow, Color.white, lerpByIter);
        Color startFinalColor = Color.Lerp(startColor1, startColor2, lerpByIter);

        Color endColor1 = Color.Lerp(Color.grey, softYellow, plerpByIter);
        Color endColor2 = Color.Lerp(softYellow, Color.white, plerpByIter);
        Color endFinalColor = Color.Lerp(endColor1, endColor2, plerpByIter);

        for (int i = 0; i < verts.Length; i++)
        {
            colors[i] = Color.Lerp(endFinalColor, startFinalColor, verts[i].y);

            if (previousWorm != null)
            {
                if (i % 4 == 0)
                {
                    tube.mesh.vertices[i].x = 0;//reviousWorm.vertices[i % 32 + 3].x;
                    tube.mesh.vertices[i].y = 0;//reviousWorm.vertices[i % 32 + 3].y;
                    tube.mesh.vertices[i].z = 0;//previousWorm.vertices[i % 32 + 3].z;
                }
                else if (i % 2 == 0)
                {
                    tube.mesh.vertices[i].x = 0;//reviousWorm.vertices[i - 1].x;
                    tube.mesh.vertices[i].y = 0;//reviousWorm.vertices[i - 1].y;
                    tube.mesh.vertices[i].z = 0;//previousWorm.vertices[i - 1].z;
                }
                tube.mesh.vertices[i].x = 0;
                tube.mesh.vertices[i].y = 0;
                tube.mesh.vertices[i].z = 0;
            }
        }
        tube.mesh.colors = colors;

        meshes.Add(tube);


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
                pos = tube.transform.MultiplyPoint(new Vector3(0, 2, 0));
                SpawnTube(iter, pos, rot, scale, baseIter, tube.mesh);
            }
            else
            {
                rot.eulerAngles = new Vector3(
                    rot.eulerAngles.x + Random.Range(-15, 15),
                    rot.eulerAngles.y + Random.Range(-5, 5),
                    rot.eulerAngles.z + Random.Range(-15, 15));
                pos = tube.transform.MultiplyPoint(new Vector3(0, 2, 0));
                SpawnTube(iter, pos, rot, scale, baseIter, tube.mesh);

                rot.eulerAngles = new Vector3(
                    rot.eulerAngles.x + Random.Range(-15, 15),
                    rot.eulerAngles.y + Random.Range(-5, 5),
                    rot.eulerAngles.z + Random.Range(-15, 15));
                SpawnTube(iter, pos, rot, scale, baseIter, tube.mesh);
            }


        }
        else //spawn a head
        {
            pos = tube.transform.MultiplyPoint(new Vector3(0, 2, 0));
            SpawnWorm(iter, pos, rot, scale);
        }
    }
    /// <summary>
    /// Spawn in a pre-made head piece for the worm, so that all worm's have an anatomically accurate set of gills.
    /// </summary>
    /// <param name="iter">The iteration of the loop, used as an escape condition.</param>
    /// <param name="pos">The position to spawn this mesh at.</param>
    /// <param name="rot">The rotation to spawn this mesh at.</param>
    /// <param name="scale">The scale to spawn this mesh at.</param>
    void SpawnWorm(int iter, Vector3 pos, Quaternion rot, float scale)
    {
        //make the mesh of the worm head
        CombineInstance worm = new CombineInstance();
        worm.mesh = wormPrefab.sharedMesh;
        worm.transform = Matrix4x4.TRS(pos, rot, Vector3.one * scale);

        //color the worm head, gradiating from white on the bottom to a read top.
        Vector3[] verts = worm.mesh.vertices;
        Color[] colors = new Color[verts.Length];

        for (int i = 0; i < verts.Length; i++)
        {
            colors[i] = Color.Lerp(Color.white, new Color(1, .15f, .15f), verts[i].y);
        }
        worm.mesh.colors = colors;

        meshes.Add(worm);
    }
}
