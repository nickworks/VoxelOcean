using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// This class spawns several VoxelChunkMesh objects.
/// </summary>
[RequireComponent(typeof(SignalFieldGenerator))]
public class VoxelUniverse : MonoBehaviour
{

    public enum MoveDirection
    {
        None,
        Up,
        Down,
        Left,
        Right,
        Front,
        Back
    }

    /// <summary>
    /// The universe singleton.
    /// </summary>
    static public VoxelUniverse main;

    public SignalFieldGenerator signals { get; private set; }

    [Tooltip("How large each voxel cube is, in meters.")]
    [Range(1, 10)] public float voxelSize = 1;
    [Tooltip("How many voxels should be in each dimension of each chunk. For example, a value of 10 will produce chunks that have 10 x 10 x 10 voxels.")]
    [Range(1, 40)] public int voxelsPerChunk = 10;

    /// <summary>
    /// How big a side of the chunk is, in meters.
    /// </summary>
    public float chunkSize
    {
        get { return voxelsPerChunk * voxelSize; }
    }

    public bool terrainOnly = false;

    [Tooltip("How many chunks to spawn laterally in either direction around the middle. For example, a value of 1 will spawn an 3 x 3 chunks.")]
    [Range(0, 8)] public int renderDistance = 3;
    [Tooltip("How many layers of chunks to spawn above and below the middle layer. For example, a value of 1 will spawn 3 layers of chunks.")]
    [Range(0, 3)] public int renderDistanceVertical = 1;
    /// <summary>
    /// The size of each voxel. Measured in meters.
    /// </summary>

    public Int3 PositionToGrid(Vector3 p)
    {
        return Int3.fromVector(p / chunkSize);
    }
    public Vector3 GridToPositionCorner(Int3 g)
    {
        return g.toVector(chunkSize);
    }
    public Vector3 GridToPositionCenter(Int3 g)
    {
        return g.toVector(chunkSize) + Int3.one.toVector(chunkSize) / 2;
    }

    [Tooltip("The prefab to use when spawning chunks.")]
    public VoxelChunk voxelChunkPrefab;

    // how large to make the biomes
    [Range(3, 200)] public float biomeScaling = 25;

    /// <summary>
    /// The currently generated list of chunks.
    /// </summary>
    List<VoxelChunk> chunks = new List<VoxelChunk>();

    void Start()
    {
        main = this;
        signals = GetComponent<SignalFieldGenerator>();
        Create();
    }
    /// <summary>
    /// Create a voxel universe.
    /// </summary>
    public void Create(bool randomizeFields = false)
    {
        if (!main) return; // don't run if Start() hasn't been called yet

        // stop active generation:
        StopAllCoroutines();
        // destroy existing chunks:
        DestroyAllChunks();

        if (randomizeFields) signals.RandomizeFields();

        // find the player's grid position:
        GenerateLevelAroundMe pawn = FindObjectOfType<GenerateLevelAroundMe>();
        Int3 newCenter = PositionToGrid(pawn.transform.position);

        // begin asyncronous generation around grid position:
        StartCoroutine(SpawnChunksAroundCenter(newCenter, MoveDirection.None));
    }

    public void CenterMoved(Int3 newCenter, MoveDirection dir)
    {
        StartCoroutine(SpawnChunksAroundCenter(newCenter, dir));
        DestroyDistantChunks(newCenter);
        SpawnChunksAroundCenter(newCenter, dir);
    }

