using UnityEngine;
using System.Collections;

/// <summary>
/// Controller of the spawning values and location of Plankton
/// </summary>
public class CreatureLuminecentPlanktonController : MonoBehaviour
{
	/// <summary>
	/// reference to the Luminecent Plankton Prefab
	/// </summary>
	public GameObject prefabCreatureLuminecentPlankton;

	/// <summary>
	/// How many should spawn
	/// </summary>
	public int spawnCount = 10;

	/// <summary>
	/// the radius in which to spawn in
	/// </summary>
	public float spawnRadius = 4.0f;

	/// <summary>
	/// an exposed variable of velocity
	/// </summary>
	[Range(0.1f, 20.0f)]
	public float velocity = 6.0f;

	/// <summary>
	/// Velocity Variation: changes in velocity
	/// </summary>
	[Range(0.0f, 0.9f)]
	public float velVar = 0.5f;

	/// <summary>
	/// The rotation of the object
	/// </summary>
	[Range(0.1f, 20.0f)]
	public float rot = 4.0f;

	/// <summary>
	/// How close the neigbor is
	/// </summary>
	[Range(0.1f, 10.0f)]
	public float neighborDist = 2.0f;

	/// <summary>
	/// variable to help us look up where the fish are later
	/// </summary>
	public LayerMask searchLayer;

	void Start()
	{
		for (var i = 0; i < spawnCount; i++) Spawn();
	}

	/// <summary>
	/// Spawn at the given position, inside a unit sphere
	/// </summary>
	public GameObject Spawn()
	{
		return Spawn(transform.position + Random.insideUnitSphere * spawnRadius);
	}

	/// <summary>
	/// Finds the rotation, planktons rotation, and the component
	/// </summary>
	public GameObject Spawn(Vector3 position)
	{
		var rotation = Quaternion.Slerp(transform.rotation, Random.rotation, 0.3f);
		var plankton = Instantiate(prefabCreatureLuminecentPlankton, position, rotation) as GameObject;
		plankton.GetComponent<CreatureLuminecentPlankton>().controller = this;
		return plankton;
	}
}