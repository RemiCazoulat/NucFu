using System;
using System.Collections.Generic;
using Magnets;
using UnityEngine;

 public class ElectroMagnet : BasicMagnet
{
    //Private
    private float _length; // m
    private float _thickness; // m
    private readonly uint _wireNumber;
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
        _length = length;
        _thickness = thickness;
        _wireNumber = wireNumber;

        var circularSurface = Const.PI * wireRadius * wireRadius;
        var resistance = resistivity * length / circularSurface;
        _current = tension / resistance;
    }
    public override void CreateMagnet()
    {
        List<Vector3> vertices = new();
        List<int> triangles = new();
        _length -= _length * SquareSpace;
        _thickness -= _thickness * SquareSpace;
        var p0 = Position - _thickness / 2 * Direction - _length / 2 * Right - _length / 2 * Up;
        var p1 = p0 + _thickness * Direction;
        var p3 = p0 + _length * Up;
        var p2 = p1 + _length * Up;
        var p7 = p0 + _length * Right;
        var p6 = p7 + _thickness * Direction;
        var p4 = p7 + _length * Up;
        var p5 = p7 + _length * Up + _thickness * Direction;
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

        Buffers = new Buffers(vertices, triangles);

        MagInfos.Add( new MagnetInfo(Position, Right, _current, _length, _wireNumber));
    }
}

