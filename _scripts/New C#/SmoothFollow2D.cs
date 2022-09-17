using UnityEngine;
using System.Collections;

public class SmoothFollow2D : MonoBehaviour
{
	private bool followPlayer;
	public float smoothTime = 0.3f;
	public Transform target;
	private Transform thisTransform;
	private float velocity;

	Vector3 targetPos;
	public float interpVelocity;
	public Vector3 offset;

	private void Start()
	{
		thisTransform = transform;
	}

	private void Update()
	{
		if (!followPlayer)
			return;

		Vector3 posNoZ = transform.position;
		posNoZ.z = target.transform.position.z;

		Vector3 targetDirection = (target.transform.position - posNoZ);

		interpVelocity = targetDirection.magnitude * 5f;

		targetPos = transform.position + (targetDirection.normalized * interpVelocity * Time.deltaTime);

		transform.position = Vector3.Lerp(transform.position, targetPos + offset, smoothTime);

	}

	private void FollowPlayer(bool state)
	{
		followPlayer = state;
	}
}