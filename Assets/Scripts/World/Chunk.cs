using UnityEngine;

using UnityEngine;

public class Chunk : MonoBehaviour
{
    public const int Size = 16;
    public const int Height = 64;

    public int seed = 12345;
    public Vector2Int ChunkPosition;

    [Header("Materials")]
    public Material grassMaterial;
    public Material dirtMaterial;
    public Material stoneMaterial;

    private BlockType[,,] blocks;

    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private MeshCollider meshCollider;

    public void Initialize(int worldSeed, Vector2Int position)
    {
        seed = worldSeed;
        ChunkPosition = position;

        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();

        blocks = new BlockType[Size, Height, Size];

        GenerateTerrain();
        ChunkMeshGenerator.Generate(this);
    }

    private void GenerateTerrain()
    {
        for (int x = 0; x < Size; x++)
        {
            for (int z = 0; z < Size; z++)
            {
                int worldX = ChunkPosition.x * Size + x;
                int worldZ = ChunkPosition.y * Size + z;

                float noise = Mathf.PerlinNoise(
                    (worldX + seed) * 0.035f,
                    (worldZ + seed) * 0.035f
                );

                int surface = Mathf.FloorToInt(noise * 20) + 15;

                for (int y = 0; y < surface; y++)
                {
                    if (y == surface - 1)
                        blocks[x, y, z] = BlockType.Grass;
                    else if (y >= surface - 4)
                        blocks[x, y, z] = BlockType.Dirt;
                    else
                        blocks[x, y, z] = BlockType.Stone;
                }
            }
        }
    }

    public BlockType GetBlock(int x, int y, int z)
    {
        if (x < 0 || x >= Size ||
            y < 0 || y >= Height ||
            z < 0 || z >= Size)
            return BlockType.Air;

        return blocks[x, y, z];
    }

    public Material[] GetMaterials()
    {
        return new Material[]
        {
            grassMaterial,
            dirtMaterial,
            stoneMaterial
        };
    }
}