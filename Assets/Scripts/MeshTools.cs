using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class for working with meshes.
/// </summary>
public static class MeshTools
{
    /// <summary>
    /// Removes duplicate vertices from a mesh by "welding" together vertices that are EXACTLY the same.
    /// </summary>
    /// <param name="complexMesh">The mesh from which to remove duplicate vertices.</param>
    /// <param name="smoothMesh">Whether or not to smooth out the normals of the new mesh.</param>
    /// <param name="printDebug">Whether or not to output to the console the before/after vertex count.</param>
    /// <returns>A copy of the complexMesh, with vertices removed. NOTE: The copy contains no UVs, vertex colors, or normals.</returns>
    static public Mesh RemoveDuplicates(Mesh complexMesh, bool smoothMesh = true, bool printDebug = false)
    {
        List<Vector3> verts = new List<Vector3>();
        int[] tris = new int[complexMesh.triangles.Length];
        
        Vector3[] oldVerts = complexMesh.vertices; // Calling complexMesh.vertices makes a copy of vertices! Only call this once!

        // this holds remapped index numbers / the key is the old index, the value is the new index
        Dictionary<int, int> remappedVerts = new Dictionary<int, int>();

        int removedCount = 0;
        for (int i = 0; i < oldVerts.Length; i++)
        {
            if (remappedVerts.ContainsKey(i)) // this vertex is a duplicate and has already been "remapped" onto a vertex to its left
            {
                removedCount++; // count this as a vertex that will be removed
                continue;
            }
            verts.Add(oldVerts[i]); // copy vert into new list (this should only run if the vert wasn't a dupe)
            remappedVerts.Add(i, i - removedCount); // remap its index number by shifting it left the number vertices that have been removed

            for (int j = i + 1; j < oldVerts.Length; j++) // find duplicates of this vertex to the right:
            {
                if (remappedVerts.ContainsKey(j)) continue; // once this vert has been remapped, ignore it
                if (oldVerts[i] == oldVerts[j]) remappedVerts.Add(j, verts.Count - 1);
            }
        }
        for (int k = 0; k < complexMesh.triangles.Length; k++) // change indices in tris list:
        {
            if (remappedVerts.ContainsKey(k)) tris[k] = remappedVerts[k];
            else Debug.Log($"index {k} doesn't exist in remappedVerts");
        }
        if (printDebug) Debug.Log($"{oldVerts.Length} reduced to {verts.Count}");
        
        Mesh mesh = new Mesh();
        mesh.SetVertices(verts);
        mesh.SetTriangles(tris, 0);
        if(smoothMesh) mesh.RecalculateNormals();
        return mesh;

    }
    /// <summary>
    /// This function generates and returns a 1m cube mesh. The anchor point is at the bottom of the mesh. This mesh has NO duplicate vertices. So shading will look a little odd due to unrealistic normals.
    /// </summary>
    /// <returns>A mesh with normals set. There are no UVs and no vertex colors on this mesh.</returns>
    public static Mesh MakeSmoothCube()
    {
        List<Vector3> verts = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<Vector3> normals = new List<Vector3>();
        List<int> tris = new List<int>();

        verts.Add(new Vector3(-0.5f, 0, -0.5f));
        verts.Add(new Vector3(+0.5f, 0, -0.5f));
        verts.Add(new Vector3(+0.5f, 0, +0.5f));
        verts.Add(new Vector3(-0.5f, 0, +0.5f));

        verts.Add(new Vector3(-0.5f, 1, -0.5f));
        verts.Add(new Vector3(+0.5f, 1, -0.5f));
        verts.Add(new Vector3(+0.5f, 1, +0.5f));
        verts.Add(new Vector3(-0.5f, 1, +0.5f));

        tris.Add(0);
        tris.Add(1);
        tris.Add(2);

        tris.Add(0);
        tris.Add(2);
        tris.Add(3);

        tris.Add(1);
        tris.Add(6);
        tris.Add(2);

        tris.Add(1);
        tris.Add(5);
        tris.Add(6);

        tris.Add(0);
        tris.Add(5);
        tris.Add(1);

        tris.Add(0);
        tris.Add(4);
        tris.Add(5);


        tris.Add(3);
        tris.Add(4);
        tris.Add(0);

        tris.Add(3);
        tris.Add(7);
        tris.Add(4);

        tris.Add(7);
        tris.Add(5);
        tris.Add(4);

        tris.Add(7);
        tris.Add(6);
        tris.Add(5);

        tris.Add(2);
        tris.Add(7);
        tris.Add(3);

        tris.Add(2);
        tris.Add(6);
        tris.Add(7);

        Mesh mesh = new Mesh();
        mesh.SetVertices(verts);
        mesh.SetTriangles(tris, 0);
        mesh.RecalculateNormals();
        return mesh;
    }
    /// <summary>
    /// This function generates and returns a 1m cube mesh. The anchor point is at the bottom of the mesh.
    /// </summary>
    /// <returns>A mesh object with normals and uvs. No color information has been set.</returns>
    public static Mesh MakeCube()
    {
        List<Vector3> verts = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<Vector3> normals = new List<Vector3>();
        List<int> tris = new List<int>();

        // Front face
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

        // Back face
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

        // Left face 
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

        // Right face
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

        // Top face
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

        // Bottom face 
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

        Mesh mesh = new Mesh();
        mesh.SetVertices(verts);
        mesh.SetUVs(0, uvs);
        mesh.SetNormals(normals);
        mesh.SetTriangles(tris, 0);
        return mesh;
    }
    /// <summary>
    /// Generates the mesh data to make a 1m sized cylinder mesh. Anchor point in in the bottom center of the mesh.
    /// </summary>
    /// <param name="points">The number of points the object has.</param>
    /// <returns></returns>
    public static Mesh MakeCylinder(int points)
    {
        List<Vector3> verts = new List<Vector3>();
        //List<Vector2> uvs = new List<Vector2>();
        List<Vector3> normals = new List<Vector3>();
        List<int> tris = new List<int>();

        //Generate vertices based on number of points & set normals:
            //Set start points in middle:
            verts.Add(new Vector3(0, 0, 0));                //verts(0) on Bottom Center
            normals.Add(new Vector3(0, -1, 0));                          //normals for verts(0)
            
            verts.Add(new Vector3(0, 1, 0));                //verts(1) on Top Center
            normals.Add(new Vector3(0, +1, 0));              //normals for verts(1)

            //Get points for all sides(bottom, then top); the sides will start at verts(2):
            for (int i = 0; i < points; i++)
            {       
                //find angle to spawn verts at:
                float angleDegrees = (i == 0) ?  0 : 360 / ((float)points / (float)i);
                float angleRadians = angleDegrees * (Mathf.PI / 180);
                float radius = .5f;

                //find x and z positions of created vertices:
                float vertX = Mathf.Sin(angleRadians) * radius;
                float vertZ = Mathf.Cos(angleRadians) * radius;

                //set bottom vert & normals
                verts.Add(new Vector3(vertX, 0, vertZ));
                int point1Bottom =  verts.Count - 1;
                normals.Add(Vector3.Normalize(verts[point1Bottom] - verts[0]));
                //set top vert & normals
                verts.Add(new Vector3(vertX, 1, vertZ));
                int point1Top = verts.Count - 1;
                normals.Add(Vector3.Normalize(verts[point1Top] - verts[1]));
            }

        //TODO: Generate UV values:

        //Generate triangles based on the created vertices
            //Side Trianges:
            //set up left and right index values:
            int leftBottomIndex = 2;
            int leftTopIndex = 3;

            int rightBottomIndex;
            int rightTopIndex;

            for(int i = 1; i <= points; i++)
            {
                if (i != points)
                {
                    //set index values for the right vertices:
                    rightBottomIndex = leftBottomIndex + 2;
                    rightTopIndex = leftTopIndex + 2;

                    
                } else
                {
                    //set right vertices to loop back to the starting side vertices:
                    rightBottomIndex = 2;
                    rightTopIndex = 3;
                }

                //create triangles:
                tris.Add(leftBottomIndex);
                tris.Add(rightTopIndex);
                tris.Add(leftTopIndex);

                tris.Add(rightTopIndex);
                tris.Add(leftBottomIndex);
                tris.Add(rightBottomIndex);

                //Set index values for next side.
                leftBottomIndex = rightBottomIndex;
                leftTopIndex = rightTopIndex;
            }   
        
            //Bottom Triangles (Even index values):
            for(int bv = 2; bv <= verts.Count - 2; bv += 2)
            {
                if (bv != verts.Count - 2)
                {
                    //grab vertices from index
                        //start tri at bv (Bottom Vertex)
                        tris.Add(bv);
                        //Set vertex at index 0
                        tris.Add(0);
                        //End at vertex bv + 2;
                        tris.Add(bv + 2);
                } else
                {
                //Set last triangle using the center and point 1:
                    tris.Add(bv);
                    tris.Add(0);            //Bottom Center Index
                    tris.Add(2);            //Bottom Point 1 Index
                }
            }
            
            //Top Triangles (Odd index values):
            for(int tv = 1; tv <= verts.Count - 1; tv += 2)
            {
                if (tv != verts.Count  - 1)
                {
                    //set tris from vertices at index
                        //Set at vertex tv + 2
                        tris.Add(tv + 2);
                        //Set at vertex at index 1
                        tris.Add(1);
                        //start tri at tv (Top Vertex)
                        tris.Add(tv);       
                } else
                {
                //Set last triangle using the center and point 1:
                    tris.Add(3);
                    tris.Add(1);                //Top Center Index
                    tris.Add(tv);                //Top Point 1 Index
                }
            }

        //Generate mesh based on verts, tris, and normals
        Mesh mesh = new Mesh();
        mesh.SetVertices(verts);
        //mesh.SetUVs(uvs);
        mesh.SetNormals(normals);
        mesh.SetTriangles(tris, 0);
        return mesh;
    }

