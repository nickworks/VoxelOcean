using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Modular Asset Coral Mesh
/// Created by Cameron Garchow to mimic Crystalline Flower Structures
/// Based on scientific and a mix of parasitic crystal structures
/// Mimicing in a Crystal Biome a naturalistic Flower\
/// With an emissive mapping and others this object 
/// Is meant to glow and spread the crystalline disease
/// The idea behind it is to create crystalline structures that 'spread' like a disease.
/// This can be changed to any value we want, we could take this into a game controller and randomize it
/// For this we can randomize its scale
/// </summary>
public class CoralCrystalFlowerSpawn : MonoBehaviour
{
    [Range(2, 10)] public int iterations = 3;
    [Range(0, 50)] public int angle1 = 45;
    [Range(0, 50)] public int angle2 = 45;
    [Range(0, 50)] public int angle3 = 45;
    [Range(0, 1)] public float scalar = 0.5f;
    [Range(.25f, .8f)] public float objpos = 0.5f;
    public Vector3 branchScale = new Vector3(.25f, 2, .25f);
    // Start is called before the first frame update
    void Start()
    {
        Build();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Build ()
    {
        List<CombineInstance> meshes = new List<CombineInstance>();

        Grow(meshes, iterations, Vector3.zero, Quaternion.identity, 1);

        Mesh mesh = new Mesh();
        mesh.CombineMeshes(meshes.ToArray());

        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;

    }
    /// <summary>
    /// Grow
    /// Grows objects from a single branch that is then randomized aned give iterations that the player can control.
    /// Objects are controlled entirely in the grow function
    /// </summary>
    /// <param name="meshes"> mesh that it takes</param>
    /// <param name="num"> number of meshes</param>
    /// <param name="pos"> postion of the object</param>
    /// <param name="rot"> rotation of the obj</param>
    /// <param name="scale"> objects scale randomized</param>
    private void Grow(List<CombineInstance> meshes, int num, Vector3 pos, Quaternion rot, float scale)
    {
        if (num <= 0) return; // stop recursive function
        RandomizeValues();

        CombineInstance inst = new CombineInstance();
        inst.mesh = MakeCube(num);
        //inst.transform =
        inst.transform = Matrix4x4.TRS(pos, rot, branchScale * scale);

        meshes.Add(inst);

        num--;

        pos = inst.transform.MultiplyPoint(new Vector3(0, 1, 0));
        Vector3 sidePos = inst.transform.MultiplyPoint(new Vector3(objpos, objpos, objpos));
        Quaternion rot1 = rot * Quaternion.Euler(angle1, angle2, angle3);
        Quaternion rot2 = rot * Quaternion.Euler(-angle1, -angle2, -angle3);
        scale *= scalar;

        Grow(meshes, num, pos, rot1, scale); //doing one for tendril like
        Grow(meshes, num, sidePos, rot2, scale); // doing for tendril 2
        Grow(meshes, num, pos, rot2, scale); // tendril 3
    }
    /// <summary>
    /// Randomize Values
    /// Randomizes the objects within the grown objects.
    /// </summary>
    private void RandomizeValues()
    {
        angle1 = Random.Range(5, 30);
        angle2 = Random.Range(5, 40);
        angle3 = Random.Range(5, 40);
        scalar = Random.Range(.5f, 1);
    }

    //Cube Data
    // Makes a cube
    /// <summary>
    /// Makes a cube with data taken from X,Y,Z coords
    /// </summary>
    /// <returns></returns>
    private Mesh MakeCube(int num)
    {
        List<Vector3> vertices = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<Vector3> normals = new List<Vector3>();
        List<Color> colors = new List<Color>();
        List<int> tris = new List<int>();
        //FRONT
        vertices.Add(new Vector3(-0.5f, 0, -0.5f));
        vertices.Add(new Vector3(-0.5f, 1, -0.5f));
        vertices.Add(new Vector3(+0.5f, 1, -0.5f));
        vertices.Add(new Vector3(+0.5f, 0, -0.5f));
        normals.Add(new Vector3(0, 0, -1));
        normals.Add(new Vector3(0, 0, -1));
        normals.Add(new Vector3(0, 0, -1));
        normals.Add(new Vector3(0, 0, -1));
        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 0));
        uvs.Add(new Vector2(1, 1));
        tris.Add(0);
        tris.Add(1);
        tris.Add(2);
        tris.Add(2);
        tris.Add(3);
        tris.Add(0);

