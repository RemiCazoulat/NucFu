using System.Collections;
using System.Collections.Generic;
using Magnets;
using UnityEngine;

public abstract class BasicMagnet : Magnet
{
    protected float wireRadius; // m
    protected float resistivity; // Ohm . m
    protected float tension; // V

    public BasicMagnet(Vector3 position, float wireRadius, float resistivity, float tension, float squareSpace) :
        base(position,squareSpace)
    {
        this.wireRadius = wireRadius;
        this.resistivity = resistivity;
        this.tension = tension;
    }
}
