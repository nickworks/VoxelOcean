using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoralGlowNew : MonoBehaviour
{
    //VARIABLES NEEDED
    /// <summary>
    /// Creates a variable to hold a reference to a renderer object
    /// </summary>
    private Renderer rend;

    /// <summary>
    /// A public enum that is used to set the state of the coral
    /// </summary>
    public enum Health
    {
        Normal, //This creates coral with the paremeters you set
        Growing,//Growing coral grows bigger and automatically gets sent through the grow function an extra number of times
        Sick,//Sick coral should be smaller than other coral
        big,
        small,
    }

    /// <summary>
    /// A modifier applied to the scale of the coral
    /// </summary>
    public float scaleMod = 1f;

    /// <summary>
    /// A enumeration variable
    /// </summary>
    [Tooltip("The state of the corals health")]
    public Health myHealth;

    /// <summary>
    /// A Vector3 used for branch scale
    /// </summary>
    [Tooltip("A Vector3 used to control the scale of branch segments for the coral")]
    public Vector3 branchScale = new Vector3(1, 1, 1);

    /// <summary>
    /// The number of iterations for fractal generation we want
    /// </summary>
    [Tooltip("The number of fractal iterations on this piece of coral. If absolute control is disabled this number will be affected by bonus iterations")]
    [Range(2, 10)] public int iterations = 3;

    /// <summary>
    /// The x rotation that should be applied to the fractal branch segments spawned
    /// </summary>
    [Tooltip("The amount of rotation that should be applied to a branch segment along the X axis")]
    public float xRotator = 45;
    /// <summary>
    /// The Z rotation that should be applied to the fractal branch segments spawned
    /// </summary>
    [Tooltip("The amount of rotation that should be applied to a branch segment along the Z axis")]
    public float zRotator = 45;
    /// <summary>
    /// The Y rotation that should be applied to the fractal branch segments spawned
    /// </summary>
    [Tooltip("The amount of rotation that should be applied to a branch segment along the Y axis")]
    public float yRotator = 45;

    //GLOW AMOUNT
    //TICKUP BOOL
    //COUNTDOWN FOR SHADER
    //BASE CHANCE
    //OTHER CHANCE

    [Range(1, 10)] public float baseChance = 1;
                         
    [Range(1, 10)] public float otherChance = 1;
                        
    [Range(1, 10)] public float healthModifier = 1;
                          
    [Range(1, 10)] public float sickModifier = 1;


    // Start is called before the first frame update
    void Start()
    {
        Build();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Build()
    {
        //A list of meshes
        List<CombineInstance> meshes = new List<CombineInstance>();

        //Our Grow function
        //Takes parameters:
        // -iterations: how many times to recourse through function
        // - meshes : the list of combined instances
        // - Vector3.zero : a zerod out vector for location
        // - Quaternion.identity : world rotation
        // - integer : the scale of the coral
        Grow(iterations, meshes, Vector3.zero, Quaternion.identity, scaleMod);
        //We create a new mesh object
        Mesh mesh = new Mesh();
        //We combine meshes and then place them inside of a meshes array
        mesh.CombineMeshes(meshes.ToArray());
        //We get our meshFilter component
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        //our meshfilters mesh is the mesh object we declared above
        meshFilter.mesh = mesh;
    }

    /// <summary>
    /// Our Grow() method to implement recursion and create our fractal coral
    /// </summary>
    /// <param name="num"> the remaining number of times to recourse through the method</param>
    /// <param name="meshes"> the list of meshes we are passing in</param>
    /// <param name="pos">the position to spawn the coral at</param>
    /// <param name="rot">the rotation to rotate the coral to</param>
    /// <param name="scale">the amount we are scaling out coral by</param>
    private void Grow(int num, List<CombineInstance> meshes, Vector3 pos, Quaternion rot, float scale)
    {
        //If remaining number of recurses is zero or less return out of the function to avoid an error
        if (num <= 0) return;

        //Create a new combined instance for our mesh
        CombineInstance inst = new CombineInstance();
        //This combined instances mesh is set equal to whatever is returned from the MakeCube() method
        inst.mesh = MakeCube();
        //We transform the instance by a 4x4 Matrix to modify it's position
        inst.transform = Matrix4x4.TRS(pos, rot, branchScale * scale);


        //We add the instance to our meshes list
        meshes.Add(inst);
        //We subtract from the amount of times we are going to recourse through the function
        num--;

        if(myHealth == Health.Normal)
        {
            pos = inst.transform.MultiplyPoint(new Vector3(0, .8f, 0));
            scale = 1;
            Quaternion rot1 = rot * Quaternion.Euler(0, Random.Range(0, 15), Random.Range(-15, 15));
            Grow(num, meshes, pos, rot1, scale);
        }



        //Our position is updated so we don't spawn coral pieces ontop of one another
        //pos = inst.transform.MultiplyPoint(new Vector3(.8f, .8f, 0)); //This is used to convert from one coordinate space to another
        //We calculate another sidePosition of the coral that is close to pos
        //Vector3 sidePos = inst.transform.MultiplyPoint(new Vector3(.10f, .8f, 0));



        //We create two rotations with only positive values in the x and z range
        //Quaternion rot1 = rot * Quaternion.Euler(0 + Random.Range(0, xRotator), 45 + Random.Range(0, yRotator), 45 + Random.Range(0, zRotator));
        //Quaternion rot2 = rot * Quaternion.Euler(0 + Random.Range(-xRotator, 0), 45 + Random.Range(0, yRotator), 45 + Random.Range(0, zRotator));
        //Scale is only reduced by 10%
       // scale *= .60f;







        //We call the GrowMethod Twice
       // Grow(num, meshes, pos, rot1, scale);
       // Grow(num, meshes, sidePos, rot2, scale);
            
        

    }//End private Grow() method


    private Health setHealth()
    {
        baseChance = Random.Range(1, 5);
        otherChance = Random.Range(1, 5);
        float newChance = baseChance + otherChance;
        newChance -= sickModifier;
        newChance += healthModifier;

        if(newChance >= 4.5)
        {
            myHealth = Health.Normal;
        }
        if(newChance >= 4.6 && newChance <= 6.5)
        {
            myHealth = Health.Growing;
            sickModifier += Random.Range(1, 3);
        }
        if(newChance >= 6.6 && newChance <= 8.0)
        {
            myHealth = Health.big;
        }
        if(newChance >= 8.1 && newChance <= 9.0)
        {
            myHealth = Health.Sick;
            sickModifier += Random.Range(1, 3);
        }
        if(newChance >= 9.1)
        {
            myHealth = Health.small;
        }


        return myHealth;
    }
    /// <summary>
    /// MakeCube() method used to generate a mesh
    /// </summary>
    /// <returns>Returns a cube mesh</returns>
    private Mesh MakeCube()
    {
        //A list of Vector3's used for vertex positions
        List<Vector3> vertices = new List<Vector3>();
        //facing front
        vertices.Add(new Vector3(0, 0, 0));
        vertices.Add(new Vector3(0, 1, 0));
        vertices.Add(new Vector3(1, 0, 0));
        vertices.Add(new Vector3(1, 1, 0));
        //Top
        vertices.Add(new Vector3(0, 1, 0));
        vertices.Add(new Vector3(0, 1, 1));
        vertices.Add(new Vector3(1, 1, 0));
        vertices.Add(new Vector3(1, 1, 1));
        //Away
        vertices.Add(new Vector3(0, 1, 1));
        vertices.Add(new Vector3(0, 0, 1));
        vertices.Add(new Vector3(1, 1, 1));
        vertices.Add(new Vector3(1, 0, 1));

        //Bottom
        vertices.Add(new Vector3(0, 0, 1));
        vertices.Add(new Vector3(0, 0, 0));
        vertices.Add(new Vector3(1, 0, 1));
        vertices.Add(new Vector3(1, 0, 0));

        //Right
        vertices.Add(new Vector3(1, 0, 0));
        vertices.Add(new Vector3(1, 1, 0));
        vertices.Add(new Vector3(1, 0, 1));
        vertices.Add(new Vector3(1, 1, 1));

        //Left


        vertices.Add(new Vector3(0, 1, 0));
        vertices.Add(new Vector3(0, 0, 0));
        vertices.Add(new Vector3(0, 1, 1));
        vertices.Add(new Vector3(0, 0, 1));



        /////////////////////////////////
        //A list of Vector2's used for mesh's uv's
        List<Vector2> uvs = new List<Vector2>();
        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 0));
        uvs.Add(new Vector2(1, 1));

        //Top
        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 0));
        uvs.Add(new Vector2(1, 1));
        //Away
        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 0));
        uvs.Add(new Vector2(1, 1));

        //Bottom
        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 0));
        uvs.Add(new Vector2(1, 1));

        //right
        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 0));
        uvs.Add(new Vector2(1, 1));

        //left
        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 0));
        uvs.Add(new Vector2(1, 1));

        //A list of Vector3's used for normals 
        List<Vector3> normals = new List<Vector3>();
        normals.Add(new Vector3(0, 0, 1));
        normals.Add(new Vector3(0, 0, 1));
        normals.Add(new Vector3(0, 0, 1));
        normals.Add(new Vector3(0, 0, 1));

        //Top
        normals.Add(new Vector3(0, 1, 0));
        normals.Add(new Vector3(0, 1, 0));
        normals.Add(new Vector3(0, 1, 0));
        normals.Add(new Vector3(0, 1, 0));

        //Away
        normals.Add(new Vector3(0, 0, 1));
        normals.Add(new Vector3(0, 0, 1));
        normals.Add(new Vector3(0, 0, 1));
        normals.Add(new Vector3(0, 0, 1));

        //Bottom
        normals.Add(new Vector3(0, -1, 0));
        normals.Add(new Vector3(0, -1, 0));
        normals.Add(new Vector3(0, -1, 0));
        normals.Add(new Vector3(0, -1, 0));

        //left
        normals.Add(new Vector3(1, 0, 0));
        normals.Add(new Vector3(1, 0, 0));
        normals.Add(new Vector3(1, 0, 0));
        normals.Add(new Vector3(1, 0, 0));

        //right
        normals.Add(new Vector3(-1, 0, 0));
        normals.Add(new Vector3(-1, 0, 0));
        normals.Add(new Vector3(-1, 0, 0));
        normals.Add(new Vector3(-1, 0, 0));

        //A list of integers used to create the triangles of the mesh
        List<int> tris = new List<int>();
        tris.Add(0);
        tris.Add(1);
        tris.Add(2);

        tris.Add(1);
        tris.Add(3);
        tris.Add(2);

        //Top
        tris.Add(4);
        tris.Add(5);
        tris.Add(6);

        tris.Add(5);
        tris.Add(7);
        tris.Add(6);

        //Away
        tris.Add(8);
        tris.Add(9);
        tris.Add(10);

        tris.Add(9);
        tris.Add(11);
        tris.Add(10);

        //Bottom
        tris.Add(12);
        tris.Add(13);
        tris.Add(14);

        tris.Add(13);
        tris.Add(15);
        tris.Add(14);

        //Left
        tris.Add(16);
        tris.Add(17);
        tris.Add(18);

        tris.Add(17);
        tris.Add(19);
        tris.Add(18);

        //Right
        tris.Add(20);
        tris.Add(21);
        tris.Add(22);

        tris.Add(21);
        tris.Add(23);
        tris.Add(22);
        //We create a new mesh object
        Mesh mesh = new Mesh();
        //We set the mesh's vertex positions using our Vector3 array
        mesh.SetVertices(vertices);
        //we set the mesh's uv's using our Vector2 uv array
        mesh.SetUVs(0, uvs);
        //we set the mesh's normals using our vector3 normal array
        mesh.SetNormals(normals);
        //we set the mesh's trianglees using our integer triangle array
        mesh.SetTriangles(tris, 0);
        //we return the mesh we created
        return mesh;
    }

}
