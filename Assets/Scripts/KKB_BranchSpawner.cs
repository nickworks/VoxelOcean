using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class KKB_BranchSpawner : MonoBehaviour
{
    /*sets the prefab this script is attached to */
    public KKB_BranchSegment prefabBranch;
    /*sets how many iterations are spawned of the branch */
    [Range(1, 8)] public int iterations = 7;
    /*sets the angle that the branches spawn at of the coral */
    [Range(5, 90)] public float angle = 35;
    /*sets how big the next size up of the iteration will be compared to the current one */
    [Range(.25f, .9f)] public float scale = .7f;

    public void DestroyChildren()
    {
        /* destroys any children left over from a previous spawn in */
        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }
    /**
     * Grows a new iteration when a branch is spawned in 
     * Transform t what will be edited by the angle, rotation, scale
     * int num skips over this section if num is less than or equal to zero
     * */
    public void Grow(Transform t, int num)
    {
        if (num <= 0) return;
        /* The different branches that spawn in when the grow button is hit. Each has a position rotation and angle it spawns into */
        KKB_BranchSegment newBranch1 = Instantiate(prefabBranch, t.position, t.rotation * Quaternion.Euler(0, 0, angle), t);
        KKB_BranchSegment newBranch2 = Instantiate(prefabBranch, t.position, t.rotation * Quaternion.Euler(0, 0, 0), t);
        KKB_BranchSegment newBranch3 = Instantiate(prefabBranch, t.position, t.rotation * Quaternion.Euler(0, 0, -angle), t);
       

        /*sets the scale of the branches when they spawn in will gradually gets smaller as each iteration happens */
        newBranch1.transform.localScale = Vector3.one * scale;
        newBranch2.transform.localScale = Vector3.one * scale;
        newBranch3.transform.localScale = Vector3.one * scale;
      

        /*sets each new branch to spawn at the endpoint that was previously set */
        Grow(newBranch1.endPoint, num -1);
        Grow(newBranch2.endPoint, num -1);
       Grow(newBranch3.endPoint, num -1);
    

    }
}
/*The editor for KKB_BranchSpawner*/
[CustomEditor(typeof(KKB_BranchSpawner))]
public class BranchSpawnerEditor : Editor
{
  /*adds in inspector gui for GUI changes */
   public override void OnInspectorGUI()
    {
        /*Places a button the player can hit on the GUI labelled GROW! So you can test the grow function of the coral */
        base.OnInspectorGUI();
        if (GUILayout.Button("GROW!"))
        {
            /*targets spawner when the grow button is hit */
            KKB_BranchSpawner b = (target as KKB_BranchSpawner);
            /*destroys any children left over from a previous grow */
            b.DestroyChildren();
            /* Starts grow command and transform and iterations */
            b.Grow(b.transform, b.iterations);
        }
    }
}
