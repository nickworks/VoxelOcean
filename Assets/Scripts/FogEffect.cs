using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Fog Effect 
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
    public Material mat;
    /// <summary>
    /// fog Color sets the color of the fog. Which is set in editor. There is no default color other than white.
    /// </summary>
    public Color _fogColor;

    /// <summary>
    /// Depth Start start of the fog depth, is the starting value of where the fog starts, the further it is away from the player (calculated in meters), the farther the fog effect is.
    /// </summary>
    public float _depthStart;


    /// <summary>
    /// Depth distance how far the fog is from the camera. The 'wall' of fog distance away from the camera.
    /// </summary>
    public float _depthDistance;

    /// <summary>
    /// Player is a reference to the player object
    /// </summary>
    public GameObject player;


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
       // PlayerYPos();
        mat.SetColor("_FogColor", _fogColor);
        mat.SetFloat("_DepthStart", _depthStart);
        mat.SetFloat("_DepthDistance", _depthDistance);
        
    }


    /// <summary>
    /// Renders on image 
    /// </summary>
    /// <param name="src"> source of the camera </param>
    /// <param name="dst">distance of the material</param>
    private void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        Graphics.Blit(src, dst, mat);
    }

    /// <summary>
    /// PlayerYpos gets a reference to the players position this is PSEUDO code
    /// </summary>
    private void PlayerYPos()
    {

        if (player.transform.localPosition.y < -50)
        {
            _fogColor = new Vector4 (0f, 0f, 0f, 0f);
        }
        else
        {
            _fogColor = new Vector4(1f, 0f, 128f, 255f);
        }
    }
}
