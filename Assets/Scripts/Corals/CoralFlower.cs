//First uploaded by Justin Dickinson. @ me on the discord if you have questions
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CoralFlower : MonoBehaviour
{
    // TODO: Please update comments to be documentation style.  You can easily do this by starting your comment with "///".
    /// <summary>
    /// controls number of times the loop will run
    /// </summary>
	[Tooltip("controls number of times the loop will run")]
	[Range(1, 15)] public int iterations = 11;

	/// <summary>
	/// controls the odds of a branch splitting
	/// </summary>
	[Tooltip("controls the odds of a branch splitting")]
	[Range(0, 1)] public float splitOdds = .5f;
	///<summary>
	/// controls the chance of assigning a random rotation
	/// </summary>
	[Tooltip("controls the chance of assigning a random rotation")]
	[Range(0, 1)] public float ranRotOdds = .37f;
	///<summary>
	/// controls the range of what the random rotations can be
	/// </summary>                                                       
	[Tooltip("controls the range of what the random rotations can be")]
	[Range(0, 360)] public float ranRotRange = 30f;
	///<summary>
	///controls the angle that causes the splits
	/// </summary>
	[Tooltip("controls the angle that causes the splits")]
	[Range(0, 360)] public float spread = 20f;
	///<summary>
	///used to set the minimum for the range of hue values for the color
	/// </summary>
	[Tooltip("used to set the minimum for the range of hue values for the color")]
	[Range(0, 1)]  public float hueMin = 0f;
	///<summary>
	///used to set the maximum for the range of hue values for the color
	/// </summary>
	[Tooltip("used to set the maximum for the range of hue values for the color")]
	[Range(0, 1)] public float hueMax = 1f;


	/// <summary>
	/// controls the proportions of the boxes
	/// </summary>
	[Tooltip("controls the proportions of the boxes")]
	public Vector3 branchScale = new Vector3(.5f, 2f, .5f);
	/// <summary>
	/// controls hot the box scale changes between iterations
	/// </summary>
	[Tooltip("controls hot the box scale changes between iterations")]
	[Range(0.1f, 1f)] public float scaleFactor = 0.9f;
	/// <summary>
	/// controls the normal distribution of branches's z rotation
	/// </summary>
	[Tooltip("controls the normal distribution of branches's z rotation")]
	public float zRot = 0;

	/// <summary>
	/// controls the normal distribution of branches's x rotation
	/// </summary>
	[Tooltip("controls the normal distribution of branches's x rotation")]
	public float xRot = -15;
	/// <summary>
	/// controls the normal distribution of branches's y rotation
	/// </summary>
	[Tooltip("controls the normal distribution of branches's y rotation")]
	public float yRot = -15;

    /// <summary>
    /// controls the normal distribution of branches's y rotation
    /// </summary>
    [Tooltip("controls the lengths of the segments")]
    [Range(0, 10)] public float length = 1;

    /// <summary>
    /// controls the normal distribution of branches's y rotation
    /// </summary>
    [Tooltip("controls how spread out the petals are")]
    [Range(1, 360)] public int degreeOfSeparation = 10; //randomize from 4 to 15

    [Tooltip("changes the angle of the petals relative to the base")]
    [Range(0, 360)] public int degreeFromBase = 115; //randomize from 100 to 118

    private List<Quaternion> rots; 

    ///<summary>Runs the Build() function</summary>
    void Start()
	{
        
        Build();	///make a coral when this script is made
		
	}
    /// <summary>
    /// Makes a new coral, and applies the mesh to itself
    /// </summary>
	public void Build()
	{
		List<CombineInstance> meshes = new List<CombineInstance>();

        if(rots != null) rots.Clear(); 
        rots = new List<Quaternion>();


        degreeOfSeparation = (int)(Mathf.Lerp(4f, 16f, Random.value));
        degreeFromBase = (int)(Mathf.Lerp(100f, 119f, Random.value));
        length = (Mathf.Lerp(.25f, .5f, Random.value));

        Grow(0, meshes, Vector3.zero, Quaternion.identity, 1);		///make the coral
		GetComponent<Transform>().Rotate(0, Random.value * 360, 0);	///randomize y rotation of the coral

		Mesh mesh = new Mesh();
		mesh.CombineMeshes(meshes.ToArray());						///combine meshes

		MeshFilter meshFilter = GetComponent<MeshFilter>();			///change assigned mesh
		meshFilter.mesh = mesh;

		MeshRenderer meshRenderer = GetComponent<MeshRenderer>();	///show assigned mesh

		//Material mat = (Material)mesh.colors; 

		//meshRenderer.material = mesh.colors; 

	}
    /// <summary>
    /// The iterative function that grows the coral
    /// </summary>
    /// <param number of iterations already performed="num"></param>
    /// <param list of meshes that remembers what the other iteration added="meshes"></param>
    /// <param where to spawn the object="pos"></param>
    /// <param the rotation of the previous object="rot"></param>
    /// <param how much smaller should this be than the last iteration="scale"></param>
	private void Grow(int num, List<CombineInstance> meshes, Vector3 pos, Quaternion rot, float scale)
	{
		if (num > iterations) return;
		int kids = 2;
		if (num > iterations - 2) kids = 1; ///makes the coral plant thinner near the end of the iterations
        

        CombineInstance inst = new CombineInstance();
		inst.mesh = MakeCube(num);
		if (num == 0)
		{
			inst.transform = Matrix4x4.TRS(pos, Quaternion.Euler(0,0,0), new Vector3(.15f * length, length, .15f * length)); ///sets the transform of the first piece
			num++;
		}
        else if(num == 1)
        {
            inst.transform = Matrix4x4.TRS(pos, rot, new Vector3(.1f * length, 2 * length, .1f * length)); ///sets the transform of the first iteration
			num++;
        }
		else
		{
            inst.transform = Matrix4x4.TRS(pos, rot, new Vector3(.3f * length, length, .05f * length));
            //inst.transform = Matrix4x4.TRS(pos, rot, branchScale * scale);	///sets the transform of the later iterations
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

			
			

			if ( num == 1)
			{
                ///sets rotations of the second iteration
                for (int i = 0; i < 360; i += degreeOfSeparation)
                {
                    Quaternion newRot = rot * Quaternion.Euler(degreeFromBase, i, 0);
                    rots.Add(newRot); 
                }

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
                for (int i = 0; i < rots.Count; i++)
                {
                    Grow(num, meshes, lSidePos, rots[i], scale);    ///spawn all the branches off of the original
                }

			}
			else if (num == 2)
			{
                int x = (int)((Random.value - .5) * 100);
                int z = (int)((Random.value - .5) * 100);
                //int y = (int)((Random.value - .5) * 100);

                lrot = Quaternion.Euler(x, 0, z); 
                Grow(num, meshes, lSidePos, lrot, scale);
            }
			else
			{
                Grow(num, meshes, lSidePos, lrot, scale);
            }
			


		}   

	}
    /// <summary>
    /// Generates a cube with vertices, uvs, normals, triangles, and colors
    /// </summary>
    /// <param which iteration this is="num"></param>
    /// <returns>a cube mesh</returns>
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

[CustomEditor(typeof(CoralFlower))]
public class CoralFlowerEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		if (GUILayout.Button("GROW!"))
		{
			CoralFlower b = (target as CoralFlower);

			b.Build();
		}
	}
}
