using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/**
  *This is the base class for the CoralMesh script 
  */
public class CoralPrecious : MonoBehaviour
{

    [Range(2, 6)] public int iterations = 3; // this allows the user to choose how many iterations they want in the editor
    [Range(1, 4)] public int branches = 5; // this allows the user to choose how many branches per iteration they want in the editor


    public float xScaling = .15f; // this allows the user to choose the X scaling in the editor
    public float yScaling = 1f; // this allows the user to choose the Y scaling in the editor
    public float zScaling = .15f; // this allows the user to choose the Z scaling in the editor

    /**
      * This is the constructor function for this class
      */
    void Start()
    {
        Build(); // calls the Build function
        
    }

    /**
      * This is the Build class which takes all this info from the MakeCube
      * function and puts the pieces together
      */
    public void Build()
    {

        Vector3 branchScaling = new Vector3(xScaling, yScaling, zScaling); // This takes the XYZ Scaling from the editor and plugs them into a Vector3

        List<CombineInstance> meshes = new List<CombineInstance>(); // Creates a List<> to hold all meshes made and combine them into one coral

        Grow(iterations, meshes, Vector3.zero, Quaternion.identity, branchScaling); // calls the grow function and plugs how many iterations, the mesh List<>, general Vector3, general Quaternion rotation, and branch scaling

        Mesh mesh = new Mesh(); // creates a new mesh object
        mesh.CombineMeshes(meshes.ToArray()); // add the mesh to the meshes List<>
        MeshFilter meshFilter = GetComponent<MeshFilter>(); // generates a new MeshFilter
        meshFilter.mesh = mesh; // adds the MeshFilter to the new mesh object
    }

    /**      
      * This function spawns a branch based on the iterations
      * @param num - passes in how many iterations this function should run
      * @param meshes - passes in the list of meshes so that this function can use its data
      * @param topPosition - passes in the current position of the object
      * @param rot - passes in the current rotation of the object
      * @param scale - passes in the current scale of the object
      */
    private void Grow(int num, List<CombineInstance> meshes, Vector3 topPosition, Quaternion rot, Vector3 scale)
    {

        bool isTop = false; // bool to tell if a branch has spawned on top of the coral
        bool isLeft = false; // bool to tell if a branch has spawned to the left of the coral
        bool isRight = false; // bool to tell if a branch has spawned to the right of the coral
        bool isFront = false; // bool to tell if a branch has spawned in front of the coral
        bool isBack = false; // bool to tell if a branch has spawned in back of the coral

        if (num <= 0) return; // stop recursive function

        CombineInstance inst = new CombineInstance(); // creates a new combined intance 
        inst.mesh = MakeCube(num); // has instance call MakeCube function
        inst.transform = Matrix4x4.TRS(topPosition, rot, scale); // places the intance on the coral and set the rotation and scale  
        meshes.Add(inst); // adds the instance to the meshes List<>
        num--; // decreases the current iteration


        scale.x *= .8f; // set the x scaling to slowly go down during each iteration
        scale.z *= .8f; // set the z scaling to slowly go down during each iteration
        scale.y *= .76f; // set the y scaling to slowly go down during each iteration

        // face brach positions
        topPosition = inst.transform.MultiplyPoint(new Vector3(0, .9f, 0));
        Vector3 frontPosition = inst.transform.MultiplyPoint(new Vector3(0, Random.Range(0.5f, .8f), 0));
        Vector3 backPosition = inst.transform.MultiplyPoint(new Vector3(0, Random.Range(0.5f, .8f), 0));
        Vector3 leftPosition = inst.transform.MultiplyPoint(new Vector3(0, Random.Range(0.5f, .8f), 0));
        Vector3 rightPosition = inst.transform.MultiplyPoint(new Vector3(0, Random.Range(0.5f, .8f), 0));

        // face branch rotations
        Quaternion topRotation = rot * Quaternion.Euler(0, 0, 0);
        Quaternion frontRotation = rot * Quaternion.Euler(0, 0, Random.Range(55, 75));
        Quaternion backRotation = rot * Quaternion.Euler(0, 0, Random.Range(-55, -75));
        Quaternion leftRotation = rot * Quaternion.Euler(Random.Range(55, 75), 0, 0);
        Quaternion rightRotation = rot * Quaternion.Euler(Random.Range(-55, -75), 0, 0);


        int randomPicker = Random.Range(1, 6); // pick a random face of the coral to place a branch


        //int randomPicker = 4;

        for (int i = 0; i < branches; i++)
        {
            if (randomPicker == 1 && isFront == false)  // if (random picker = n) and (face = false) do this
            {
                Grow(num, meshes, frontPosition, frontRotation, scale); // spawn branch in this location
                isFront = true; // set That face to true. (this is saying "There is a branch already here")
            }
            else if (randomPicker == 2 && isBack == false) // if (random picker = n) and (face = false) do this
            {
                Grow(num, meshes, backPosition, backRotation, scale); // spawn branch in this location
                isBack = true; // set That face to true. (this is saying "There is a branch already here")
            }
            else if (randomPicker == 3 && isRight == false) // if (random picker = n) and (face = false) do this
            {
                Grow(num, meshes, leftPosition, leftRotation, scale); // spawn branch in this location
                isRight = true; // set That face to true. (this is saying "There is a branch already here")
            }
            else if (randomPicker == 4 && isLeft == false) // if (random picker = n) and (face = false) do this
            {
                Grow(num, meshes, rightPosition, rightRotation, scale); // spawn branch in this location
                isLeft = true; // set That face to true. (this is saying "There is a branch already here")
            }
            else if (randomPicker == 5 && isTop == false) // if (random picker = n) and (face = false) do this
            {
                Grow(num, meshes, topPosition, topRotation, scale); // spawn branch in this location
                isTop = true; // set That face to true. (this is saying "There is a branch already here")
            }
            else // if a face wasn't picked, pick again
            {
                randomPicker = Random.Range(1, 6); // Pick a new random number
                i--; // Reset that current turn
            }
        }
    }

