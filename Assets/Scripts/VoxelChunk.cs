using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// One chunk of voxels.
/// </summary>
[RequireComponent(typeof(MeshFilter))]
public class VoxelChunk : MonoBehaviour
{
    /// <summary>
    /// Cached noise data. This is used as "density" to determin whether or not voxels are solid.
    /// </summary>
    float[,,] data;
    /// <summary>
    /// The mesh to render out.
    /// </summary>
    MeshFilter mesh;
    /// <summary>
    /// Whether or not this mesh is completely empty.
    /// </summary>
    public bool isEmpty
    {
        get
        {
            return (mesh.mesh.vertexCount == 0);
        }
    }

    /// <summary>
    /// When called, this function generates noise data and rebuilds the mesh.
    /// </summary>
    public void Rebuild()
    {
        if (!EditorApplication.isPlaying) return;

        mesh = GetComponent<MeshFilter>();
        GenerateNoiseData();
        GenerateMesh();
    }
    /// <summary>
    /// Generate noise data and cache in a huge array
    /// </summary>
    void GenerateNoiseData()
    {
        int res = VoxelUniverse.main.resPerChunk;
        data = new float[res, res, res];
        for (int x = 0; x < data.GetLength(0); x++)
        {
            for (int y = 0; y < data.GetLength(1); y++)
            {
                for (int z = 0; z < data.GetLength(2); z++)
                {
                    data[x, y, z] = GetDensitySample(new Vector3(x, y, z));
                }
            }
        }
    }
    /// <summary>
    /// Samples a given grid position and returns the noise value at this position.
    /// </summary>
    /// <param name="pos">A position in local space.</param>
    /// <returns>The density of the given position. Depending on the noise function used, this should be in the -1 to +1 range.</returns>
    float GetDensitySample(Vector3 pos)
    {
        float res = 0;
        foreach (VoxelUniverse.SignalField field in VoxelUniverse.main.signalFields)
        {
            Vector3 p = pos + transform.position; // convert to world position
            p /= field.zoom; // "zoom" in/out of the noise
            float val = Noise.Sample(p); // simplex.noise(pos.x, pos.y, pos.z);

            // use the vertical position to influence the density:
            val -= (p.y + field.verticalOffset) * field.flattenAmount;
            val -= field.densityBias;
            switch (field.type)
            {
                case VoxelUniverse.SignalType.AddOnly:
                    if (Mathf.Sign(val) == Mathf.Sign(res)) res += val;
                    break;
                case VoxelUniverse.SignalType.SubtractOnly:
                    if (Mathf.Sign(val) != Mathf.Sign(res)) res += val;
                    break;
                case VoxelUniverse.SignalType.Multiply:
                    res *= val;
                    break;
                case VoxelUniverse.SignalType.Average:
                    res = (val + res) / 2;
                    break;
            }
        }
        return res;
    }
    /// <summary>
    /// This function builds the mesh by copying the cube over and over again
    /// </summary>
    void GenerateMesh()
    {
        mesh.mesh = VoxelUniverse.main.voxelMesh;
        List<Vector3> voxels = new List<Vector3>();
        for (int x = 0; x < data.GetLength(0); x++)
        {
            for (int y = 0; y < data.GetLength(1); y++)
            {
                for (int z = 0; z < data.GetLength(2); z++)
                {
                    if (IsSolid(x,y,z))
                    {
                        bool isSolidRight = IsSolid(x + 1, y, z);
                        bool isSolidLeft = IsSolid(x - 1, y, z);
                        bool isSolidAbove = IsSolid(x, y + 1, z);
                        bool isSolidBelow = IsSolid(x, y - 1, z);
                        bool isSolidFront = IsSolid(x, y, z + 1);
                        bool isSolidBack = IsSolid(x, y, z - 1);

                        // if a block is completely surrounded by other blocks, it should be hidden:
                        bool isHidden = (isSolidRight && isSolidLeft && isSolidAbove && isSolidBelow && isSolidFront && isSolidBack);

                        // add this position to our list of voxels:
                        if (!isHidden) voxels.Add(new Vector3(x, y, z));
                    }
                }
            }
        }
        // for each position in our list, add a cube mesh to our list of meshes:
        CombineInstance[] combine = new CombineInstance[voxels.Count];
        for (int i = 0; i < combine.Length; i++)
        {
            combine[i].mesh = mesh.sharedMesh;
            combine[i].transform = Matrix4x4.Translate(voxels[i] * VoxelUniverse.VOXEL_SEPARATION);
        }
        // combine all of the meshes together into one mesh:
        mesh.mesh.CombineMeshes(combine, true);
    }

    /// <summary>
    /// This function checks a particular position and returns whether or not that position is "Solid"
    /// </summary>
    /// <param name="x">x coordinate</param>
    /// <param name="y">y coordinate</param>
    /// <param name="z">z coordinate</param>
    /// <returns>If true, a voxel should be rendered at this location.</returns>
    bool IsSolid(int x, int y, int z)
    {

        float val = 0; 

        if (x < 0 || x >= data.GetLength(0) ||
            y < 0 || y >= data.GetLength(1) ||
            z < 0 || z >= data.GetLength(2))
        {
            val = GetDensitySample(new Vector3(x, y, z));
        } else
        {
            val = data[x, y, z];
        }

        return (val > 0);
    }
}
