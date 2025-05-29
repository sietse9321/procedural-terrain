using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    [SerializeField] private bool drawGizmos;
    [SerializeField] MeshFilter meshFilter;
    [SerializeField] MeshCollider meshCollider;
    [SerializeField] private Vector2Int size;

    [Header("Offset Settings")] 
    [SerializeField] private Vector2 offset;

    [Header("Noise Settings")]
    [SerializeField] [Range(0.01f, 0.99f)] private float noiseScale = 0.3f;
    [SerializeField] private float amplitude = 2f;

    // Octaves and Persistence
    [Header("Fractal Noise Settings")]
    [SerializeField] private int octaves = 4;
    [SerializeField] [Range(0f, 1f)] private float persistence = 0.5f;
    [SerializeField] private float lacunarity = 2f;

    //Power Normalization
    [Header("Height Distribution")]
    [SerializeField] private float heightPower = 2f; // > 1 creates flatter plains; < 1 creates taller peaks

    // Additional height control for highly varied terrain
    [SerializeField] private float heightBias = 0.1f;

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
        mesh.normals = CalculateNormals(vertices);
        meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;
        AnalyzeMap();
    }

    private Vector3[] CreateVertices()
    {
        vertices = new Vector3[(size.x + 1) * (size.y + 1)];
        colors = new Color[vertices.Length];

        float minHeight = float.MaxValue;
        float maxHeight = float.MinValue;

        // Generate vertices with fractal noise (octaves and persistence)
        for (int i = 0, z = 0; z <= size.y; z++)
        {
            for (int x = 0; x <= size.x; x++)
            {
                // Calculate fractal noise
                float height = GenerateFractalNoise(x + offset.x, z + offset.y);

                // Apply height bias and power normalization
                height = Mathf.Pow(height + heightBias, heightPower);

                vertices[i] = new Vector3(x, height, z);

                // Track min and max height for normalization
                minHeight = Mathf.Min(minHeight, height);
                maxHeight = Mathf.Max(maxHeight, height);

                i++;
            }
        }

        // Second pass: Normalize heights and assign vertex colors
        for (int i = 0; i < vertices.Length; i++)
        {
            // Normalize the height range to 0 and 1
            float normalizedHeight = Mathf.InverseLerp(minHeight, maxHeight, vertices[i].y);
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


    private float GenerateFractalNoise(float x, float z)
    {
        float totalNoise = 0f;
        float currentAmplitude = amplitude;
        float currentFrequency = noiseScale;

        for (int i = 0; i < octaves; i++)
        {
            float noiseValue = Mathf.PerlinNoise(x * currentFrequency, z * currentFrequency);
            totalNoise += noiseValue * currentAmplitude;

            currentAmplitude *= persistence;  // Reduce amplitude for finer details
            currentFrequency *= lacunarity;  // Increase frequency for finer details
        }

        return totalNoise;
    }
    private Vector3[] CalculateNormals(Vector3[] verts)
    {
        Vector3[] normals = new Vector3[verts.Length];
        int width = size.x + 1;
        int height = size.y + 1;

        for (int z = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                int index = z * width + x;

                // Get surrounding heights with bounds checking
                float hL = x > 0 ? verts[z * width + x - 1].y : verts[index].y;
                float hR = x < width - 1 ? verts[z * width + x + 1].y : verts[index].y;
                float hD = z > 0 ? verts[(z - 1) * width + x].y : verts[index].y;
                float hU = z < height - 1 ? verts[(z + 1) * width + x].y : verts[index].y;

                Vector3 normal = new Vector3(hL - hR, 2f, hD - hU).normalized;
                normals[index] = normal;
            }
        }

        return normals;
    }

    private void OnDrawGizmos()
    {
        if (vertices == null || !drawGizmos) return;

        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }

    private void AnalyzeMap()
    {
        MapAnalyzer mapAnalyzer = GetComponent<MapAnalyzer>();
        if (mapAnalyzer != null && vertices != null)
        {
            mapAnalyzer.PrintHighestValue(vertices);
        }
    }
}