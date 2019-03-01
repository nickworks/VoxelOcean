using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
/// <summary>
/// BranchSpawner
/// </summary>
public class Garch_BranchSpawner : MonoBehaviour
{
    public BranchSegment prefabBranch; //reference to game object and requires objects identified as a prefabBranch
    [Range(1, 8)] public int iterations = 5;
    [Range(5, 30)] public float angle = 15;
    [Range(5, 30)] public float angle2 = 15;
    [Range(.25f, .9f)] public float scale = .75f;

    // Start is called before the first frame update
    public void DestroyChildren()
    {
        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }
        /// <summary>
        /// Grow
        /// Object grows additional objects from a single 'branch'
        /// </summary>
        /// <param name="t"> transform of the object</param>
        /// <param name="num"> iteration of the object</param>
       public void Grow(Transform t, int num)
    {
        if (num <= 0) return;
        angle = Random.Range(5, 30);
        angle2 = Random.Range(5, 30);

        BranchSegment newBranch1 = Instantiate(prefabBranch, t.position, t.rotation * Quaternion.Euler(0, angle2, angle), t);
        BranchSegment newBranch2 = Instantiate(prefabBranch, t.position, t.rotation * Quaternion.Euler(0, -angle2, -angle), t);

        scale = Random.Range(.25f, .9f);

        newBranch1.transform.localScale = Vector3.one * scale;
        newBranch2.transform.localScale = Vector3.one * scale;

        

        Grow(newBranch1.endPoint, num - 1);
        Grow(newBranch2.endPoint, num - 1);

    }
}
/// <summary>
/// Creates an editor
/// </summary>
[CustomEditor(typeof(Garch_BranchSpawner))]
public class Garch_BranchSpawnerEditor : Editor
{

    override public void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("GROW!"))
        {
            Garch_BranchSpawner b = (target as Garch_BranchSpawner);
            b.DestroyChildren();
            b.Grow(b.transform, b.iterations);
        }

    }
}