using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
/// <summary>
/// This class searches each GameObject in the scen
/// finds each unique name in the 'creator' property
/// and populates a dropdown list with the names.
/// Then lets users search for any object in the scene by that creator
/// </summary>
public class ReplacementShader : MonoBehaviour
{
    //x -ray material
    [Tooltip("The material used to find objects")]public Material xrayMat;
    //tracks whether there is something actively being searched for
    public bool activeScan = false;
    //list used to store materials on scanned objects
    List<Material> mats = new List<Material>();

    
    [Tooltip("The dropdown menu to populate")]public Dropdown dropDown;
    [Tooltip("The text property of the 'scan' button")]public Text scanButtonText;

    //list of meshrenderers, populated by what the user is scanning for
    List<MeshRenderer> renderers = new List<MeshRenderer>();

    void Start()
    {
        //start with a clean slate
        dropDown.ClearOptions();
    }
    /// <summary>
    /// Goes through the properties.all dictionary and makes a list of every unique creator name.
    /// Then populates the list of the dropdown with those strings.
    /// </summary>
    public void Populate()
    {
        dropDown.ClearOptions();
        List<string> names = new List<string>();
            foreach(PropertiesBase p in PropertiesBase.all.Values)
            {
                if(p.creator != "" && !names.Contains(p.creator))
            {
                names.Add(p.creator);
            }
            }
        dropDown.AddOptions(names);        
    }
    /// <summary>
    /// This function can be called if one needs to turn off all the scanning
    /// Just in case someone needs it
    /// </summary>
    /// <param name="populate">When calling this function, pass in true if you want the dropdown to reset as well</param>
    public void Reset(bool populate = false)
    {
        if (populate)
        {
            Populate();
        }

        activeScan = true;
        BoolCheck();
    }
    /// <summary>
    /// Check the current state of the scan function.
    /// If no active scan, start a scan, else, 
    /// put all the materials back to normal.
    /// </summary>
    public void BoolCheck()
    {
        if (dropDown.captionText.text != "")
        {
        if (!activeScan)
        {
            REPLACEMENT(dropDown.captionText.text);
            scanButtonText.text = "Stop Scan";
        }
        if (activeScan)
        {
            SwitchBack();
            scanButtonText.text = "Scan";
        }
        activeScan = !activeScan;
        }
    }
    /// <summary>
    /// This function searches the properties.all dictionary for each item with the selected Creator from the dropdown
    /// stores a reference to it's meshrenderer component
    /// makes a copy of the material
    /// then replaces the current material with the x ray material so it can be found in the scene.
    /// </summary>
    /// <param name="searchTarget">The creator name to search for</param>
    void REPLACEMENT(string searchTarget)
    {
        renderers.Clear();

        foreach(PropertiesBase p in PropertiesBase.all.Values)
        {
        //if object has the matching creator, get reference to the meshrenderer
            if(p.creator == searchTarget)
            {                
                renderers.Add(p.GetComponent<MeshRenderer>());
            }
        }
        //foreach meshrenderer, get reference to the material, and replace the default material
        foreach(MeshRenderer r in renderers)
        {
            mats.Add(r.material);
            r.material = xrayMat;
        }
    }
    /// <summary>
    /// for each item whose material was replaced
    /// put the old one back.
    /// Try/catch block used in case a referenced object is unloaded at the time of swapping.
    /// </summary>
    void SwitchBack()
    {   
        for (int i = 0; i < renderers.Count; i++)
        {
            try
            {
            renderers[i].material = mats[i];
            }
            catch
            {
                //TODO: if unloaded objects are stored in their current state, this may cause them to be reloaded with the
                //xray material, leaving this comment here for future reference
               // Debug.Log("Possible issue due to loading /unloading, please message Josh for follow up debugging");
            }
        }
    }
}
/// <summary>
/// Custom editor button for basic development testing
/// </summary>
[CustomEditor(typeof(ReplacementShader))]
public class ReplacementShaderEditor: Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ReplacementShader test = (ReplacementShader)target;
        if (GUILayout.Button("SWITCH"))
        {
            test.BoolCheck();
        }
    }
}
