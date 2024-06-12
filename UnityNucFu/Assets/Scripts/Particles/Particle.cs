using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Particle
{
    public Vector3 position;
    public Vector3 direction;
    public float charge;
    public float mass;

    public Particle(Vector3 p, Vector3 d, float c, float m)
    {
        position = p;
        direction = d;
        charge = c;
        mass = m;
    }

    public static  int GetSize()
    {
        return 12 + 12 + 4 + 4;
    }
}
