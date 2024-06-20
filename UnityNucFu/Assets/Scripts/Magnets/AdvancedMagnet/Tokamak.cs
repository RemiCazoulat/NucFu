using System;
using System.Collections;
using System.Collections.Generic;
using Magnets;
using UnityEngine;

public class Tokamak : Magnet
{
    private Tore _tore;
    private Solenoid _centralSolenoid;
    
    public Tokamak(
        Vector3 position,
        float squareSpace,

        float toreWireRadius, 
        float toreResistivity, 
        float toreTension,
        
        float centralSolenoidWireRadius, 
        float centralSolenoidResistivity, 
        float centralSolenoidTension,
        
        
        float toreRadius, 
        float solenoidRadius,
        float solenoidNumber,
        float solenoidWidth,
        float spirePrecision,
        float solenoidSpirePrecision,
        uint spireNumber,
        
        float centralSolenoidRadius,
        float centralSolenoidSpirePrecision,
        float centralSolenoidSolenoidSpirePrecision,

        uint centralSolenoidSpireNumber
        )
        : base(
            position,
            squareSpace)
    {
        _tore = new Tore(
            position, 
            toreWireRadius, 
            toreResistivity, 
            toreTension,
            squareSpace, 
            toreRadius,
            solenoidRadius,
            solenoidNumber, 
            solenoidWidth, 
            spirePrecision, 
            solenoidSpirePrecision,
            spireNumber);
        
        
        float csRadius = centralSolenoidRadius > _tore.innerRadius ? _tore.innerRadius : centralSolenoidRadius;
        
        _centralSolenoid = new Solenoid(
            position,
            centralSolenoidWireRadius, 
            centralSolenoidResistivity, 
            centralSolenoidTension,
            squareSpace,
            csRadius,
            Math.Abs(_tore.outerRadius - _tore.innerRadius) * 1.5f, 
            centralSolenoidSpirePrecision,
            centralSolenoidSolenoidSpirePrecision,
            
            centralSolenoidSpireNumber
            );
        _centralSolenoid.SetDirectionVectors(Up, Vector3.Cross(Up, Right), Right);
    }

    public new void ModifyRotation(Vector3 rotation)
    {
        base.ModifyRotation(rotation);
        _tore.SetDirectionVectors(Direction, Up, Right);
        _centralSolenoid.SetDirectionVectors(Up, Vector3.Cross(Up, Right), Right);
        
    }
    public new void SetDirectionVectors(Vector3 d, Vector3 u, Vector3 r)
    {
        base.SetDirectionVectors(d, u, r);
        _tore.SetDirectionVectors(d, u, r);
        _centralSolenoid.SetDirectionVectors(Up, Vector3.Cross(Up, Right), Right);
    }
    public override void CreateMagnet()
    {
        _tore.CreateMagnet();
        _centralSolenoid.CreateMagnet();
        MagInfos.AddRange(_tore.MagInfos);
        MagInfos.AddRange(_centralSolenoid.MagInfos);
        Buffers.AddBuffers(_tore.Buffers);
        Buffers.AddBuffers(_centralSolenoid.Buffers);
    }
}
