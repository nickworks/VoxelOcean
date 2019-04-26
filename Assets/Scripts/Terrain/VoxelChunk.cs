using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// This class generates the mesh and biome data of one "chunk" of the world. Each chunk contains a bunch of VoxelData objects that contain density data about the universe.
/// </summary>
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class VoxelChunk : MonoBehaviour
{
    /// <summary>
    /// This Chunk's grid-space position. Set once, in the SpawnChunk() function
    /// </summary>
    public Int3 gridPos { get; private set; }
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
    public void SpawnChunk(Int3 gridPos)
    {
        if (!EditorApplication.isPlaying) return;

        mesh = GetComponent<MeshFilter>();
        GenerateNoiseData();
        GenerateMesh();

        this.gridPos = gridPos;

        if (!VoxelUniverse.main.terrainOnly)
        {
            LifeSpawner spawner = GetComponent<LifeSpawner>();
            if (spawner) spawner.SpawnSomeLife();
        }
    }
    /// <summary>
    /// Generate noise data and cache in a huge array
    /// </summary>
    void GenerateNoiseData()
    {
        int res = VoxelUniverse.main.voxelsPerChunk;
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
                    // for reach voxel in this chunk:

                    // find voxel's position:
                    Vector3 pos = new Vector3(x, y, z) * VoxelUniverse.main.voxelSize;

                    // build voxel data object (cached)
                    VoxelData voxel = new VoxelData(pos);

                    // tell the voxel to sample the density in its 8 corners:
                    voxel.SetDensityData(transform.position);

                    // store the data:
                    data[x, y, z] = voxel;
                }
            }
        }
    }
    
    /// <summary>
    /// Generates a Biome at a given position
    /// </summary>
    /// <param name="pos">The position to sample the biome field, in local space.</param>
    /// <returns>A Biome object with data about which biome inhabits this space.</returns>
    Biome PickBiomeAtPos(Vector3 pos)
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
        h = Mathf.Round(h * Biome.COUNT);
        int biome_num = (int)h;

        // create and return the biome:
        return Biome.FromInt(biome_num);
    }
    /// <summary>
    /// This function builds the mesh by implementing a Cube Marching algorithm, removing duplicate vertices, calculating normals, and generating biome-based vertex colors.
    /// </summary>
    private void GenerateMesh()
    {
        List<VoxelData.Tri> geom = new List<VoxelData.Tri>();

        for (int x = 0; x < data.GetLength(0); x++)
        {
            for (int y = 0; y < data.GetLength(1); y++)
            {
                for (int z = 0; z < data.GetLength(2); z++)
                {
                    if (!data[x, y, z].IsHidden(0))
                    {
                        data[x, y, z].MarchCube(0, geom);
                    }
                }
            }
        }

        // combine all of the tris together into one mesh:
        Mesh mesh = MakeMeshFromTris(geom);

        // remove duplicate vertices:
        this.mesh.mesh = MeshTools.RemoveDuplicates(mesh, true);

        SetVertexColors();
    }
    /// <summary>
    /// This function makes a Mesh from a list of Tri objects.
    /// </summary>
    /// <param name="geom">The list of Tris to use</param>
    /// <returns>the generated mesh object.</returns>
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
    /// <summary>
    /// Generates vertex colors for every vertex in the MeshFilter's mesh.
    /// </summary>
    private void SetVertexColors()
    {
        List<Color> colors = new List<Color>();
        foreach (Vector3 p in mesh.mesh.vertices)
        {
            //Vector3 pos = p + transform.position;
            Biome biome = PickBiomeAtPos(p);
            colors.Add(biome.GetVertexColor());
        }
        mesh.mesh.SetColors(colors);
    }
    void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        Gizmos.color = Color.yellow;
        
        Gizmos.color = Color.blue;
        VoxelUniverse.main.DrawGizmoChunk(gridPos);

    }
}
