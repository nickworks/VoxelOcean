//First uploaded by Justin Dickinson. @ me on the discord if you have questions
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CoralBauble : MonoBehaviour
{
    // TODO: Please update comments to be documentation style.  You can easily do this by starting your comment with "///".
	[Range(1, 15)] public int iterations = 11;			//controls number of times the loop will run
				
	[Range(0, 1)] public float splitOdds = .5f;			//controls the odds of a branch splitting

	[Range(0, 1)] public float ranRotOdds = .37f;		//control the chance of assigning a random rotation

	[Range(0, 360)] public float ranRotRange = 30f;		//controls the range of what the random rotations can be

	[Range(0, 360)] public float spread = 20f;			//controls the angle that causes the splits

    [Range(0, 1)]  public float hueMin = 0f;			//used to set the range of hue values for the color
    [Range(0, 1)] public float hueMax = 1f; 



	public Vector3 branchScale = new Vector3(.25f, 1f, .25f);	//controls the proportions of the boxes

	[Range(0.1f, 1f)] public float scaleFactor = 0.9f;			//controls hot the box scale changes between iterations

	public float zRot = 0;		//controls the normal distribution of branches using angles
	public float xRot = -15;
	public float yRot = -15;

	void Start()
	{
        Build();	//make a coral when this script is made
		
	}

	public void Build()
	{
		List<CombineInstance> meshes = new List<CombineInstance>();

		Grow(0, meshes, Vector3.zero, Quaternion.identity, 1);		//make the coral
		GetComponent<Transform>().Rotate(0, Random.value * 360, 0);	//randomize y rotation of the coral

		Mesh mesh = new Mesh();
		mesh.CombineMeshes(meshes.ToArray());						//combine meshes

		MeshFilter meshFilter = GetComponent<MeshFilter>();			//change assigned mesh
		meshFilter.mesh = mesh;

		MeshRenderer meshRenderer = GetComponent<MeshRenderer>();	//show assigned mesh

		//Material mat = (Material)mesh.colors; 

		//meshRenderer.material = mesh.colors; 

	}

	private void Grow(int num, List<CombineInstance> meshes, Vector3 pos, Quaternion rot, float scale)
	{
		if (num > iterations) return;
		int kids = 2;
		if (num > iterations - 2) kids = 1; //makes the coral plant thinner near the end of the iterations

		CombineInstance inst = new CombineInstance();
		inst.mesh = MakeCube(num);
		if (num == 0)
		{
			inst.transform = Matrix4x4.TRS(pos, Quaternion.Euler(0,0,0), new Vector3(.2f, 1f, .2f)); //sets the transform of the first piece
			num++;
		}
		else
		{
			inst.transform = Matrix4x4.TRS(pos, rot, branchScale * scale);	//sets the transform of the later iterations
			num++;
		}

		meshes.Add(inst);

		//pos = inst.transform.MultiplyPoint(new Vector3(0, 1, 0));



		Vector3 lSidePos = Vector3.zero;


		Quaternion rot1 = rot * Quaternion.Euler(xRot, yRot, zRot);
		Quaternion rot2 = rot * Quaternion.Euler(xRot, yRot, -zRot);
		scale *= scaleFactor;



		float ran = Random.value;
		if ((ran < 1) && kids > 0) 
		{
			if (num == 1)
			{
				lSidePos = inst.transform.MultiplyPoint(new Vector3(0f, 1f, 0f));
			}
			else
			{
				lSidePos = inst.transform.MultiplyPoint(new Vector3(0f, 1f, 0f));
			}

			Quaternion lrot = Quaternion.identity;
			Quaternion rtrot = Quaternion.identity;
			Quaternion rbrot = Quaternion.identity;
			Quaternion rot4 = Quaternion.identity;
			Quaternion rot5 = Quaternion.identity;
			Quaternion rot6 = Quaternion.identity;
			Quaternion rot7 = Quaternion.identity;
			Quaternion rot8 = Quaternion.identity;
			
			//TODO: allow a piece to know which side of its parent it is growing off

			if ( num == 1)
			{
																				//sets rotations of the second iteration
				rtrot = rot * Quaternion.Euler(45, yRot + 180, -45);
				lrot = rot * Quaternion.Euler(xRot, yRot, 45);
				rbrot = rot * Quaternion.Euler(45, 0, 45);
				rot4 = rot * Quaternion.Euler(45, 0, 0);
				rot5 = rot * Quaternion.Euler(45, 0, -45);
				rot6 = rot * Quaternion.Euler(0, 0, -45);
				rot7 = rot * Quaternion.Euler(-45, 0, -45);
				rot8 = rot * Quaternion.Euler(-45, 0, 0);
				

			}
			else if (ran < ranRotOdds)
			{
				float xRan = Random.value * ranRotRange;
				float yRan = Random.value * ranRotRange;
				float zRan = Random.value * ranRotRange;
				lrot = rot * Quaternion.Euler(xRan, yRan, zRan);
			}
			else 
			{
				lrot = rot * Quaternion.Euler(xRot, yRot, 0);
			}

			kids--;

			if (num == 1)
			{
				Grow(num, meshes, lSidePos, lrot, scale);		//spawn all the branches off of the original
				Grow(num, meshes, lSidePos, rtrot, scale);
				Grow(num, meshes, lSidePos, rbrot, scale);
				Grow(num, meshes, lSidePos, rot4, scale);
				Grow(num, meshes, lSidePos, rot5, scale);
				Grow(num, meshes, lSidePos, rot6, scale);
				Grow(num, meshes, lSidePos, rot7, scale);
				Grow(num, meshes, lSidePos, rot8, scale);

			}
			else if (Random.value > splitOdds)
			{
				Grow(num, meshes, lSidePos, lrot, scale);
			}
			else
			{
				Grow(num, meshes, lSidePos, lrot * Quaternion.Euler(spread, spread, spread), scale);
				Grow(num, meshes, lSidePos, lrot * Quaternion.Euler(-spread, -spread, -spread), scale);
			}
			


		}   

	}

	private Mesh MakeCube(int num)
	{
		List<Vector3> verts = new List<Vector3>();
		List<Vector2> uvs = new List<Vector2>();
		List<Vector3> normals = new List<Vector3>();
		List<int> tris = new List<int>();
		List<Color> colors = new List<Color>();

		//Color col = new Color(0, (float)(num / iterations), 0, 1);

		//front
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

		//back
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

		//left
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

		//right
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

		//top
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

		//bottom
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




        float hue = Mathf.Lerp(hueMin, hueMax, (num/(float)iterations));
        

        foreach (Vector3 pos in verts)
		{
            //colors.Add(new Color(0,0,1));
            //colors.Add(Random.ColorHSV(0, 1)); 

            float tempHue = hue + (1/(float)iterations) * pos.y; 

            Color color = Color.HSVToRGB(tempHue, 1, 1);

            colors.Add(color); 
		}

		Mesh mesh = new Mesh();
		mesh.SetVertices(verts);
		mesh.SetUVs(0, uvs);
		mesh.SetNormals(normals);
		mesh.SetTriangles(tris, 0);
		mesh.SetColors(colors);
		//mesh.colors32(num / iterations * 255, 0, 0);
		return mesh;


	}


}

[CustomEditor(typeof(CoralBauble))]
public class CoralBaubleEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if (GUILayout.Button("GROW!"))
		{
			CoralBauble b = (target as CoralBauble);

			b.Build();
		}
	}
}
