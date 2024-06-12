using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public abstract class Magnet 
{
    public Vector3 position;
    //public Vector3 rotation; // degrees
    private float yaw;
    private float pitch;
    private float roll;
    public Vector3 direction; // Radian
    public Vector3 up; // Radian 
    public Vector3 right; // Radian

    public List<MagnetInfo> mi = new List<MagnetInfo>();

    protected float squareSpace;
    
    public Buffers buffers;
    protected Magnet(Vector3 position, float squareSpace) {
        this.position = position;
       
        this.squareSpace = Math.Max(Math.Min(squareSpace, 0.9f), 0.1f);
        
        buffers = new Buffers();
        
        direction = new Vector3(1, 0, 0);
        up = new Vector3(0, 1, 0);
        right = new Vector3(0, 0, 1);

        var rotX = RotationX(yaw);
        var rotY = RotationY(pitch);
        var rotZ = RotationZ(roll);

        var rotationMatrix = MatMult( MatMult(rotZ, rotY), rotX);
        
        direction = MatVecMult(rotationMatrix, direction);
        up = MatVecMult(rotationMatrix, up);
        right = MatVecMult(rotationMatrix, right);

    }
    // ***** ABSTACT METHODS *****
    public abstract Vector3 CalculateMagneticField(Vector3 p);
    public abstract void CreateMagnet();
    
    // ***** EVENTS METHODS *****
    public void ModifyRotation(Vector3 rotation)
    {
        var modifyRot = rotation * Const.PI / 180;
        yaw = modifyRot.x % (2.0f * Const.PI);
        pitch = modifyRot.y % (2.0f * Const.PI);
        roll = modifyRot.z % (2.0f * Const.PI);
        
        direction = new Vector3(1, 0, 0);
        up = new Vector3(0, 1, 0);
        right = new Vector3(0, 0, 1);
        var rotX = RotationX(yaw);
        var rotY = RotationY(pitch);
        var rotZ = RotationZ(roll);
        var rotationMatrix = MatMult( MatMult(rotZ, rotY), rotX);
        direction = MatVecMult(rotationMatrix, direction);
        up = MatVecMult(rotationMatrix, up);
        right = MatVecMult(rotationMatrix, right);
    }

    public void SetDirectionVectors(Vector3 d, Vector3 u, Vector3 r)
    {
        direction = d;
        up = u;
        right = r;
    }
    
    // ***** MATHS METHODS *****
    
    /// <summary>
    /// Calculs the rotation along the x axis of an angle.
    /// </summary>
    /// <param name="angle">the angle</param>
    /// <returns>the rotation matrix</returns>
    private static float[,] RotationX(float angle)
    {
        float cosAngle = (float)Math.Cos(angle);
        float sinAngle = (float)Math.Sin(angle);
        return new[,]
        {
            {1, 0, 0},
            {0, cosAngle, -sinAngle},
            {0, sinAngle, cosAngle}
        };
    }
    /// <summary>
    /// Calculs the rotation along the y axis of an angle.
    /// </summary>
    /// <param name="angle">the angle</param>
    /// <returns>the rotation matrix</returns>
    private static float[,] RotationY(double angle)
    {
        float cosAngle = (float)Math.Cos(angle);
        float sinAngle = (float)Math.Sin(angle);
        return new[,]
        {
            {cosAngle, 0, sinAngle},
            {0, 1, 0},
            {-sinAngle, 0, cosAngle}
        };
    }
    /// <summary>
    /// Calculs the rotation along the z axis of an angle.
    /// </summary>
    /// <param name="angle">the angle</param>
    /// <returns>the rotation matrix</returns>
    private static float[,] RotationZ(double angle)
    {
        float cosAngle = (float)Math.Cos(angle);
        float sinAngle = (float)Math.Sin(angle);
        return new[,]
        {
            {cosAngle, -sinAngle, 0},
            {sinAngle, cosAngle, 0},
            {0, 0, 1}
        };
    }
    /// <summary>
    /// Multiplicates 2 matrices.
    /// </summary>
    /// <param name="matrix1">the first matrix</param>
    /// <param name="matrix2">the seccond matrix</param>
    /// <returns>the multiplication between these two matrices.</returns>
    private static float[,] MatMult(float[,] matrix1, float[,] matrix2)
    {
        int rows = matrix1.GetLength(0);
        int cols = matrix2.GetLength(1);
        float[,] result = new float[rows, cols];
        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < cols; j++) {
                result[i, j] = 0;
                for (int k = 0; k < matrix1.GetLength(1); k++) {
                    result[i, j] += matrix1[i, k] * matrix2[k, j];
                }
            }
        }
        return result;
    }
    /// <summary>
    /// Multiplicates a vector with a matrix.
    /// </summary>
    /// <param name="matrix">the matrix</param>
    /// <param name="vector">the vector</param>
    /// <returns>the multiplication between the matrix and the vector</returns>
    private static Vector3 MatVecMult(float[,] matrix, Vector3 vector)
    {
        
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);
        Vector3 result = new();
        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < cols; j++) {
                result[i] += matrix[i, j] * vector[j];
            }
        }
        return result;
    }
    /*
    protected String Mat2String(float[,] matrix)
    {
        int rows = matrix.GetLength(0);
        int cols = matrix.GetLength(1);
        String result = "";
        for (int i = 0; i < rows; i ++)
        {
            result += " / ";
            for (var j = 0; j < cols; j++)
            {
                result += matrix[i, j];
                result += ", ";
            }

        }

        return result;
    }
    */
    
}

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
