using UnityEngine;

namespace Magnets
{
    public abstract class MagnetCreator : MonoBehaviour
    {
        public Magnet Magnet;
        protected Mesh Mesh;
        [Button(nameof(CreateMagnet))]
        public bool buttonField;
        public bool magneticFieldLines;
        public Vector3 fieldLinesStartingPoint;
        public float fieldLinesLength;
        public int fieldLinesNumber;
        public int fieldLinesFloorsNumber;
        public float fieldLinesSpacing;
        public float fieldLineStep;
        public abstract void CreateMagnet();
        private void Start()
        {
            CreateMagnet();
            Debug.Log(Magnet.ToString());
            if(magneticFieldLines) Magnet.DrawFieldLines(fieldLinesStartingPoint, fieldLinesLength, fieldLinesNumber, fieldLinesFloorsNumber, fieldLinesSpacing, fieldLineStep);
        }
        private void OnDrawGizmos()
        {
            //Magnet?.GizmosDrawing();
        }
        protected void AssignMesh() {
            Mesh.Clear();
            Mesh.vertices =  Magnet.Buffers.Vertices.ToArray();
            Mesh.triangles =  Magnet.Buffers.Triangles.ToArray();
            Mesh.RecalculateNormals();
            transform.TransformPoint(Vector3.zero);
        }
    }
}
