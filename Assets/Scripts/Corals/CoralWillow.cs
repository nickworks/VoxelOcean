using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class CoralWillow : MonoBehaviour
{
    /// <summary>
    /// The number of iterations that the recursive Grow() function will iterate
    /// </summary>
    [Range(2, 8)] public int iterations = 6;
    /// <summary>
    /// A value of up to 30 degrees off of 120 degrees (360/3 for 3 branches) for the branches to grow from the center
    /// </summary>
    [Range(0, 30)] public int randomDir1Offset = 0;

    /// <summary>
    /// The number of branches around the top of the CoralWillow
    /// </summary>
    [Range(6, 20)] public int NumberBranches = 6;

    /// <summary>
    /// Boolean value to override the OldColor and YoungColor variables and spawn the coral with 2 randomly decided values.
    /// </summary>
    public bool RandomizeColors;

    /// <summary>
    /// The color to spawn nearest the center of the coral
    /// </summary>
    public Color OldColor;

    /// <summary>
    /// The color to spawn furthest from the center of the coral.
    /// </summary>
    public Color YoungColor;
    /// <summary>
    /// The color of the stem.
    /// </summary>
    public Color StemColor;


    /// <summary>
    /// The X Y and Z scaling for the rectangular portions of the coral.
    /// </summary>
    public Vector3 branchScaling = new Vector3(.25f, 1, .25f);


    /// <summary>
    /// The random object used for determining randomness
    /// </summary>
    private System.Random random = new System.Random();

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {

        Build();
        
    }

    /// <summary>
    /// Build function sets up and begins the recursive function Grow() 
    /// </summary>
    public void Build()
    {
        if (RandomizeColors)
        {
            YoungColor = new Color((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble(), 1);
            OldColor = new Color((float)random.NextDouble(), (float)random.NextDouble(), (float)random.NextDouble(), 1);
        }

        List<CombineInstance> meshes = new List<CombineInstance>();
        GrowStem(iterations, meshes, Vector3.zero, Quaternion.identity, 1, iterations);

        Mesh mesh = new Mesh();
        mesh.CombineMeshes(meshes.ToArray());
        
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;
        
    }

    /// <summary>
    /// The Recursive Grow Function
    /// Each recursion spawns more branches.
    /// </summary>
    /// <param name="num">The number of iterations left in the recursive function. If less than or = to 0, returns.</param>
    /// <param name="meshes">A list of CombineInstance objects which is expanded each iteration of the function.</param>
    /// <param name="pos">The position of the transform of the previous iteration. If the first iteration, is the transform of the GameObject.</param>
    /// <param name="rot">The rotation of the transform of the previous iteration. If the first iteration, is the transform of the GameObject.</param>
    /// <param name="scale">The Scale of the transform of the previous iteration. If the first iteration, is the transform of the GameObject.</param>
    /// <param name="maxNum">The Initial number of iterations. Should not change.</param>
    private void Grow(int num, List<CombineInstance> meshes, Vector3 pos, Quaternion rot, float scale, int maxNum)
    {
        if (num <= 0) return;

        

        CombineInstance inst = new CombineInstance();
        //inst.mesh = MakeCube(num, maxNum);
        inst.mesh = MeshTools.MakeCube();
        Color[] topAndBottomColors = GetColors(num, maxNum);
        List<Color> vertexColors = new List<Color>();
        foreach (var vert in inst.mesh.vertices)
        {
            if (vert.y == 1)
                vertexColors.Add(topAndBottomColors[1]);
            else
                vertexColors.Add(topAndBottomColors[0]);
        }
        inst.mesh.colors = vertexColors.ToArray();

        inst.transform = Matrix4x4.TRS(pos, rot, branchScaling * scale);

        meshes.Add(inst);
        num--;

        pos = inst.transform.MultiplyPoint(new Vector3(0, 1, 0));
        Quaternion rot1 = rot * Quaternion.Euler(20, (random.Next(-randomDir1Offset, randomDir1Offset)), (random.Next(-5,5)));
        
        
    
        Grow(num, meshes, pos, rot1, scale, maxNum);
   
        
        
    }

    /// <summary>
    /// Grows the initial 'stem' of the species, and when the exit condition is met, grows the rest of the specimen
    /// </summary>
    /// <param name="num">The number of iterations left in the recursive function. If less than or = to 0, returns.</param>
    /// <param name="meshes">A list of CombineInstance objects which is expanded each iteration of the function.</param>
    /// <param name="pos">The position of the transform of the previous iteration. If the first iteration, is the transform of the GameObject.</param>
    /// <param name="rot">The rotation of the transform of the previous iteration. If the first iteration, is the transform of the GameObject.</param>
    /// <param name="scale">The Scale of the transform of the previous iteration. If the first iteration, is the transform of the GameObject.</param>
    /// <param name="maxNum">The Initial number of iterations. Should not change.</param>
    private void GrowStem(int num, List<CombineInstance> meshes, Vector3 pos, Quaternion rot, float scale, int maxNum)
    {

        if (num < 0)
        {
            var degrees = 360f / NumberBranches;

            for (int i = 0; i < NumberBranches; i++)
            {
                Quaternion rot1 = rot * Quaternion.Euler(100, (i * degrees) + (random.Next(-randomDir1Offset, randomDir1Offset)), 0);
                Grow(maxNum + 2, meshes, pos, rot1, scale, maxNum);
            }
            return;
        }

        CombineInstance inst = new CombineInstance();
        inst.mesh = MeshTools.MakeCube();
        
        List<Color> vertexColors = new List<Color>();
        foreach (var vert in inst.mesh.vertices)
        {
            vertexColors.Add(StemColor);
        }
        inst.mesh.colors = vertexColors.ToArray();

        inst.transform = Matrix4x4.TRS(pos, rot, new Vector3(.5f, 1f, .5f) * scale);

        meshes.Add(inst);
        num--;
        pos = inst.transform.MultiplyPoint(new Vector3(0, 1, 0));
        GrowStem(num, meshes, pos, rot, scale, maxNum);
    }

   

    /// <summary>
    /// Gets the colors for the vertices for the current generation. The num and maxNum arguments are used to find proportions and to Lerp the colors.
    /// </summary>
    /// <param name="num">The current iteration</param>
    /// <param name="maxNum">The maximum iteration.</param>
    /// <returns>A Color array of size 2, containing the colors which the current generation will gradiant between. </returns>
    private Color[] GetColors(int num, int maxNum)
    {

        float ratio1 = (num - 1f) / maxNum;
        float ratio2 = (float)num / maxNum;

        

        Color color1 = Color.Lerp(YoungColor, OldColor, ratio1);
        Color color2 = Color.Lerp(YoungColor, OldColor, ratio2);
        return new Color[2] { color1, color2 };
    }

    

}
/// <summary>
/// Adds the GROW button to the inspector, and stuff.
/// </summary>
[CustomEditor(typeof(CoralWillow))]
public class CoralWillowEditor : Editor
{
    public override void OnInspectorGUI()
    {
        
        base.OnInspectorGUI();

        if (GUILayout.Button("GROW!"))
        {
            CoralWillow cm =  (target as CoralWillow);
                    
            cm.Build();
        }
        
    }
}

