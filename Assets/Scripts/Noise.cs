using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class generates a continuous field of noise.
/// </summary>
public class Noise
{
    /// <summary>
    /// Produces a field of 3d noise.
    /// </summary>
    /// <param name="position">Where to sample the noise field.</param>
    /// <returns>A value between -0.5 and +0.5. Think of this as a kind of "density".</returns>
    static public float Sample(Vector3 position)
    {
        return Sample(position.x, position.y, position.z);
    }
    static public float Sample(float x, float y, float z)
    {
        return Perlin(x + 100, y - 200, z + 300) - 0.5f;
    }
    /// <summary>
    /// Produces a field of 3d noise. This function samples Mathf.PerlinNoise on three different 2d planes and averages the results.
    /// </summary>
    /// <param name="sampleX">noise coord x</param>
    /// <param name="sampleY">noise coord y</param>
    /// <param name="sampleZ">noise coord z</param>
    /// <returns>A value between 0 and 1.</returns>
    static float Perlin(float sampleX, float sampleY, float sampleZ)
    {

        float a = Mathf.PerlinNoise(sampleX, sampleY);
        float b = Mathf.PerlinNoise(sampleZ, -sampleX);
        float c = Mathf.PerlinNoise(sampleY, -sampleZ);
        return (a + b + c) / 3;
    }
}
