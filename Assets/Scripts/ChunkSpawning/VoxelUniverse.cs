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

    public enum SignalType
    {
        AddOnly,
        SubtractOnly,
        Multiply,
        Average,
        None
    }

    [System.Serializable]
    public class SignalField
    {
        public SignalType type;
        [Range(1, 100)] public float zoom = 20;
        [Range(-0.2f, 0.2f)] public float densityBias = 0;
        [Range(0, 1)] public float flattenAmount = 0;
        [Range(-1, 1)] public float verticalOffset = 0;

        static public SignalField Random()
        {
            SignalField res = new SignalField();
            res.zoom = UnityEngine.Random.Range(1f, 100);
            res.densityBias = UnityEngine.Random.Range(-0.2f, 0.2f);
            res.flattenAmount = UnityEngine.Random.Range(0f, 1);
            res.verticalOffset = 0;
            res.type = (SignalType) UnityEngine.Random.Range(0, 3);
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
    List<VoxelChunk> chunks = new List<VoxelChunk>();

    Coroutine coroutine;

    void Start()
    {
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
        DestroyChunks();
        // begin asyncronous generation:
        coroutine = StartCoroutine(SpawnChunks());
    }
    void DestroyChunks()
    {
        foreach (VoxelChunk chunk in chunks)
        {
            Destroy(chunk.gameObject);
        }
        chunks.Clear();
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
        for(int x = -renderDistance; x <= renderDistance; x++)
        {
            for (int y = -renderDistanceVertical; y <= renderDistanceVertical; y++)
            {
                for (int z = -renderDistance; z <= renderDistance; z++)
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
                        chunks.Add(chunk);
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
