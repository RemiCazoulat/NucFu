using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Particle
{
    public Vector3 pos;
    public Vector3 dir;
    public Vector3 force;
    public float charge;
    public float mass;

    public Particle(Vector3 p, Vector3 d, float c, float m)
    {
        pos = p;
        dir = d;
        force = Vector3.zero;
        charge = c;
        mass = m;
    }

    public void ResetForce()
    {
        force = Vector3.zero;
    }

    public void AddForce(Vector3 newForce)
    {
        force += newForce;
    }

    public void ModifyPos(Vector3 newPos)
    {
        pos = newPos;
    }

    public void ModifyDir(Vector3 newDir)
    {
        dir = newDir;
    }

    public static  int GetSize()
    {
        return 12 + 12 + 12 + 4 + 4;
    }
}
