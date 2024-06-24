using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaSquare
{
    public float particlesNumber;
    public float particleMass;
    public float volume;
    public float density;
    public float meanDistance;
    public float massDensity;

    public PlasmaSquare(float particlesNumber, float particleMass)
    {
        this.particlesNumber = particlesNumber;
        this.particleMass = particleMass;
        this.density = particlesNumber / volume;
        this.volume = particlesNumber * particleMass / density;
        this.meanDistance = Mathf.Sqrt(particleMass / massDensity);
        this.massDensity = particlesNumber * particleMass / volume;
        
    }
}
