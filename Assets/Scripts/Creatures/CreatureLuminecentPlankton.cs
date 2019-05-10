using UnityEngine;
using System.Collections;
/// <summary>
/// Main class for the movment of the Plankton
/// </summary>
public class CreatureLuminecentPlankton : MonoBehaviour
{
	/// <summary>
	/// Reference to the controller
	/// </summary>
	public CreatureLuminecentPlanktonController controller;


	/// <summary>
	/// random seed for calculations
	/// </summary>
	float noiseOffset;

	/// <summary>
	///  Caluculates the separation vector with a target.
	/// </summary>
	Vector3 GetSeparationVector(Transform target)
	{
		var diff = transform.position - target.transform.position;
		var diffLen = diff.magnitude;
		var scaler = Mathf.Clamp01(1.0f - diffLen / controller.neighborDist);
		return diff * (scaler / diffLen);
	}

	void Start()
	{
		noiseOffset = Random.value * 10.0f;

	}

	void Update()
	{
		var currentPosition = transform.position;
		var currentRotation = transform.rotation;

		var noise = Mathf.PerlinNoise(Time.time, noiseOffset) * 2.0f - 1.0f;
		var velocity = controller.velocity * (1.0f + noise * controller.velVar);


		var separation = Vector3.zero;
		var alignment = controller.transform.forward;
		var cohesion = controller.transform.position;

		var nearbyBoids = Physics.OverlapSphere(currentPosition, controller.neighborDist, controller.searchLayer);

		foreach (var boid in nearbyBoids)
		{
			if (boid.gameObject == gameObject) continue;
			var t = boid.transform;
			separation += GetSeparationVector(t);
			alignment += t.forward;
			cohesion += t.position;
		}

		var avg = 1.0f / nearbyBoids.Length;
		alignment *= avg;
		cohesion *= avg;
		cohesion = (cohesion - currentPosition).normalized;


		var direction = separation + alignment + cohesion;
		var rotation = Quaternion.FromToRotation(Vector3.forward, direction.normalized);


		if (rotation != currentRotation)
		{
			var ip = Mathf.Exp(-controller.rot * Time.deltaTime);
			transform.rotation = Quaternion.Slerp(rotation, currentRotation, ip);
		}


		transform.position = currentPosition + transform.forward * (velocity * Time.deltaTime);
	}
}