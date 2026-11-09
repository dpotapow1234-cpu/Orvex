using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public Transform player;
    public GameObject chunkPrefab;

    [Header("World Settings")]
    public int renderDistance = 3;
    public int seed = 12345;

    private Dictionary<Vector2Int, Chunk> chunks =
        new Dictionary<Vector2Int, Chunk>();

    private Vector2Int lastPlayerChunk;

    private void Start()
    {
        UpdateWorld();
    }

    private void Update()
    {
        Vector2Int current = GetPlayerChunk();

        if (current != lastPlayerChunk)
            UpdateWorld();
    }

    private Vector2Int GetPlayerChunk()
    {
        return new Vector2Int(
            Mathf.FloorToInt(player.position.x / Chunk.Size),
            Mathf.FloorToInt(player.position.z / Chunk.Size)
        );
    }

    private void UpdateWorld()
    {
        lastPlayerChunk = GetPlayerChunk();

        for (int x = -renderDistance; x <= renderDistance; x++)
        {
            for (int z = -renderDistance; z <= renderDistance; z++)
            {
                Vector2Int position = lastPlayerChunk +
                    new Vector2Int(x, z);

                if (chunks.ContainsKey(position))
                    continue;

                GameObject obj = Instantiate(
                    chunkPrefab,
                    new Vector3(
                        position.x * Chunk.Size,
                        0,
                        position.y * Chunk.Size
                    ),
                    Quaternion.identity,
                    transform
                );

                Chunk chunk = obj.GetComponent<Chunk>();

                chunk.Initialize(seed, position);

                chunks.Add(position, chunk);
            }
        }
    }
}