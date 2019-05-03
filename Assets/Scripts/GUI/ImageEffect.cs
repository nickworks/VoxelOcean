using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Image Effect - Under Water 
/// Creates a distortion effect on the camera that can be seen in editor view,
/// This was created by Cameron Garchow with Inspiration from PeerPlay
/// Items are also designed for use in basically any other part.
/// This is also taken form the ideas from nick pattison's warp effect as seen in class
/// This effect is essential to feeling underwater
/// Inspiration Taken from : https://www.youtube.com/watch?v=SN_syWoNBYs by PeerPlay - Not a Direct Copy of how it is done.
/// TODO : Add a Sound matrix that gives the feeling of underwater (subtle not too much)
/// </summary>
public class ImageEffect : MonoBehaviour
{
    /// <summary>
    /// material is a reference to the shader material
    /// </summary>
    public Material material;

    /// <summary>
    /// Noise Scale is the scale of the noise volume
    /// </summary>
    [Range(0.0f, 0.2f)]
    public float _noiseScale = 0.1f;

    /// <summary>
    /// _NoiseSpeed sets the speed of the noise volume on the camera, the higher the faster the movement of the noise.
    /// </summary>
    public Vector2 _noiseSpeed = new Vector2(.2f, .3f);


    /// <summary>
    /// Update gives a reference to unity of the effect
    /// is called once a frame
    /// This allows us to set the object within the editor's camera settings.
    /// Also allows us to set all parts of the shader from the editor. DOES NOT NEED TO BE DELETED!
    /// Code is very similar to tutorial because there is no other way to do it unless you want me to jump through hoola hoops!
    /// </summary>
    void Update()
    {
        material.SetFloat("_NoiseSpeedX", _noiseSpeed.x);
        material.SetFloat("_NoiseSpeedY", _noiseSpeed.y);
        material.SetFloat("_NoiseScale", _noiseScale);
    }
    /// <summary>
    /// Renders on image 
    /// </summary>
    /// <param name="src"> source of the camera </param>
    /// <param name="dst">distance of the material</param>
    private void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        Graphics.Blit(src, dst, material);
    }
}