    /**
      * This function generates a cube mesh by making the Vertices,
      * UVs, Normals, Triangles, and colors for the cube
      * @param num - this stores which iteration the mesh is currently on
      */
    private Mesh MakeCube(int num)
    {

        float hueMin = .94f;
        float hueMax = .99f;
        float hue = Mathf.Lerp(hueMax, hueMin, (num / (float)iterations));

        List<Color> colors = new List<Color>();
        Mesh mesh = MeshTools.MakeCube();

        for(int i = 0; i < mesh.vertexCount; i++)
        {
            float tempHue = hue;// + (1/(float)iterations) * pos.y;

            Color color = Color.HSVToRGB(tempHue, 1, 1);
            colors.Add(color);
        }
        mesh.SetColors(colors);

        return mesh;
    }

    /**
      * This function allows the coral to cast a ray out from every face to see if it is to close to any other coral
      */
    public void Detect()
    {
        Vector3 fwd = transform.TransformDirection(Vector3.forward); // holds a vector that points forward
        Vector3 back = transform.TransformDirection(Vector3.back); // holds a vector that points backwards
        Vector3 right = transform.TransformDirection(Vector3.right); // holds a vector that points right
        Vector3 left = transform.TransformDirection(Vector3.left); // holds a vector that points left
        Vector3 up = transform.TransformDirection(Vector3.up); // holds a vector that points up
        Vector3 dwn = transform.TransformDirection(Vector3.down); // holds a vector that points down
        RaycastHit hit; // Structure used to get information back from a raycast

        if (Physics.Raycast(gameObject.transform.position, fwd, out hit, 2)) // cast a ray 2 units ahead of the coral
        {
            
        }
        else if (Physics.Raycast(gameObject.transform.position, back, out hit, 2)) // cast a ray 2 units behind the coral
        {
            
        }
        else if (Physics.Raycast(gameObject.transform.position, right, out hit, 2)) // cast a ray 2 units to the right of the coral
        {
            
        }
        else if (Physics.Raycast(gameObject.transform.position, left, out hit, 2)) // cast a ray 2 units to the left of the coral
        {
            
        }
        else if (Physics.Raycast(gameObject.transform.position, up, out hit, 2)) // cast a ray 2 units above the coral
        {
            
        }
        else if (Physics.Raycast(gameObject.transform.position, dwn, out hit, 2)) // cast a ray 2 units below the coral
        {
            
        }

    } // end detect method

} // End CoralMesh monobehavior class

[CustomEditor(typeof(CoralPrecious))]
public class CoralMeshEditor : Editor // New Class
{
    public override void OnInspectorGUI() // overiding the Inspector in Unity
    {
        base.OnInspectorGUI(); // connects the code to the Grow button

        if (GUILayout.Button("GROW!")) // if the grow button is pressed
        {
            CoralPrecious b = (target as CoralPrecious); // basically getting CoralMeshes info
            b.Build(); // then telling it to build

        } // end if

    } // end OnInspectorGUI method

} // end CoralMesh Class