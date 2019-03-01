using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HydrothermicBiomSpawner : MonoBehaviour
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
    /// An easy to access public function to allow for other scripts to call for the generation of a tube worm.
    /// </summary>
    /// <param name="position">The position to spawn this tube worm at in world space.</param>
    /// <param name="rotation">the rotation to spawn this tube worm at in world space.</param>
    public void SpawnTubeWorms(Vector3 position, Quaternion rotation)
    {
        GameObject worms = Generate();

        worms.transform.position = position;
        worms.transform.rotation = rotation;

        worms.transform.localScale = Vector3.one * .1f;
    }

    /// <summary>
    /// The starting point for actual generation which initilizes the recursive loop.
    /// </summary>
    /// <returns>The resulting game object to be placed in the scene.</returns>
    GameObject Generate()
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

//TODO: impliment some of this maybe?
//or just use branch branch code?

/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Generator : MonoBehaviour
{

    public Material mat;
    public int baseIterations = 5;
    public float baseScale = 5;
    public float scaleRate = .9f;
    public float tubeWormScale = .25f;
    public float tubeWormSpawnRate = .25f;
    public float tubeWormSpawnDelay = .5f;
    public float tubeWormBranchChance = .05f;
    public float tubeWormSegmentLength = 2;
    public int wormBonusIter = 0;
    public int seed = 0;

    public MeshFilter rockPrefab;
    public MeshFilter wormPrefab;
    public MeshFilter tubePrefab;

    Color softYellow = new Color(.9f, .9f, .75f);

    private List<CombineInstance> rockMeshes = new List<CombineInstance>();
    private List<CombineInstance> tubeMeshes = new List<CombineInstance>();
    private List<CombineInstance> wormMeshes = new List<CombineInstance>();
    private List<CombineInstance> rocks = new List<CombineInstance>();

    void Generate()
    {
        rockMeshes.Clear();
        Random.InitState(seed);

        SpawnRock(baseIterations, Vector3.zero, Quaternion.identity, baseScale);
        CleanUpRocks();

        //Mesh rockMesh = new Mesh();
        //rockMesh.CombineMeshes(rockMeshes.ToArray(), true);

        Mesh wormMesh = new Mesh();
        wormMesh.CombineMeshes(wormMeshes.ToArray(), true);

        //Mesh smoothRockMesh = RemoveDuplicates(rockMesh, false);

        //CombineInstance[] final = new CombineInstance[2];
        //final[0] = new CombineInstance();
        //final[1] = new CombineInstance();
        //final[0].mesh = rockMesh;
        //final[0].transform = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);
        //final[1].mesh = wormMesh;
        //final[1].transform = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);
        //
        //Mesh finalMesh = new Mesh();
        //finalMesh.CombineMeshes(final, true);

        GameObject ob = new GameObject();
        ob.AddComponent<MeshFilter>().mesh = wormMesh;
        ob.AddComponent<MeshRenderer>().material = mat;
    }
    void CleanUpRocks()
    {
        return;
        foreach(CombineInstance rock1 in rocks)
        {
            Vector3 pos1 = (rock1.transform.MultiplyPoint(Vector3.zero));
            foreach(CombineInstance rock2 in rocks)
            {
                Vector3 pos2 = (rock2.transform.MultiplyPoint(Vector3.zero));

                if (pos1.x == pos2.x && pos1.y == pos2.y && pos1.z == pos2.z) continue;

                CombineInstance bigger = rock1;
                CombineInstance smaller = rock2;

                if(rock2.transform.lossyScale.x > rock1.transform.lossyScale.x)
                {
                    bigger = rock2;
                    smaller = rock1;
                }

                if(Vector3.Distance(pos1, pos2) < bigger.transform.lossyScale.x * 1.5 - (smaller.transform.lossyScale.x / 2) * 1.5)
                {
                    if (rockMeshes.Contains(smaller)) rockMeshes.Remove(smaller);
                }
            }
        }
    }
    void SpawnRock(int iter, Vector3 pos, Quaternion rot, float scale)
    {
        iter--;

        CombineInstance rock = new CombineInstance();
        rock.mesh = rockPrefab.sharedMesh;
        rock.transform = Matrix4x4.TRS(pos, rot, Vector3.one * scale);

        Vector3[] verts = rock.mesh.vertices;
        Color[] colors = new Color[verts.Length];

        for (int i = 0; i < verts.Length; i++)
        {
            colors[i] = Color.Lerp(Color.black, softYellow, verts[i].y * verts[i].y * verts[i].y);
        }
        rock.mesh.colors = colors;

        wormMeshes.Add(rock);
        rocks.Add(rock);

        //
        scale *= scaleRate;

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
                    switch (rand)
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
                    switch (rand)
                    {
                        case 0:
                            pos = rock.transform.MultiplyPoint(new Vector3(0, 1, 0));
                            SpawnTube(iter + wormBonusIter, pos, rot, scale * tubeWormScale, iter + wormBonusIter, new CombineInstance());
                            break;
                        case 1:
                            pos = rock.transform.MultiplyPoint(new Vector3(1, 0, 0));
                            SpawnTube(iter + wormBonusIter, pos, rot, scale * tubeWormScale, iter + wormBonusIter, new CombineInstance());
                            break;
                        case 2:
                            pos = rock.transform.MultiplyPoint(new Vector3(-1, 0, 0));
                            SpawnTube(iter + wormBonusIter, pos, rot, scale * tubeWormScale, iter + wormBonusIter, new CombineInstance());
                            break;
                        case 3:
                            pos = rock.transform.MultiplyPoint(new Vector3(0, 0, 1));
                            SpawnTube(iter + wormBonusIter, pos, rot, scale * tubeWormScale, iter + wormBonusIter, new CombineInstance());
                            break;
                        case 4:
                            pos = rock.transform.MultiplyPoint(new Vector3(0, 0, -1));
                            SpawnTube(iter + wormBonusIter, pos, rot, scale * tubeWormScale, iter + wormBonusIter, new CombineInstance());
                            break;
                    }
                }

            }
        }
    }
    void SpawnTube(int iter, Vector3 pos, Quaternion rot, float scale, int baseIter, CombineInstance previousWorm)
    {
        iter--;

        CombineInstance tube = new CombineInstance();
        tube.mesh = Instantiate(tubePrefab.sharedMesh);

        tube.transform = Matrix4x4.TRS(pos, rot, new Vector3(scale, scale * tubeWormSegmentLength, scale));

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

        //List<Vector3> vertList = new List<Vector3>();

        for (int i = 0; i < verts.Length; i++)
        {
            colors[i] = Color.Lerp(endFinalColor, startFinalColor, verts[i].y);

            //if (previousWorm.mesh != null)
            //{
            //    if (i % 4 == 0)
            //    {
            //        verts[i] = RotatePointAroundPivot(verts[i], Vector3.zero, -rot.eulerAngles + previousWorm.transform.rotation.eulerAngles);
            //    }
            //    else if (i % 2 == 0)
            //    {
            //        verts[i] = RotatePointAroundPivot(verts[i], Vector3.zero, -rot.eulerAngles + previousWorm.transform.rotation.eulerAngles);
            //    }
            //}
            //
            //vertList.Add(new Vector3(verts[i].x, verts[i].y, verts[i].z));
        }
        tube.mesh.colors = colors;

        //tube.mesh.SetVertices(vertList);

        wormMeshes.Add(tube);

        //
        //scale *= scaleRate;

        if (iter >= 0)
        {
            rot.eulerAngles = new Vector3(
                rot.eulerAngles.x + Random.Range(-5, 5),
                rot.eulerAngles.y + Random.Range(-5, 5),
                rot.eulerAngles.z + Random.Range(-5, 5));

            if (Random.Range(0, 1f) > tubeWormBranchChance)
            {
                rot.eulerAngles = new Vector3(
                    rot.eulerAngles.x + Random.Range(-5, 5),
                    rot.eulerAngles.y + Random.Range(-5, 5),
                    rot.eulerAngles.z + Random.Range(-5, 5));
                pos = tube.transform.MultiplyPoint(new Vector3(0, 2, 0));
                SpawnTube(iter, pos, rot, scale, baseIter, tube);
            }
            else
            {
                rot.eulerAngles = new Vector3(
                    rot.eulerAngles.x + Random.Range(-15, 15),
                    rot.eulerAngles.y + Random.Range(-5, 5),
                    rot.eulerAngles.z + Random.Range(-15, 15));
                pos = tube.transform.MultiplyPoint(new Vector3(0, 2, 0));
                SpawnTube(iter, pos, rot, scale, baseIter, tube);

                rot.eulerAngles = new Vector3(
                    rot.eulerAngles.x + Random.Range(-15, 15),
                    rot.eulerAngles.y + Random.Range(-5, 5),
                    rot.eulerAngles.z + Random.Range(-15, 15));
                SpawnTube(iter, pos, rot, scale, baseIter, tube);
            }
        }
        else
        {
            pos = tube.transform.MultiplyPoint(new Vector3(0, 2, 0));
            SpawnWorm(iter, pos, rot, scale);
        }
    }
    void SpawnWorm(int iter, Vector3 pos, Quaternion rot, float scale)
    {
        CombineInstance worm = new CombineInstance();
        worm.mesh = wormPrefab.sharedMesh;
        worm.transform = Matrix4x4.TRS(pos, rot, Vector3.one * scale);

        Vector3[] verts = worm.mesh.vertices;
        Color[] colors = new Color[verts.Length];

        for (int i = 0; i < verts.Length; i++)
        {
            colors[i] = Color.Lerp(Color.white, new Color(1, .15f, .15f), verts[i].y);
        }
        worm.mesh.colors = colors;

        wormMeshes.Add(worm);
    }
    private Mesh RemoveDuplicates(Mesh complexMesh, bool printDebug = false)
    {
        List<Vector3> verts = new List<Vector3>();
        List<int> tris = new List<int>();
        List<Vector3> norms = new List<Vector3>();
        List<Color> cols = new List<Color>();

        complexMesh.GetVertices(verts);
        complexMesh.GetTriangles(tris, 0);
        complexMesh.GetNormals(norms);
        complexMesh.GetColors(cols);

        int count1 = verts.Count;
        //Debug.Log(count1);
        //return;

        for (int i = 0; i < verts.Count; i++)
        {
            // find duplicates:
            for (int j = 0; j < verts.Count; j++)
            {
                if (i == j) continue;
                if (j >= verts.Count) break;
                if (i >= verts.Count) break;
                if (Vector3.Distance(verts[i], verts[j]) < .1) // if a duplicate vert:
                {
                    verts.RemoveAt(j); // remove it
                    norms[i] = (norms[i] + norms[j]) / 2;
                    norms.RemoveAt(j);
                    cols.RemoveAt(j);
                    for (int k = 0; k < tris.Count; k++)
                    {
                        if (tris[k] == j) tris[k] = i;
                        if (tris[k] > j) tris[k] = tris[k] - 1;
                    }
                }
            }
        }
        int count2 = verts.Count;

        //if (printDebug) print($"{count1} reduced to {count2}");

        Mesh mesh = new Mesh();
        mesh.SetVertices(verts);
        mesh.SetTriangles(tris, 0);
        mesh.SetNormals(norms);
        mesh.SetColors(cols);
        return mesh;

    } // RemoveDuplicates()

    [CustomEditor(typeof(Generator))]
    class GeneratorEditor : Editor
    {
        override public void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Make Tube Worms"))
            {
                (target as Generator).Generate();
            }

        }
    }
}
*/