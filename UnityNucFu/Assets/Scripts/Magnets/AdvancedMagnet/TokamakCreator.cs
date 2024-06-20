using System.Collections;
using System.Collections.Generic;
using Magnets;
using UnityEngine;

public class TokamakCreator : MagnetCreator
{
    // ***** PUBLIC ATTRIBUTES ***** 
    [Header ("Basic infos :")]
    [Space(5)]
    public Vector3 position; // m
    public Vector3 rotation;
    [Range(0.0f, 1f)] public float squareSpace; // %
    [Space(10)] [Header("Tore :")] [Space(5)]
    public float toreWireRadius;
    public float toreResistivity;
    public float toreTension;
    [Space(10)]
    public float toreRadius;
    public float solenoidRadius;
    public float solenoidNumber;
    public float solenoidWidth;
    public float spirePrecision;
    public float solenoidSpirePrecision;
    public uint spireNumber;
    [Space(10)]
    [Header ("Central solenoid :")]
    public float centralSolenoidWireRadius;
    public float centralSolenoidResistivity;
    public float centralSolenoidTension;
    [Space(10)]
    public float centralSolenoidRadius;
    public float centralSolenoidSpirePrecision;
    public float centralSolenoidSolenoidSpirePrecision;

    public uint centralSolenoidSpireNumber;

    public override void CreateMagnet()
    {
        Magnet = new Tokamak(
            position,
            squareSpace,
            toreWireRadius,
            toreResistivity,
            toreTension,
            centralSolenoidWireRadius,
            centralSolenoidResistivity,
            centralSolenoidTension,
            toreRadius,
            solenoidRadius,
            solenoidNumber,
            solenoidWidth,
            spirePrecision,
            solenoidSpirePrecision,
            spireNumber,
            centralSolenoidRadius,
            centralSolenoidSpirePrecision,
            centralSolenoidSolenoidSpirePrecision,
            centralSolenoidSpireNumber
        );
        Magnet.ModifyRotation(rotation);
        Mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = Mesh;
        transform.position = Magnet.Position;
        Magnet.CreateMagnet();
        AssignMesh();
    }
}
