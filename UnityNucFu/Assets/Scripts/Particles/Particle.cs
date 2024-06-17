using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Particle
{
    public Vector3 pos;
    public Vector3 vel;
    public float charge;
    public float mass;

    public Particle(Vector3 p, Vector3 v, float c, float m)
    {
        pos = p;
        vel = v;
        charge = c;
        mass = m;
    }

    

    public static  int GetSize()
    {
        return 12 + 12 + 4 + 4;
    }
}
