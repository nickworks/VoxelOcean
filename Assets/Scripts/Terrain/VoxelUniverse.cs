using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// This class spawns several VoxelChunkMesh objects.
/// </summary>
public class VoxelUniverse : MonoBehaviour
{

    //private int xOffset = 0;
    //private int yOffset = 0;
    //private int zOffset = 0;

    public enum SignalType
    {
        AddOnly,
        SubtractOnly,
        Multiply,
        Average,
        Sphere,
        None
    }

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
            res.type = (SignalType)UnityEngine.Random.Range(0, 5);
            return res;
        }
    }


    /// <summary>
    /// The universe singleton.
    /// </summary>
    static public VoxelUniverse main;

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

    public SignalField[] signalFields;

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

        // stop active generation:
        StopAllCoroutines();
        // destroy existing chunks:
        DestroyAllChunks();

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
        int l = renderDistance;

        Int3 min = new Int3(-w, -h, -l);
        Int3 max = new Int3(+w, +h, +l);

        if (dir == MoveDirection.Up)    min.y = max.y;
        if (dir == MoveDirection.Right) min.x = max.x;
        if (dir == MoveDirection.Front) min.z = max.z;

        if (dir == MoveDirection.Down) max.y = min.y;
        if (dir == MoveDirection.Left) max.x = min.x;
        if (dir == MoveDirection.Back) max.z = min.z;

        for (int x = min.x; x <= max.x; x++)
        {
            for (int y = min.y; y <= max.y; y++)
            {
                for (int z = min.z; z <= max.z; z++)
                {
                    Int3 p = new Int3(x, y, z);
                    SpawnChunkAt(p + center);
                    yield return null;
                }
            }
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
