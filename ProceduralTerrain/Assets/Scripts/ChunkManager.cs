using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    [SerializeField] MeshGenerator meshPrefab;
    [SerializeField] int chunksX = 4;
    [SerializeField] int chunksY = 4;
    [SerializeField] Vector2Int chunkSize = new Vector2Int(16, 16);

    [Header("World Seed")]
    [SerializeField] string worldSeed;
    [SerializeField] private Vector2 worldOffset;

    private void Start()
    {
        worldSeed = GameManager.Instance.seed;
        if (worldSeed == "") worldSeed = System.DateTime.Now.Ticks.ToString();

        var prng = new System.Random(worldSeed.GetHashCode());
        float randomOffsetX = prng.Next(-100000, 100000);
        float randomOffsetY = prng.Next(-100000, 100000);
        Vector2 baseRandomOffset = new Vector2(randomOffsetX, randomOffsetY);

        int halfChunksX = chunksX / 2;
        int halfChunksY = chunksY / 2;

        for (int x = -halfChunksX; x < halfChunksX; x++)
        {
            for (int y = -halfChunksY; y < halfChunksY; y++)
            {
                int worldX = x * chunkSize.x;
                int worldY = y * chunkSize.y;
                Vector3 chunkPosition = new Vector3(worldX, 0f, worldY);

                MeshGenerator chunk = Instantiate(meshPrefab, chunkPosition, Quaternion.identity, transform);
                chunk.size = chunkSize;
                chunk.seed = worldSeed;
                chunk.useRandomSeed = false;

                chunk.offset = new Vector2(worldX, worldY) + baseRandomOffset;
                chunk.GenerateMesh();
            }
        }
        worldOffset = baseRandomOffset;
    }
}