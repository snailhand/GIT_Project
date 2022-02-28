using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Collectable : MonoBehaviour
{
	public GameObject display;
	public UnityEvent OnCollect;

	private Collider _collider;


	public virtual void Collect()
	{
		display.SetActive(false);
		_collider.enabled = false;
		OnCollect.Invoke();
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
				Collect();
			}
		}
	}
}
