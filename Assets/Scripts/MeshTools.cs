using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshTools
{
    /// <summary>
    /// Removes duplicate vertices from a mesh by "welding" together vertices that are EXACTLY the same.
    /// </summary>
    /// <param name="complexMesh">The mesh from which to remove duplicate vertices.</param>
    /// <param name="printDebug">Whether or not to output to the console the before/after vertex count.</param>
    /// <returns>A copy of the complexMesh, with vertices removed. NOTE: The copy contains no UVs, vertex colors, or normals.</returns>
    static public Mesh RemoveDuplicates(Mesh complexMesh, bool printDebug = false)
    {
        List<Vector3> verts = new List<Vector3>();
        List<int> vertOffset = new List<int>();
        List<Vector3> normals = new List<Vector3>();
        List<int> tris = new List<int>();

        List<Vector3> newVerts = new List<Vector3>();

        complexMesh.GetVertices(verts);
        complexMesh.GetTriangles(tris, 0);
        complexMesh.GetNormals(normals);

        int count1 = verts.Count;

        // sets of duplicate vertices: (used for averaging normals)
        Dictionary<int, List<int>> sets = new Dictionary<int, List<int>>();
        List<int> vertsToSkip = new List<int>();

        for (int i = 0; i < verts.Count; i++)
        {
            if (vertsToSkip.Contains(i)) continue; // once this vert is in a set, don't add it to other sets
            newVerts.Add(verts[i]); // copy vert into new list (this should only run if the vert wasn't a dupe)
            
            // find duplicates:
            for (int j = i + 1; j < verts.Count; j++)
            {
                if (vertsToSkip.Contains(j)) continue; // once this vert is in a set, don't add it to other sets
                if (verts[i] == verts[j]) // if a duplicate vert:
                {
                    if (!sets.ContainsKey(i)) sets.Add(i, new List<int>());
                    sets[i].Add(j);
                    vertsToSkip.Add(j);
                }
            }
        }
        foreach(int j in vertsToSkip)
        {

        }
        foreach (KeyValuePair<int, List<int>> set in sets)
        {
            // average normals
            foreach (int j in set.Value)
            {
                for (int k = 0; k < tris.Count; k++) // change indices in tris list:
                {
                    if (tris[k] == j) tris[k] = set.Key;
                    if (tris[k] > j) tris[k] = tris[k] - 1;
                }
            }
        }
        int count2 = newVerts.Count;


        if (printDebug) Debug.Log($"{count1} reduced to {count2}");

        Mesh mesh = new Mesh();
        mesh.SetVertices(newVerts);
        mesh.SetTriangles(tris, 0);
        return mesh;

    } // RemoveDuplicates()
}