    private IEnumerator SpawnChunksAroundCenter(Int3 center, MoveDirection dir)
    {
        int w = renderDistance;
        int h = renderDistanceVertical;

        Int3 min = new Int3(-w, -h, -w) + center;
        Int3 max = new Int3(+w, +h, +w) + center;

        if (dir == MoveDirection.Right) min.x = max.x;
        if (dir == MoveDirection.Up)    min.y = max.y;
        if (dir == MoveDirection.Front) min.z = max.z;

        if (dir == MoveDirection.Left) max.x = min.x;
        if (dir == MoveDirection.Down) max.y = min.y;
        if (dir == MoveDirection.Back) max.z = min.z;

        List<Int3> spawnPoints = new List<Int3>(); // make a list of locations to spawn chunks at

        for (int x = min.x; x <= max.x; x++)
        {
            for (int y = min.y; y <= max.y; y++)
            {
                for (int z = min.z; z <= max.z; z++)
                {
                    Int3 p = new Int3(x, y, z);

                    // calculate this chunks distance from the center:

                    p.tag = Int3.ManhattanDis(p, center);
                    // add the chunks in order by distance

                    // find the index number to insert the new chunk:
                    int i = 0;
                    while(i < spawnPoints.Count)
                    {
                        // if this other location is further away,
                        // we've found where to insert our chunk, so break out
                        if (p.tag < spawnPoints[i].tag) break; 
                        i++;
                    }
                    spawnPoints.Insert(i, p); // insert position
                }
            }
        }

        // spawn the chunks:
        foreach (Int3 p in spawnPoints)
        {
            SpawnChunkAt(p);
            yield return null;
        }
    }
    private void SpawnChunkAt(Int3 gridPos)
    {
        Vector3 pos = gridPos.toVector(chunkSize);
        VoxelChunk chunk = Instantiate(voxelChunkPrefab, pos, Quaternion.identity, transform);
        chunk.SpawnChunk(gridPos);

        if (chunk.isEmpty)
        {
            Destroy(chunk.gameObject);
        }
        else
        {
            chunks.Add(chunk);
            /*
            TextMesh txt = chunk.GetComponentInChildren<TextMesh>();
            txt.transform.localPosition = Vector3.one * chunkSize / 2;
            txt.text = text;
            */
        }
    }

    private void DestroyDistantChunks(Int3 center)
    {

        for(int i = chunks.Count - 1; i >= 0; i--) // loop backwards to avoid skip-update glitch
        {
            Int3 dis = Int3.Dis(chunks[i].gridPos, center);

            bool thisChunkIsTooFar = (dis.x > renderDistance) || (dis.z > renderDistance) || (dis.y > renderDistanceVertical);

            if (thisChunkIsTooFar)
            {
                Destroy(chunks[i].gameObject);
                chunks.RemoveAt(i);
            }
        }
    }
    void DestroyAllChunks()
    {
        foreach (VoxelChunk chunk in chunks)
        {
            Destroy(chunk.gameObject);
        }
        chunks.Clear();
    }
    public void DrawGizmoChunk(Int3 gridPos)
    {
        
        Vector3 size = Int3.one.toVector(chunkSize); // the width of the box, in meters:
        Vector3 position = GridToPositionCenter(gridPos); // the center of the box

        Gizmos.DrawWireCube(position, size);

        Vector3 half = size / 2;
        DrawGizmoLine(position, new Vector3(+half.x, +half.y, +half.z));
        DrawGizmoLine(position, new Vector3(-half.x, +half.y, +half.z));
        DrawGizmoLine(position, new Vector3(-half.x, +half.y, -half.z));
        DrawGizmoLine(position, new Vector3(+half.x, +half.y, -half.z));

        DrawGizmoLine(position, new Vector3(+half.x, -half.y, +half.z));
        DrawGizmoLine(position, new Vector3(-half.x, -half.y, +half.z));
        DrawGizmoLine(position, new Vector3(-half.x, -half.y, -half.z));
        DrawGizmoLine(position, new Vector3(+half.x, -half.y, -half.z));
    }
    public void DrawGizmoLine(Vector3 position, Vector3 offset)
    {
        Vector3 corner = position + offset;

        Gizmos.DrawLine(Vector3.Lerp(position, corner, .25f), corner);
    }
    public void DrawGizmoWorldBox(Int3 gridPos)
    {
        Int3 cells = new Int3(
            renderDistance * 2 + 1,
            renderDistanceVertical * 2 + 1,
            renderDistance * 2 + 1
        );

        Vector3 size = cells.toVector(chunkSize); // the width of the box, in meters:
        Vector3 position = GridToPositionCenter(gridPos); // the center of the box

        Gizmos.DrawWireCube(position, size);
    }

    /// <summary>
    /// Samples a given grid position and returns the noise value at this position.
    /// </summary>
    /// <param name="pos">A position in world space.</param>
    /// <returns>The density of the given position. Depending on the noise function used, this should be in the -1 to +1 range.</returns>
    public float GetDensitySample(Vector3 pos)
    {
        return signals.GetDensitySample(pos);
    }

    [CustomEditor(typeof(VoxelUniverse))]
    class VoxelUniverseEditor : Editor
    {
        override public void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Build Universe"))
            {
                (target as VoxelUniverse).Create();
            }
            
        }
    }
}
