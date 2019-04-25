using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is used to create a mesh for a sea star.
/// Author: Kyle Lowery
/// Last Date Updated: 04/24/2019
/// </summary>
public class CreatureSeaStarMesh : MonoBehaviour
{
    // Ints:
    /// <summary>
    /// Controls the number of arms segmenting off of the mesh.
    /// </summary>
    private int numberOfArms = 5;
    /// <summary>
    /// Sets the minimum number of arms the object can have.
    /// </summary>
    [Range(5, 16)] public int minArms = 5;
    /// <summary>
    /// Sets the maximum number of arms the object can have.
    /// </summary>
    [Range(5, 16)] public int maxArms = 8;

    // Scaling Vectors:
    /// <summary>
    /// Controls the overall scale of the object.
    /// </summary>
    private float objectScaling = 1f;
    /// <summary>
    /// Sets the minimum value for the object scale.
    /// </summary>
    [Range(.5f, 5f)] public float minObjectScaling = .5f;
    /// <summary>
    /// Sets the maximum value for the object scale.
    /// </summary>
    [Range(.5f, 5f)] public float maxObjectScaling = 5f;

    /// <summary>
    /// Controls the scale of the center of the sea star.
    /// </summary>
    public Vector3 centerScale = new Vector3(1, 1, 1);
    /// <summary>
    /// Controls the scale of each of the arms on the mesh.
    /// </summary>
    public Vector3 armScale = new Vector3(.5f, 1, .75f);
    /// <summary>
    /// Sets the minimum value the object can randomly be scaled to.
    /// </summary>
    [Range(.1f, 1.5f)] public float minRandomScaling = .5f;
    /// <summary>
    /// Sets the maximum value the object can randomly be scaled to.
    /// </summary>
    [Range(.1f, 1.5f)] public float maxRandomScaling = 5f;

    /// <summary>
    /// Sets the minimum length the arms can be stretched to.
    /// </summary>
    [Range(.5f, 2f)]public float minArmLength = .5f;
    /// <summary>
    /// Sets the maximum length the arms can be stretched to.
    /// </summary>
    [Range(.5f, 2f)] public float maxArmLength = 2f;
    /// <summary>
    /// Sets the minimum width the arms can be stretched to.
    /// </summary>
    [Range(.5f, 2f)] public float minArmWidth = .5f;
    /// <summary>
    /// Sets the maximum width the arms can be stretched to.
    /// </summary>
    [Range(.5f, 2f)] public float maxArmWidth = 2f;

    //Object-Specific Values:
    /// <summary>
    /// Is used to control the amount each of the arms tapers at their end.
    /// </summary>
    private float taperAmount = 0.5f;
    /// <summary>
    /// Sets the minimum taper amount for the arms.
    /// </summary>
    [Range(.1f, 1f)]public float minTaperAmount = .5f;
    /// <summary>
    /// Sets the maximum taper amount for the arms.
    /// </summary>
    [Range(.1f, 1f)] public float maxTaperAmount = 1f;

    /// <summary>
    /// Sets how high the center of the object rises up from its initial position.
    /// </summary>
    [Range(0f, 1f)] public float centerRiseAmount = .3f;

    //Hue modifiers:
    /// <summary>
    /// Stores the hue value for the object.
    /// </summary>
    private float hue = 1f;
    /// <summary>
    /// Sets the minimum value of the hue color.
    /// </summary>
    [Range(.1f, 1f)] public float hueMin = .1f;
    /// <summary>
    /// Sets the maximum value of the hue color.
    /// </summary>
    [Range(.1f, 1f)] public float hueMax = 1f;

    // Start is called before the first frame update
    void Start()
    {
        //Check to make sure Unity values don't break the program.
        if (minArms > maxArms || minArms == maxArms) minArms = maxArms - 1;
        if (minObjectScaling > maxObjectScaling || minObjectScaling == maxObjectScaling) minObjectScaling = maxObjectScaling - .1f;
        if (minRandomScaling > maxRandomScaling || minRandomScaling == maxRandomScaling) minRandomScaling = maxRandomScaling - .1f;
        if (hueMin > hueMax || hueMin == hueMax) hueMin = hueMax - .1f;

        //Set variables that SHOULDN'T be changed in the recursive function:
        objectScaling = Random.Range(minObjectScaling, maxObjectScaling);
        taperAmount = Random.Range(minTaperAmount, maxTaperAmount);
        hue = Mathf.Lerp(hueMin, hueMax, Random.Range(0f, 1f));

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
        //Set randomized values:
        numberOfArms = Random.Range(minArms, maxArms);
        float tempRandomScaling = Random.Range(minRandomScaling, maxRandomScaling);

        //Set random values to adjust arm width/height:
        float tempRandomArmScaleX = Random.Range(minRandomScaling, maxRandomScaling);
        float tempRandomArmScaleZ = Random.Range(minRandomScaling, maxRandomScaling);

        //Create temporary scaling for center and arms:
        Vector3 tempCenterScale = (centerScale * tempRandomScaling) * objectScaling;
        Vector3 tempArmScale = (armScale * tempRandomScaling) * objectScaling;

        //Add arm length & width
        tempArmScale.x += Random.Range(minArmWidth, maxArmWidth);
        tempArmScale.z += Random.Range(minArmLength, maxArmLength);

        //Add randomness to arm width/height:
        tempArmScale.x *= tempRandomArmScaleX;
        tempArmScale.z *= tempRandomArmScaleZ;

        //Make center of the sea star:
        CombineInstance center = new CombineInstance();
        center.mesh = MeshTools.MakePentagonalCylinder();
        AddColorToVertices(center.mesh);

        //Adjust center values:
        Quaternion adjustRot =  Quaternion.Euler(0, 15, 0) * rot;
        Vector3 adjustPos = pos;
        adjustPos.y += centerRiseAmount;

        //Set center transform:
        center.transform = Matrix4x4.TRS(adjustPos, adjustRot, tempCenterScale);
        meshes.Add(center);
  
        //Make arms of sea star:
        for (int i = 0; i < numberOfArms; i++)
        {
            CombineInstance arm = new CombineInstance();
            arm.mesh = MeshTools.MakeTaperCube(taperAmount);
            AddColorToVertices(arm.mesh);
            float rotDegrees = (360 / numberOfArms) * i;
            float rotRadians = rotDegrees * (Mathf.PI / 180.0f);
                
            //Build out arm in that direction:
            float armX = Mathf.Sin(rotRadians) * 1f;
            float armZ = Mathf.Cos(rotRadians) * 1f;

            Vector3 armPos = pos + new Vector3(armX, 0.5f, armZ);
            Quaternion armRot = Quaternion.Euler(0, rotDegrees, 0) * rot;

            // TODO: adjust arm position by scale
                

            arm.transform = Matrix4x4.TRS(armPos, armRot, tempArmScale);
            meshes.Add(arm);
        }
    }

    /// <summary>
    /// Adds color to vertices based on the number of iterations that have passed making the object.
    /// </summary>
    /// <param name="mesh">The mesh you wish to add vertex colors to.</param>
    private void AddColorToVertices(Mesh mesh)
    {
        List<Color> colors = new List<Color>();

        foreach (Vector3 pos in mesh.vertices)
        {
            float tempHue = hue;
            Color tempColor = Color.HSVToRGB(tempHue, 1, 1);
            colors.Add(tempColor);
        }

        mesh.SetColors(colors);
    }
}

