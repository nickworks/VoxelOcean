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
    public GameObject prefabCreatureJellyfish;
    public GameObject prefabCoralCrystal;
    public GameObject prefabCoralBauble;
    public GameObject prefabCoralFlower; 
    public GameObject prefabCoralPurpleFan;
    public GameObject prefabCoralFingers;
    public GameObject prefabFishGobies;
    public GameObject prefabObjectChest;
    public GameObject prefabCreatureMinnow;
    public GameObject prefabObjectCrowsNest;
    /// <summary>
    /// Prfab reference for PlantKelp. (Kyle Lowery)
    /// </summary>
    public GameObject prefabPlantKelp;
    /// <summary>
    /// Prefab reference for CreatureSeaStar (Kyle Lowery)
    /// </summary>
    public GameObject prefabCreatureSeaStar;
    /// <summary>
    /// Prefab reference for NonlivingTrident (Kyle Lowery)
    /// </summary>
    public GameObject prefabObjectTrident;

    public GameObject prefabCoralPrecious;
    /// <summary>
    /// Prefab reference for CoralPyramid
    /// </summary>
	/// <summary>
	/// Prefab reference for Luminent Plankton (Jess P)
	/// </summary>
	public GameObject prefabLuminentPlankton;
	/// <summary>
	/// Prefab reference for Nonliving Column (Jess P)
	/// </summary>
	public GameObject prefabColumn;
    public GameObject prefabCoralPyramid;
    /// <summary>
    /// Prefab reference for Sea Dragon
    /// </summary>
    public GameObject prefabSeaDragon;
    /// <summary>
    /// Prefab reference for Hydrothermic Tube Worms (Chris's "coral").
    /// </summary>
    public GameObject prefabCoralTubeWorm;
    /// <summary>
    /// Prefab reference for lost anchors. (Chris's "non living").
    /// </summary>
    public GameObject prefabOtherAnchor;
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
    /// Prefab Reference for MossBall, Lion Fish, and Sea Urchin (Kaylee K)
    /// </summary>
    public GameObject prefabMossBall;
    public GameObject prefabSeaUrchin;
    public GameObject prefabLionFish;

    /// <summary>
    /// Prefab Reference for Coral Supreme (Zack G)
    /// </summary>
    public GameObject prefabCoralSupreme;
    /// <summary>
    /// Prefab Reference for CoralWillow (Zack G)
    /// </summary>
    public GameObject prefabCoralWillow;

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

        int seed = (int)(transform.position.x + transform.position.y + transform.position.z) * 100;
        Random.InitState(seed);

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
            prefab = (Random.Range(0f, 5f) > 1f) ? prefabCoralTubeWorm : (Random.Range(0f, 2f) > 1) ? prefabPlantDrifter : prefabOtherAnchor;
            if (Random.Range(0f, 5f) < 1f) SpawnPrefab(prefabCreatureBlindShrimp, pos, rot, 1);
        }
        if (biome.owner == BiomeOwner.Dominic) prefab = prefabCoralVoronoi;
        if (biome.owner == BiomeOwner.Eric)
        {
            prefab = (Random.Range(0f, 5f) >= 3) ? prefabCoralTree : prefabCreatureJellyfish;
            if (prefab = prefabCreatureJellyfish) SpawnPrefab(prefabCreatureJellyfish, pos/8, rot, (Random.Range(0,2)));
        }
        if (biome.owner == BiomeOwner.Josh){
            //chance of spawning pyramid, or plant
         //   prefab = (Random.Range(1, 5) > 3) ? prefabCoralPyramid : prefabPlantLeaf;
            int rand = Random.Range(1, 10);
            if (rand < 2)
            {
                prefab = prefabCoralPyramid;
            }
            else if(rand > 8)
            {
                prefab = prefabPlantLeaf;
            }
            else
            {
                prefab = prefabSeaDragon;
            }         
        }
		if (biome.owner == BiomeOwner.Jess){
			float ran = Random.Range(1, 9);
			if(ran < 3 && ran !=  1){
				prefab =prefabCoralBroccoli;
			} else if (ran < 6 && ran > 3){
				prefab =prefabPlantSeagrass;
			} else if (ran == 1){
				prefab = prefabColumn;
			}else {
				prefab = prefabLuminentPlankton;
			}
		}

        if (biome.owner == BiomeOwner.Justin) prefab = prefabCoralBauble;
        if (biome.owner == BiomeOwner.Jesse)
        {
            int num = Random.Range(1, 7) + Random.Range(1, 7);
            if (num < 4)
            {
                prefab = prefabObjectCrowsNest;
            }
            else if (num > 3 & num < 6 )
            {
                prefab = prefabFishGobies;
            }
            else if (num > 10)
            {
            prefab = prefabObjectChest;
            }
            else 
            {
                prefab = prefabCoralFingers;
            }
        }

        if (biome.owner == BiomeOwner.Justin) {
            prefab = (Random.Range(1, 5) >= 3) ? prefabCoralBauble: prefabCoralFlower;

            if (Random.Range(0f, 5f) < 1f) SpawnPrefab(prefabCreatureMinnow, pos, rot, 1);
        }
        if (biome.owner == BiomeOwner.Kaylee) prefab = (Random.Range(0f, 5f) >= 3) ? prefabCoralPurpleFan : (Random.Range(0f, 10f) > 5f) ? prefabMossBall : prefabSeaUrchin;
        if (biome.owner == BiomeOwner.Kyle)
        {
            int num = Random.Range(1, 10) + Random.Range(1, 10);
            if (num > 12)
            {
                prefab = prefabCreatureSeaStar;
            }
            else if (num > 3)
            {
                prefab = prefabPlantKelp;
            } else
            {
                prefab = prefabObjectTrident;
            }
        }
        
        if (biome.owner == BiomeOwner.Zach) prefab = (Random.Range(1, 5) > 3) ? prefabCoralSupreme : prefabCoralWillow;
        if (biome.owner == BiomeOwner.Keegan) prefab = prefabCoralPrecious;

        float scale = Random.Range(.1f, .75f) + Random.Range(.1f, .75f);

        if (prefab != null)
        {
            //spawn prefab
            GameObject obj = SpawnPrefab(prefab, pos, rot, 1);

            // instantiate Coroutine for adding mesh collider
            IEnumerator cr = AddMeshCollider(obj);
            StartCoroutine(cr);
        }
        return true;
    }
    /// <summary>
    /// This coroutine will add a mesh collider to the passed in gameobject when it resolves
    /// </summary>
    /// <param name="obj">the gameobject to be affected</param>
    /// <returns></returns>
    IEnumerator AddMeshCollider(GameObject obj)
    {
        yield return new WaitForSeconds(1f);
        MeshCollider test = null;

        test = obj.GetComponent<MeshCollider>();

        //if obj doesn't have a meshcollider
        if (test == null)
        {
            obj.AddComponent<BoxCollider>();

          //  MeshCollider test2 = obj.GetComponent<MeshCollider>();
          //  test2.convex = true;
        }
    }
    /// <summary>
    /// start the AddMeshCollider coroutine
    /// </summary>
    /// <param name="obj">the gameobject to be affected</param>
    /// <returns>starts the AddMeshCollider courtine</returns>
    IEnumerator StartCoroutine(GameObject obj)
    {
        yield return StartCoroutine("AddMeshCollider");
    }

    /// <summary>
    /// Spawns a specific prefab
    /// </summary>
    /// <param name="prefab">The prefab to spawn</param>
    /// <param name="position">The world position to spawn the prefab</param>
    /// <param name="rotation">The world rotation to use when spawning the prefab</param>
    /// <param name="scale">The local scale to spawn the prefab</param>
    GameObject SpawnPrefab(GameObject prefab, Vector3 position, Quaternion? rotation = null, float scale = 1)
    {
        Quaternion rot = (rotation != null) ? (Quaternion)rotation : Quaternion.identity;
        GameObject obj = Instantiate(prefab, position, rot, transform);
        obj.transform.localScale = Vector3.one * scale;
        

        // TODO: register the coral in a list of coral?
        return obj;
    }
}