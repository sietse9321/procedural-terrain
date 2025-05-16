using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    [SerializeField] private bool drawGizmos;
    [SerializeField] MeshFilter meshFilter;
    [SerializeField] private Vector2Int size;

    [Header("Offset Settings")] [SerializeField]
    Vector2 offset;

    [Header("Noise Settings")] [SerializeField] [Range(0.01f, 0.99f)]
    private float noiseScale = 0.3f;

    [SerializeField] private float amplitude = 2f;

    Vector3[] vertices;
    Color[] colors;

    private void OnValidate()
    {
        if (Application.isPlaying) return;
        GenerateMesh();
    }

    public void GenerateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = CreateVertices();
        mesh.triangles = CreateTriangles();
        mesh.colors = colors;

        mesh.RecalculateNormals();
        meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;
    }

    private Vector3[] CreateVertices()
    {
        vertices = new Vector3[(size.x + 1) * (size.y + 1)];
        colors = new Color[vertices.Length];

        float minHeight = float.MaxValue;
        float maxHeight = float.MinValue;

        //generate vertices
        for (int i = 0, z = 0; z <= size.y; z++)
        {
            for (int x = 0; x <= size.x; x++)
            {
                //generate height based on noise
                float height = Mathf.PerlinNoise((x + offset.x) * noiseScale, (z + offset.y) * noiseScale) * amplitude;
                vertices[i] = new Vector3(x, height, z);

                minHeight = Mathf.Min(minHeight, height);
                maxHeight = Mathf.Max(maxHeight, height);

                i++;
            }
        }

        // Second pass: Normalize heights and set colors
        for (int i = 0; i < vertices.Length; i++)
        {
            //normalize the height between 0 and 1
            float normalizedHeight = Mathf.InverseLerp(minHeight, maxHeight, vertices[i].y);
            //sets the color of the vertex based on the normalized height
            colors[i] = Color.Lerp(Color.black, Color.white, normalizedHeight);
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
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + size.x + 1;
                triangles[tris + 2] = vert + 1;
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