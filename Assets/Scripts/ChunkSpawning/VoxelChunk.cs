using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// One chunk of voxels. Each chunk contains a bunch of "voxels". 
/// </summary>
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class VoxelChunk : MonoBehaviour
{
    [Range(-.2f, .2f)] public float threshold = 0;

    /// <summary>
    /// Cached noise data. This is used as "density" to determin whether or not voxels are solid.
    /// </summary>
    VoxelData[,,] data;
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

        LifeSpawner spawner = GetComponent<LifeSpawner>();
        if (spawner) spawner.SpawnSomeLife();
    }
    /// <summary>
    /// Generate noise data and cache in a huge array
    /// </summary>
    void GenerateNoiseData()
    {
        int res = VoxelUniverse.main.resPerChunk;
        data = new VoxelData[res, res, res];

        int sizeX = data.GetLength(0);
        int sizeY = data.GetLength(1);
        int sizeZ = data.GetLength(2);

        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                for (int z = 0; z < sizeZ; z++)
                {
                    Vector3 pos = new Vector3(x, y, z);
                    VoxelData voxel = new VoxelData(pos, PickBiomeAtPos(pos));
                    // set the densities of the 8 corners of the cube:
                    for (int i = 0; i < voxel.positions.Length; i++)
                        voxel.densities[i] = GetDensitySample(voxel.center + voxel.positions[i]);
                    // store the data:
                    data[x, y, z] = voxel;
                }
            }
        }
    }
    /// <summary>
    /// Samples a given grid position and returns the noise value at this position.
    /// </summary>
    /// <param name="pos">A position in world space.</param>
    /// <returns>The density of the given position. Depending on the noise function used, this should be in the -1 to +1 range.</returns>
    float GetDensitySample(Vector3 pos)
    {
        float res = 0;
        foreach (VoxelUniverse.SignalField field in VoxelUniverse.main.signalFields)
        {
            Vector3 p = pos + transform.position; // convert from local coordinates to world coordinates
            p /= field.zoom; // "zoom" in/out of the noise
            float val = Noise.Sample(p); // simplex.noise(pos.x, pos.y, pos.z);

            // use the vertical position to influence the density:
            val -= (p.y + field.verticalOffset) * field.flattenAmount;

            // adjust the final density using the densityBias:
            val -= field.densityBias;

            // adjust how various fields are mixed together:
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
                case VoxelUniverse.SignalType.None:
                    break;
            }
        }
        return res;
    }

    LifeSpawner.Biome PickBiomeAtPos(Vector3 pos)
    {
        // TODO: maybe a bunch of this logic can be moved into the LifeSpawner.Biome struct?

        pos += transform.position;
        pos /= VoxelUniverse.main.biomeScaling;

        // SAMPLE 1 noisefield at 3 arbitrary locations:
        Vector3 offsetR = new Vector3(123, 456, 789);
        Vector3 offsetG = new Vector3(-99, 999, 300);
        Vector3 offsetB = new Vector3(900, 500, -99);
        float r = Noise.Sample(pos + offsetR);
        float g = Noise.Sample(pos + offsetG);
        float b = Noise.Sample(pos + offsetB);

        // TODO: redo/improve the calculations below

        // convert values from approx. (-.2 to .2) to (-1.1 to 1.1):
        r *= 5.5f;
        g *= 5.5f;
        b *= 5.5f;
        // redistribute values non-linearly:
        r *= r;
        g *= g;
        b *= b;
        // shift range from (+- 1.21) to (+- .5):
        r /= 2.4f;
        g /= 2.4f;
        b /= 2.4f;
        // shift from range from (+- .5) to (0 to 1):
        r += .5f;
        g += .5f;
        b += .5f;

        // Convert from RGB to HSV:
        Color.RGBToHSV(new Color(r,g,b),out float h, out float s, out float v);

        //posterize the hue value:
        h = Mathf.Round(h * LifeSpawner.Biome.COUNT);
        int biome_num = (int)h;

        // create and return the biome:
        return LifeSpawner.Biome.FromInt(biome_num);
    }
    /// <summary>
    /// This function builds the mesh by copying the cube over and over again
    /// </summary>
    void GenerateMesh()
    {
        mesh.mesh = new Mesh();

        List<CombineInstance> meshes = new List<CombineInstance>();

        for (int x = 0; x < data.GetLength(0); x++)
        {
            for (int y = 0; y < data.GetLength(1); y++)
            {
                for (int z = 0; z < data.GetLength(2); z++)
                {
                    if (!data[x, y, z].IsHidden(threshold))
                    {
                        // make a voxel mesh
                        meshes.Add(data[x, y, z].MakeMesh(threshold));
                    }
                }
            }
        }

        // combine all of the meshes together into one mesh:
        mesh.mesh.CombineMeshes(meshes.ToArray(), true);
        mesh.mesh.RecalculateNormals(); // TODO: calculate our normals by hand instead
    }
}