    /// <summary>
    /// Creates a 1m cube that tapers in on it's front end by a variable amount.
    /// </summary>
    /// <param name="taperAmount">Sets the amount the cube tapers in at it's front.</param>
    /// <returns>Mesh data for a cube that tapers in on the end by the taper amount.</returns>
    public static Mesh MakeTaperCube(float taperAmount)
    {
        List<Vector3> verts = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<Vector3> normals = new List<Vector3>();
        List<int> tris = new List<int>();

        // Front face
        verts.Add(new Vector3(-0.5f * taperAmount, +0.5f * taperAmount, 1));
        verts.Add(new Vector3(+0.5f * taperAmount, +0.5f * taperAmount, 1));
        verts.Add(new Vector3(+0.5f * taperAmount, -0.5f * taperAmount, 1));
        verts.Add(new Vector3(-0.5f * taperAmount, -0.5f * taperAmount, 1));
        normals.Add(new Vector3(0, 0, -1));
        normals.Add(new Vector3(0, 0, -1));
        normals.Add(new Vector3(0, 0, -1));
        normals.Add(new Vector3(0, 0, -1));
        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 1));
        uvs.Add(new Vector2(1, 0));
        tris.Add(2);
        tris.Add(1);
        tris.Add(0);
        tris.Add(0);
        tris.Add(3);
        tris.Add(2);

        // Back face
        verts.Add(new Vector3(-0.5f, +0.5f, 0));
        verts.Add(new Vector3(+0.5f, +0.5f, 0));
        verts.Add(new Vector3(+0.5f, -0.5f, 0));
        verts.Add(new Vector3(-0.5f, -0.5f, 0));
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

        // Left face 
        verts.Add(new Vector3(-0.5f, +0.5f, 0));
        verts.Add(new Vector3(-0.5f * taperAmount, +0.5f * taperAmount, 1));
        verts.Add(new Vector3(-0.5f * taperAmount, -0.5f * taperAmount, 1));
        verts.Add(new Vector3(-0.5f, -0.5f, 0));
        normals.Add(new Vector3(-1, 0, 0));
        normals.Add(new Vector3(-1, 0, 0));
        normals.Add(new Vector3(-1, 0, 0));
        normals.Add(new Vector3(-1, 0, 0));
        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 1));
        uvs.Add(new Vector2(1, 0));
        tris.Add(10);
        tris.Add(9);
        tris.Add(8);
        tris.Add(8);
        tris.Add(11);
        tris.Add(10);

        // Right face
        verts.Add(new Vector3(+0.5f, +0.5f, 0));
        verts.Add(new Vector3(+0.5f * taperAmount, +0.5f * taperAmount, 1));
        verts.Add(new Vector3(+0.5f * taperAmount, -0.5f * taperAmount, 1));
        verts.Add(new Vector3(+0.5f, -0.5f, 0));
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

        // Top face
        verts.Add(new Vector3(-0.5f, +0.5f, 0));
        verts.Add(new Vector3(+0.5f, +0.5f, 0));
        verts.Add(new Vector3(+0.5f * taperAmount, +0.5f * taperAmount, 1));
        verts.Add(new Vector3(-0.5f * taperAmount, +0.5f * taperAmount, 1));
        normals.Add(new Vector3(0, +1, 0));
        normals.Add(new Vector3(0, +1, 0));
        normals.Add(new Vector3(0, +1, 0));
        normals.Add(new Vector3(0, +1, 0));
        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 1));
        uvs.Add(new Vector2(1, 0));
        tris.Add(18);
        tris.Add(17);
        tris.Add(16);
        tris.Add(16);
        tris.Add(19);
        tris.Add(18);

        // Bottom face 
        verts.Add(new Vector3(-0.5f, -0.5f, 0));
        verts.Add(new Vector3(+0.5f, -0.5f, 0));
        verts.Add(new Vector3(+0.5f * taperAmount, -0.5f * taperAmount, 1));
        verts.Add(new Vector3(-0.5f * taperAmount, -0.5f * taperAmount, 1));
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

        Mesh mesh = new Mesh();
        mesh.SetVertices(verts);
        mesh.SetUVs(0, uvs);
        mesh.SetNormals(normals);
        mesh.SetTriangles(tris, 0);
        return mesh;
    }
}
