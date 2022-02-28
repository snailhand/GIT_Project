using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
	public GameObject display;
	public AudioClip clip;

	private Collider _collider;

	public virtual void Collect(AmongUs player)
	{
		display.SetActive(false);
		_collider.enabled = false;
	}

	private void Awake()
	{
		_collider = GetComponent<Collider>();
		_collider.isTrigger = true;
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			if (other.TryGetComponent<AmongUs>(out var player))
			{
				Collect(player);
			}
		}
	}
}
}
