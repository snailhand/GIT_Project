using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmongUsParticles : MonoBehaviour
{
    public ParticleSystem footstepParticle;
    public ParticleSystem landingParticle;
    public ParticleSystem hitParticle;

    private AmongUs player;

	void Start()
	{
		player = GetComponent<AmongUs>();
		player.OnGroundEnter.AddListener(LandParticle);
		player.OnHurt.AddListener(HurtParticle);
	}

	void Update()
	{
		WalkParticle();
	}

	//Start playing a particle
	public virtual void PlayParticle(ParticleSystem particle)
    {
        if(!particle.isPlaying)
        {
            particle.Play();
        }
    }

    //Stop particle that is playing
    public virtual void StopParticle(ParticleSystem particle)
    {
        if(particle.isPlaying)
        {
            particle.Stop();
        }
    }

	protected virtual void WalkParticle()
	{
		if (player.isGrounded)
		{
			if (player.horizontalVelocity.sqrMagnitude > 0)
			{
				PlayParticle(footstepParticle);
			}
			else
			{
				StopParticle(footstepParticle);
			}
		}
		else
		{
			StopParticle(footstepParticle);
		}
	}

	protected virtual void LandParticle()
	{
		PlayParticle(landingParticle);
	}

	protected virtual void HurtParticle()
	{
		PlayParticle(hitParticle);
	}
}
