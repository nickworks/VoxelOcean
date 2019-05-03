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
        big,//Big coral should be bigger than other coral
        small,//Small coral should be smaller than other coral
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
    [Range(0, 1)] float glowAmount = 0;
    //TICKUP BOOL
    private bool tickup = true;

    
    

    /// <summary>
    /// A random number that the coral uses to choose its health state
    /// </summary>
    private float baseChance = 1;
           
    /// <summary>
    /// Another random number that is added to the coral to determine its health state
    /// </summary>
    private float otherChance = 1;
           
    /// <summary>
    /// A random number generated in a method that is subtracted from
    /// the total of baseChance and otherChance to increase the odds we will
    /// get normal, big, or growing coral
    /// </summary>
    private float healthModifier = 1;
    /// <summary>
    /// A random number generated in a method that is added t0
    /// the total of baseChance and otherChance to increase the odds
    /// we will get sick or small coral
    /// </summary>
    private float sickModifier = 1;


    // Start is called before the first frame update
    void Start()
    {
        //We get a reference to our renderer
        rend = GetComponent<Renderer>();
        //We get a reference to our CoralGlow Shader
        rend.material.shader = Shader.Find("CoralShaders/CoralGlow");
        //create a new color
        Color newColor = new Color(0.2f, 0.5f, 0.7f, 1);
        //we set the shaders color
        rend.material.SetColor("_PrimaryColor", newColor);
        //We set the shaders glow amount to random amount between 0 and 1
        rend.material.SetFloat("_Glow", glowAmount = Random.Range(0, 1));

        //We call the SetHealth method to determine what our corals health will be
        SetHealth();
        //We then build the coral 
        Build();
    }

    // Update is called once per frame
    void Update()
    {
       
        glowAmount = Mathf.PingPong(Time.time, 2.0f);
        rend.material.SetFloat("_Glow", glowAmount);
       
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

        //A series of if statements to change how the coral is made

        //If our coral is health.Normal
        if(myHealth == Health.Normal)
        {
            //Our position is updated so we don't spawn coral pieces ontop of one another
            pos = inst.transform.MultiplyPoint(new Vector3(.5f, .8f, 0));
            scale *= .6f;//We multiply the scale to keep it at whatever it was from the last iteration
            Quaternion rot1 = rot * Quaternion.Euler(0, Random.Range(0, 15), Random.Range(-15, 15));//Rotation values are keep small
            SetHealth(); //We set health for the next iteration of coral
            Grow(num, meshes, pos, rot1, scale);//We call grow only once
        } //End if Health.normal
        if(myHealth == Health.big)//If our coral is health.big
        {
            //We add a value from 1 to 3 to our num variable to increase coral iterations
            num += Random.Range(1, 3);
            //Our position is updated so we don't spawn coral pieces ontop of one another
            pos = inst.transform.MultiplyPoint(new Vector3(.6f, .8f, 0));
            //We multiply the scale by a random value so it is either the same or greater
            scale *= Random.Range(.8f, 1.2f);
            //Rotation values are keep small
            Quaternion rot1 = rot * Quaternion.Euler(0, Random.Range(0, 15), Random.Range(-15, 15));
            //We set health for the next iteration of coral
            SetHealth();
            //We call grow only once
            Grow(num, meshes, pos, rot1, scale);
        }//End if Health.normal
        if (myHealth == Health.small)//If our coral is health.small
        {
            //Our position is updated so we don't spawn coral pieces ontop of one another
            pos = inst.transform.MultiplyPoint(new Vector3(0, .8f, 0));
            //We multiply the scale by a random value so it is either smaller or the same
            scale *= Random.Range(.6f, .9f);
            //Rotation values are keep small
            Quaternion rot1 = rot * Quaternion.Euler(0, Random.Range(0, 15), Random.Range(-15, 15));
            //We set health for the next iteration of coral
            SetHealth();
            //We call grow only once
            Grow(num, meshes, pos, rot1, scale);
        }//End if Health.small
        if (myHealth == Health.Sick)//If our coral is health.Sick
        {
            //Our position is updated so we don't spawn coral pieces ontop of one another
            pos = inst.transform.MultiplyPoint(new Vector3(0, .8f, 0));
            //We multiply the scale by a random value so it is either smaller or the same
            scale *= Random.Range(.5f, .85f);
            //Rotation values are made more drastic
            Quaternion rot1 = rot * Quaternion.Euler(0, Random.Range(- 25, 45), Random.Range(-35, 35));
            //We set health for the next iteration of coral
            SetHealth();
            //We call grow only once
            Grow(num, meshes, pos, rot1, scale);
        }//End if Health.Sick
        if (myHealth == Health.Growing)//If our coral is health.Growing
        {
            //We add a value from 3 to 5 to our num variable to increase coral iterations
            num += Random.Range(3, 5);
            //Our position is updated so we don't spawn coral pieces ontop of one another
            pos = inst.transform.MultiplyPoint(new Vector3(.8f, .8f, 0));
            // We generate a sidePosition to pass to our other call of the grow method
            Vector3 sidePos = inst.transform.MultiplyPoint(new Vector3(.10f, .8f, 0));
            //We multiply the scale by a random value so it is either smaller or greater
            scale *= Random.Range(1f, 1.2f);
            //Rotation values are keep small
            Quaternion rot1 = rot * Quaternion.Euler(0, Random.Range(-15, 15), Random.Range(-15, 15));
            //We generate another rotation quaternion for our second call to the grow method
            Quaternion rot2 = rot * Quaternion.Euler(0, Random.Range(-15, 15), Random.Range(-15, 15));
            SetHealth();
            //We call grow twice 
            Grow(num, meshes, pos, rot1, scale);
            Grow(num, meshes, sidePos, rot2, scale);
        }//End if Health.Growing

    }//End private Grow() method

    /// <summary>
    /// A private method to set the Health enumeration of the coral
    /// </summary>
    /// <returns>returns myHealth doesn't really need to return this though now that I think about it</returns>
    private Health SetHealth()
    {
        //baseChance will always be a float from 1 to 5
        baseChance = Random.Range(1, 5);
        //otherChance will always be a float from 1 to 5
        otherChance = Random.Range(1, 5);
        //newChance is generated from baseChance being added to otherChance
        float newChance = baseChance + otherChance;
        //We then add the sickModifier to increase the chances that we will get sick or small coral
        newChance += sickModifier;
        //Then we subtract the health modifier so that we will more likely get normal, big or growing coral
        newChance -= healthModifier;

        //If the chance is between 0 and 2.5 we get normal coral
        if(newChance >= 0 && newChance <= 2.5)
        {
            myHealth = Health.Normal;
        }
        //If the chance is between 2.6 and 6.5 we get growing coral 
        //and increase the healthModifier by 1 to 3
        if(newChance >= 2.6 && newChance <= 6.5)
        {
            myHealth = Health.Growing;
            healthModifier += Random.Range(1, 3);
        }
        //If the chance is between 6.6 and 8.0
        //we get big coral
        if(newChance >= 6.6 && newChance <= 8.0)
        {
            myHealth = Health.big;
        }
        //If the chance is between 8.1 and 9.0
        //we get sick coral
        //and increase the sick modifier by a value from 1 to 3
        if(newChance >= 8.1 && newChance <= 9.0)
        {
            myHealth = Health.Sick;
            sickModifier += Random.Range(1, 3);
        }
        //If the chance is greater than 9.1 we get small coral
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
