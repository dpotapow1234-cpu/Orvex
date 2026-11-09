using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class BlockOutline : MonoBehaviour
{
    [SerializeField]private LineRenderer line;

    private static readonly Vector3[] Corners =
    {
        new Vector3(0, 0, 0),
        new Vector3(1, 0, 0),
        new Vector3(1, 0, 1),
        new Vector3(0, 0, 1),
        new Vector3(0, 0, 0),

        new Vector3(0, 1, 0),
        new Vector3(1, 1, 0),
        new Vector3(1, 0, 0),

        new Vector3(1, 1, 0),
        new Vector3(1, 1, 1),
        new Vector3(1, 0, 1),

        new Vector3(1, 1, 1),
        new Vector3(0, 1, 1),
        new Vector3(0, 0, 1),

        new Vector3(0, 1, 1),
        new Vector3(0, 1, 0)
    };

    private void Awake()
    {
        line = GetComponent<LineRenderer>();

        line.positionCount = Corners.Length;
        line.useWorldSpace = true;
        line.loop = false;
        line.startWidth = 0.025f;
        line.endWidth = 0.025f;

        line.material = new Material(Shader.Find("Sprites/Default"));
        line.startColor = Color.black;
        line.endColor = Color.black;

        line.enabled = false;
    }

    public void Show(Chunk chunk, Vector3Int blockPosition)
    {
        if (line == null)
            return;

        line.positionCount = Corners.Length;

        Vector3 offset = new Vector3(
            blockPosition.x,
            blockPosition.y,
            blockPosition.z
        );

        for (int i = 0; i < Corners.Length; i++)
        {
            Vector3 local = offset + Corners[i];

            line.SetPosition(
                i,
                chunk.transform.TransformPoint(local)
            );
        }

        line.enabled = true;
    }
    public void Hide()
    {
        line.enabled = false;
    }
}