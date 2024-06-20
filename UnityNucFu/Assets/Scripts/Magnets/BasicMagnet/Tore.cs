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
    private float solenoidSpirePrecision;

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
        float solenoidSpirePrecision,
        uint spireNumber
    ) : base(position, wireRadius, resistivity, tension, squareSpace)
    {
        innerRadius = radius - solenoidRadius;
        outerRadius = radius + solenoidRadius;
        middleRadius = radius;
        this.solenoidNumber = solenoidNumber;
        this.solenoidWidth = solenoidWidth;
        this.spirePrecision = spirePrecision;
        this.solenoidSpirePrecision = solenoidSpirePrecision;
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

            Vector3 solenoidPosition = Position + (cos * Right - sin * Direction);
            Vector3 solenoidRight = Vector3.Normalize(solenoidPosition);
            Vector3 solenoidDirection = Vector3.Cross(Up, solenoidRight);
            
            var solenoid = new Solenoid(
                solenoidPosition, 
                wireRadius, 
                resistivity, 
                tension, 
                SquareSpace, 
                solenoidRadius , 
                solenoidWidth, 
                spirePrecision,
                solenoidSpirePrecision,
                spireNumber);
            
            solenoid.Direction = solenoidDirection;
            solenoid.Up = Up;
            solenoid.Right = solenoidRight;
            solenoids.Add(solenoid);
        }
    }
    public override void CreateMagnet()
    {
        CreateSolenoids();
        foreach(var s in solenoids)
        {
            s.CreateMagnet();
            MagInfos.AddRange(s.MagInfos);
            Buffers.AddBuffers(s.Buffers);
        }

    }
}
