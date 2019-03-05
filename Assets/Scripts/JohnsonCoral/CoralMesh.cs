using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/**
  *This is the base class for the CoralMesh script 
  */
public class CoralMesh : MonoBehaviour
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
        Detect(); // calls the Detect function
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
                                                                                    
        Mesh mesh = new Mesh();
        mesh.CombineMeshes(meshes.ToArray());
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;
    }


    private void Grow(int num, List<CombineInstance> meshes, Vector3 pos, Quaternion rot, Vector3 scale)
    {

        bool isTop = false;
        bool isLeft = false;
        bool isRight = false;
        bool isFront = false;    
        bool isBack = false;
       
        if (num <= 0) return; // stop recursive function

        CombineInstance inst = new CombineInstance();
        inst.mesh = MakeCube(num);
        inst.transform = Matrix4x4.TRS(pos, rot, scale);
        meshes.Add(inst);
        num--;


        scale.x *= .8f;
        scale.z *= .8f;
        scale.y *= .65f;



        pos = inst.transform.MultiplyPoint(new Vector3(0, .9f, 0));

        // face positions
        Vector3 sidePosition03 = inst.transform.MultiplyPoint(new Vector3(0, Random.Range(0.5f, .8f), 0));
        Vector3 sidePosition04 = inst.transform.MultiplyPoint(new Vector3(0, Random.Range(0.5f, .8f), 0));
        Vector3 sidePosition07 = inst.transform.MultiplyPoint(new Vector3(0, Random.Range(0.5f, .8f), 0));
        Vector3 sidePosition08 = inst.transform.MultiplyPoint(new Vector3(0, Random.Range(0.5f, .8f), 0));

        //top rotations
        Quaternion rot1 = rot * Quaternion.Euler(Random.Range(-15, 15), Random.Range(-15, 15), Random.Range(-15, 15));


        // face rotations
        Quaternion rot5 = rot * Quaternion.Euler(0, 0, Random.Range(45, 95));
        Quaternion rot6 = rot * Quaternion.Euler(0, 0, Random.Range(-45, -95));
        Quaternion rot9 = rot * Quaternion.Euler(Random.Range(75, 95), 0, 0);
        Quaternion rot10 = rot * Quaternion.Euler(Random.Range(-75, -95), 0, 0);


        int randomPicker = Random.Range(1, 6); // pick a random face of the coral to place a branch


        //int randomPicker = 3;

        for (int i = 0; i < branches; i++)
        {
            if (randomPicker == 1 && isFront == false)  // if (random picker = n) and (face = false) do this
            {
                Grow(num, meshes, sidePosition03, rot5, scale); // spawn branch in this location
                isFront = true; // set That face to true. (this is saying "There is a branch already here")
            }
            else if (randomPicker == 2 && isBack == false)
            {
                Grow(num, meshes, sidePosition04, rot6, scale);
                isBack = true;
            }
            else if (randomPicker == 3 && isRight == false)
            {
                Grow(num, meshes, sidePosition07, rot9, scale);
                isRight = true;
            }
            else if (randomPicker == 4 && isLeft == false)
            {
                Grow(num, meshes, sidePosition08, rot10, scale);
                isLeft = true;
            }
            else if (randomPicker == 5 && isTop == false)
            {
                Grow(num, meshes, pos, rot1, scale);
                isTop = true;
            }
            else // if a face wasn't picked, pick again
            {
                randomPicker = Random.Range(1, 6);
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

        List<Vector3> verts = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<Vector3> normals = new List<Vector3>();
        List<Color> colors = new List<Color>();
        List<int> tris = new List<int>();

        // Front face
        verts.Add(new Vector3(-0.5f, 0, -0.5f));
        verts.Add(new Vector3(-0.5f, 1, -0.5f));
        verts.Add(new Vector3(+0.5f, 1, -0.5f));
        verts.Add(new Vector3(+0.5f, 0, -0.5f));
        normals.Add(new Vector3(0, 0, -1));
        normals.Add(new Vector3(0, 0, -1));
        normals.Add(new Vector3(0, 0, -1));
        normals.Add(new Vector3(0, 0, -1));
        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 1));
        uvs.Add(new Vector2(1, 0));
        tris.Add(0);
        tris.Add(1);
        tris.Add(2);
        tris.Add(2);
        tris.Add(3);
        tris.Add(0);

        // Back face
        verts.Add(new Vector3(-0.5f, 0, +0.5f));
        verts.Add(new Vector3(+0.5f, 0, +0.5f));
        verts.Add(new Vector3(+0.5f, 1, +0.5f));
        verts.Add(new Vector3(-0.5f, 1, +0.5f));
        normals.Add(new Vector3(0, 0, +1));
        normals.Add(new Vector3(0, 0, +1));
        normals.Add(new Vector3(0, 0, +1));
        normals.Add(new Vector3(0, 0, +1));
        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 1));
        uvs.Add(new Vector2(1, 0));
        tris.Add(4);
        tris.Add(5);
        tris.Add(6);
        tris.Add(6);
        tris.Add(7);
        tris.Add(4);

        // Left face 
        verts.Add(new Vector3(-0.5f, 0, -0.5f));
        verts.Add(new Vector3(-0.5f, 0, +0.5f));
        verts.Add(new Vector3(-0.5f, 1, +0.5f));
        verts.Add(new Vector3(-0.5f, 1, -0.5f));
        normals.Add(new Vector3(-1, 0, 0));
        normals.Add(new Vector3(-1, 0, 0));
        normals.Add(new Vector3(-1, 0, 0));
        normals.Add(new Vector3(-1, 0, 0));
        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 1));
        uvs.Add(new Vector2(1, 0));
        tris.Add(8);
        tris.Add(9);
        tris.Add(10);
        tris.Add(10);
        tris.Add(11);
        tris.Add(8);

        // Right face
        verts.Add(new Vector3(+0.5f, 0, -0.5f));
        verts.Add(new Vector3(+0.5f, 1, -0.5f));
        verts.Add(new Vector3(+0.5f, 1, +0.5f));
        verts.Add(new Vector3(+0.5f, 0, +0.5f));
        normals.Add(new Vector3(+1, 0, 0));
        normals.Add(new Vector3(+1, 0, 0));
        normals.Add(new Vector3(+1, 0, 0));
        normals.Add(new Vector3(+1, 0, 0));
        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 1));
        uvs.Add(new Vector2(1, 0));
        tris.Add(12);
        tris.Add(13);
        tris.Add(14);
        tris.Add(14);
        tris.Add(15);
        tris.Add(12);

        // Top face
        verts.Add(new Vector3(-0.5f, 1, -0.5f));
        verts.Add(new Vector3(-0.5f, 1, +0.5f));
        verts.Add(new Vector3(+0.5f, 1, +0.5f));
        verts.Add(new Vector3(+0.5f, 1, -0.5f));
        normals.Add(new Vector3(0, +1, 0));
        normals.Add(new Vector3(0, +1, 0));
        normals.Add(new Vector3(0, +1, 0));
        normals.Add(new Vector3(0, +1, 0));
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

        // Bottom face 
        verts.Add(new Vector3(-0.5f, 0, -0.5f));
        verts.Add(new Vector3(+0.5f, 0, -0.5f));
        verts.Add(new Vector3(+0.5f, 0, +0.5f));
        verts.Add(new Vector3(-0.5f, 0, +0.5f));
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

        float hueMin = .94f;
        float hueMax = .99f;

        float hue = Mathf.Lerp(hueMax, hueMin, (num / (float)iterations));

        foreach (Vector3 pos in verts)
        {
            float tempHue = hue;// + (1/(float)iterations) * pos.y;

            Color color = Color.HSVToRGB(tempHue, 1, 1);
            colors.Add(color);
        }

        Mesh mesh = new Mesh();
        mesh.SetVertices(verts);
        mesh.SetUVs(0, uvs);
        mesh.SetNormals(normals);
        mesh.SetTriangles(tris, 0);
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
            print("There is something in front of the object!");
        }
        else if (Physics.Raycast(gameObject.transform.position, back, out hit, 2)) // cast a ray 2 units behind the coral
        {
            print("There is something behind of the object!");
        }
        else if (Physics.Raycast(gameObject.transform.position, right, out hit, 2)) // cast a ray 2 units to the right of the coral
        {
            print("There is something to the right of the object!");
        }
        else if (Physics.Raycast(gameObject.transform.position, left, out hit, 2)) // cast a ray 2 units to the left of the coral
        {
            print("There is something to the left of the object!");
        }
        else if (Physics.Raycast(gameObject.transform.position, up, out hit, 2)) // cast a ray 2 units above the coral
        {
            print("There is something above of the object!");
        }
        else if (Physics.Raycast(gameObject.transform.position, dwn, out hit, 2)) // cast a ray 2 units below the coral
        {
            print("There is something below of the object!");
        }

    } // end detect method

} // End CoralMesh monobehavior class

[CustomEditor(typeof(CoralMesh))]
public class CoralMeshEditor : Editor // New Class
{
    public override void OnInspectorGUI() // overiding the Inspector in Unity
    {
        base.OnInspectorGUI(); // connects the code to the Grow button

        if (GUILayout.Button("GROW!")) // if the grow button is pressed
        {
            CoralMesh b = (target as CoralMesh); // basically getting CoralMeshes info
            b.Build(); // then telling it to build

        } // end if

    } // end OnInspectorGUI method

} // end CoralMesh Class