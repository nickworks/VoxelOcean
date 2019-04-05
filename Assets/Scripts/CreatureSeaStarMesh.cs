using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// This script is used to create a mesh for a sea star within VoxelOcean.
/// Author: Kyle Lowery
/// Last Date Updated: 04/05/2019
/// </summary>
public class CreatureSeaStarMesh : MonoBehaviour
{
    // Ints:
    public int numberOfArms = 5;

    // Scaling Vectors:
    public Vector3 meshScale = new Vector3(1, 1, 1);

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

    public void Build()
    {
        List<CombineInstance> meshes = new List<CombineInstance>();
        Grow(meshes, Vector3.zero, Quaternion.identity);

        Mesh mesh = new Mesh();
        mesh.CombineMeshes(meshes.ToArray());

        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;
    }

    void Grow(List<CombineInstance> meshes, Vector3 pos, Quaternion rot)
    {
            //Make center of the sea star:
            CombineInstance center = new CombineInstance();
            center.mesh = MeshTools.MakePentagonalCylinder();
            center.transform = Matrix4x4.TRS(pos, rot, meshScale);
            meshes.Add(center);
  
            //Make arms of sea star:
            for (int i = 1; i <= numberOfArms; i++)
            {
                CombineInstance arm = new CombineInstance();
                arm.mesh = MeshTools.MakeCube();
                float rotDegrees = (360 / numberOfArms) * i + 180.0f;
                float rotRadians = rotDegrees * (Mathf.PI / 180.0f);
                
                //Build out arm in that direction:
                float armX = Mathf.Cos(rotRadians) * meshScale.x;
                float armZ = Mathf.Sin(rotRadians) * meshScale.z;

                Vector3 armPos = pos + new Vector3(armX, 0, armZ);

                Quaternion armRot = rot * Quaternion.Euler(0, rotDegrees, 0);
                arm.transform = Matrix4x4.TRS(armPos, armRot, meshScale);
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

