using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode, ImageEffectAllowedInSceneView]
public class WaterDistEffect : MonoBehaviour
{
    public Material _mat;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //call this after all rendering is complete to render image
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        // Raw interface to Unity's drawing functions
        Graphics.Blit(source, destination, _mat); // Copies source texture into destination render texture with a shader 
    }
}
