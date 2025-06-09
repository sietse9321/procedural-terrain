using UnityEngine;

public class ChunkSpawner : MonoBehaviour
{
    public MeshGenerator meshPrefab;
    public int chunksX = 4;
    public int chunksY = 4;
    public Vector2Int chunkSize = new Vector2Int(16, 16);

    [Header("World Seed")]
    public string worldSeed;
    public bool useRandomSeed = true;

    void Start()
    {
        // Determine one random base offset for the whole terrain using the seed
        string actualSeed = worldSeed;
        if (useRandomSeed) actualSeed = System.DateTime.Now.Ticks.ToString();

        // Generate one deterministic base offset for the whole world
        var prng = new System.Random(actualSeed.GetHashCode());
        float randomOffsetX = prng.Next(-100000, 100000);
        float randomOffsetY = prng.Next(-100000, 100000);
        Vector2 baseRandomOffset = new Vector2(randomOffsetX, randomOffsetY);

        for (int x = 0; x < chunksX; x++)
        {
            for (int y = 0; y < chunksY; y++)
            {
                int worldX = x * chunkSize.x;
                int worldY = y * chunkSize.y;
                Vector3 chunkPosition = new Vector3(worldX, 0f, worldY);

                MeshGenerator chunk = Instantiate(meshPrefab, chunkPosition, Quaternion.identity, transform);
                chunk.size = chunkSize;
                chunk.seed = actualSeed;
                chunk.useRandomSeed = false;

                // Add the same base random offset to each chunk, then add the logical offset for tiling
                chunk.offset = new Vector2(worldX, worldY) + baseRandomOffset;
                chunk.GenerateMesh();
            }
        }
    }
}