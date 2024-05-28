using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePlayable : MonoBehaviour
{
    public ParticleSystem[] particles;
    private readonly Dictionary<ParticleSystem, float> playableDict = new Dictionary<ParticleSystem, float>();

    private float delay;

    private void Awake()
    {
        particles = transform.GetComponentsInChildren<ParticleSystem>();
        for (int i = 0; i < particles.Length; i++)
        {
            var ps = particles[i];
            var main = ps.main;
            playableDict[ps] = main.simulationSpeed;
            float startDelay = main.startDelay.constant;

            if (delay < startDelay)
            {
                delay = startDelay;
            }
        }
    }

    public float GetDelay()
    {
        return delay;
    }

    public void SetSpeed(float speed)
    {
        for (int i = 0; i < particles.Length; i++)
        {
            var ps = particles[i];
            var main = ps.main;
            main.simulationSpeed = speed;
        }
    }

    private void ResetSpeed()
    {
        for (int i = 0; i < particles.Length; i++)
        {
            var ps = particles[i];
            if (playableDict.TryGetValue(ps, out var speed))
            {
                var main = ps.main;
                main.simulationSpeed = speed;
            }
        }
    }

    public void OnDisable()
    {
        ResetSpeed();
    }
}
