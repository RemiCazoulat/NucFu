using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class Solenoid : BasicMagnet
{
                                
    
    //Geometry
    private readonly float radius; // m
    private readonly float thickness; // m
    private readonly float magnetPrecision; // magnet / m
    private readonly uint spireNumber;


    private readonly List<ElectroMagnet> magnets; // in formulas, it is called dl.
    
    private readonly float _circumference; // m


    public Solenoid(
        Vector3 position, 
        float wireRadius,
        float resistivity, 
        float tension,
        float squareSpace,
        float radius, 
        float thickness,
        float magnetPrecision,
        uint spireNumber
        ) :
        base(position, wireRadius, resistivity, tension, squareSpace)
    {
        this.radius = radius;
        this.thickness = thickness;
        this.magnetPrecision = magnetPrecision;
        this.spireNumber = spireNumber;
        _circumference = Const.PI * radius * 2;
       
        magnets = new();

    }

    private void CreateElectroMagnets()
    {
        float length = 1f / magnetPrecision;
        int magnetNumber = (int)(magnetPrecision * _circumference);


        float magnetDelta =  (2 * Const.PI) / magnetNumber;
        for (int i = 0; i < magnetNumber; i++)
        {
            float posOnCircle = i * magnetDelta;
            float cos = radius * (float)Math.Cos(posOnCircle);
            float sin = radius * (float)Math.Sin(posOnCircle);

             
            Vector3 magnetPosition = (cos * Right + sin * Up);
            Vector3 relativeMagnetPosition = Position + magnetPosition;
            Vector3 magnetDir = Direction;           
            Vector3 magnetUp = Vector3.Normalize(magnetPosition);
            Vector3 magnetRight = Vector3.Normalize(Vector3.Cross(magnetDir, magnetUp));
            var magnet = new ElectroMagnet(
                relativeMagnetPosition, 
                wireRadius,
                resistivity,
                tension,
                SquareSpace,
                length,
                thickness,
                spireNumber
            );
            magnet.SetDirectionVectors(magnetDir, magnetUp, magnetRight);
            magnets.Add(magnet);
        }
    }
    public override void CreateMagnet()
    {
        if (magnets.Count == 0)
        {
            CreateElectroMagnets();
        }

        foreach(var em in magnets)
        {
            em.CreateMagnet();
            MagInfos.AddRange(em.MagInfos);
            Buffers.AddBuffers(em.Buffers);
        }

    }
}

