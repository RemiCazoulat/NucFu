using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tore : BasicMagnet
{
    public float innerRadius;
    public float outerRadius;
    private float middleRadius;
    private float solenoidNumber;
    private float solenoidWidth;
    private float spirePrecision;
    private uint spireNumber;

    public List<Solenoid> solenoids;
    
    public Tore(
        Vector3 position, 
        float wireRadius, 
        float resistivity, 
        float tension,
        float squareSpace,
        
        float radius, 
        float solenoidRadius,
        float solenoidNumber,
        float solenoidWidth,
        float spirePrecision,
        uint spireNumber
    ) : base(position, wireRadius, resistivity, tension, squareSpace)
    {
        innerRadius = radius - solenoidRadius;
        outerRadius = radius + solenoidRadius;
        middleRadius = radius;
        this.solenoidNumber = solenoidNumber;
        this.solenoidWidth = solenoidWidth;
        this.spirePrecision = spirePrecision;
        this.spireNumber = spireNumber;
        this.spireNumber = spireNumber;
        
        solenoids = new List<Solenoid>();
    }

    private void CreateSolenoids()
    {
        float solenoidRadius = Math.Abs(outerRadius - innerRadius) / 2;
        float solenoidDelta = (2 * Const.PI) / solenoidNumber;
        for (int i = 0; i < solenoidNumber; i++)
        {
           
            
            float posOnCircle = i * solenoidDelta;
            float cos = middleRadius * (float)Math.Cos(posOnCircle);
            float sin = middleRadius * (float)Math.Sin(posOnCircle);

            Vector3 solenoidPosition = position + (cos * right - sin * direction);
            Vector3 solenoidRight = Vector3.Normalize(solenoidPosition);
            Vector3 solenoidDirection = Vector3.Cross(up, solenoidRight);
            
            var solenoid = new Solenoid(
                solenoidPosition, 
                wireRadius, 
                resistivity, 
                tension, 
                squareSpace, 
                solenoidRadius , 
                solenoidWidth, 
                spirePrecision,
                spireNumber);
            
            solenoid.direction = solenoidDirection;
            solenoid.up = up;
            solenoid.right = solenoidRight;
            solenoids.Add(solenoid);
        }
    }
    public override Vector3 CalculateMagneticField(Vector3 p)
    {
        Vector3 magneticField = new Vector3();
        foreach(Solenoid solenoid in solenoids)
        {
            magneticField += solenoid.CalculateMagneticField(p);
        }
        return magneticField;
    }

    public override void CreateMagnet()
    {
        CreateSolenoids();
        foreach(var s in solenoids)
        {
            s.CreateMagnet();
            mi.AddRange(s.mi);
            buffers.AddBuffers(s.buffers);
        }

    }
}