        //Back
        vertices.Add(new Vector3(-0.5f, 0, +0.5f));
        vertices.Add(new Vector3(+0.5f, 0, +0.5f));
        vertices.Add(new Vector3(+0.5f, 1, +0.5f));
        vertices.Add(new Vector3(-0.5f, 1, +0.5f));
        normals.Add(new Vector3(0, 0, +1));
        normals.Add(new Vector3(0, 0, +1));
        normals.Add(new Vector3(0, 0, +1));
        normals.Add(new Vector3(0, 0, +1));
        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 0));
        uvs.Add(new Vector2(1, 1));
        tris.Add(4);
        tris.Add(5);
        tris.Add(6);
        tris.Add(6);
        tris.Add(7);
        tris.Add(4);


        //Left
        vertices.Add(new Vector3(-0.5f, 0, -0.5f));
        vertices.Add(new Vector3(-0.5f, 0, +0.5f));
        vertices.Add(new Vector3(-0.5f, 1, +0.5f));
        vertices.Add(new Vector3(-0.5f, 1, -0.5f));
        normals.Add(new Vector3(-1, 0, 0));
        normals.Add(new Vector3(-1, 0, 0));
        normals.Add(new Vector3(-1, 0, 0));
        normals.Add(new Vector3(-1, 0, 0));
        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 0));
        uvs.Add(new Vector2(1, 1));
        tris.Add(8);
        tris.Add(9);
        tris.Add(10);
        tris.Add(10);
        tris.Add(11);
        tris.Add(8);

        //Right
        vertices.Add(new Vector3(+0.5f, 0, -0.5f));
        vertices.Add(new Vector3(+0.5f, 1, -0.5f));
        vertices.Add(new Vector3(+0.5f, 1, 0.5f));
        vertices.Add(new Vector3(+0.5f, 0, +0.5f));
        normals.Add(new Vector3(+1, 0, 0));
        normals.Add(new Vector3(+1, 0, 0));
        normals.Add(new Vector3(+1, 0, 0));
        normals.Add(new Vector3(+1, 0, 0));
        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 0));
        uvs.Add(new Vector2(1, 1));
        tris.Add(12);
        tris.Add(13);
        tris.Add(14);
        tris.Add(14);
        tris.Add(15);
        tris.Add(12);


        //top
        vertices.Add(new Vector3(-0.5f, 1, -0.5f));
        vertices.Add(new Vector3(-0.5f, 1, 0.5f));
        vertices.Add(new Vector3(0.5f, 1, 0.5f));
        vertices.Add(new Vector3(0.5f, 1, -0.5f));
        normals.Add(new Vector3(0, 1, 0));
        normals.Add(new Vector3(0, 1, 0));
        normals.Add(new Vector3(0, 1, 0));
        normals.Add(new Vector3(0, 1, 0));
        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 1));
        uvs.Add(new Vector2(1, 0));
        tris.Add(16);
        tris.Add(17);
        tris.Add(18);
        tris.Add(18);
        tris.Add(19);
        tris.Add(16);

        //bottom
        vertices.Add(new Vector3(-0.5f, 0, -0.5f));
        vertices.Add(new Vector3(-0.5f, 0, 0.5f));
        vertices.Add(new Vector3(0.5f, 0, 0.5f));
        vertices.Add(new Vector3(0.5f, 0, -0.5f));
        normals.Add(new Vector3(0, -1, 0));
        normals.Add(new Vector3(0, -1, 0));
        normals.Add(new Vector3(0, -1, 0));
        normals.Add(new Vector3(0, -1, 0));
        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 1));
        uvs.Add(new Vector2(1, 0));
        tris.Add(20);
        tris.Add(21);
        tris.Add(22);
        tris.Add(22);
        tris.Add(23);
        tris.Add(20);

        //Set hue min and Max
        float hueMin = .6f;
        float hueMax = Random.Range(.7f, 1);

        float hue = Mathf.Lerp(hueMin, hueMax, (num / (float)iterations));
        /*
         * for each vertices located in the array color them in a hue 
        */
        foreach (Vector3 pos in vertices)
        {
            float tempHue = hue;

            Color color = Color.HSVToRGB(tempHue, 1, 1);

            colors.Add(color);
        }


        Mesh mesh = new Mesh();

        mesh.SetVertices(vertices);
        mesh.SetUVs(0, uvs);
        mesh.SetNormals(normals);
        mesh.SetTriangles(tris, 0);
        mesh.SetColors(colors);

        return mesh;
    }


}
/// <summary>
/// Editor
/// Editor for the CoralCrystalFlowerSpawn, allows building in editor for testing
/// </summary>
[CustomEditor(typeof(CoralCrystalFlowerSpawn))]
public class CoralCrystalFlowerSpawnEditor : Editor
{

    override public void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("GROW!"))
        {
            CoralCrystalFlowerSpawn c = (target as CoralCrystalFlowerSpawn);
            c.Build();
        }

    }
}
