using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
/// <summary>
/// Spawns Coral type Broccoli
/// </summary>
public class CoralBroccoli : MonoBehaviour {

	/// <summary>
	/// how many iterations per branch we should spawn
	/// </summary>
	[Range(2, 8)]public int iterations = 3;
	/// <summary>
	/// the scale and size of each cube
	/// </summary>
	public Vector3 branchScaling = new Vector3 (.25f, 1, .25f);
	// Use this for initialization
	void Start () {
		Build();
	}

	// Update is called once per frame
	/// <summary>
	/// this combines all of the cubes into one mesh
	/// </summary>
	public void Build () {
		List<CombineInstance> meshes = new List<CombineInstance> ();
		Grow (iterations, meshes, Vector3.zero, Quaternion.identity, 1);
		Mesh mesh = new Mesh ();
		mesh.CombineMeshes (meshes.ToArray());

		MeshFilter meshFilter = GetComponent<MeshFilter> ();
		meshFilter.mesh = mesh;
	}
	/// <summary>
	/// This is where the recursion occurs
	/// </summary>
	/// <param name="num">the number of iterations</param>
	/// <param name="meshes">list of the meshes we combined</param>
	/// <param name="pos">position of our meshes</param>
	/// <param name="rot">rotation of our meshes</param>
	/// <param name="scale">scale of our meshes</param>
	private void Grow(int num, List<CombineInstance> meshes, Vector3 pos, Quaternion rot, float scale){

		if(num <= 0) return;

		CombineInstance inst = new CombineInstance ();

		inst.mesh = MakeCube(num);
		inst.transform = Matrix4x4.TRS(pos, rot, branchScaling*scale);

		meshes.Add (inst);



		num--;

		pos = inst.transform.MultiplyPoint (new Vector3 (0, 1, 0));
		Quaternion randomPosRot = rot * Quaternion.Euler (Random.Range(0, 60), Random.Range(0, 15), Random.Range(0, 75));
		Quaternion randomNegRot = rot * Quaternion.Euler (Random.Range(0, -60), Random.Range(0, -15),  Random.Range(0, -75));

        scale *= .75f;

		Grow (num, meshes, pos, randomPosRot, scale);
		Grow (num, meshes, pos, randomNegRot, scale);
		Grow (num, meshes, pos, randomNegRot, scale);
		Grow (num, meshes, pos, randomPosRot, scale);
		Grow (num, meshes, pos, randomNegRot, scale);
		Grow (num, meshes, pos, randomPosRot, scale);
	}
	/// <summary>
	/// This creates the actual mesh
	/// </summary>
	/// <param name="num">the number of iterations</param>
	/// <returns></returns>
	private Mesh MakeCube(int num){
		
		List<Color> colors = new List<Color> ();
		

		float hue = 1;
        Mesh mesh = MeshTools.MakeCube();
        Vector3[] verts = mesh.vertices;
        for (int i = 0; i < mesh.vertexCount; i++) {

			float tempHue = hue + (1 * verts[i].y);
			Color color = Color.HSVToRGB (tempHue, .5f, 1);
			colors.Add(color);

		}

		mesh.SetColors (colors);
		return mesh;
	}
}
[CustomEditor(typeof(CoralBroccoli))]
public class BroccoliSpawnerEditor : Editor
{
    /*adds in inspector gui for GUI changes */
    public override void OnInspectorGUI()
    {
        /*Places a button the player can hit on the GUI labelled GROW! So you can test the grow function of the coral */
        base.OnInspectorGUI();
        if (GUILayout.Button("GROW!"))
        {
            /*targets spawner when the grow button is hit */
            CoralBroccoli b = (target as CoralBroccoli);
            /* Starts grow command and transform and iterations */
            b.Build();
        }
    }
}