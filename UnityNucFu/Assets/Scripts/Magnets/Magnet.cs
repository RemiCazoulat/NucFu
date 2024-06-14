using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Utilities;
public abstract class Magnet 
{
    public Vector3 position;
    private float yaw;
    private float pitch;
    private float roll;
    public Vector3 direction; // Radian
    public Vector3 up; // Radian 
    public Vector3 right; // Radian
    public List<MagnetInfo> mi = new();
    protected float squareSpace;
    public Buffers buffers;
    protected Magnet(Vector3 position, float squareSpace) {
        this.position = position;
        this.squareSpace = Math.Max(Math.Min(squareSpace, 0.9f), 0.1f);
        buffers = new Buffers();
        MakeRotation();
    }
    // ----------------------------------------------------------------
    // ----------{ ABSTRACT METHODS }----------------------------------
    // ----------------------------------------------------------------
    public abstract Vector3 CalculateMagneticField(Vector3 p);
    public abstract void CreateMagnet();
    // ----------------------------------------------------------------
    // ----------{ EVENT METHODS }-------------------------------------
    // ----------------------------------------------------------------
    public void ModifyRotation(Vector3 rotation)
    {
        var modifyRot = rotation * Const.PI / 180;
        yaw = modifyRot.x % (2.0f * Const.PI);
        pitch = modifyRot.y % (2.0f * Const.PI);
        roll = modifyRot.z % (2.0f * Const.PI);
        MakeRotation();
    }
    public void SetDirectionVectors(Vector3 d, Vector3 u, Vector3 r)
    {
        direction = d;
        up = u;
        right = r;
    }
    // ----------------------------------------------------------------
    // ----------{ MATHS METHODS }-------------------------------------
    // ----------------------------------------------------------------
    private void MakeRotation()
    {
        direction = new Vector3(1, 0, 0);
        up = new Vector3(0, 1, 0);
        right = new Vector3(0, 0, 1);
        var rotX = RotationX(yaw);
        var rotY = RotationY(pitch);
        var rotZ = RotationZ(roll);
        var rotationMatrix = rotZ * rotY * rotX;
        direction = Matrix3X3.MatVecMultiply(rotationMatrix, direction);
        up = Matrix3X3.MatVecMultiply(rotationMatrix, up);
        right = Matrix3X3.MatVecMultiply(rotationMatrix, right);
    }
    /// <summary>
    /// Calculs the rotation along the x axis of an angle.
    /// </summary>
    /// <param name="angle">the angle</param>
    /// <returns>the rotation matrix</returns>
    private static Matrix3X3 RotationX(float angle)
    {
        float cosAngle = (float)Math.Cos(angle);
        float sinAngle = (float)Math.Sin(angle);
        return new Matrix3X3
        (
            1, 0, 0,
            0, cosAngle, -sinAngle,
            0, sinAngle, cosAngle
        );
    }
    /// <summary>
    /// Calculs the rotation along the y axis of an angle.
    /// </summary>
    /// <param name="angle">the angle</param>
    /// <returns>the rotation matrix</returns>
    private static Matrix3X3 RotationY(double angle)
    {
        float cosAngle = (float)Math.Cos(angle);
        float sinAngle = (float)Math.Sin(angle);
        return new Matrix3X3
        ( 
            cosAngle, 0, sinAngle,
            0, 1, 0,
            -sinAngle, 0, cosAngle
        );
    }
    /// <summary>
    /// Calculs the rotation along the z axis of an angle.
    /// </summary>
    /// <param name="angle">the angle</param>
    /// <returns>the rotation matrix</returns>
    private static Matrix3X3 RotationZ(double angle)
    {
        float cosAngle = (float)Math.Cos(angle);
        float sinAngle = (float)Math.Sin(angle);
        return new Matrix3X3
        (
            cosAngle, -sinAngle, 0,
            sinAngle, cosAngle, 0,
            0, 0, 1
        );
    }
}
// ----------------------------------------------------------------
// ----------{ BUFFER CLASS }--------------------------------------
// ----------------------------------------------------------------
public class Buffers
{
    public readonly List<Vector3> vertices;
    public readonly List<Int32> triangles;
    public Buffers(List<Vector3> vertices, List<Int32> triangles) {
        this.vertices = vertices;
        this.triangles = triangles;
    }
    public Buffers() {
        vertices = new();
        triangles = new();
    }
    public void AddBuffers(Buffers buffers ) {
        int startingIndex = vertices.Count;
        vertices.AddRange(buffers.vertices);
        foreach(int index in buffers.triangles) {
            triangles.Add(index + startingIndex);
        }
    }
}
// ----------------------------------------------------------------
// ----------{ MAGNETINFO STRUCT }---------------------------------
// ----------------------------------------------------------------
public struct MagnetInfo
{
    public Vector3 pos;
    public Vector3 right;
    public readonly float current;
    public readonly float length;
    public readonly uint wireNumber;
    public MagnetInfo(Vector3 pos, Vector3 right, float current, float length, uint wireNumber)
    {
        this.pos = pos;
        this.right = right;
        this.current = current;
        this.length = length;
        this.wireNumber = wireNumber;
    }
    public static int GetSize()
    {
        return 12 + 12 + 4 + 4 + 4;
    }
}
