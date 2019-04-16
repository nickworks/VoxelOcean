using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Fog Effect - Under Water 
/// Creates a fog effect on the camera that can be seen in editor view,
/// This was created by Cameron Garchow
/// Simple and easy Fog Effect for underwater and or other uses.
/// Feel Free to use this in your game projects an alternative to the other fog effect unity provides as it is not distance based.
/// This one is distance based and does not mess with the camera settings.
/// </summary>
[ExecuteInEditMode, ImageEffectAllowedInSceneView]
public class FogEffect : MonoBehaviour
{
    /// <summary>
    /// _mat is a reference to the shader
    /// </summary>
    public Material _mat;
    /// <summary>
    /// fog Color sets the fog color
    /// </summary>
    public Color _fogColor;

    /// <summary>
    /// Depth Start start of the fog depth
    /// </summary>
    public float _depthStart;
    /// <summary>
    /// Depth distance how far the fog is
    /// </summary>
    public float _depthDistance;


/// <summary>
/// Start
/// Gets a reference to the camera and converts to allow for depth effect with fog.
/// </summary>
    void Start()
    {
        GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
    }

    /// <summary>
    /// Update
    /// Update gives a reference to unity of the effect
    /// is called once a frame
    /// This allows us to set the object within the editor's camera settings.
    /// Also allows us to set all parts of the shader from the editor. DOES NOT NEED TO BE DELETED!
    /// </summary>
    void Update()
    {
        _mat.SetColor("_FogColor", _fogColor);
        _mat.SetFloat("_DepthStart", _depthStart);
        _mat.SetFloat("_DepthDistance", _depthDistance);
    }
    /// <summary>
    /// Renders on image 
    /// 
    /// </summary>
    /// <param name="src"> source of the camera </param>
    /// <param name="dst">distance of the material</param>
    private void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        //     material.SetFloat("_WarpAmount", Mathf.Sin(Time.time);
        Graphics.Blit(src, dst, _mat);
    }
}
