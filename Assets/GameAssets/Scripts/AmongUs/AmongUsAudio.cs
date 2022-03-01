using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmongUsAudio : MonoBehaviour
{
	public AudioClip spawn;
	public AudioClip respawn;
	public AudioClip jump;
	public AudioClip[] hurt;

	protected AmongUs player;
	protected AudioSource audioSource;

	protected virtual void Start()
	{
		player = GetComponent<AmongUs>();

		InitializeAudio();
		InitializeCallbacks();
	}

	protected virtual void InitializeAudio()
	{
		if (!TryGetComponent(out audioSource))
		{
			audioSource = gameObject.AddComponent<AudioSource>();
		}
		else
        {
			audioSource = GetComponent<AudioSource>();
        }
	}

	protected virtual void InitializeCallbacks()
	{
		player.OnJump.AddListener(() => audioSource.PlayOneShot(jump));
		player.OnHurt.AddListener(() => audioSource.PlayOneShot(hurt[Random.Range(0, hurt.Length)]));
		player.OnSpawn.AddListener(() => audioSource.PlayOneShot(spawn));
		player.OnRespawn.AddListener(() => audioSource.PlayOneShot(respawn));
	}
}
