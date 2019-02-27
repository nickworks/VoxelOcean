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
                    VoxelData voxel = new VoxelData(pos);
                    // set the densities of the 8 corners of the cube:
                    for (int i = 0; i < VoxelData.positions.Length; i++)
                        voxel.densities[i] = GetDensitySample(voxel.center + VoxelData.positions[i]);
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
        List<VoxelData.Tri> geom = new List<VoxelData.Tri>();

        for (int x = 0; x < data.GetLength(0); x++)
        {
            for (int y = 0; y < data.GetLength(1); y++)
            {
                for (int z = 0; z < data.GetLength(2); z++)
                {
                    if (!data[x, y, z].IsHidden(threshold))
                    {
                        data[x, y, z].MarchCube(threshold, geom);
                    }
                }
            }
        }

        // combine all of the tris together into one mesh:
        Mesh mesh = MakeMeshFromTris(geom);

        // remove duplicate vertices:
        this.mesh.mesh = RemoveDuplicates(mesh);

        // calculate those normals:
        this.mesh.mesh.RecalculateNormals();

        SetVertexColors();
    }
    private Mesh MakeMeshFromTris(List<VoxelData.Tri> geom)
    {

        List<Vector3> verts = new List<Vector3>();
        List<int> tris = new List<int>();

        for (int i = 0; i < geom.Count; i++)
        {
            int index = verts.Count;
            verts.Add(geom[i].a);
            verts.Add(geom[i].b);
            verts.Add(geom[i].c);
            tris.Add(index + 0);
            tris.Add(index + 1);
            tris.Add(index + 2);
        }

        // create a mesh from the verts and tris:
        Mesh mesh = new Mesh();
        mesh.SetVertices(verts);
        mesh.SetTriangles(tris, 0);
        return mesh;
    }
    private Mesh RemoveDuplicates(Mesh complexMesh, bool printDebug = false)
    {
        List<Vector3> verts = new List<Vector3>();
        List<int> tris = new List<int>();

        complexMesh.GetVertices(verts);
        complexMesh.GetTriangles(tris, 0);

        int count1 = verts.Count;

        for (int i = 0; i < verts.Count; i++)
        {
            // find duplicates:
            for (int j = 0; j < verts.Count; j++)
            {
                if (i == j) continue;
                if (j >= verts.Count) break;
                if (i >= verts.Count) break;
                if (verts[i] == verts[j]) // if a duplicate vert:
                {
                    verts.RemoveAt(j); // remove it
                    for(int k = 0; k < tris.Count; k++)
                    {
                        if (tris[k] == j) tris[k] = i;
                        if (tris[k] > j) tris[k] = tris[k] - 1;
                    }
                }
            }
        }
        int count2 = verts.Count;

        if(printDebug) print($"{count1} reduced to {count2}");

        Mesh mesh = new Mesh();
        mesh.SetVertices(verts);
        mesh.SetTriangles(tris, 0);
        return mesh;

    } // RemoveDuplicates()

    void SetVertexColors()
    {
        List<Color> colors = new List<Color>();
        foreach (Vector3 p in mesh.mesh.vertices)
        {
            //Vector3 pos = p + transform.position;
            LifeSpawner.Biome biome = PickBiomeAtPos(p);
            colors.Add(biome.GetVertexColor());
        }
        mesh.mesh.SetColors(colors);
    }
}
