using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateLevelAroundMe : MonoBehaviour
{

    private Int3 gridPos = new Int3();
    private bool hasCachedPosition = false;

    public bool spawnNewChunksOnMove = true;
    void Update()
    {
        if (spawnNewChunksOnMove) GenerateNewChunks();
    }
    void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        Gizmos.color = Color.yellow;
        VoxelUniverse.main.DrawGizmoChunk(gridPos);
        Gizmos.color = Color.red;
        VoxelUniverse.main.DrawGizmoWorldBox(gridPos);

    }
    /// <summary>
    /// If the player swims beyond a threshold, generate more chunks...
    /// </summary>
    private void GenerateNewChunks()
    {
        Int3 pos = VoxelUniverse.main.PositionToGrid(transform.position);

        if (!hasCachedPosition)
        {
            gridPos = pos;
            hasCachedPosition = true;
        }

        if (pos.x > gridPos.x) VoxelUniverse.main.CenterMoved(pos, VoxelUniverse.MoveDirection.Right);
        if (pos.x < gridPos.x) VoxelUniverse.main.CenterMoved(pos, VoxelUniverse.MoveDirection.Left);
        if (pos.z > gridPos.z) VoxelUniverse.main.CenterMoved(pos, VoxelUniverse.MoveDirection.Front);
        if (pos.z < gridPos.z) VoxelUniverse.main.CenterMoved(pos, VoxelUniverse.MoveDirection.Back);
        if (pos.y > gridPos.y) VoxelUniverse.main.CenterMoved(pos, VoxelUniverse.MoveDirection.Up);
        if (pos.y < gridPos.y) VoxelUniverse.main.CenterMoved(pos, VoxelUniverse.MoveDirection.Down);

        gridPos = pos;
    }
}
