using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// This class spawns several VoxelChunkMesh objects.
/// </summary>
public class VoxelUniverse : MonoBehaviour
{
    /// <summary>
    /// The distance between the centers of adjacent voxels. Measured in meters.
    /// </summary>
    public const float VOXEL_SEPARATION = 1;

    private int xOffset = 0;
    private int yOffset = 0;
    private int zOffset = 0;

    public enum SignalType
    {
        AddOnly,
        SubtractOnly,
        Multiply,
        Average,
        Sphere,
        None
    }

    [System.Serializable]
    public class SignalField
    {
        public SignalType type;
        [Range(1, 100)] public float zoom = 20;
        [Range(-0.2f, 0.2f)] public float densityBias = 0;
        [Range(0, 1)] public float flattenAmount = 0;
        [Range(-20, 20)] public float flattenOffset = 0;

        static public SignalField Random()
        {
            SignalField res = new SignalField();
            res.zoom = UnityEngine.Random.Range(1f, 100);
            res.densityBias = UnityEngine.Random.Range(-0.2f, 0.2f);
            res.flattenAmount = UnityEngine.Random.Range(0f, 1);
            res.flattenOffset = 0;
            res.type = (SignalType) UnityEngine.Random.Range(0, 5);
            return res;
        }
    }


    /// <summary>
    /// The universe singleton.
    /// </summary>
    static public VoxelUniverse main;

    [Tooltip("How many chunks to spawn laterally in either direction around the middle. For example, a value of 1 will spawn an 3 x 3 chunks.")]
    [Range(0,8)] public int renderDistance = 3;
    [Tooltip("How many layers of chunks to spawn above and below the middle layer. For example, a value of 1 will spawn 3 layers of chunks.")]
    [Range(0,3)] public int renderDistanceVertical = 1;
    [Tooltip("How many voxels should be in each dimension of each chunk. For example, a value of 10 will produce chunks that have 10 x 10 x 10 voxels.")]
    [Range(1,40)] public int resPerChunk = 10;

    [Tooltip("The prefab to use when spawning chunks.")]
    public VoxelChunk voxelChunkPrefab;
    
    // how large to make the biomes
    [Range(3, 200)] public float biomeScaling = 25;

    public SignalField[] signalFields;

    public bool isGenerating { get; private set; }
    public float percentGenerated { get; private set; }

    /// <summary>
    /// The currently generated list of chunks.
    /// </summary>

    VoxelChunk[,,] chunks;

    Coroutine coroutine;

    void Start()
    {
        chunks = new VoxelChunk[1 + renderDistance * 2, 1 + renderDistanceVertical * 2, 1 + renderDistance * 2];
        main = this;
        Create();
    }
    /// <summary>
    /// Create a voxel universe.
    /// </summary>
    public void Create()
    {
        if (!main) return; // don't run if Start() hasn't been called yet
        //if (isGenerating) return;

        // stop active generation:
        if (coroutine != null) StopCoroutine(coroutine);
        // destroy existing chunks:
        DestroyAllChunks();
        // begin asyncronous generation:
        coroutine = StartCoroutine(SpawnChunks());
    }

