using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
///   This is the base class for the CoralMesh script 
/// adds colors to the mesh and builds itself.
/// </summary>
public class CoralFingers : MonoBehaviour
{
    MeshFilter meshFilter;
    /// <summary>
    ///  number of times this loop will run
    /// </summary>
    int iterations = 0;
    /// <summary>
    /// size of the branch
    /// </summary>
    public Vector3 branchScaling = new Vector3(.25f, 1, .25f);
    
    void Start()
    {
        Build();
    }

  /// <summary>
    /// instantiates the making of the coral
    /// </summary>  
    public void Build()
    {
        
      
        List<CombineInstance> meshes = new List<CombineInstance>();
        Grow2(iterations, meshes, Vector3.zero, Quaternion.identity, 1);
        Grow(iterations, meshes, Vector3.zero, Quaternion.identity, 1);
        Grow3(iterations, meshes, Vector3.zero, Quaternion.identity, 1);
        Grow4(iterations, meshes, Vector3.zero, Quaternion.identity, 1);


        Mesh mesh = new Mesh();
        mesh.CombineMeshes(meshes.ToArray());
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;

    }
    /// <summary>
    /// makes recursion 
    /// </summary>
    /// <param name="num">how many times it will run</param>
    /// <param name="meshes"> this will be where all the meshes will be combined</param>
    /// <param name="pos">where our meshes location is</param>
    /// <param name="rot">rotation of our mesh</param>
    /// <param name="scale">size of are mesh</param>
    private void Grow4(int num, List<CombineInstance> meshes, Vector3 pos, Quaternion rot, float scale)
    {
        iterations = Random.Range(3, 15);
        int twist = 45;
        int twist2 = Random.Range(20, 30);
        int branch1 = Random.Range(0, 6);
        int branch2 = Random.Range(0, 6);
        int branch3 = Random.Range(0, 6);
        int branch4 = Random.Range(0, 6);
        int branch5 = Random.Range(-15, -5);
        int branch6 = Random.Range(5, 10);
        if (num <= 0) return;
        CombineInstance inst = new CombineInstance();
        inst.mesh = MakeCube(num);
        inst.transform = Matrix4x4.TRS(pos, rot, branchScaling * scale);
        meshes.Add(inst);
        num--;
        pos = inst.transform.MultiplyPoint(new Vector3(0, 1, 0));
        //Vector3 sidePos = inst.transform.MultiplyPoint(new Vector3(0, 2, 0));
        Quaternion rot1 = rot * Quaternion.Euler(0, 0, 0);
        Quaternion rot2 = rot * Quaternion.Euler(0, twist2, -twist2);
        Quaternion rot3 = rot * Quaternion.Euler(0, twist, -twist);
        Quaternion rot4 = rot * Quaternion.Euler(0, -twist, -twist);
        Quaternion rot5 = rot * Quaternion.Euler(0, twist, twist);
        Quaternion rot6 = rot * Quaternion.Euler(0, -twist, twist);
        Quaternion rot7 = rot * Quaternion.Euler(0, 0, twist);
        Quaternion rot8 = rot * Quaternion.Euler(0, 0, -twist);

        scale *= .75f;

        Grow4(num, meshes, pos, rot1, scale);

        if (num == 9 & branch1 > 2)
        {
            Grow4(num, meshes, pos, rot2, scale);
        }
        if (num == 8 & branch2 > 2)
        {
            Grow4(num, meshes, pos, rot2, scale);
        }
        if (num == 7 & branch3 > 2)
        {
            Grow4(num, meshes, pos, rot2, scale);
        }
        if (num == 6 & branch4 > 2)
        {
            Grow4(num, meshes, pos, rot2, scale);

        }


        if (num == 1)
        {

            scale *= 1.5f;

            Grow4(num, meshes, pos, rot8, scale);
            Grow4(num, meshes, pos, rot7, scale);
            Grow4(num, meshes, pos, rot3, scale);
            Grow4(num, meshes, pos, rot4, scale);
            Grow4(num, meshes, pos, rot5, scale);
            Grow4(num, meshes, pos, rot6, scale);
        }

    }
    /// <summary>
    /// makes recursion 
    /// </summary>
    /// <param name="num">how many times it will run</param>
    /// <param name="meshes"> this will be where all the meshes will be combined</param>
    /// <param name="pos">where our meshes location is</param>
    /// <param name="rot">rotation of our mesh</param>
    /// <param name="scale">size of are mesh</param>
    private void Grow3(int num, List<CombineInstance> meshes, Vector3 pos, Quaternion rot, float scale)
    {
        iterations = Random.Range(3, 15);
        int twist = 45;
        int twist2 = Random.Range(20, 30);
        int branch1 = Random.Range(0, 6);
        int branch2 = Random.Range(0, 6);
        int branch3 = Random.Range(0, 6);
        int branch4 = Random.Range(0, 6);
        int branch5 = Random.Range(-15, -5);
        int branch6 = Random.Range(5, 10);
        if (num <= 0) return;
        CombineInstance inst = new CombineInstance();
        inst.mesh = MakeCube(num);
        inst.transform = Matrix4x4.TRS(pos, rot, branchScaling * scale);
        meshes.Add(inst);
        num--;
        pos = inst.transform.MultiplyPoint(new Vector3(0, 1, 0));
        //Vector3 sidePos = inst.transform.MultiplyPoint(new Vector3(0, 2, 0));
        Quaternion rot1 = rot * Quaternion.Euler(branch5, -branch5, branch6);
        Quaternion rot2 = rot * Quaternion.Euler(0, twist2, -twist2);
        Quaternion rot3 = rot * Quaternion.Euler(0, twist, -twist);
        Quaternion rot4 = rot * Quaternion.Euler(0, -twist, -twist);
        Quaternion rot5 = rot * Quaternion.Euler(0, twist, twist);
        Quaternion rot6 = rot * Quaternion.Euler(0, -twist, twist);
        Quaternion rot7 = rot * Quaternion.Euler(0, 0, twist);
        Quaternion rot8 = rot * Quaternion.Euler(0, 0, -twist);

        scale *= .75f;

        Grow3(num, meshes, pos, rot1, scale);

        if (num == 9 & branch1 > 2)
        {
            Grow3(num, meshes, pos, rot2, scale);
        }
        if (num == 8 & branch2 > 2)
        {
            Grow3(num, meshes, pos, rot2, scale);
        }
        if (num == 7 & branch3 > 2)
        {
            Grow3(num, meshes, pos, rot2, scale);
        }
        if (num == 6 & branch4 > 2)
        {
            Grow3(num, meshes, pos, rot2, scale);

        }


        if (num == 1)
        {

            scale *= 1.5f;

            Grow3(num, meshes, pos, rot8, scale);
            Grow3(num, meshes, pos, rot7, scale);
            Grow3(num, meshes, pos, rot3, scale);
            Grow3(num, meshes, pos, rot4, scale);
            Grow3(num, meshes, pos, rot5, scale);
            Grow3(num, meshes, pos, rot6, scale);
        }

    }
    /// <summary>
    /// makes recursion 
    /// </summary>
    /// <param name="num">how many times it will run</param>
    /// <param name="meshes"> this will be where all the meshes will be combined</param>
    /// <param name="pos">where our meshes location is</param>
    /// <param name="rot">rotation of our mesh</param>
    /// <param name="scale">size of are mesh</param>
    private void Grow2(int num, List<CombineInstance> meshes, Vector3 pos, Quaternion rot, float scale)
    {
        iterations = Random.Range(3, 15);
        int twist = 45;
        int twist2 = Random.Range(20, 30);
        int branch1 = Random.Range(0, 6);
        int branch2 = Random.Range(0, 6);
        int branch3 = Random.Range(0, 6);
        int branch4 = Random.Range(0, 6);
        int branch5 = Random.Range(-10, -5);

        if (num <= 0) return;
        CombineInstance inst = new CombineInstance();
        inst.mesh = MakeCube(num);
        inst.transform = Matrix4x4.TRS(pos, rot, branchScaling * scale);
        meshes.Add(inst);
        num--;
        pos = inst.transform.MultiplyPoint(new Vector3(0, 1, 0));
        //Vector3 sidePos = inst.transform.MultiplyPoint(new Vector3(0, 2, 0));
        Quaternion rot1 = rot * Quaternion.Euler(0, 0, branch5);
        Quaternion rot2 = rot * Quaternion.Euler(0, twist2, -twist2);
        Quaternion rot3 = rot * Quaternion.Euler(0, twist, -twist);
        Quaternion rot4 = rot * Quaternion.Euler(0, -twist, -twist);
        Quaternion rot5 = rot * Quaternion.Euler(0, twist, twist);
        Quaternion rot6 = rot * Quaternion.Euler(0, -twist, twist);
        Quaternion rot7 = rot * Quaternion.Euler(0, 0, twist);
        Quaternion rot8 = rot * Quaternion.Euler(0, 0, -twist);

        scale *= .75f;

        Grow2(num, meshes, pos, rot1, scale);

        if (num == 9 & branch1 > 2)
        {
            Grow2(num, meshes, pos, rot2, scale);
        }
        if (num == 8 & branch2 > 2)
        {
            Grow2(num, meshes, pos, rot2, scale);
        }
        if (num == 7 & branch3 > 2)
        {
            Grow2(num, meshes, pos, rot2, scale);
        }
        if (num == 6 & branch4 > 2)
        {
            Grow2(num, meshes, pos, rot2, scale);

        }


        if (num == 1)
        {

            scale *= 1.5f;

            Grow2(num, meshes, pos, rot8, scale);
            Grow2(num, meshes, pos, rot7, scale);
            Grow2(num, meshes, pos, rot3, scale);
            Grow2(num, meshes, pos, rot4, scale);
            Grow2(num, meshes, pos, rot5, scale);
            Grow2(num, meshes, pos, rot6, scale);
        }

    }
    /// <summary>
    /// makes recursion 
    /// </summary>
    /// <param name="num">how many times it will run</param>
    /// <param name="meshes"> this will be where all the meshes will be combined</param>
    /// <param name="pos">where our meshes location is</param>
    /// <param name="rot">rotation of our mesh</param>
    /// <param name="scale">size of are mesh</param>
    private void Grow(int num, List<CombineInstance> meshes, Vector3 pos, Quaternion rot, float scale)
    {
        iterations = Random.Range(3, 15);
        int twist = 45;
        int twist2 = Random.Range(20, 30);
        int branch1 = Random.Range(0, 6);
        int branch2 = Random.Range(0, 6);
        int branch3 = Random.Range(0, 6);
        int branch4 = Random.Range(0, 6);
        int branch5 = Random.Range(0, 6);
        int branch6 = Random.Range(0, 6);
        int branch7 = Random.Range(0, 6);
        int branch8 = Random.Range(5, 10);
        if (num <= 0) return;
        CombineInstance inst = new CombineInstance();
        inst.mesh = MakeCube(num);
        inst.transform = Matrix4x4.TRS(pos, rot, branchScaling * scale);
        meshes.Add(inst);
        num--;
        pos = inst.transform.MultiplyPoint(new Vector3(0, 1, 0));
        //Vector3 sidePos = inst.transform.MultiplyPoint(new Vector3(0, 2, 0));
        Quaternion rot1 = rot * Quaternion.Euler(0, 0, branch8);
        Quaternion rot2 = rot * Quaternion.Euler(0, twist2, -twist2);
        Quaternion rot3 = rot * Quaternion.Euler(0, twist, -twist);
        Quaternion rot4 = rot * Quaternion.Euler(0, -twist, -twist);
        Quaternion rot5 = rot * Quaternion.Euler(0, twist, twist);
        Quaternion rot6 = rot * Quaternion.Euler(0, -twist, twist);
        Quaternion rot7 = rot * Quaternion.Euler(0, 0, twist);
        Quaternion rot8 = rot * Quaternion.Euler(0, 0, -twist);

        scale *= .75f;

        Grow(num, meshes, pos, rot1, scale);

        if (num == 9 & branch1 > 2)
        {
            Grow(num, meshes, pos, rot2, scale);
        }
        if (num == 8 & branch2 > 2)
        {
            Grow(num, meshes, pos, rot2, scale);
        }
        if (num == 7 & branch3 > 2)
        {
            Grow(num, meshes, pos, rot2, scale);
        }
        if (num == 6 & branch4 > 2)
        {
            Grow(num, meshes, pos, rot2, scale);

        }


        if (num == 1)
        {

            scale *= 1.5f;

            Grow(num, meshes, pos, rot8, scale);
            Grow(num, meshes, pos, rot7, scale);
            Grow(num, meshes, pos, rot3, scale);
            Grow(num, meshes, pos, rot4, scale);
            Grow(num, meshes, pos, rot5, scale);
            Grow(num, meshes, pos, rot6, scale);
        }

    }
    /// <summary>
    /// creates the mesh and colors it by vertex
    /// </summary>
    /// <param name="num">the amount of iterations</param>
    /// <returns></returns>
    private Mesh MakeCube(int num)
    {

       
       
        
 List<Color> colors = new List<Color>();
        float hueMin = .1f;
        float hueMax = .001f;
        float hue = Mathf.Lerp(hueMin,hueMax, (num / (float)iterations));
        Mesh mesh = MeshTools.MakeCube();
        for (int i = 0; i < mesh.vertexCount; i++)
        {
            float tempHue = hue;// + (1/(float)iterations) * pos.y;
              Color color = Color.HSVToRGB(tempHue, 1, 1);
            colors.Add(color);
        }

     
        mesh.SetColors(colors);
        return mesh;
    }
}

//[CustomEditor(typeof(CoralFingers))]
//public class CoralMeshEditor : Editor
//{
  //  public override void OnInspectorGUI()
    //{

      //  base.OnInspectorGUI();
        //if (GUILayout.Button("GROW!"))
        //{
            //CoralFingers b = (target as CoralFingers);

          //  b.Build();
        //}
    //}


//}