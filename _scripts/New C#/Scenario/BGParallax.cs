using UnityEngine;
using System.Collections;

public class BGParallax : MonoBehaviour
{
	public Transform[] backgrounds;                        // Array of all the backgrounds to be parallaxed.
	public float parallaxScale;                            // The proportion of the camera's movement to move the backgrounds by.
	public float parallaxReductionFactor;            // How much less each successive layer should parallax.
	public float smoothing;                                // How smooth the parallax effect should be.
	public Transform cam;                                  // Shorter reference to the main camera's transform.
	public Vector3 previousCamPos;                   // The postion of the camera in the previous frame.

	void Awake()
	{
		// Setting up the reference shortcut.
		cam = Camera.main.transform;
	}

	void Start()
	{
		// The 'previous frame' had the current frame's camera position.
		previousCamPos = cam.position;
	}

	void Update()
	{

		// The parallax is the opposite of the camera movement since the previous frame multiplied by the scale.
		float parallaxX = (cam.position.x - previousCamPos.x) * parallaxScale;
		float parallaxY = (cam.position.y - previousCamPos.y) * parallaxScale;

		// For each successive background...
		for (int i = 0; i < backgrounds.Length; i++)
		{
			// ... set a target x position which is their current position plus the parallax multiplied by the reduction.
			float backgroundTargetPosX = backgrounds[i].position.x + parallaxX * (i * parallaxReductionFactor + 1);
			float backgroundTargetPosY = backgrounds[i].position.y + parallaxY * (i * parallaxReductionFactor + 1);

			// Create a target position which is the background's current position but with it's target x position.
			Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgroundTargetPosY, backgrounds[i].position.z);

			// Lerp the background's position between itself and it's target position.
			backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
		}

		// Set the previousCamPos to the camera's position at the end of this frame.
		previousCamPos = cam.position;
	}
}