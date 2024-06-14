using System;
using UnityEngine;

namespace Particles
{
    public class ParticleManager : MonoBehaviour
    {
        // ----------------------------------------------------------------------------
        // ----------------------------{ ATTRIBUTES }----------------------------------
        // ----------------------------------------------------------------------------
        // ----{ PUBLIC ATTRIBUTES }----
        public float subStep;
        public int particleNumber;
        public float particleSize;
        public int particleResolution;
        public Vector3 particlePosition;
        public Vector3 particleDirection;
        public float particleCharge;
        public float particleMass;
        public bool isArrows;
        public MagnetCreator magnetCreator;
        public ComputeShader compute;
    
        // ----{ COMPUTE BUFFERS }----
        // Only for init
        private ComputeBuffer _intermediateTrianglesBuffer;
        private ComputeBuffer _verticesNotEmptyBuffer;
        private ComputeBuffer _trianglesBuffer;
        // For init and update
        private ComputeBuffer _verticesBuffer;
        private ComputeBuffer _particlesBuffer;
        // Only for update
        private ComputeBuffer _magnetsBuffer;
        // Only for arrows
        private ComputeBuffer _arrowsBuffer;
        private ComputeBuffer _arrowsTrianglesBuffer;
    
        // ----{ ARRAYS FOR COMPUTE BUFFERS }----
        private int[] _verticesNotEmpty; // Only for initialization
        private int[] _triangles; // only for initialization, get it after, and release it.
        private Vector3[] _vertices; // Stocking particles positions
        private Particle[] _particles; // Stocking particles infos
        private MagnetInfo[] _magnets;
        private Vector3[] _arrows; // vertices for arrows
        private int[] _arrowsTriangles; // triangles for arrows
    
        // ----{ BUTTONS }----
        [Button(nameof(CreateParticles))]
        public bool buttonField1;
        // ----{ MESHS }---- 
        private Mesh _particlesMesh;
        // ----{ BUFFER IDS }---- 
        private int _resolutionID ;
        private int _interTriSizeID ;
        private int _radiusID ;
        private int _intermediateTrianglesID ;
        private int _verticesNotEmptyID ;
        private int _trianglesID ;
        private int _verticesID ;
        private int _particlesID ;
        private int _timeID ;
        private int _magnetNumberID;
        private int _magnetID;
        private int _arrowsID;
        private int _arrowsTrianglesID;
    
        // ----------------------------------------------------------------------------
        // ------------------------{ BUTTONS FUNCTIONS }-------------------------------
        // ----------------------------------------------------------------------------
        public void CreateParticles()
        {
            InitParticles();
            ReleaseParticlesBuffers();
        }
        // ----------------------------------------------------------------------------
        // ---------------------{ INITIALIZATION FUNCTIONS }---------------------------
        // ----------------------------------------------------------------------------
    
