using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class CoralGlow : MonoBehaviour
{
    #region Variables
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
    }

    /// <summary>
    /// A enumeration variable
    /// </summary>
    [Tooltip("The state of the corals health")]
    public Health myHealth;

    /// <summary>
    /// Wiether or not you want the script to add in randomness to your coral
    /// If this is false the coral script will generate a enumeration for the corals health state
    /// and use randomness to controll branch scale annd x,y,z rotations
    /// </summary>
    [Tooltip("Wiether or not the script should create random values for x,y,z rotations and coral health state")]
    public bool absoluteControl;


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
    /// The number of iterations for fractal generation we want
    /// </summary>
    [Tooltip("The number of bonus iterations on this piece of coral")]
    [Range(1, 10)]public int bonusIterations = 2;
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

    /// <summary>
    /// A integer used to control the state of the coral
    /// This is only set if absolute control is off
    /// </summary>
    private int state;

    /// <summary>
    /// A  variable used to set the properties of the ///TODO GET SHADER NAME
    /// </summary>
    [Tooltip("The amoung of glow this piece of coral starts with")]
    [Range(0.1f, 1)] public float glow = .5f;

    /// <summary>
    /// A boolean used to conrol the glow value ticking up and down in this shader this is handled through this script
    /// </summary>
    private bool tickup = false;

    /// <summary>
    /// A float used to subtract and add from the glow variable above
    /// </summary>
    private float countDown = .1f;

    /// <summary>
    /// A modifier applied to the scale of the coral
    /// </summary>
    public float scaleMod = 1f;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        //We get the renderer component on the mesh so that we can manipulate shader values
        rend = GetComponent<Renderer>();
        //We set the materials shader to the CoralGlow Shader in the CoralShaders Folder
        rend.material.shader = Shader.Find("CoralShaders/CoralGlow");//TODO: Implement Coral Shader
       

        //if we do not want absolute control in our game object
        if(!absoluteControl)
        {
            //We call the SetHealth Method passing in a state integer that is from one to three
            SetHealth(state = Random.Range(0, 4));

            
            
        }//End absoluteControl if statement
        //A call to the Build() method
        Build();
    }//End Start() method

    // Update is called once per frame
    void Update()
    {
        //If our coral is not sick proceed
        if(myHealth != Health.Sick)
        {
            //If tickup is false
            if(!tickup)
            {
                //Then we subtract  to our glow variable using countdown multiplied by delta time
                glow -= countDown * Time.deltaTime;
            }
            else//If Tickup is true
            {
                //Then we add to our glow variable using countdown multiplied by delta time
                glow += countDown * Time.deltaTime;
            }//End of if(!tickup) statement
            //Call the check countdown method to see if we need to change tickup to true or false
            CheckCountDown();
            //Set our material value _Glow to our glow value
            rend.material.SetFloat("_Glow", glow);
        }
    }//End Update() method

    /// <summary>
    /// A Method used to see if we need to change tickup to true or false
    /// </summary>
    private void CheckCountDown()
    {
        //If our glow amount is higher than .9f
        if(glow > .9f)
        {
            //Tickup is changed to false and in our update method we start to subtract from glow
            tickup = false;
        }
        else if(glow < 0)//If our glow is less than 0
        {
            //Tickup is changed to true and in our update method we start to add to glow
            tickup = true;
        }//End if(glow > .9f) statement
    }

    /// <summary>
    /// A private method used to set our corals health state
    /// </summary>
    /// <param name="state">a integer value passed in from the start method that is used to determine what the corals health is</param>
   private void SetHealth(int state)
    {
        //We start our switch statement here
        //We switch based on the state variable passed into this method
        switch (state)
        {
            //If state is one
            case 1:
                myHealth = Health.Growing;//myHealth is set to growing
                glow = Random.Range(0, 100) * .010f; //Glow is set to a random amount
                iterations += bonusIterations = Random.Range(5, 10); //We get bonus iterations representing the coral being larger and healthier

                scaleMod += 2;
                break;//End of case 1
            case 2://If case is 2
                myHealth = Health.Normal; //health is set to normal
                glow = Random.Range(0, 100) * .010f;//Glow is set to a random amount
                iterations +=  bonusIterations = Random.Range(0, 5); //We get bonus iterations but there is a wider range so we could only get one or we could get 4

                scaleMod += 1;
                break;//End of case 2
            case 3://If case is 3
                myHealth = Health.Sick;// My health is set to sick
                glow = Random.Range(0, 100) * .010f;//Glow is set to a random value
                if(iterations >= 3)//If iterations is greater than or equal to 3
                {
                    iterations -= bonusIterations = Random.Range(1, 3);//We subtract iterations
                }//End of if(iterations >=3) statement

                scaleMod -= .5f;
                break;//end of case 3

        }//end of switch(state) statement
    
    }//End of SetHealth() Method


    /// <summary>
    /// Our build method to actually build our coral game object
    /// </summary>
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
    }//end private Build() method


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
        //Our position is updated so we don't spawn coral pieces ontop of one another
        pos = inst.transform.MultiplyPoint(new Vector3(.8f, .8f, 0)); //This is used to convert from one coordinate space to another


        //If we want absoluteControl
        if (absoluteControl)
        {
            scale *= .75f;//Scale is reduced to 75% of original
            Grow(num, meshes, pos, rot, scale);//We call our Grow() method once
        }
        else //If we do not want absoluteControl
        {
            //We calculate another sidePosition of the coral that is close to pos
            Vector3 sidePos = inst.transform.MultiplyPoint(new Vector3(.10f, .8f, 0));
            //If our myHealth enum is set to growing
            if (myHealth == Health.Growing)
            {
             //We create two rotations with only positive values in the x and z range
             Quaternion rot1 = rot * Quaternion.Euler(0 + Random.Range(0, xRotator), 45 + Random.Range(0, yRotator), 45 + Random.Range(0, zRotator));
             Quaternion rot2 = rot * Quaternion.Euler(0 + Random.Range(-xRotator, 0), 45 + Random.Range(0, yRotator), 45 + Random.Range(0, zRotator));
            //Scale is only reduced by 10%
             scale *= .60f;
            //We call the GrowMethod Twice
             Grow(num, meshes, pos, rot1, scale);
             Grow(num, meshes, sidePos, rot2, scale);
            }//End of If(myHealth == Health.Growing) statement
            else if (myHealth == Health.Normal)
             {
                //We create two rotations with base values in the y, and z range, and a possibility of negative values
                Quaternion rot1 = rot * Quaternion.Euler(0 + Random.Range(0, xRotator), 20 + Random.Range(-yRotator, yRotator), 25 + Random.Range(-zRotator, zRotator));
                 Quaternion rot2 = rot * Quaternion.Euler(0 + Random.Range(-xRotator, 0), 20 + Random.Range(-yRotator, yRotator), 25 + Random.Range(-zRotator, zRotator));
                //Scale is only reduced by 50%
                scale *= .50f;
                //We call the GrowMethod Twice
                Grow(num, meshes, pos, rot1, scale);
                 Grow(num, meshes, sidePos, rot2, scale);
            }//End of If(myHealth == Health.Normal) statement
            else if (myHealth == Health.Sick)
             {
                //We create two rotations with the possibility of a positive value in the x range, but only the possibility of a negative value in the y and z range
                 Quaternion rot1 = rot * Quaternion.Euler(0 + Random.Range(0, xRotator), 10 + Random.Range(-yRotator,0), 15 + Random.Range(-zRotator, 0));
                 Quaternion rot2 = rot * Quaternion.Euler(0 + Random.Range(-xRotator, 0), 10 + Random.Range(-yRotator,0), 15 + Random.Range(-zRotator, 0));
                //The coral is reduced by 80% 
                 scale *= .80f;                                                                                              
                //We call the GrowMethod Twice
                Grow(num, meshes, pos, rot1, scale);
                 Grow(num, meshes, sidePos, rot2, scale);
            }//End of If(myHealth == Health.Sick) statement
        }

    }//End private Grow() method

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
