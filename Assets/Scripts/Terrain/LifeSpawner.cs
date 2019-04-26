using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This behavior is responsible for spawning prefabs on a VoxelChunk. It's currently being used to spawn coral, but could be used to spawn anything, really.
/// </summary>
[RequireComponent(typeof(VoxelChunk))]
[RequireComponent(typeof(MeshFilter))]
public class LifeSpawner : MonoBehaviour
{

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
        /// Creates a Biome associated with a specified BiomeOwner
        /// </summary>
        /// <param name="owner">The BiomeOwner who for this Biome</param>
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
    }
    /// <summary>
    /// The minimum amount of life to spawn.
    /// </summary>
    public int lifeAmountMin = 1;
    /// <summary>
    /// The maximum amount of life to spawn.
    /// </summary>
    public int lifeAmountMax = 5;


    /// <summary>
    /// Socket for Dom's Vornoi Coral 
    /// </summary>
    public GameObject prefabCoralVoronoi;
	/// <summary>
	/// Prefab reference for Broccoli Coral (Jess P)
	/// </summary>
    public GameObject prefabCoralBroccoli;
    public GameObject prefabCoralTree;
    public GameObject prefabCoralCrystal;
    public GameObject prefabCoralBauble;
    public GameObject prefabCoralFlower; 
    public GameObject prefabCoralPurpleFan;
    public GameObject prefabCoralFingers;
    /// <summary>
    /// Prfab reference for PlantKelp. (Kyle Lowery)
    /// </summary>
    public GameObject prefabPlantKelp;
    /// <summary>
    /// Prefab reference for CreatureSeaStar (Kyle Lowery)
    /// </summary>
    public GameObject prefabCreatureSeaStar;
    public GameObject prefabCoralPrecious;
    public GameObject prefabCoralPyramid;
    /// <summary>
    /// Prefab reference for Hydrothermic Tube Worms (Chris's "coral").
    /// </summary>
    public GameObject prefabCoralTubeWorm;
    /// <summary>
    /// Prefab reference for Hydrothermic blind shrimp (Chris's fish).
    /// </summary>
    public GameObject prefabCreatureBlindShrimp;
    /// <summary>
    /// Prefab reference for drifting kelp (Chris's flora).
    /// </summary>
    public GameObject prefabPlantDrifter;
    public GameObject prefabPlantLeaf;
	/// <summary>
	/// Prefab reference for Seagrass (Jess P)
	/// </summary>
	public GameObject prefabPlantSeagrass;
    /// <summary>
    /// PRefab Reference for Crystal Flower (Cameron G)
    /// </summary>
    public GameObject prefabCrystalFlower;
    /// <summary>
    /// Prefab Reference for Crystal Rock (Cameron G)
    /// </summary>
    public GameObject prefabCrystalRock;

    /// <summary>
    /// Prefab Reference for Crystal Rock and Sea Urchin (Kaylee K)
    /// </summary>
    public GameObject prefabMossBall;
    public GameObject prefabSeaUrchin;

    /// <summary>
    /// The MeshFilter that's (hopefully) loaded onto this VoxelChunk 
    /// </summary>
    MeshFilter mesh;

    /// <summary>
    /// Prefab reference for Glowing coral
    /// </summary>
    public GameObject prefabCoralGlow;

    /// <summary>
    /// Attempt to spawn a bunch of life on this VoxelChunk
    /// </summary>
    public void SpawnSomeLife()
    {

        Random.InitState(0);

        mesh = GetComponent<MeshFilter>();
        if (!mesh) return;

        int amt = Random.Range(lifeAmountMin, lifeAmountMax + 1);

        List<Vector3> pts = new List<Vector3>();

        for(int i = 0; i < amt; i++)
        {
            int attempts = 0; // holds all attempts
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
    /// <param name="printDebug">Whether or not to print out when spawning new life</param>
    /// <returns>If successful, return true. Otherwise, return false.</returns>
    bool SpawnLifeAtVertex(int index, bool printDebug = false)
    {

        if (index < 0) return false;
        if (index >= mesh.mesh.vertexCount) return false;

        Vector3 pos = mesh.mesh.vertices[index] + transform.position;
        Color color = mesh.mesh.colors[index];
        Vector3 normal = mesh.mesh.normals[index];
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, normal);

        Biome biome = Biome.FromColor(color);

        if (printDebug) print("Spawning life in " + biome.owner + "'s biome...");

        // TODO: add a kind of "proximity" check to ensure that plants aren't growing too close to each other

        GameObject prefab = null;
        if (biome.owner == BiomeOwner.Andrew) prefab = prefabCoralGlow;
        if (biome.owner == BiomeOwner.Cameron) prefab = (Random.Range(0f, 5f) >= 3) ? prefabCoralCrystal : (Random.Range(0f, 10f) > 9f) ? prefabCrystalFlower : prefabCrystalRock;
        if (biome.owner == BiomeOwner.Chris)
        {
            prefab = (Random.Range(0f, 5f) > 1f) ? prefabCoralTubeWorm : prefabPlantDrifter;

            if (Random.Range(0f, 5f) < 1f) SpawnPrefab(prefabCreatureBlindShrimp, pos, rot, 1);
        }
        if (biome.owner == BiomeOwner.Dominic) prefab = prefabCoralVoronoi;
        if (biome.owner == BiomeOwner.Eric) prefab = prefabCoralTree;
        if (biome.owner == BiomeOwner.Josh){
            //chance of spawning pyramid, or plant
            prefab = (Random.Range(1, 5) > 3) ? prefabCoralPyramid : prefabPlantLeaf;
        }
		if (biome.owner == BiomeOwner.Jess){
			prefab = (Random.Range(1, 5) >= 3) ? prefabCoralBroccoli : prefabPlantSeagrass;
		}
        if (biome.owner == BiomeOwner.Justin) prefab = (Random.Range(1, 5) >= 3) ? prefabCoralBauble : prefabCoralFlower;
        if (biome.owner == BiomeOwner.Jesse) prefab = prefabCoralFingers ;
        //if (biome.owner == BiomeOwner.Josh) prefab = ;
        if (biome.owner == BiomeOwner.Kaylee) prefab = (Random.Range(0f, 5f) >= 3) ? prefabCoralPurpleFan : (Random.Range(0f, 10f) > 5f) ? prefabMossBall : prefabSeaUrchin;
        //if (biome.owner == BiomeOwner.Keegan) prefab = ;
        if (biome.owner == BiomeOwner.Kyle) prefab = (Random.Range(1, 5) > 3) ? prefabCreatureSeaStar : prefabPlantKelp;
        
        //if (biome.owner == BiomeOwner.Zach) prefab = ;
        if (biome.owner == BiomeOwner.Keegan) prefab = prefabCoralPrecious;

        float scale = Random.Range(.1f, .75f) + Random.Range(.1f, .75f);

        if (prefab != null) SpawnPrefab(prefab, pos, rot, scale);

        return true;
    }

    /// <summary>
    /// Spawns a specific prefab
    /// </summary>
    /// <param name="prefab">The prefab to spawn</param>
    /// <param name="position">The world position to spawn the prefab</param>
    /// <param name="rotation">The world rotation to use when spawning the prefab</param>
    /// <param name="scale">The local scale to spawn the prefab</param>
    void SpawnPrefab(GameObject prefab, Vector3 position, Quaternion? rotation = null, float scale = 1)
    {
        Quaternion rot = (rotation != null) ? (Quaternion)rotation : Quaternion.identity;
        GameObject obj = Instantiate(prefab, position, rot, transform);
        obj.transform.localScale = Vector3.one * scale;

        // TODO: register the coral in a list of coral?

    }
}