        /// <summary>
        /// Initialize all the particles, at the position attributed in the SetParticles() function.
        /// The number of particles is decided with particlesNumber.
        /// This function will create the vertices array and the triangles indexes array and store them,
        /// so that the particles can be displayed with the mesh. 
        /// </summary>
        ///<remarks>
        /// It is not mandatory that the magnet is initialized to init the particles.
        /// </remarks>
        private void InitParticles()
        { 
            // ----{ INIT PARTICLES MESH }----
            transform.position = Vector3.zero;
            _particlesMesh = new Mesh();
            GetComponent<MeshFilter>().mesh = _particlesMesh;
            _particlesMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
            // ----{ CALCULATING COUNTS OF BUFFERS }----
            var interTriNumber = 20; // initial faces number = 20
            for(var i = 1; i <= particleResolution; i ++) { interTriNumber += 20 * (int)Math.Pow(4.0, i); }
            interTriNumber *= 3;
            var facesPerSphere = 20 * (int)Math.Pow(4, particleResolution);
            var totalVerticesNumber = particleNumber * (facesPerSphere / 2 + 2);
            var totalTrianglesIndicesNumber = particleNumber * facesPerSphere * 3;
            var totalInterTriNumber = interTriNumber * particleNumber;
            // ----{ INIT ARRAYS }----
            _particles = new Particle[particleNumber];
            _verticesNotEmpty = new int[totalVerticesNumber ];
            _triangles = new int[totalTrianglesIndicesNumber];
            _vertices = new Vector3[totalVerticesNumber];
            // ----{ SETTING INITIAL DATAS }----
            for (var i = 0; i < totalVerticesNumber  ; i++) { _verticesNotEmpty[i] = -1; }
            SetParticles();
            // ----{ INIT BUFFERS }----
            _intermediateTrianglesBuffer = new ComputeBuffer(totalInterTriNumber,4);
            _verticesNotEmptyBuffer = new ComputeBuffer(totalVerticesNumber, 4);
            _trianglesBuffer = new ComputeBuffer(totalTrianglesIndicesNumber,4);
            _particlesBuffer = new ComputeBuffer(particleNumber, Particle.GetSize());
            _verticesBuffer = new ComputeBuffer(totalVerticesNumber, 12);
            // ----{ SETTING DATAS, BUFFERS & IDS }----
            _verticesNotEmptyBuffer.SetData(_verticesNotEmpty);
            _particlesBuffer.SetData(_particles);
            _resolutionID = Shader.PropertyToID("resolution");
            _interTriSizeID = Shader.PropertyToID("inter_tri_size");
            _radiusID = Shader.PropertyToID("radius");
            _intermediateTrianglesID = Shader.PropertyToID("intermediate_triangles");
            _verticesNotEmptyID = Shader.PropertyToID("vertices_not_empty");
            _trianglesID = Shader.PropertyToID("triangles");
            _verticesID = Shader.PropertyToID("vertices");
            _particlesID = Shader.PropertyToID("particles");
            _timeID = Shader.PropertyToID("time");
            compute.SetInt(_resolutionID, particleResolution);
            compute.SetInt(_interTriSizeID, interTriNumber);
            compute.SetFloat(_radiusID, particleSize);
            compute.SetBuffer(0, _intermediateTrianglesID, _intermediateTrianglesBuffer);
            compute.SetBuffer(0, _verticesNotEmptyID, _verticesNotEmptyBuffer);
            compute.SetBuffer(0, _trianglesID, _trianglesBuffer);
            compute.SetBuffer(0, _verticesID, _verticesBuffer);
            compute.SetBuffer(0, _particlesID, _particlesBuffer);
            compute.SetBuffer(1, _particlesID, _particlesBuffer);
            // ----{ DISPATCH AND GET DATAS }---- 
            compute.SetFloat(_timeID, Time.deltaTime);
            compute.Dispatch(0, particleNumber / 128 + 1, 1, 1);
            _verticesBuffer.GetData(_vertices);
            _trianglesBuffer.GetData(_triangles);
            // ----{ SETTING PARTICLES MESH TRIANGLES }----
            _particlesMesh.vertices = _vertices;
            _particlesMesh.triangles = _triangles;
            _particlesMesh.RecalculateNormals();
        }
        private void InitMagnet()
        {
            _magnetNumberID = Shader.PropertyToID("magnet_number");
            _magnetID = Shader.PropertyToID("magnet");
            _magnets = magnetCreator.magnet.mi.ToArray();
            _magnetsBuffer = new ComputeBuffer(magnetCreator.magnet.mi.Count, MagnetInfo.GetSize());
            compute.SetInt(_magnetNumberID, magnetCreator.magnet.mi.Count);
            compute.SetBuffer(1, _verticesID, _verticesBuffer);
            compute.SetBuffer(1, _magnetID, _magnetsBuffer);
            compute.SetBuffer(1, _particlesID, _particlesBuffer);
            _magnetsBuffer.SetData(magnetCreator.magnet.mi);
        }
        private void InitArrows()
        {
            _arrowsID = Shader.PropertyToID("arrows");
            var arrowCountPerSphere = _magnets.Length + 1;
            var totalArrowCount = arrowCountPerSphere * particleNumber;
            _arrows = new Vector3[totalArrowCount];
            _arrowsBuffer = new ComputeBuffer(totalArrowCount, 12);
            compute.SetBuffer(1, _arrowsID, _arrowsBuffer);
        }
        // ----------------------------------------------------------------------------
        // --------------------------{ UPDATE FUNCTIONS }------------------------------
        // ----------------------------------------------------------------------------
        private void UpdateParticles()
        {
            GPUStepParticles();
        }
        private void GPUStepParticles()
        {
            compute.SetFloat(_timeID, Time.deltaTime);
            compute.Dispatch(1, particleNumber / 128 + 1, 1, 1);
            _verticesBuffer.GetData(_vertices);
            _particlesBuffer.GetData(_particles);
            _arrowsBuffer.GetData(_arrows);
        
            _particlesMesh.vertices = _vertices;
            _particlesMesh.RecalculateNormals();
        }
        private void SetParticles()
        {
            for (var i = 0; i < particleNumber; i++)
            {
                var pos = new Vector3(particleSize * 10 * i, 0f, 0f);
                _particles[i] = new Particle(pos + particlePosition, particleDirection, particleCharge, particleMass);
            }
        }


        private void ReleaseParticlesBuffers()
        {
            _intermediateTrianglesBuffer.Release();
            _verticesNotEmptyBuffer.Release();
            _trianglesBuffer.Release();
            _particlesBuffer.Release();
            _verticesBuffer.Release();
            _intermediateTrianglesBuffer = null;
            _verticesNotEmptyBuffer = null;
            _trianglesBuffer = null;
            _particlesBuffer = null;
            _verticesBuffer = null;
        }

        private void ReleaseMagnetBuffers()
        {
            _magnetsBuffer.Release();
            _magnetsBuffer = null;
        }
    
        private void ReleaseArrowsBuffers()
        {
            _arrowsBuffer.Release();
            _arrowsTrianglesBuffer.Release();
            _arrowsBuffer = null;
            _arrowsTrianglesBuffer = null;
        }

        private void ReleaseAllBuffers()
        {
            ReleaseParticlesBuffers();
            ReleaseMagnetBuffers();
            ReleaseArrowsBuffers();
        }

        private void Start()
        {
            InitParticles();
            InitMagnet();
            if(isArrows) InitArrows();

        }

        private void FixedUpdate()
        {
            for (var i = 0; i < subStep; i++)
            {
                UpdateParticles();
            }
        }
    
        private void OnDisable ()
        {
            ReleaseAllBuffers();
        }

    }
}


