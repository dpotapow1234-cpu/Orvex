using UnityEditor.SceneManagement;
using UnityEngine;

public class BlockDestroyer : MonoBehaviour
{
    public BlockCrackEffect crackEffect;

    [Header("Settings")]
    public Camera playerCamera;
    public float reachDistance = 5f;
    public float destroyTime = 1f;

    private Chunk targetChunk;
    private Vector3Int targetBlock;

    private float destroyProgress;
    public BlockOutline outline;


    private void Update()
    {
        Ray ray = new Ray(
            playerCamera.transform.position,
            playerCamera.transform.forward
        );

        if (!Physics.Raycast(
            ray,
            out RaycastHit hit,
            reachDistance))
        {
            ResetMining();
            return;
        }

        Chunk chunk = hit.collider.GetComponent<Chunk>();

        if (chunk == null)
        {
            ResetMining();
            return;
        }

        // Переводим точку попадания в локальные координаты чанка.
        Vector3 localPoint = chunk.transform.InverseTransformPoint(
            hit.point - hit.normal * 0.01f
        );

        Vector3Int blockPosition = new Vector3Int(
            Mathf.FloorToInt(localPoint.x),
            Mathf.FloorToInt(localPoint.y),
            Mathf.FloorToInt(localPoint.z)
        );

        if (!chunk.IsValidPosition(
            blockPosition.x,
            blockPosition.y,
            blockPosition.z))
        {
            ResetMining();
            return;
        }

        outline.Show(chunk, blockPosition);

        if (chunk.GetBlock(
            blockPosition.x,
            blockPosition.y,
            blockPosition.z) == BlockType.Air)
        {
            ResetMining();
            return;
        }

        // Проверяем, изменился ли блок под прицелом.
        if (targetChunk != chunk || targetBlock != blockPosition)
        {
            targetChunk = chunk;
            targetBlock = blockPosition;
            crackEffect.Show(chunk, blockPosition);
            destroyProgress = 0f;
        }

        // Разрушаем только при зажатой ЛКМ.
        if (Input.GetMouseButton(0))
        {
            destroyProgress += Time.deltaTime;

            if (destroyProgress >= destroyTime)
            {
                chunk.SetBlock(
                    blockPosition.x,
                    blockPosition.y,
                    blockPosition.z,
                    BlockType.Air
                );

                ResetMining();
            }

            float progress = Mathf.Clamp01(
    destroyProgress / destroyTime
);

            int stage = Mathf.FloorToInt(
                progress * crackEffect.crackStages.Length
            );

            stage = Mathf.Clamp(
                stage,
                0,
                crackEffect.crackStages.Length - 1
            );

            crackEffect.SetStage(stage);
        }
        else
        {
            destroyProgress = 0f;
            crackEffect.SetStage(-1);
        }
    }

    private void ResetMining()
    {
        targetChunk = null;
        destroyProgress = 0f;
        outline.Hide();
        if (crackEffect != null)
            crackEffect.Hide();
    }

    public float GetProgress()
    {
        if (destroyTime <= 0f)
            return 0f;

        return Mathf.Clamp01(destroyProgress / destroyTime);
    }
}