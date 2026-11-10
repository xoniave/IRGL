// UiManager.cs (VERSI LENGKAP DENGAN FITUR RESET FINISH)
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public Button moveButton;
    public Button stopButton;
    private GridManager gridManager;

    void Start()
    {
        // ✨ DITAMBAHKAN: Dapatkan referensi GridManager saat Start
        gridManager = FindAnyObjectByType<GridManager>();

        SetDraggable(true);

        if (moveButton != null)
        {
            moveButton.onClick.AddListener(OnMoveButtonClick);
        }
        if (stopButton != null)
        {
            stopButton.onClick.AddListener(OnStopButtonClick);
        }

        stopButton.interactable = false;
    }

    void OnMoveButtonClick()
    {
        SetDraggable(false);

        AutoMove[] allMovers = FindObjectsOfType<AutoMove>();
        foreach (AutoMove mover in allMovers)
        {
            if (mover.gameObject.name.Contains("Push Kanan"))
            {
                mover.StartAutoMove(Vector3.right);
            }
            else if (mover.gameObject.name.Contains("Push Atas"))
            {
                mover.StartAutoMove(Vector3.up);
            }
        }

        stopButton.interactable = true;
        moveButton.interactable = false;
    }

    void OnStopButtonClick()
    {
        AutoMove[] allMovers = FindObjectsOfType<AutoMove>();
        foreach (AutoMove mover in allMovers)
        {
            mover.StopAutoMove();
        }

        SetDraggable(true);
        ResetAllPositions();

        // LOGIKA RESET FINISH
        if (gridManager != null)
        {
            gridManager.ResetAllFinishObjects();
        }

        moveButton.interactable = true;
        stopButton.interactable = false;
    }

    void SetDraggable(bool isDraggable)
    {
        DragDropHandler.isDragEnabled = isDraggable;
    }

    private IEnumerator ForceResetLoop()
    {
        float duration = 0.5f;
        float timer = 0f;

        while (timer < duration)
        {
            DragDropHandler[] allDraggableItems = FindObjectsByType<DragDropHandler>(FindObjectsSortMode.None);
            foreach (DragDropHandler item in allDraggableItems)
            {
                item.ResetPositionToLastDrag();
            }

            AutoMove[] allMovers = FindObjectsByType<AutoMove>(FindObjectsSortMode.None);
            foreach (AutoMove mover in allMovers)
            {
                mover.transform.rotation = Quaternion.identity;
            }

            timer += Time.deltaTime;
            yield return null;
        }
    }

    private void ResetAllPositions()
    {
        StartCoroutine(ForceResetLoop());
    }
}