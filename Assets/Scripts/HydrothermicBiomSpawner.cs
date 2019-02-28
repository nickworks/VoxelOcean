using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HydrothermicBiomSpawner : MonoBehaviour
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

    private List<CombineInstance> meshes = new List<CombineInstance>();

    public void SpawnTubeWorms(Vector3 position, Quaternion rotation)
    {
        GameObject worms = Generate();

        worms.transform.position = position;
        worms.transform.rotation = rotation;

        worms.transform.localScale = Vector3.one * .1f;
    }

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

        meshes.Add(rock);

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
    void SpawnTube(int iter, Vector3 pos, Quaternion rot, float scale, int baseIter, Mesh previousWorm = null)
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

        meshes.Add(worm);
    }

}
