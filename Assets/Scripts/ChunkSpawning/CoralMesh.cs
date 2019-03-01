using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
//This class makes The Broccoli Coral
public class CoralMesh : MonoBehaviour {
	//range for how many iterations we want to spawn out of each section
	[Range(2, 8)]public int iterations = 3;
	//The scaling for the size of each branch
	public Vector3 branchScaling = new Vector3 (.25f, 1, 25f);
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	/// 
	/// This combinds the mesh together and then continues to build it
	public void Build () {
		List<CombineInstance> meshes = new List<CombineInstance> ();

		Grow (iterations, meshes, Vector3.zero, Quaternion.identity, 1);

		Mesh mesh = new Mesh ();
		mesh.CombineMeshes (meshes.ToArray());

		MeshFilter meshFilter = GetComponent<MeshFilter> ();
		meshFilter.mesh = mesh;
	}
	///This actually makes the cubess
	/// int num: number of iterations we want to put on each branch
	/// List<CombineINstance>: a list of all of the objects we want to combine
	/// Vector3 pos: the position we want to grow at
	/// Quaterion rot: the rotation we want to give the objects
	/// float scale: the scale we want to give the objects
	private void Grow(int num, List<CombineInstance> meshes, Vector3 pos, Quaternion rot, float scale){

		if(num <= 0) return;

		CombineInstance inst = new CombineInstance ();

		inst.mesh = MakeCube(num);
		inst.transform = Matrix4x4.TRS(pos, rot, branchScaling*scale);

		meshes.Add (inst);



		num--;

		pos = inst.transform.MultiplyPoint (new Vector3 (0, 1, 0));//decides how far away from the last cube the next one is
		Quaternion randomPosRot = rot * Quaternion.Euler (Random.Range(0, 60), Random.Range(0, 15), Random.Range(0, 75));
		Quaternion randomNegRot = rot * Quaternion.Euler (Random.Range(0, -60), Random.Range(0, -15),  Random.Range(0, -75));
		scale *= .65f;

		Grow (num, meshes, pos, randomPosRot, scale);
		Grow (num, meshes, pos, randomNegRot, scale);
		Grow (num, meshes, pos, randomNegRot, scale);
		Grow (num, meshes, pos, randomPosRot, scale);
		Grow (num, meshes, pos, randomNegRot, scale);
		Grow (num, meshes, pos, randomPosRot, scale);
	}
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
		float hue = 1;

		foreach (Vector3 pos in verts) {

			float tempHue = hue + (1 * pos.y);
			Color color = Color.HSVToRGB (tempHue, .5f, 1);
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


[CustomEditor(typeof(CoralMesh))]
public class CoralMeshEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI ();

		if(GUILayout.Button("Grow!"))
		{
			CoralMesh b = (target as CoralMesh);
			b.Build();
		}
	}
}