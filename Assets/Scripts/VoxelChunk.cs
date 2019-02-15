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

    struct VoxelData
    {
        enum FaceDirection
        {
            Front,
            Back,
            Left,
            Right,
            Top,
            Bottom
        }
        delegate void MakeAFace(FaceDirection dir);
        static Vector3[] cube = new Vector3[] {
            new Vector3(0, 0, 0), //LBF 0
            new Vector3(0, 1, 0), //LTF 1
            new Vector3(1, 1, 0), //RTF 2
            new Vector3(1, 0, 0), //RBF 3
            new Vector3(0, 0, 1), //LBB 4
            new Vector3(0, 1, 1), //LTB 5
            new Vector3(1, 1, 1), //RTB 6
            new Vector3(1, 0, 1)  //RBB 7
        };

        public Vector3 pos;
        public float density;
        public int biome;

        public bool isSolid { get { return density > 0; } }
        public bool isHidden { get { return (isSolidAbove && isSolidBack && isSolidBelow && isSolidFront && isSolidLeft && isSolidRight);  } }

        public bool isSolidRight;
        public bool isSolidLeft;
        public bool isSolidAbove;
        public bool isSolidBelow;
        public bool isSolidFront;
        public bool isSolidBack;

        public VoxelData(float density, Vector3 pos, int biome)
        {
            this.density = density;
            this.pos = pos;
            this.biome = biome;
            isSolidRight = isSolidLeft = isSolidAbove = isSolidBelow = isSolidBack = isSolidFront = false;
        }
        public CombineInstance MakeVoxel()
        {
            CombineInstance voxel = new CombineInstance();
            voxel.mesh = MakeGeometry();
            voxel.transform = Matrix4x4.Translate(pos * VoxelUniverse.VOXEL_SEPARATION);
            return voxel;
        }
        public Mesh MakeGeometry()
        {
            List<Vector3> verts = new List<Vector3>();
            List<Vector2> uvs = new List<Vector2>();
            List<Vector3> normals = new List<Vector3>();
            List<int> tris = new List<int>();
            List<Color> colors = new List<Color>();

            Color color =Color.HSVToRGB(biome/13f, 1, 1);

            MakeAFace addFace = (FaceDirection dir) =>
            {
                int index = verts.Count;
                if (dir == FaceDirection.Back) verts.AddRange(new Vector3[] { cube[0], cube[1], cube[2], cube[3] });
                if (dir == FaceDirection.Front) verts.AddRange(new Vector3[] { cube[4], cube[7], cube[6], cube[5] });
                if (dir == FaceDirection.Left) verts.AddRange(new Vector3[] { cube[0], cube[4], cube[5], cube[1] });
                if (dir == FaceDirection.Right) verts.AddRange(new Vector3[] { cube[3], cube[2], cube[6], cube[7] });
                if (dir == FaceDirection.Top) verts.AddRange(new Vector3[] { cube[1], cube[5], cube[6], cube[2] });
                if (dir == FaceDirection.Bottom) verts.AddRange(new Vector3[] { cube[0], cube[3], cube[7], cube[4] });
                Vector3 normal = Vector3.zero;
                if (dir == FaceDirection.Back) normal = Vector3.back;
                if (dir == FaceDirection.Front) normal = Vector3.forward;
                if (dir == FaceDirection.Left) normal = Vector3.left;
                if (dir == FaceDirection.Right) normal = Vector3.right;
                if (dir == FaceDirection.Top) normal = Vector3.up;
                if (dir == FaceDirection.Bottom) normal = Vector3.down;
                normals.Add(normal);
                normals.Add(normal);
                normals.Add(normal);
                normals.Add(normal);
                uvs.Add(new Vector2(0, 0));
                uvs.Add(new Vector2(0, 1));
                uvs.Add(new Vector2(1, 1));
                uvs.Add(new Vector2(1, 0));
                colors.Add(color);
                colors.Add(color);
                colors.Add(color);
                colors.Add(color);
                tris.Add(index + 0);
                tris.Add(index + 1);
                tris.Add(index + 2);
                tris.Add(index + 2);
                tris.Add(index + 3);
                tris.Add(index + 0);
            };

            if (!isSolidBack) addFace(FaceDirection.Back);
            if (!isSolidBelow) addFace(FaceDirection.Bottom);
            if (!isSolidFront) addFace(FaceDirection.Front);
            if (!isSolidLeft) addFace(FaceDirection.Left);
            if (!isSolidRight) addFace(FaceDirection.Right);
            if (!isSolidAbove) addFace(FaceDirection.Top);

            Mesh mesh = new Mesh();
            mesh.SetVertices(verts);
            mesh.SetUVs(0, uvs);
            mesh.SetNormals(normals);
            mesh.SetTriangles(tris, 0);
            mesh.SetColors(colors);
            return mesh;
        }

    }

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
                    float density = GetDensitySample(pos);
                    int biome = PickBiomeAtPos(pos);
                    data[x, y, z] = new VoxelData(density, pos, biome);
                }
            }
        }

        // update each voxel to store whether or not each neighbor is solid:
        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                for (int z = 0; z < sizeZ; z++)
                {
                    data[x, y, z].isSolidRight = IsSolid(x + 1, y, z);
                    data[x, y, z].isSolidAbove = IsSolid(x, y + 1, z);
                    data[x, y, z].isSolidFront = IsSolid(x, y, z + 1);
                    data[x, y, z].isSolidLeft  = IsSolid(x - 1, y, z);
                    data[x, y, z].isSolidBelow = IsSolid(x, y - 1, z);
                    data[x, y, z].isSolidBack  = IsSolid(x, y, z - 1);
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
                case VoxelUniverse.SignalType.None:
                    break;
            }
        }
        return res;
    }

    int PickBiomeAtPos(Vector3 pos)
    {
        // pick biome, associate with color:
        pos += transform.position;
        float val = Noise.Sample(pos / 150);
        val += .5f;
        val *= 200;
        return (int)(val % 13); // 13 students, 13 biomes
    }
    /// <summary>
    /// This function builds the mesh by copying the cube over and over again
    /// </summary>
    void GenerateMesh()
    {
        mesh.mesh = new Mesh();

        List<CombineInstance> voxels = new List<CombineInstance>();

        for (int x = 0; x < data.GetLength(0); x++)
        {
            for (int y = 0; y < data.GetLength(1); y++)
            {
                for (int z = 0; z < data.GetLength(2); z++)
                {
                    if (data[x,y,z].isSolid && !data[x,y,z].isHidden)
                    {
                        // make a voxel mesh
                        voxels.Add(data[x, y, z].MakeVoxel());
                    }
                }
            }
        }

        // combine all of the meshes together into one mesh:
        mesh.mesh.CombineMeshes(voxels.ToArray(), true);
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
            val = data[x, y, z].density;
        }

        return (val > 0);
    }
}
