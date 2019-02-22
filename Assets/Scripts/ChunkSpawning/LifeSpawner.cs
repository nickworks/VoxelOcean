using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(VoxelChunk))]
[RequireComponent(typeof(MeshFilter))]
public class LifeSpawner : MonoBehaviour
{
    public struct Biome
    {
        public const int COUNT = 13;
        public int num;
        public float GetHue()
        {
            return num / (float)COUNT;
        }
        public Color GetVertexColor()
        {
            return Color.HSVToRGB(GetHue(), 1, 1);
        }
        public Biome(int num)
        {
            this.num = num;
        }
        public static Biome FromColor(Color color)
        {
            Color.RGBToHSV(color, out float h, out float s, out float v);
            return FromHue(h);
        }
        public static Biome FromHue(float hue)
        {
            int num = Mathf.RoundToInt(hue * COUNT);
            return new Biome(num);
        }
    }


    void Start()
    {
        
    }
    void Update()
    {
        
    }
}
