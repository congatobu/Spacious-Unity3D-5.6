using UnityEngine;
using System.Collections;

public class MainAudioCtrl : MonoBehaviour
{
	private AudioSource myAudio;

	public AudioClip guiSound;
	public AudioClip deathTune;

	void Awake()
	{
		myAudio = gameObject.GetComponent<AudioSource>();
	}

	void Start()
	{

	}

	void FixedUpdate()
	{

	}

	void GUISound()
	{
		PlaySound(guiSound, 1);
	}

	void DeathSound()
	{
		PlaySound(deathTune, 0.7f);
	}

	void PlaySound(AudioClip sfx, float vol)
	{
		myAudio.volume = vol;
		myAudio.clip = sfx;
		myAudio.pitch = Random.Range(0.9f, 1.2f);
		myAudio.Play();
	}
}