using Unity.AI.Navigation;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    [SerializeField] MeshGenerator meshPrefab;
    [SerializeField] int chunksX;
    [SerializeField] int chunksY;
    [SerializeField] Vector2Int chunkSize = new Vector2Int(16, 16);

    [Header("World Seed")] [SerializeField]
    string worldSeed;

    [SerializeField] private Vector2 worldOffset;

    private void Start()
    {
        //get seed from gamemanager
        worldSeed = GameManager.Instance.seed;
        Debug.Log(worldSeed);
        //set seed to random if empty
        if (worldSeed == "")
        {
            worldSeed = System.DateTime.Now.Ticks.ToString();
        }
        else
        {
            //remove spaces from seed
            worldSeed = worldSeed.Replace(" ", "");
            //if seed is too long, remove characters
            if (worldSeed.Length > 18)
            {
                worldSeed = worldSeed.Substring(0, 18);
            }
        }
        
        //set random offset
        var prng = new System.Random(worldSeed.GetHashCode());
        float randomOffsetX = prng.Next(-100000, 100000);
        float randomOffsetY = prng.Next(-100000, 100000);
        Vector2 baseRandomOffset = new Vector2(randomOffsetX, randomOffsetY);
        
        int halfChunksX = chunksX / 2;
        int halfChunksY = chunksY / 2;

        //generate chunks
        for (int x = -halfChunksX; x < halfChunksX; x++)
        {
            for (int y = -halfChunksY; y < halfChunksY; y++)
            {
                int worldX = x * chunkSize.x;
                int worldY = y * chunkSize.y;
                Vector3 chunkPosition = new Vector3(worldX, 0f, worldY);

                //makes a new chunk
                MeshGenerator chunk = Instantiate(meshPrefab, chunkPosition, Quaternion.identity, transform);
                //sets chunk properties
                chunk.size = chunkSize;
                chunk.seed = worldSeed;

                //sets chunk offset
                chunk.offset = new Vector2(worldX, worldY) + baseRandomOffset;
                chunk.GenerateMesh();
            }
        }

        worldOffset = baseRandomOffset;
        //build navmesh
        NavMeshSurface navMesh = GetComponent<NavMeshSurface>();
        navMesh.BuildNavMesh();
    }
}