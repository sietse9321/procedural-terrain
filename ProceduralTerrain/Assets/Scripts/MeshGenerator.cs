using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;


[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    [SerializeField] private bool drawGizmos;
    [SerializeField] MeshFilter meshFilter;
    [SerializeField] private Vector2Int size;
    Vector3[] vertices;

    //editor only: when script is loaded or values changed
    private void OnValidate()
    {
        //skip during playmode
        if (Application.isPlaying) return;
        GenerateMesh();
    }

    public void GenerateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = CreateVertices();
        mesh.triangles = CreateTriangles();
        
        mesh.RecalculateNormals();
        meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;
    }


    private Vector3[] CreateVertices()
    {
        vertices = new Vector3[(size.x + 1) * (size.y + 1)];
        for (int i = 0, z = 0; z <= size.y; z++)
        {
            for (int x = 0; x <= size.x; x++)
            {
                vertices[i] = new Vector3(x, 0, z);
                i++;
            }
        }

        return vertices;
    }
    
    private int[] CreateTriangles()
    {
        int[] triangles = new int[size.x * size.y * 6];
        for (int z = 0, vert = 0, tris = 0; z < size.y; z++)
        {
            for (int x = 0; x < size.x; x++)
            {
                //creates two triangles for each quad in the mesh:
                //first triangle: bottom-left, top-left, bottom-right
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + size.x + 1;
                triangles[tris + 2] = vert + 1;
                //second triangle: bottom-right, top-left, top-right
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + size.x + 1;
                triangles[tris + 5] = vert + size.x + 2;

                vert++;
                tris += 6;
            }

            vert++;
        }

        return triangles;
    }

    private void OnDrawGizmos()
    {
        if (vertices == null || !drawGizmos) return;

        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }
}