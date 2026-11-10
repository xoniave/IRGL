using System.Collections;
using UnityEngine;

public class PushableObject : MonoBehaviour
{
    private GridManager gridManager;
    private bool isMoving = false;


    void Start()
    {
        gridManager = FindAnyObjectByType<GridManager>();
        if (gridManager == null)
        {
            Debug.LogError("GridManager tidak ditemukan!");
        }
    }

    public void BePushed(Vector3 direction)
    {
        if (isMoving) return;
        StartCoroutine(MoveToPosition(transform.position + direction));
    }

    private IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        isMoving = true;
        float moveSpeed = 3.5f;

        if (IsValidPosition(targetPosition))
        {
            Vector3 startPos = transform.position;
            float elapsedTime = 0;

            while (elapsedTime < 1.0f)
            {
                transform.position = Vector3.Lerp(startPos, targetPosition, elapsedTime / 1.0f);
                elapsedTime += Time.deltaTime * moveSpeed;
                yield return null;
            }
            transform.position = targetPosition;
        }

        isMoving = false;
    }

    public bool IsValidPosition(Vector3 position)
    {
        if (gridManager == null) return false;

        float minX = 0f;
        float minY = 0f;
        float maxX = (gridManager.gridWidth - 1) * gridManager.cellSize;
        float maxY = (gridManager.gridHeight - 1) * gridManager.cellSize;

        // Memeriksa apakah posisi berada di dalam grid
        return position.x >= minX && position.x <= maxX && position.y >= minY && position.y <= maxY;
    }
}