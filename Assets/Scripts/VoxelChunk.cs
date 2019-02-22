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


    [Range(-.2f, .2f)] public float threshold = 0;

    struct VoxelData2
    {
        public Vector3 center;
        public Vector3[] positions;
        public float[] densities;
        public int biome;
        private Color biomecolor;

        struct Tri
        {
            public Vector3 a;
            public Vector3 b;
            public Vector3 c;
            public Color color;
            public Tri(Vector3 a, Vector3 b, Vector3 c)
            {
                this.a = a;
                this.b = b;
                this.c = c;
                this.color = Color.white;
            }
            public Tri(Vector3 a, Vector3 b, Vector3 c, Color color)
            {
                this.a = a;
                this.b = b;
                this.c = c;
                this.color = color;
            }
            Vector3 normal
            {
                get { return Vector3.Cross(b - a, c - a).normalized; }
            }
            public void AddTo(List<Vector3> verts, List<int> tris, List<Color> colors, List<Vector3> normals, bool flip = false)
            {
                int offset = verts.Count;
                verts.Add(a);
                verts.Add(flip ? c : b);
                verts.Add(flip ? b : c);
                tris.Add(offset + 0);
                tris.Add(offset + 1);
                tris.Add(offset + 2);
                colors.Add(color);
                colors.Add(color);
                colors.Add(color);
                Vector3 normal = this.normal;
                normals.Add(normal);
                normals.Add(normal);
                normals.Add(normal);
            }
        }

        public VoxelData2(Vector3 pos, int biome)
        {
            this.center = pos;
            this.biome = biome;
            this.biomecolor = new Color();
            this.positions = new Vector3[] {
                new Vector3(+0.5f, +0.5f, -0.5f), // R T F
                new Vector3(-0.5f, +0.5f, -0.5f), // L T F
                new Vector3(+0.5f, +0.5f, +0.5f), // R T B
                new Vector3(-0.5f, +0.5f, +0.5f), // L T B
                new Vector3(+0.5f, -0.5f, -0.5f), // R B F
                new Vector3(-0.5f, -0.5f, -0.5f), // L B F
                new Vector3(+0.5f, -0.5f, +0.5f), // R B B
                new Vector3(-0.5f, -0.5f, +0.5f)  // L B B
            };


            this.densities = new float[this.positions.Length];
        }
        public void SetDensity(int index, float value)
        {
            densities[index] = value;
        }
        public bool IsHidden(float threshold = 0)
        {
            int solidness = -1;
            foreach(float density in densities)
            {
                bool isSolid = (density > threshold);
                if (solidness == 0 && isSolid == true) return false; // a mix of solid & unsolid
                if (solidness == 1 && isSolid == false) return false; // a mix of solid & unsolid
                if (solidness == -1) solidness = isSolid ? 1 : 0;
            }
            return true; // all densities were solid or were unsolid
        }
        public CombineInstance MakeMesh(float densityThreshold = 0)
        {
            CombineInstance voxel = new CombineInstance();
            voxel.mesh = MakeGeometry(densityThreshold);
            voxel.transform = Matrix4x4.Translate(center * VoxelUniverse.VOXEL_SEPARATION);
            return voxel;
        }
        private Mesh MakeGeometry(float densityThreshold = 0)
        {
            List<Vector3> verts = new List<Vector3>();
            List<Vector2> uvs = new List<Vector2>();
            List<Vector3> normals = new List<Vector3>();
            List<int> tris = new List<int>();
            List<Color> colors = new List<Color>();

            biomecolor = Color.HSVToRGB(biome / (float)VoxelUniverse.BIOME_COUNT, 1, 1);

            MarchTetrahedron(densityThreshold, verts, normals, colors, tris, 0, 2, 3, 7);
            MarchTetrahedron(densityThreshold, verts, normals, colors, tris, 0, 6, 2, 7);
            MarchTetrahedron(densityThreshold, verts, normals, colors, tris, 0, 4, 6, 7);
            MarchTetrahedron(densityThreshold, verts, normals, colors, tris, 0, 5, 4, 7);
            MarchTetrahedron(densityThreshold, verts, normals, colors, tris, 0, 1, 5, 7);
            MarchTetrahedron(densityThreshold, verts, normals, colors, tris, 0, 3, 1, 7);

            // TODO: remove duplicate verts?
            // TODO: calculate normals from tris?

            Mesh mesh = new Mesh();
            mesh.SetVertices(verts);
            mesh.SetTriangles(tris, 0);
            //mesh.SetUVs(0, uvs);
            //mesh.SetNormals(normals);
            mesh.SetColors(colors);
            return mesh;
        }
        private void MarchTetrahedron(float iso, List<Vector3> verts, List<Vector3> normals, List<Color> colors, List<int> tris, int i0, int i1, int i2, int i3)
        {

            int offset = verts.Count;
            byte bitfield = 0;
            if (densities[i0] > iso) bitfield |= 1;
            if (densities[i1] > iso) bitfield |= 2;
            if (densities[i2] > iso) bitfield |= 4;
            if (densities[i3] > iso) bitfield |= 8;

            List<Tri> geom = new List<Tri>();

            switch (bitfield) // generate Tri objects
            {
                case 0x00: // 0000
                case 0x0F: // 1111
                    break;
                case 0x0E: // 1110
                case 0x01: // 0001
                    {
                        Vector3 a = LerpEdge(iso, positions[i0], positions[i1], densities[i0], densities[i1]);
                        Vector3 b = LerpEdge(iso, positions[i0], positions[i2], densities[i0], densities[i2]);
                        Vector3 c = LerpEdge(iso, positions[i0], positions[i3], densities[i0], densities[i3]);
                        geom.Add(new Tri(a, b, c, biomecolor));
                    }
                    break;
                
                case 0x0D: // 1101
                case 0x02: // 0010
                    {
                        Vector3 a = LerpEdge(iso, positions[i1], positions[i0], densities[i1], densities[i0]);
                        Vector3 b = LerpEdge(iso, positions[i1], positions[i3], densities[i1], densities[i3]);
                        Vector3 c = LerpEdge(iso, positions[i1], positions[i2], densities[i1], densities[i2]);
                        geom.Add(new Tri(a, b, c, biomecolor));
                    }
                    break;
                case 0x0C: // 1100 
                case 0x03: // 0011
                    {
                        Vector3 a = LerpEdge(iso, positions[i0], positions[i3], densities[i0], densities[i3]); //0
                        Vector3 b = LerpEdge(iso, positions[i0], positions[i2], densities[i0], densities[i2]); //1
                        Vector3 c = LerpEdge(iso, positions[i1], positions[i3], densities[i1], densities[i3]); //2
                        Vector3 d = LerpEdge(iso, positions[i1], positions[i2], densities[i1], densities[i2]); //3
                        geom.Add(new Tri(a, c, b, biomecolor));
                        geom.Add(new Tri(c, d, b, biomecolor));
                    }
                    break;
                case 0x0B: // 1011
                case 0x04: // 0100
                    {
                        Vector3 a = LerpEdge(iso, positions[i2], positions[i0], densities[i2], densities[i0]);
                        Vector3 b = LerpEdge(iso, positions[i2], positions[i1], densities[i2], densities[i1]);
                        Vector3 c = LerpEdge(iso, positions[i2], positions[i3], densities[i2], densities[i3]);
                        geom.Add(new Tri(a, b, c, biomecolor));
                    }
                    break;
                case 0x0A: // 1010
                case 0x05: // 0101
                    {
                        Vector3 a = LerpEdge(iso, positions[i0], positions[i1], densities[i0], densities[i1]); // 0
                        Vector3 b = LerpEdge(iso, positions[i2], positions[i3], densities[i2], densities[i3]); // 1
                        Vector3 c = LerpEdge(iso, positions[i0], positions[i3], densities[i0], densities[i3]); // 2
                        Vector3 d = LerpEdge(iso, positions[i1], positions[i2], densities[i1], densities[i2]); // 3
                        geom.Add(new Tri(a, b, c, biomecolor));
                        geom.Add(new Tri(a, d, b, biomecolor));
                    }
                    break;
                case 0x09: // 1001
                case 0x06: // 0110
                    {
                        Vector3 a = LerpEdge(iso, positions[i0], positions[i1], densities[i0], densities[i1]);
                        Vector3 b = LerpEdge(iso, positions[i1], positions[i3], densities[i1], densities[i3]);
                        Vector3 c = LerpEdge(iso, positions[i2], positions[i3], densities[i2], densities[i3]);
                        Vector3 d = LerpEdge(iso, positions[i0], positions[i2], densities[i0], densities[i2]);
                        geom.Add(new Tri(a, b, c, biomecolor));
                        geom.Add(new Tri(a, c, d, biomecolor));
                    }
                    break;
                case 0x07: // 0111
                case 0x08: // 1000
                    {
                        Vector3 a = LerpEdge(iso, positions[i3], positions[i0], densities[i3], densities[i0]);
                        Vector3 b = LerpEdge(iso, positions[i3], positions[i2], densities[i3], densities[i2]);
                        Vector3 c = LerpEdge(iso, positions[i3], positions[i1], densities[i3], densities[i1]);
                        geom.Add(new Tri(a, c, b, biomecolor));
                    }
                    break;
                    /*
                    /**/
                    
            }

            bool flipWindingOrder = (bitfield >= 0x08);
            foreach(Tri t in geom)
            {
                t.AddTo(verts, tris, colors, normals, flipWindingOrder);
            }


        }
        private Vector3 LerpEdge(float iso, Vector3 p1, Vector3 p2, float val1, float val2)
        {

            if (Mathf.Abs(iso - val1) < 0.00001f) return p1; // if p1 is very close to the threshold, just return p1
            if (Mathf.Abs(iso - val2) < 0.00001f) return p2; // if p2 is very close to the threshold, just return p2
            if (Mathf.Abs(val1 - val2) < 0.00001f) return p1; // if val1 and val2 are (almost) the same density
            float percent = (iso - val1) / (val2 - val1);

            return Vector3.Lerp(p1, p2, percent);
        }
    }
    
    /// <summary>
    /// Cached noise data. This is used as "density" to determin whether or not voxels are solid.
    /// </summary>
    VoxelData2[,,] data;
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
        data = new VoxelData2[res, res, res];

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
                    int biome = PickBiomeAtPos(pos);
                    VoxelData2 voxel = new VoxelData2(pos, biome);
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
        pos /= VoxelUniverse.main.biomeScaling;

        Vector3 offsetR = new Vector3(123, 456, 789);
        Vector3 offsetG = new Vector3(-99, 999, 300);
        Vector3 offsetB = new Vector3(900, 500, -99);

        float r = Noise.Sample(pos + offsetR) * 5.5f;
        float g = Noise.Sample(pos + offsetG) * 5.5f;
        float b = Noise.Sample(pos + offsetB) * 5.5f;

        r *= r;
        g *= g;
        b *= b;

        r += .5f;
        g += .5f;
        b += .5f;


        float h = 0;
        float s = 0;
        float v = 0;

        Color.RGBToHSV(new Color(r,g,b),out h, out s, out v);

        //posterize the noise:
        h = Mathf.Round(h * VoxelUniverse.BIOME_COUNT);
        int biome_num = (int)h;
        
        return biome_num;
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
                    if (!data[x,y,z].IsHidden(threshold))
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
