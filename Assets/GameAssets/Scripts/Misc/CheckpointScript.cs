using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointScript : MonoBehaviour
{
    private bool activated;

    private GameMasterScript gm;
    private AudioSource _audio;
    public AudioClip activate;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMasterScript>();
        _audio = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(other.TryGetComponent<AmongUs>(out var player))
            {
                if(!activated)
                {
                    _audio.PlayOneShot(activate);
                    activated = true;
                }

                gm.lastCheckPointPos = transform.position;
                player.SetRespawn(this.transform.position, this.transform.rotation);
            }
        }
    }
}
