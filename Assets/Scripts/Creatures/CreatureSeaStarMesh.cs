using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// This script is used to create a mesh for a sea star.
/// Author: Kyle Lowery
/// Last Date Updated: 04/05/2019
/// </summary>
public class CreatureSeaStarMesh : MonoBehaviour
{
    // Ints:
    /// <summary>
    /// Controls the number of arms segmenting off of the mesh.
    /// </summary>
    public int numberOfArms = 5;

    // Scaling Vectors:
    /// <summary>
    /// Controls the scale of the center of the sea star.
    /// </summary>
    public Vector3 centerScale = new Vector3(1, 1, 1);
    /// <summary>
    /// Controls the scale of each of the arms on the mesh.
    /// </summary>
    public Vector3 armScale = new Vector3(.5f, 1, .75f);

    /// <summary>
    /// Is used to control the amount each of the arms tapers at their end.
    /// </summary>
    [Range(0.1f, 1)]public float taperAmount = 0.5f;

    //Hue modifiers:
    /// <summary>
    /// Sets the minimum value of the hue color.
    /// </summary>
    [Range(.1f, .9f)] public float hueMin = .1f;
    /// <summary>
    /// Sets the maximum value of the hue color.
    /// </summary>
    [Range(.1f, 1f)] public float hueMax = 1f;

    // Start is called before the first frame update
    void Start()
    {
        //Check to make sure Unity values don't break the program.
        if (hueMin > hueMax || hueMin == hueMax) hueMin = hueMax - .1f;

        Build();
    }
    /// <summary>
    /// Builds a mesh that shapes like a sea star, and adds the mesh to the spawner.
    /// </summary>
    public void Build()
    {
        List<CombineInstance> meshes = new List<CombineInstance>();
        Grow(meshes, Vector3.zero, Quaternion.identity);

        Mesh mesh = new Mesh();
        mesh.CombineMeshes(meshes.ToArray());

        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;
    }
    /// <summary>
    /// Grow is called to create all the mesh data to add to the spawner's mesh filter.
    /// </summary>
    /// <param name="meshes">A collection of meshes to push to the object.</param>
    /// <param name="pos">The x,y,z position of the object.</param>
    /// <param name="rot">The rotation of the object.</param>
    void Grow(List<CombineInstance> meshes, Vector3 pos, Quaternion rot)
    {
            //Make center of the sea star:
            CombineInstance center = new CombineInstance();
            center.mesh = MeshTools.MakePentagonalCylinder();
            Quaternion adjustRot =  Quaternion.Euler(0, 15, 0) * rot;
            center.transform = Matrix4x4.TRS(pos, rot, centerScale);
            meshes.Add(center);
  
            //Make arms of sea star:
            for (int i = 0; i < numberOfArms; i++)
            {
                CombineInstance arm = new CombineInstance();
                arm.mesh = MeshTools.MakeTaperCube(taperAmount);
                float rotDegrees = (360 / numberOfArms) * i;
                float rotRadians = rotDegrees * (Mathf.PI / 180.0f);
                
                //Build out arm in that direction:
                float armX = Mathf.Sin(rotRadians) * 1f;
                float armZ = Mathf.Cos(rotRadians) * 1f;

                Vector3 armPos = pos + new Vector3(armX, 0.5f, armZ);
                Quaternion armRot = Quaternion.Euler(0, rotDegrees, 0) * rot;

                //adjust arm position by scale:
                

                arm.transform = Matrix4x4.TRS(armPos, armRot, armScale);
                meshes.Add(arm);
            }
    }
}

[CustomEditor(typeof(CreatureSeaStarMesh))]
public class CreatureSeaStarMeshEditor : Editor
{
    /// <summary>
    /// Runs the base OnInspectorGUI command and adds a "Grow" button to the inspector in the GUI.
    /// </summary>
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("GROW!"))                              //if the "Grow" button is pressed...
        {
            CreatureSeaStarMesh s = (target as CreatureSeaStarMesh);                //create a new PlantKelpMesh on the script target.
            s.Build();                                                  //Call the new PlantKelpMesh's Build function.
        }
    }
}

