using System.Collections;
using System.Collections.Generic;
using Magnets;
using UnityEngine;
using UnityEngine.Serialization;

public enum MagnetType {
    Solenoid,
    ElectroMagnet,
    Tore
}
public class BasicMagnetCreator : MagnetCreator
{
    public MagnetType magnetType = MagnetType.Solenoid; 
    // ***** PUBLIC ATTRIBUTES ***** 
    [Header ("Basic infos :")]
    [Space(5)]
    public Vector3 position; // m
    public Vector3 rotation;
    [Min(0f)]public float wireRadius; // m
    [Min(0f)]public float resistivity; // Ohm . m
    [Min(0f)]public float tension; // V
    [Range(0.1f, 0.9f)] public float squareSpace; // %
    public uint wireNumber;
    [Space(10)]
    [Header ("Only ElectroMagnet :")]
    [Space(5)]
    [Min(0f)]public float length;
    [Space(10)]
    [Header ("Only ElectroMagnet & Spire :")]
    [Space(5)]
    [Min(0f)]public float thickness;
    [Space(10)]
    [Header ("Only Spire & Solenoid & Tore:")]
    [Space(5)]
    [Min(0f)]public float spireRadius; // m
    [Min(0f)]public float spirePrecision; // magnet / m   
    [Space(10)]
    [Header ("Only Solenoid & Tore :")]
    [Space(5)]
    [Min(0f)]public float solenoidWidth; // m
    [Header("Only Tore :")] 
    [Min(0f)] public float toreRadius;
    [Min(0f)] public float solenoidNumber;

   public override void CreateMagnet()
   {
       switch (magnetType) {
           case MagnetType.ElectroMagnet :
               Magnet = new ElectroMagnet(
                   position, 
                   wireRadius,
                   resistivity, 
                   tension, 
                   squareSpace,
                   length, 
                   thickness,
                   wireNumber);
               break;
           case MagnetType.Solenoid :
               Magnet = new Solenoid(
                   position,
                   wireRadius,
                   resistivity,
                   tension,
                   squareSpace,
                   spireRadius,
                   solenoidWidth,
                   spirePrecision,
                   wireNumber);
               break;
           case MagnetType.Tore :
               Magnet = new Tore(
                   position, 
                   wireRadius,
                   resistivity, 
                   tension, 
                   squareSpace, 
                   toreRadius,
                   spireRadius,
                   solenoidNumber, 
                   solenoidWidth, 
                   spirePrecision, 
                   wireNumber);
               break;
       }
       Magnet.ModifyRotation(rotation);
       Mesh = new Mesh();
       GetComponent<MeshFilter>().mesh = Mesh;
       var tr = transform;
       tr.position = Magnet.Position;
       Magnet.CreateMagnet();
       AssignMesh();
   }
   
}
