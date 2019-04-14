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

			branchScaling = new Vector3 (.25f, Random.Range(1.1f, 2.1f), .25f);

		if(num <= 0) return; 
	
		CombineInstance inst = new CombineInstance (); 

		inst.mesh = MakeCube(num); 
		inst.transform = Matrix4x4.TRS(pos, rot, branchScaling*scale); 

		meshes.Add (inst); 

		float localScale = scale; 

		num--; 

		pos = inst.transform.MultiplyPoint (new Vector3 (0, 1, 0)); 
		Quaternion randomPosRot = rot * Quaternion.Euler (Random.Range(0, 60), Random.Range(0, 15), Random.Range(0, 75)); 
		Quaternion randomNegRot = rot * Quaternion.Euler (Random.Range(0, -60), Random.Range(0, -15),  Random.Range(0, -75)); 
		localScale *= .5f; 

		Grow (num, meshes, pos, randomPosRot, localScale); 
		Grow (num, meshes, pos, randomNegRot, localScale); 
		Grow (num, meshes, pos, randomNegRot, localScale); 
		Grow (num, meshes, pos, randomPosRot, localScale); 
		Grow (num, meshes, pos, randomNegRot, localScale); 
		Grow (num, meshes, pos, randomPosRot, localScale); 
	} 
	public void DestroyChildren()
	{
		/* destroys any children left over from a previous spawn in */
		while (transform.childCount > 0)
		{
			DestroyImmediate(transform.GetChild(0).gameObject);
		}
	}
	/// <summary> 
	/// This creates the actual mesh 
	/// </summary> 
	/// <param name="num">the number of iterations</param> 
	/// <returns></returns> 
	private Mesh MakeCube(int num){ 
		List<Vector3> verts = new List<Vector3> (); 
		List<Vector2> uvs = new List<Vector2> (); 
		List<Vector3> normals = new List<Vector3> (); 
		List<Color> colors = new List<Color> (); 
		List<int> tris = new List<int> (); 

		//Front 
		verts.Add(new Vector3(-0.5f, 0, -0.5f)); 
		verts.Add(new Vector3(-0.5f, 1, -0.5f)); 
		verts.Add(new Vector3(0.5f, 1, -0.5f)); 
		verts.Add(new Vector3(0.5f, 0, -0.5f)); 
		normals.Add (new Vector3 (0, 0, -1)); 
		normals.Add (new Vector3 (0, 0, -1)); 
		normals.Add (new Vector3 (0, 0, -1)); 
		normals.Add (new Vector3 (0, 0, -1)); 
		uvs.Add(new Vector2(0,0)); 
		uvs.Add(new Vector2(0,1)); 
		uvs.Add(new Vector2(1,1)); 
		uvs.Add(new Vector2(1,0)); 
		tris.Add (0); 
		tris.Add (1); 
		tris.Add (2); 
		tris.Add (2); 
		tris.Add (3); 
		tris.Add (0); 

		//Back 
		verts.Add(new Vector3(-0.5f, 0, 0.5f)); 
		verts.Add(new Vector3(0.5f, 0, 0.5f)); 
		verts.Add(new Vector3(0.5f, 1, 0.5f)); 
		verts.Add(new Vector3(-0.5f, 1, 0.5f)); 
		normals.Add (new Vector3 (0, 0, 1)); 
		normals.Add (new Vector3 (0, 0, 1)); 
		normals.Add (new Vector3 (0, 0, 1)); 
		normals.Add (new Vector3 (0, 0, 1)); 
		uvs.Add(new Vector2(0,0)); 
		uvs.Add(new Vector2(0,1)); 
		uvs.Add(new Vector2(1,1)); 
		uvs.Add(new Vector2(1,0)); 
		tris.Add (4); 
		tris.Add (5); 
		tris.Add (6); 
		tris.Add (6); 
		tris.Add (7); 
		tris.Add (4); 

		//left 
		verts.Add(new Vector3(-0.5f, 0, -0.5f)); 
		verts.Add(new Vector3(-0.5f, 0, 0.5f)); 
		verts.Add(new Vector3(-0.5f, 1, 0.5f)); 
		verts.Add(new Vector3(-0.5f, 1, -0.5f)); 
		normals.Add (new Vector3 (-1, 0, 0)); 
		normals.Add (new Vector3 (-1, 0, 0)); 
		normals.Add (new Vector3 (-1, 0, 0)); 
		normals.Add (new Vector3 (-1, 0, 0)); 
		uvs.Add(new Vector3(0,0)); 
		uvs.Add(new Vector3(0,1)); 
		uvs.Add(new Vector3(1,1)); 
		uvs.Add(new Vector3(1,0)); 
		tris.Add (8); 
		tris.Add (9); 
		tris.Add (10); 
		tris.Add (10); 
		tris.Add (11); 
		tris.Add (8); 

		//right 
		verts.Add(new Vector3(0.5f, 0, -0.5f)); 
		verts.Add(new Vector3(0.5f, 1, -0.5f)); 
		verts.Add(new Vector3(0.5f, 1, 0.5f)); 
		verts.Add(new Vector3(0.5f, 0, 0.5f)); 
		normals.Add (new Vector3 (1, 0, 0)); 
		normals.Add (new Vector3 (1, 0, 0)); 
		normals.Add (new Vector3 (1, 0, 0)); 
		normals.Add (new Vector3 (1, 0, 0)); 
		uvs.Add(new Vector3(0,0)); 
		uvs.Add(new Vector3(0,1)); 
		uvs.Add(new Vector3(1,1)); 
		uvs.Add(new Vector3(1,0)); 
		tris.Add (12); 
		tris.Add (13); 
		tris.Add (14); 
		tris.Add (14); 
		tris.Add (15); 
		tris.Add (12); 

		//top 
		verts.Add(new Vector3(-0.5f, 1, -0.5f)); 
		verts.Add(new Vector3(-0.5f, 1, +0.5f)); 
		verts.Add(new Vector3(+0.5f, 1, +0.5f)); 
		verts.Add(new Vector3(+0.5f, 1, -0.5f)); 
		normals.Add (new Vector3 (0, 1, 0)); 
		normals.Add (new Vector3 (0, 1, 0)); 
		normals.Add (new Vector3 (0, 1, 0)); 
		normals.Add (new Vector3 (0, 1, 0)); 
		uvs.Add(new Vector3(0,0)); 
		uvs.Add(new Vector3(0,1)); 
		uvs.Add(new Vector3(1,1)); 
		uvs.Add(new Vector3(1,0)); 
		tris.Add (16); 
		tris.Add (17); 
		tris.Add (18); 
		tris.Add (18); 
		tris.Add (19); 
		tris.Add (16); 

		//bottom 
		verts.Add(new Vector3(-0.5f, 0, -0.5f)); 
		verts.Add(new Vector3(+0.5f, 0, -0.5f)); 
		verts.Add(new Vector3(+0.5f, 0, +0.5f)); 
		verts.Add(new Vector3(-0.5f, 0, +0.5f)); 
		normals.Add (new Vector3 (0, -1, 0)); 
		normals.Add (new Vector3 (0, -1, 0)); 
		normals.Add (new Vector3 (0, -1, 0)); 
		normals.Add (new Vector3 (0, -1, 0)); 
		uvs.Add(new Vector3(0,0)); 
		uvs.Add(new Vector3(0,1)); 
		uvs.Add(new Vector3(1,1)); 
		uvs.Add(new Vector3(1,0)); 
		tris.Add (20); 
		tris.Add (21); 
		tris.Add (22); 
		tris.Add (22); 
		tris.Add (23); 
		tris.Add (20); 
		float hue = Random.Range(.82f, .873f); 
		Color color;
		foreach (Vector3 pos in verts) { 
			color = Color.HSVToRGB (Random.Range(.82f, .9f), .5f, 1);
			colors.Add(color); 
		} 

		Mesh mesh = new Mesh (); 
		mesh.SetVertices (verts); 
		mesh.SetUVs (0, uvs); 
		mesh.SetNormals (normals); 
		mesh.SetTriangles (tris, 0); 
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
			/*destroys any children left over from a previous grow */
			b.DestroyChildren();
			/* Starts grow command and transform and iterations */
			b.Build();
		}
	}
}
