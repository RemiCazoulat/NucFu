using System;
using System.Collections;
using System.Collections.Generic;
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
        uint spireNumber,
        
        float centralSolenoidRadius,
        float centralSolenoidSpirePrecision,
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
            spireNumber);
        
        
        float csRadius = centralSolenoidRadius > _tore.innerRadius ? _tore.innerRadius : centralSolenoidRadius;
        
        _centralSolenoid = new Solenoid(
            position,
            centralSolenoidWireRadius, 
            centralSolenoidResistivity, 
            centralSolenoidTension,
            squareSpace,
            csRadius,
            Math.Abs(_tore.outerRadius - _tore.innerRadius) * 2, 
            centralSolenoidSpirePrecision,
            centralSolenoidSpireNumber
            );
        _centralSolenoid.SetDirectionVectors(up, Vector3.Cross(up, right), right);
    }

    public new void ModifyRotation(Vector3 rotation)
    {
        base.ModifyRotation(rotation);
        _tore.SetDirectionVectors(direction, up, right);
        _centralSolenoid.SetDirectionVectors(up, Vector3.Cross(up, right), right);
        
    }
    public new void SetDirectionVectors(Vector3 d, Vector3 u, Vector3 r)
    {
        base.SetDirectionVectors(d, u, r);
        _tore.SetDirectionVectors(d, u, r);
        _centralSolenoid.SetDirectionVectors(up, Vector3.Cross(up, right), right);
    }
    
    public override Vector3 CalculateMagneticField(Vector3 p)
    {
        return _tore.CalculateMagneticField(p) + _centralSolenoid.CalculateMagneticField(p);
    }

    public override void CreateMagnet()
    {
        _tore.CreateMagnet();
        _centralSolenoid.CreateMagnet();
        mi.AddRange(_tore.mi);
        mi.AddRange(_centralSolenoid.mi);
        buffers.AddBuffers(_tore.buffers);
        buffers.AddBuffers(_centralSolenoid.buffers);
    }
}
