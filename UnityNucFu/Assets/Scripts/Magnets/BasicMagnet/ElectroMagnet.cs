using System;
using System.Collections.Generic;

using UnityEngine;

 public class ElectroMagnet : BasicMagnet
{
    //Public
    private float length; // m
    private float thickness; // m

    private uint wireNumber;
    //Private
    private readonly float _current; // A, in formulas, _current is named I

    public ElectroMagnet(
        Vector3 position,
        float wireRadius,
        float resistivity,
        float tension,
        float squareSpace,
        float length,
        float thickness,
        uint wireNumber) : base (position, wireRadius, resistivity, tension, squareSpace)
    {
        this.length = length;
        this.thickness = thickness;
        this.wireNumber = wireNumber;

        var circularSurface = Const.PI * wireRadius * wireRadius;
        var resistance = resistivity * length / circularSurface;
        _current = tension / resistance;
        

    }
    public override Vector3 CalculateMagneticField(Vector3 p)
    {
        // we need here a point p, [the position of the magnet, its current, and its direction]
        float distance = Vector3.Magnitude(p - position);
        float constant = (Const.MAGNETIC_CONST * _current) / (4 * Const.PI * (float)Math.Pow(distance, 3));
        Vector3 crossProduct = Vector3.Cross(
            direction * length, // dl in literature
            p - position
            );
        
        return  constant * crossProduct;
    }

    public override void CreateMagnet()
    {
        List<Vector3> vertices = new();
        List<int> triangles = new();
        length -= length * squareSpace;
        thickness -= thickness * squareSpace;

        Vector3 p0 = position - thickness / 2 * direction - length / 2 * right - length / 2 * up;
        Vector3 p1 = p0 + thickness * direction;
        Vector3 p3 = p0 + length * up;
        Vector3 p2 = p1 + length * up;
        Vector3 p7 = p0 + length * right;
        Vector3 p6 = p7 + thickness * direction;
        Vector3 p4 = p7 + length * up;
        Vector3 p5 = p7 + length * up + thickness * direction;

        vertices.Add(p0);
        vertices.Add(p1);
        vertices.Add(p2);
        vertices.Add(p3);
        vertices.Add(p4);
        vertices.Add(p5);
        vertices.Add(p6);
        vertices.Add(p7);

        triangles.Add(0); triangles.Add(2); triangles.Add(1);
        triangles.Add(0); triangles.Add(3); triangles.Add(2);

        triangles.Add(2); triangles.Add(3); triangles.Add(4); 
        triangles.Add(2); triangles.Add(4); triangles.Add(5);

        triangles.Add(1); triangles.Add(2); triangles.Add(5);
        triangles.Add(1); triangles.Add(5); triangles.Add(6);

        triangles.Add(0); triangles.Add(7); triangles.Add(4);
        triangles.Add(0); triangles.Add(4); triangles.Add(3);

        triangles.Add(5); triangles.Add(4); triangles.Add(7);
        triangles.Add(5); triangles.Add(7); triangles.Add(6);

        triangles.Add(0); triangles.Add(6); triangles.Add(7);
        triangles.Add(0); triangles.Add(1); triangles.Add(6);

        buffers = new Buffers(vertices, triangles);

        mi.Add( new MagnetInfo(position, right, _current, length, wireNumber));
    }
}

