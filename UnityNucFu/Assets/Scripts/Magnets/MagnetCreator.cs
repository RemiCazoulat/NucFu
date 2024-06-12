using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MagnetCreator : MonoBehaviour
{
    public Magnet magnet;
    
  
    
    protected Mesh mesh;
    protected MeshFilter meshFilter;
    
    [Button(nameof(CreateMagnet))]
    public bool buttonField;

    public abstract void CreateMagnet();
    
    
    private void Start()
    {
        CreateMagnet();
    }
    
    protected void AssignMesh() {
        mesh.Clear();
        mesh.vertices =  magnet.buffers.vertices.ToArray();
        mesh.triangles =  magnet.buffers.triangles.ToArray();
        mesh.RecalculateNormals();
    }
}
