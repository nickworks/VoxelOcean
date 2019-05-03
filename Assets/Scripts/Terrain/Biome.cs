using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The possible Biomes our game supports
/// </summary>
public enum BiomeOwner
{
    Andrew,
    Cameron,
    Chris,
    Dominic,
    Eric,
    Jess,
    Jesse,
    Josh,
    Justin,
    Kaylee,
    Keegan,
    Kyle,
    Zach
}
/// <summary>
/// This struct contains one value: BiomeOwner. It also contains several convenience methods for converting from color to biome and vice versa.
/// </summary>
public struct Biome
{
    /// <summary>
    /// How many biomes the game currently supports.
    /// </summary>
    public static int COUNT = System.Enum.GetNames(typeof(BiomeOwner)).Length;
    /// <summary>
    /// Whose biome this is
    /// </summary>
    public BiomeOwner owner;
    /// <summary>
    /// Gets a hue value to use for vertex color
    /// </summary>
    /// <returns>A value from 0.0 to 1.0</returns>
    public float GetHue()
    {
        return ((int)owner) / ((float)COUNT);
    }
    /// <summary>
    /// Gets a Color value to use for vertex color
    /// </summary>
    /// <returns></returns>
    public Color GetVertexColor()
    {
        return Color.HSVToRGB(GetHue(), 1, 1);
    }
    /// <summary>
    /// Creates a Biome associated with a specified Biome.Owner
    /// </summary>
    /// <param name="owner">The Biome.Owner who for this Biome</param>
    public Biome(BiomeOwner owner)
    {
        this.owner = owner;
    }
    /// <summary>
    /// Creates a Biome from a specified Color
    /// </summary>
    /// <param name="color">The color to use. In our case, this will be a vertex color stored in a mesh.</param>
    /// <returns>A Biome</returns>
    public static Biome FromColor(Color color)
    {
        Color.RGBToHSV(color, out float h, out float s, out float v);
        return FromHue(h);
    }
    /// <summary>
    /// Creates a Biome from a specified hue value
    /// </summary>
    /// <param name="hue">The hue value to use. Should be 0.0 to 1.0</param>
    /// <returns>A Biome</returns>
    public static Biome FromHue(float hue)
    {
        int num = Mathf.RoundToInt(hue * COUNT);
        return new Biome((BiomeOwner)num);
    }
    /// <summary>
    /// Creates Biome from a specified integer.
    /// </summary>
    /// <param name="i">This integer should correspond to an index value in BiomeOwner</param>
    /// <returns>A Biome</returns>
    public static Biome FromInt(int i)
    {
        return new Biome((BiomeOwner)i);
    }

    static float map(float v, float min1, float max1, float min2, float max2)
    {
        float p = (v - min1) / (max1 - min1);
        return (p * (max2 - min2)) + min2;
    }

    /// <summary>
    /// Generates a Biome at a given position
    /// </summary>
    /// <param name="pos">The position to sample the biome field, in world space.</param>
    /// <returns>A Biome object with data about which biome inhabits this space.</returns>
    public static Biome AtLocation(Vector3 pos)
    {

        pos /= VoxelUniverse.main.biomeScaling;

        // SAMPLE 1 noisefield at 3 arbitrary locations:
        Vector3 offsetR = new Vector3(123, 456, 789);
        Vector3 offsetG = new Vector3(-99, 999, 300);
        Vector3 offsetB = new Vector3(900, 500, -99);
        float r = Noise.Sample(pos + offsetR);
        float g = Noise.Sample(pos + offsetG);
        float b = Noise.Sample(pos + offsetB);

        // TODO: redo/improve the calculations below

        r = map(r, -.2f, .2f, 0, 1);
        g = map(g, -.2f, .2f, 0, 1);
        b = map(b, -.2f, .2f, 0, 1);

        // redistribute values non-linearly:
        float pow = 1.5f;
        r = Mathf.Pow(r, pow);
        g = Mathf.Pow(g, pow);
        b = Mathf.Pow(b, pow);

        // Convert from RGB to HSV:
        Color.RGBToHSV(new Color(r, g, b), out float h, out float s, out float v);

        //posterize the hue value:
        h = Mathf.Round(h * Biome.COUNT);
        int biome_num = (int)h;

        // create and return the biome:
        return Biome.FromInt(biome_num);
    }

}