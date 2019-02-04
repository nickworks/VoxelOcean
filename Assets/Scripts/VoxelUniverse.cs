using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// This class spawns several VoxelChunkMesh objects.
/// </summary>
public class VoxelUniverse : MonoBehaviour
{
    static public VoxelUniverse main;

    [Tooltip("How many chunks to spawn laterally in either direction around the middle. For example, a value of 1 will spawn an 3 x 3 chunks.")]
    public int renderDistance = 3;
    [Tooltip("How many layers of chunks to spawn above and below the middle layer. For example, a value of 1 will spawn 3 layers of chunks.")]
    public int renderDistanceVertical = 1;
    [Tooltip("How many voxels should be in each dimension of each chunk. For example, a value of 10 will produce chunks that have 10 x 10 x 10 voxels.")]
    public int resPerChunk = 10;
    [Tooltip("How far to zoom into the noise data.")]
    public float zoom = 20;
    [Tooltip("Separation between the centers of individual voxels in meters.")]
    public float separation = 1;
    [Tooltip("Influences the density threshold. Controls where the solid/non-solid boundary is.")]
    [Range(-1, 1)] public float thresholdBias = 0;

    [Tooltip("How much to flatten out the universe.")]
    [Range(0, 1)] public float flattenAmount = 0.3f;

    [Tooltip("The mesh to use for our voxels. Fewer vertices is better!")]
    public Mesh voxelMesh;
    [Tooltip("The prefab to use when spawning chunks.")]
    public VoxelChunk voxelChunkPrefab;
    
    /// <summary>
    /// The currently generated list of chunks.
    /// </summary>
    List<VoxelChunk> chunks = new List<VoxelChunk>();

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
        
        SpawnChunks();
        Rebuild();
    }
    /// <summary>
    /// Spawns chunks of voxels.
    /// </summary>
    void SpawnChunks()
    {
        foreach(VoxelChunk chunk in chunks)
        {
            Destroy(chunk.gameObject);
        }
        chunks.Clear();

        for(int x = -renderDistance; x <= renderDistance; x++)
        {
            for (int y = -renderDistanceVertical; y <= renderDistanceVertical; y++)
            {
                for (int z = -renderDistance; z <= renderDistance; z++)
                {
                    Vector3 pos = new Vector3(x, y, z) * resPerChunk;
                    VoxelChunk chunk = Instantiate(voxelChunkPrefab, pos, Quaternion.identity);
                    chunks.Add(chunk);
                }
            }
        }
    }
    /// <summary>
    /// This function tells each chunk to regenerate.
    /// </summary>
    void Rebuild()
    {
        foreach (VoxelChunk chunk in chunks)
        {
            chunk.Rebuild();
        }
    }
}
