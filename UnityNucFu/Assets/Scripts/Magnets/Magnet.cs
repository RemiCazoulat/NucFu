using System;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace Magnets
{
    public abstract class Magnet 
    {
        public Vector3 Position;
        private float _yaw;
        private float _pitch;
        private float _roll;
        public Vector3 Direction; // Radian
        public Vector3 Up; // Radian 
        public Vector3 Right; // Radian
        public readonly List<MagnetInfo> MagInfos = new();
        protected readonly float SquareSpace;
        public Buffers Buffers;
        protected Magnet(Vector3 position, float squareSpace) {
            Position = position;
            SquareSpace = Math.Max(Math.Min(squareSpace, 0.9f), 0.1f);
            Buffers = new Buffers();
            MakeRotation();
        }
        // ----------------------------------------------------------------
        // ----------{ ABSTRACT METHODS }----------------------------------
        // ----------------------------------------------------------------
        public abstract void CreateMagnet();
        // ----------------------------------------------------------------
        // ----------{ EVENT METHODS }-------------------------------------
        // ----------------------------------------------------------------
        public void ModifyRotation(Vector3 rotation)
        {
            var modifyRot = rotation * Const.PI / 180;
            _yaw = modifyRot.x % (2.0f * Const.PI);
            _pitch = modifyRot.y % (2.0f * Const.PI);
            _roll = modifyRot.z % (2.0f * Const.PI);
            MakeRotation();
        }
        public void SetDirectionVectors(Vector3 d, Vector3 u, Vector3 r)
        {
            Direction = d;
            Up = u;
            Right = r;
        }
        public void GizmosDrawing()
        {
            if (MagInfos == null) return;
            
            Gizmos.color = Color.blue;
            foreach(var m in MagInfos)
            {
                var pos1 = m.Pos;
                var pos2 = pos1 + m.Right;
                Gizmos.DrawLine(pos1, pos2);
            }
        }

        public new string ToString()
        {
            var result = "";
            var i = 0;
            var numberSize = MagInfos.Count.ToString().Length;
            foreach (var m in MagInfos)
            {
                var zerosNumber = numberSize - i.ToString().Length;
                var magNumber = "";
                for (int j = 0; j < zerosNumber; j++) { magNumber += "0"; }
                magNumber += i;
                result += "Magnet " + magNumber + m.ToString() + "\n";
                i++;
            }
            return result;
        }
        // ----------------------------------------------------------------
        // ----------{ MATHS METHODS }-------------------------------------
        // ----------------------------------------------------------------
        
        private Vector3 CalculateMagneticField(Vector3 p)
        {
            var magneticField = Vector3.zero;
            foreach (var m in MagInfos)
            {
                var difPos = p - m.Pos;
                var distance = Vector3.Magnitude(difPos);
                var dL = m.Right * m.Length;
                if (distance == 0)
                {
                    continue;
                }
                var dB = m.WireNumber * Const.MAGNETIC_CONST * m.Current / (4 * Const.PI) * Vector3.Cross(dL, difPos) / Mathf.Pow(distance, 3);
                magneticField += dB;
                
            }
            return magneticField;
        }
        public string CalculateMagneticForce(Particle p, float time)
        {
            Vector3 magneticField = CalculateMagneticField(p.pos);
            Vector3 magneticForce = p.charge / p.mass * Vector3.Cross(p.vel, magneticField);
            return "time : " + time + "\n magnetic field : " + magneticField + "\n magnetic force : " + magneticForce;
        }
        private void DrawFieldLine(Vector3 startPoint, float lineLength, float stepSize)
        {
            List<Vector3> linePoints = new List<Vector3>();
            Vector3 currentPoint = startPoint;
            int maxIteration = (int)(lineLength / stepSize);
            for (float t = 0; t < maxIteration; t ++)
            {
                Vector3 magneticField = CalculateMagneticField(currentPoint);
                magneticField = magneticField.normalized * stepSize;
                linePoints.Add(currentPoint);
                currentPoint += magneticField;
            }
            LineRenderer lineRenderer = new GameObject("FieldLine").AddComponent<LineRenderer>();
            lineRenderer.positionCount = linePoints.Count;
            lineRenderer.SetPositions(linePoints.ToArray());
            lineRenderer.startWidth = 0.02f;
            lineRenderer.endWidth = 0.02f;
            //lineRenderer.material = lineMaterial;
        }
        /// <summary>
        /// Draw field lines in a grid
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="lineLength"></param>
        /// <param name="lineNumber"></param>
        /// <param name="floors"></param>
        /// <param name="spacing"></param>
        /// <param name="stepSize"></param>
        public void DrawFieldLines(Vector3 startPoint, float lineLength, int lineNumber, int floors, float spacing, float stepSize)
        {
            var z = 0f;
            for (var i = 0; i < lineNumber; i++)
            {
                var x = i * spacing;
                for(var j = 0; j < floors; j++)
                {   
                    var y = j * spacing;
                    var newStartPoint = startPoint + new Vector3(x, y, z);
                    DrawFieldLine(newStartPoint, lineLength, stepSize);
                }
            }
        }

        public void DrawMagneticField(Vector3 plane, int gridSize, float gridStep)
        {
            int dotsNumber = (int)(gridSize / gridStep);
            plane = plane.normalized;
            Vector3 y = new Vector3(-plane.y, plane.x, plane.z);
            Vector3 z = Vector3.Cross(plane, y);
            
            
            
            for (int i = - dotsNumber / 2; i < dotsNumber / 2; i++)
            {
                for(int j = - dotsNumber / 2; j <  dotsNumber / 2 ; j ++)
                {
                    Vector3 p =  Position + y * i * gridStep + z * j * gridStep;
                    Debug.Log("p : " + p);
                    Vector3 magneticField = CalculateMagneticField(p);
                    Debug.Log("mf : " + magneticField);

                    Debug.DrawLine(p, p + magneticField.normalized * gridStep, Color.red, 1000f);
                }
            }
        }
        private void MakeRotation()
        {
            Direction = new Vector3(1, 0, 0);
            Up = new Vector3(0, 1, 0);
            Right = new Vector3(0, 0, 1);
            var rotX = RotationX(_yaw);
            var rotY = RotationY(_pitch);
            var rotZ = RotationZ(_roll);
            var rotationMatrix = rotZ * rotY * rotX;
            Direction = Matrix3X3.MatVecMultiply(rotationMatrix, Direction);
            Up = Matrix3X3.MatVecMultiply(rotationMatrix, Up);
            Right = Matrix3X3.MatVecMultiply(rotationMatrix, Right);
        }
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
        public readonly List<Vector3> Vertices;
        public readonly List<Int32> Triangles;
        public Buffers(List<Vector3> vertices, List<Int32> triangles) {
            this.Vertices = vertices;
            this.Triangles = triangles;
        }
        public Buffers() {
            Vertices = new();
            Triangles = new();
        }
        public void AddBuffers(Buffers buffers ) {
            int startingIndex = Vertices.Count;
            Vertices.AddRange(buffers.Vertices);
            foreach(int index in buffers.Triangles) {
                Triangles.Add(index + startingIndex);
            }
        }
    }
// ----------------------------------------------------------------
// ----------{ MAGNET INFO STRUCT }---------------------------------
// ----------------------------------------------------------------
    public struct MagnetInfo
    {
        public Vector3 Pos;
        public Vector3 Right;
        public readonly float Current;
        public readonly float Length;
        public readonly uint WireNumber;
        public MagnetInfo(Vector3 pos, Vector3 right, float current, float length, uint wireNumber)
        {
            Pos = pos;
            Right = right;
            Current = current;
            Length = length;
            WireNumber = wireNumber;
        }
        public static int GetSize()
        {
            return 12 + 12 + 4 + 4 + 4;
        }

        public new string ToString()
        {
            return "(pos : " + Pos + ", right : " + Right + ", current : " + Current + ", length : " + Length + ")";
        }
    }
}