    /// <summary>
    /// Create a new chunk layer.
    /// </summary>
    IEnumerator CreateMoreChunks()
    {
        int whereToSpawnNewChunks = 1; //change this based on some parameter that tells us in what direction the player was moving when the switched chunks

        isGenerating = true;
        percentGenerated = 0;

        switch (whereToSpawnNewChunks)
        {
            case 1: // move postive x one chunk
                xOffset++; 
                int numChunks = (renderDistance * 2 + 1);

                int i = 0; 

                if (chunks.Length > 0)
                {
                    for (int x = 0; x < renderDistance * 2 ; x++)
                    {
                        for (int y = 0; y < renderDistanceVertical * 2; y++)
                        {
                            for (int z = 0; z < renderDistance * 2; z++)
                            {
                                if(x == 0) //removes least x grid of chunks and replaces it with what is one x slot more positive
                                {
                                    if (chunks[x, y, z] == null) continue;
                                    Destroy(chunks[x, y, z].gameObject);
                                    chunks[x, y, z] = chunks[x + 1, y, z];
                                }
                                else if (x == renderDistance * 2 - 1) //adds new grid of chunks at the highest x values
                                {
                                    if (chunks[x, y, z] != null)
                                    {
                                        Destroy(chunks[x, y, z].gameObject);
                                        chunks[x, y, z] = null;
                                    }

                                    Vector3 pos = new Vector3(x + xOffset, y, z) * resPerChunk;
                                    VoxelChunk chunk = Instantiate(voxelChunkPrefab, pos, Quaternion.identity, transform);
                                    chunk.Rebuild();
                                    if (chunk.isEmpty)
                                    {
                                        Destroy(chunk.gameObject);
                                    }
                                    else
                                    {
                                        chunks[x, y, z] = chunk;
                                    }
                                    i++;
                                    percentGenerated = i / (float)numChunks;
                                    yield return null;
                                }
                                else //shifts all of the chunks one x value towards negative by grabbing the chunk to their x positive pointing to it
                                {
                                    chunks[x, y, z] = chunks[x + 1, y, z];
                                }

                                
                            }
                        }
                    }
                }
                break;

            case 2: // move negative x one chunk
                xOffset--;


                numChunks = (renderDistance * 2 + 1); // won't let me redeclare these variables that I declared in case one. declaring outside of switch statement didn't help
                i = 0;

                if (chunks.Length > 0)
                {
                    for (int x = renderDistance * 2 - 1; x > -1; x--)
                    {
                        for (int y = 0; y < renderDistanceVertical * 2; y++)
                        {
                            for (int z = 0; z < renderDistance * 2; z++)
                            {
                                if (x == renderDistance * 2 - 1) //removes most x grid of chunks and replaces it with what is one x slot more negative
                                {
                                    if (chunks[x, y, z] == null) continue;
                                    Destroy(chunks[x, y, z].gameObject);
                                    chunks[x, y, z] = chunks[x - 1, y, z];
                                }
                                else if (x == 0) //adds new grid of chunks at the lowest x values
                                {
                                    if (chunks[x, y, z] != null)
                                    {
                                        Destroy(chunks[x, y, z].gameObject);
                                        chunks[x, y, z] = null;
                                    }

                                    Vector3 pos = new Vector3(x + xOffset, y, z) * resPerChunk;
                                    VoxelChunk chunk = Instantiate(voxelChunkPrefab, pos, Quaternion.identity, transform);
                                    chunk.Rebuild();
                                    if (chunk.isEmpty)
                                    {
                                        Destroy(chunk.gameObject);
                                    }
                                    else
                                    {
                                        chunks[x, y, z] = chunk;
                                    }
                                    i++;
                                    percentGenerated = i / (float)numChunks;
                                    yield return null;
                                }
                                else //shifts all of the chunks one x value towards positive by grabbing the chunk to their x negative pointing to it
                                {
                                    chunks[x, y, z] = chunks[x - 1, y, z];
                                }
                            }
                        }
                    }
                }
                break;

            case 3: // move postive z one chunk

                break;

            case 4: // move negative z one chunk

                break;

            case 5: // move postive y one chunk

                break;

            case 6: // move negative y one chunk

                break;

                
        }

        isGenerating = false;

    }

    void DestroyAllChunks()
    {
        

        if(chunks.Length > 0)
        {
            for (int x = 0; x < renderDistance * 2; x++)
            {
                for (int y = 0; y < renderDistanceVertical * 2; y++)
                {
                    for (int z = 0; z < renderDistance * 2; z++)
                    {
                        if (chunks[x, y, z] == null) continue;
                        Destroy(chunks[x, y, z].gameObject);
                        chunks[x, y, z] = null;
                    }
                }
            }
        }
        
        

    }

    /// <summary>
    /// Spawns chunks of voxels.
    /// </summary>
    IEnumerator SpawnChunks()
    {
        isGenerating = true;
        percentGenerated = 0;

        int numChunks = (renderDistance * 2 + 1);
        numChunks *= numChunks;
        numChunks *= (renderDistanceVertical * 2 + 1);

        // renderdistance = 1
        // renderdistancevertical = 1
        // result = 3x3x3

        int i = 0;
        for(int x = 0; x <= renderDistance * 2 ; x++)
        {
            for (int y = 0; y <= renderDistanceVertical * 2 ; y++)
            {
                for (int z = 0; z <= renderDistance * 2 ; z++)
                {
                     
                    Vector3 pos = new Vector3(x, y, z) * resPerChunk;
                    VoxelChunk chunk = Instantiate(voxelChunkPrefab, pos, Quaternion.identity, transform);
                    chunk.Rebuild();
                    if (chunk.isEmpty)
                    {
                        Destroy(chunk.gameObject);
                    }
                    else
                    {
                        //chunks.Add(coord);
                        chunks[x, y, z] = chunk; 
                    }
                    i++;
                    percentGenerated = i / (float)numChunks;
                    yield return null;
                }
            }
        }
        isGenerating = false;
    }

    public void RandomizeFields()
    {
        int fieldCount = Random.Range(1, 8);
        List<SignalField> fields = new List<SignalField>();
        for(int i = 0; i < fieldCount; i++)
        {
            fields.Add(SignalField.Random());
        }
        fields[0].type = SignalType.AddOnly;
        signalFields = fields.ToArray();
    }

    [CustomEditor(typeof(VoxelUniverse))]
    class VoxelUniverseEditor : Editor
    {
        override public void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Randomize Noise"))
            {
                (target as VoxelUniverse).RandomizeFields();
            }
            if (GUILayout.Button("Build Universe"))
            {
                (target as VoxelUniverse).Create();
            }
            
        }
    }



}
