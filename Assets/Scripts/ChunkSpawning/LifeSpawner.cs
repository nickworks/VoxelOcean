using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(VoxelChunk))]
[RequireComponent(typeof(MeshFilter))]
public class LifeSpawner : MonoBehaviour
{
    public enum BiomeOwner
    {
        Andrew,
        Cameron,
        Christopher,
        Dominc,
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
    public struct Biome
    {
        public static int COUNT = System.Enum.GetNames(typeof(BiomeOwner)).Length;
        public BiomeOwner owner;
        public float GetHue()
        {
            return ((int)owner) / ((float)COUNT);
        }
        public Color GetVertexColor()
        {
            return Color.HSVToRGB(GetHue(), 1, 1);
        }
        public Biome(BiomeOwner num)
        {
            this.owner = num;
        }
        public static Biome FromColor(Color color)
        {
            Color.RGBToHSV(color, out float h, out float s, out float v);
            return FromHue(h);
        }
        public static Biome FromHue(float hue)
        {
            int num = Mathf.RoundToInt(hue * COUNT);
            return new Biome((BiomeOwner)num);
        }
        public static Biome FromInt(int i)
        {
            return new Biome((BiomeOwner)i);
        }
    }

    public int lifeAmountMin = 1;
    public int lifeAmountMax = 5;

    MeshFilter mesh;

    public void SpawnSomeLife()
    {
        mesh = GetComponent<MeshFilter>();
        if (!mesh) return;

        int amt = Random.Range(lifeAmountMin, lifeAmountMax + 1);

        List<Vector3> pts = new List<Vector3>();

        for(int i = 0; i < amt; i++)
        {
            int attempts = 0;
            while (attempts < 5) // try 5 times:
            {
                int index = Random.Range(0, mesh.mesh.vertexCount);
                bool success = SpawnLifeAtVertex(index);
                if (success) break;
                attempts++;
            }
        }
    }
    /// <summary>
    /// Tries to spawn life at a given mesh vertex.
    /// </summary>
    /// <param name="index">The index of the vertex to use as a spawn point.</param>
    /// <returns>If successful, return true. Otherwise, return false.</returns>
    bool SpawnLifeAtVertex(int index)
    {
        if (index < 0) return false;
        if (index >= mesh.mesh.vertexCount) return false;

        Vector3 pos = mesh.mesh.vertices[index] + transform.position;
        Color color = mesh.mesh.colors[index];
        Vector3 normal = mesh.mesh.normals[index];

        Biome biome = Biome.FromColor(color);

        print("Spawning life in " + biome.owner + "'s biome...");

        // TODO: add a kind of "proximity" check to ensure that plants aren't growing too close to each other

        switch (biome.owner)
        {
            case BiomeOwner.Andrew:
                break;
            case BiomeOwner.Cameron:
                break;
            case BiomeOwner.Christopher:
                break;
            case BiomeOwner.Dominc:
                break;
            case BiomeOwner.Eric:
                break;
            case BiomeOwner.Jess:
                break;
            case BiomeOwner.Jesse:
                break;
            case BiomeOwner.Josh:
                break;
            case BiomeOwner.Justin:
                break;
            case BiomeOwner.Kaylee:
                break;
            case BiomeOwner.Keegan:
                break;
            case BiomeOwner.Kyle:
                break;
            case BiomeOwner.Zach:
                break;
        }

        return true;
    }
}
