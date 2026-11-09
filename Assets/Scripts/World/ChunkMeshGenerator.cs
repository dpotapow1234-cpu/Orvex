using System.Collections;
using UnityEngine;

using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;

public static class ChunkMeshGenerator
{
    private static readonly Vector3[][] Faces =
    {
        // Верх
        new Vector3[]
        {
            new Vector3(0,1,0),
            new Vector3(0,1,1),
            new Vector3(1,1,1),
            new Vector3(1,1,0)
        },

        // Низ
        new Vector3[]
        {
            new Vector3(0,0,0),
            new Vector3(1,0,0),
            new Vector3(1,0,1),
            new Vector3(0,0,1)
        },

        // Перед
        new Vector3[]
        {
            new Vector3(0,0,1),
            new Vector3(1,0,1),
            new Vector3(1,1,1),
            new Vector3(0,1,1)
        },

        // Зад
        new Vector3[]
        {
            new Vector3(1,0,0),
            new Vector3(0,0,0),
            new Vector3(0,1,0),
            new Vector3(1,1,0)
        },

        // Право
        new Vector3[]
        {
            new Vector3(1,0,1),
            new Vector3(1,0,0),
            new Vector3(1,1,0),
            new Vector3(1,1,1)
        },

        // Лево
        new Vector3[]
        {
            new Vector3(0,0,0),
            new Vector3(0,0,1),
            new Vector3(0,1,1),
            new Vector3(0,1,0)
        }
    };

    private static readonly Vector3Int[] Directions =
    {
        new Vector3Int(0, 1, 0),
        new Vector3Int(0, -1, 0),
        new Vector3Int(0, 0, 1),
        new Vector3Int(0, 0, -1),
        new Vector3Int(1, 0, 0),
        new Vector3Int(-1, 0, 0)
    };

    public static void Generate(Chunk chunk)
    {
        List<Vector3> vertices = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();

        List<int>[] triangles =
        {
            new List<int>(),
            new List<int>(),
            new List<int>()
        };

        for (int x = 0; x < Chunk.Size; x++)
        {
            for (int y = 0; y < Chunk.Height; y++)
            {
                for (int z = 0; z < Chunk.Size; z++)
                {
                    BlockType type = chunk.GetBlock(x, y, z);

                    if (type == BlockType.Air)
                        continue;

                    for (int face = 0; face < 6; face++)
                    {
                        Vector3Int direction = Directions[face];

                        BlockType neighbor = chunk.GetBlock(
                            x + direction.x,
                            y + direction.y,
                            z + direction.z
                        );

                        if (neighbor != BlockType.Air)
                            continue;

                        AddFace(
                            x, y, z,
                            face,
                            (int)type - 1,
                            vertices,
                            uvs,
                            triangles
                        );
                    }
                }
            }
        }

        Mesh mesh = new Mesh();

        mesh.indexFormat = IndexFormat.UInt32;

        mesh.SetVertices(vertices);
        mesh.SetUVs(0, uvs);

        mesh.subMeshCount = 3;

        for (int i = 0; i < 3; i++)
        {
            mesh.SetTriangles(triangles[i], i);
        }

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        MeshFilter filter = chunk.GetComponent<MeshFilter>();
        MeshRenderer renderer = chunk.GetComponent<MeshRenderer>();
        MeshCollider collider = chunk.GetComponent<MeshCollider>();

        filter.sharedMesh = mesh;
        renderer.sharedMaterials = chunk.GetMaterials();

        if (collider != null)
        {
            collider.sharedMesh = null;
            collider.sharedMesh = mesh;
        }
    }

    private static void AddFace(
        int x, int y, int z,
        int face,
        int materialIndex,
        List<Vector3> vertices,
        List<Vector2> uvs,
        List<int>[] triangles)
    {
        int start = vertices.Count;

        foreach (Vector3 vertex in Faces[face])
        {
            vertices.Add(vertex + new Vector3(x, y, z));
        }

        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 1));
        uvs.Add(new Vector2(1, 0));

        triangles[materialIndex].Add(start);
        triangles[materialIndex].Add(start + 1);
        triangles[materialIndex].Add(start + 2);

        triangles[materialIndex].Add(start);
        triangles[materialIndex].Add(start + 2);
        triangles[materialIndex].Add(start + 3);
    }
}