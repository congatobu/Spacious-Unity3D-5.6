using System.Collections;
using UnityEngine;

public class CharAudioCtrl : MonoBehaviour
{
	public AudioSource charAudio;
	public CharCtrl charCtrl;
	public AudioClip crystalSound;
	public AudioClip fallingSound;
	public AudioClip hitFloorSound;
	public AudioClip hurtSound;
	public AudioClip jetSound;
	public AudioClip jumpSound;
	public AudioClip newSpawnerSound;

	public AudioClip[] walkingSounds;
	public AudioClip walkSound;
	public float walkStepLength = 0.35f;

	private void Awake()
	{
		charCtrl = gameObject.GetComponent<CharCtrl>();
		charAudio = gameObject.GetComponent<AudioSource>();
	}

	private void Start()
	{
		StartCoroutine("Init");
	}

	private IEnumerator Init()
	{
		while (true)
		{
			if (charCtrl.grounded && charCtrl.moveSpeed > 0.8f && !charCtrl.charDead)
			{
				walkSound = walkingSounds[Random.Range(0, walkingSounds.Length)];
				PlaySound(walkSound, 0.8f);
				yield return new WaitForSeconds(walkStepLength/charCtrl.moveSpeed);
			}
			else
				yield return 0;
		}
	}

	private void FixedUpdate()
	{
		if (charCtrl.jetOn)
			PlaySound(jetSound, 0.3f);

		if (!charCtrl.grounded && GetComponent<Rigidbody2D>().velocity.y < -3)
		{
			charAudio.volume = GetComponent<Rigidbody2D>().velocity.y*-0.02f;
			charAudio.pitch = 1.5f + GetComponent<Rigidbody2D>().velocity.y*0.05f;
			charAudio.clip = fallingSound;
			charAudio.Play();
		}
	}

	private void TouchFloor()
	{
		PlaySound(hitFloorSound, 1);
	}

	private void Jump()
	{
		PlaySound(jumpSound, 0.8f);
	}

	private void Hurt()
	{
		PlaySound(hurtSound, 1);
	}

	private void CrystalSound()
	{
		PlaySound(crystalSound, 0.4f);
	}

	private void NewSpawnerSound()
	{
		PlaySound(newSpawnerSound, 0.5f);
	}

	private void PlaySound(AudioClip sfx, float vol)
	{
		charAudio.volume = vol;
		charAudio.clip = sfx;
		charAudio.pitch = Random.Range(0.9f, 1.2f);
		charAudio.Play();
	}
}