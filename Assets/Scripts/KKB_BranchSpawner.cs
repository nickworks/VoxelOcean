using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class KKB_BranchSpawner : MonoBehaviour
{
    public BranchSegment prefabBranch;
    [Range(1, 8)] public int iterations = 7;
    [Range(5, 90)] public float angle = 35;
    [Range(.25f, .9f)] public float scale = .7f;

    public void DestroyChildren()
    {
        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }
    public void Grow(Transform t, int num)
    {
        if (num <= 0) return;

        BranchSegment newBranch1 = Instantiate(prefabBranch, t.position, t.rotation * Quaternion.Euler(0, 0, angle), t);
        BranchSegment newBranch2 = Instantiate(prefabBranch, t.position, t.rotation * Quaternion.Euler(0, 0, 0), t);
        BranchSegment newBranch3 = Instantiate(prefabBranch, t.position, t.rotation * Quaternion.Euler(0, 0, -angle), t);
       


        newBranch1.transform.localScale = Vector3.one * scale;
        newBranch2.transform.localScale = Vector3.one * scale;
        newBranch3.transform.localScale = Vector3.one * scale;
      


        Grow(newBranch1.endPoint, num -1);
        Grow(newBranch2.endPoint, num -1);
       Grow(newBranch3.endPoint, num -1);
    

    }
}

[CustomEditor(typeof(KKB_BranchSpawner))]
public class BranchSpawnerEditor : Editor
{
   public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("GROW!"))
        {
            KKB_BranchSpawner b = (target as KKB_BranchSpawner);
            b.DestroyChildren();
            b.Grow(b.transform, b.iterations);
        }
    }
}
