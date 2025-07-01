using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    [SerializeField] private bool drawGizmos;
    [SerializeField] MeshFilter meshFilter;
    [SerializeField] MeshCollider meshCollider;
    [SerializeField] public Vector2Int size;

    [Header("Offset Settings")] 
    [SerializeField] public Vector2 offset;

    [Header("Noise Settings")]
    [SerializeField] [Range(0.01f, 0.99f)] private float noiseScale = 0.029f;
    [SerializeField] private float amplitude = 1.45f;

    [Header("Fractal Noise Settings")]
    [SerializeField] private int octaves = 5;
    [SerializeField] [Range(0f, 1f)] private float persistence = 0.374f;
    [SerializeField] private float lacunarity = 2f;

    [Header("Height Distribution")]
    [SerializeField] private float heightPower = 4.71f;
    [SerializeField] private float heightBias = 0.1f;

    [Header("Seed")]
    [SerializeField] public string seed = "default";

    [Header("Height Reference")]
    [SerializeField] public float maximumHeight = 10f;
    [SerializeField] Gradient gradient;
    
    //vertex spacing 
    [SerializeField] private float vertexSpacing = 1f;

    Vector3[] _vertices;
    Color[] _colors;

    private System.Random _prng;

    //switch to OnValidate for debug 
    private void Awake()
    {
        GenerateMesh();
    }

    public void GenerateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = CreateVertices();
        mesh.triangles = CreateTriangles();
        mesh.colors = _colors;

        mesh.RecalculateNormals();
        mesh.normals = CalculateNormals(_vertices);
        meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;
        AnalyzeMap();
    }

    /// <summary>
    /// Creates a mesh with noise heightmap
    /// </summary>
    /// <returns></returns>
    private Vector3[] CreateVertices()
    {
        _vertices = new Vector3[(size.x + 1) * (size.y + 1)];
        _colors = new Color[_vertices.Length];

        float localMinHeight = float.MaxValue;
        float localMaxHeight = float.MinValue;

        for (int i = 0, z = 0; z <= size.y; z++)
        {
            for (int x = 0; x <= size.x; x++)
            {
                float height = GenerateFractalNoise(x + offset.x, z + offset.y);
                height = Mathf.Pow(height + heightBias, heightPower);

                _vertices[i] = new Vector3(x * vertexSpacing, height, z * vertexSpacing);
                localMinHeight = Mathf.Min(localMinHeight, height);
                localMaxHeight = Mathf.Max(localMaxHeight, height);

                i++;
            }
        }

        //use the provided maximumHeight for all color normalization across chunks
        for (int i = 0; i < _vertices.Length; i++)
        {
            float normalizedHeight = Mathf.InverseLerp(0, maximumHeight, _vertices[i].y);
            _colors[i] = gradient.Evaluate(normalizedHeight);
        }

        return _vertices;
    }

    /// <summary>
    /// Makes the vertices into triangles
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Generates a fractal noise map
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <returns>the total noise amount</returns>
    private float GenerateFractalNoise(float x, float z)
    {
        float totalNoise = 0f;
        float currentAmplitude = amplitude;
        float currentFrequency = noiseScale;

        for (int i = 0; i < octaves; i++)
        {
            float noiseValue = Mathf.PerlinNoise(x * currentFrequency, z * currentFrequency);
            totalNoise += noiseValue * currentAmplitude;

            currentAmplitude *= persistence;
            currentFrequency *= lacunarity;
        }

        return totalNoise;
    }

    /// <summary>
    /// Calculates the normals for the mesh
    /// </summary>
    /// <param name="verts"></param>
    /// <returns></returns>
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

   
    /// <summary>
    /// Draws a sphere at each vertex of the mesh
    /// </summary>
    private void OnDrawGizmos()
    {
        if (_vertices == null || !drawGizmos) return;

        for (int i = 0; i < _vertices.Length; i++)
        {
            Gizmos.DrawSphere(_vertices[i], 0.1f);
        }
    }

    /// <summary>
    /// Debug function to analyze the map and print the highest value in the map
    /// </summary>
    private void AnalyzeMap()
    {
        MapAnalyzer mapAnalyzer = GetComponent<MapAnalyzer>();
        if (mapAnalyzer != null && _vertices != null)
        {
            mapAnalyzer.PrintHighestValue(_vertices);
        }
    }
}