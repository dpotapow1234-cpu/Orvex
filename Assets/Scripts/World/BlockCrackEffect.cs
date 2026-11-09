using UnityEngine;

public class BlockCrackEffect : MonoBehaviour
{
    [Header("Crack Textures")]
    public Texture2D[] crackStages;

    [Header("Settings")]
    public Material crackMaterial;
    public float offset = 0.003f;

    private GameObject overlay;
    private MeshRenderer meshRenderer;
    private Material runtimeMaterial;

    private int currentStage = -1;

    private bool initialized;

    private void InitializeOverlay()
    {
        if (initialized)
            return;

        if (crackMaterial == null)
        {
            Debug.LogError("BlockCrackEffect: Crack Material не назначен!");
            return;
        }

        CreateOverlay();

        initialized = true;
        Hide();
    }

    private void CreateOverlay()
    {
        overlay = new GameObject("CrackOverlay");

        MeshFilter filter = overlay.AddComponent<MeshFilter>();
        meshRenderer = overlay.AddComponent<MeshRenderer>();

        filter.sharedMesh = CreateCubeMesh();

        runtimeMaterial = new Material(crackMaterial);

        meshRenderer.sharedMaterial = runtimeMaterial;

        meshRenderer.shadowCastingMode =
            UnityEngine.Rendering.ShadowCastingMode.Off;

        meshRenderer.receiveShadows = false;

        overlay.SetActive(false);
    }

    private Mesh CreateCubeMesh()
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices =
        {
            // Front
            new Vector3(0,0,1), new Vector3(1,0,1),
            new Vector3(1,1,1), new Vector3(0,1,1),

            // Back
            new Vector3(1,0,0), new Vector3(0,0,0),
            new Vector3(0,1,0), new Vector3(1,1,0),

            // Right
            new Vector3(1,0,1), new Vector3(1,0,0),
            new Vector3(1,1,0), new Vector3(1,1,1),

            // Left
            new Vector3(0,0,0), new Vector3(0,0,1),
            new Vector3(0,1,1), new Vector3(0,1,0),

            // Top
            new Vector3(0,1,1), new Vector3(1,1,1),
            new Vector3(1,1,0), new Vector3(0,1,0),

            // Bottom
            new Vector3(0,0,0), new Vector3(1,0,0),
            new Vector3(1,0,1), new Vector3(0,0,1)
        };

        Vector2[] uvs = new Vector2[24];

        for (int i = 0; i < 6; i++)
        {
            uvs[i * 4] = new Vector2(0, 0);
            uvs[i * 4 + 1] = new Vector2(1, 0);
            uvs[i * 4 + 2] = new Vector2(1, 1);
            uvs[i * 4 + 3] = new Vector2(0, 1);
        }

        int[] triangles = new int[36];

        for (int i = 0; i < 6; i++)
        {
            int v = i * 4;
            int t = i * 6;

            triangles[t] = v;
            triangles[t + 1] = v + 1;
            triangles[t + 2] = v + 2;

            triangles[t + 3] = v;
            triangles[t + 4] = v + 2;
            triangles[t + 5] = v + 3;
        }

        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();

        return mesh;
    }

    public void Show(Chunk chunk, Vector3Int blockPosition)
    {
        InitializeOverlay();

        if (!initialized)
            return;
        overlay.transform.SetParent(chunk.transform, false);

        overlay.transform.localPosition =
            (Vector3)blockPosition - Vector3.one * (offset / 2f);

        overlay.transform.localScale =
            Vector3.one * (1f + offset);

        overlay.SetActive(true);
    }

    public void SetStage(int stage)
    {
        if (crackStages == null || crackStages.Length == 0)
            return;

        if (stage == -1) Hide();
        else overlay.SetActive(true);

        stage = Mathf.Clamp(stage, 0, crackStages.Length - 1);

        if (currentStage == stage)
            return;

        currentStage = stage;

        runtimeMaterial.mainTexture = crackStages[stage];
    }

    public void Hide()
    {
        if (overlay != null)
            overlay.SetActive(false);

        currentStage = -1;
    }

    private void OnDestroy()
    {
        if (runtimeMaterial != null)
        {
            if (Application.isPlaying)
                Destroy(runtimeMaterial);
            else
                DestroyImmediate(runtimeMaterial);
        }
    }
}