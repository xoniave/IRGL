using UnityEngine;

public class DragDropHandler : MonoBehaviour
{
    private Vector3 offset;
    private GridManager gridManager;
    private Vector3 initialPosition;
    private Vector3 lastDraggedPosition;

    // Variabel statis baru
    public static bool isDragEnabled = true;

    // KONSTANTA BARU: Batasan area drag and drop (kolom 0-4, baris 0-11)
    private const int MAX_DRAG_COLUMN = 4; // Max index kolom yang diizinkan (x=4)
    private const int MAX_DRAG_ROW = 11; // Max index baris yang diizinkan (y=11)

    void Start()
    {
        gridManager = FindAnyObjectByType<GridManager>();
        if (gridManager == null)
        {
            Debug.LogError("GridManager tidak ditemukan!");
        }
        else
        {
            initialPosition = transform.position;
            lastDraggedPosition = initialPosition;
            CheckAndDisableDrag(); // Cek dan nonaktifkan/warnai jika di luar batas
        }
    }

    // LOGIKA BARU: Cek posisi awal dan nonaktifkan/warnai jika di luar batas
    private void CheckAndDisableDrag()
    {
        if (gridManager == null) return;

        // Posisi maksimum yang valid (misal, jika cellSize=1f, max X adalah 4.0f dan max Y adalah 11.0f)
        float maxAllowedX = MAX_DRAG_COLUMN * gridManager.cellSize + 0.001f; // Tambahkan sedikit offset untuk float comparison
        float maxAllowedY = MAX_DRAG_ROW * gridManager.cellSize + 0.001f;

        // Periksa apakah posisi awal berada di luar zona yang diizinkan
        if (transform.position.x > maxAllowedX ||
            transform.position.x < 0f ||
            transform.position.y > maxAllowedY ||
            transform.position.y < 0f)
        {
            // Nonaktifkan skrip ini (Drag and Drop tidak bisa dilakukan)
            this.enabled = false;

        }
    }

    void OnMouseDown()
    {
        // Periksa apakah drag diizinkan
        if (!isDragEnabled || !this.enabled) return;
        offset = transform.position - GetMouseWorldPosition();
    }

    void OnMouseDrag()
    {
        // Periksa apakah drag diizinkan
        if (!isDragEnabled || !this.enabled) return;
        if (gridManager == null) return;
        Vector3 newPosition = GetMouseWorldPosition() + offset;

        // BATASAN AREA DRAG: Kolom 0-4 (X=0 hingga X=4) dan Baris 0-11 (Y=0 hingga Y=11)
        float minX = 0f;
        float minY = 0f;
        float maxX = MAX_DRAG_COLUMN * gridManager.cellSize;
        float maxY = MAX_DRAG_ROW * gridManager.cellSize;

        // Clamp posisi baru ke dalam batas yang ditentukan
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);
        transform.position = newPosition;
    }

    void OnMouseUp()
    {
        // Periksa apakah drag diizinkan
        if (!isDragEnabled || !this.enabled) return;
        if (gridManager == null) return;

        // Pembulatan ke posisi grid terdekat
        float roundedX = Mathf.Round(transform.position.x / gridManager.cellSize) * gridManager.cellSize;
        float roundedY = Mathf.Round(transform.position.y / gridManager.cellSize) * gridManager.cellSize;
        Vector3 targetPosition = new Vector3(roundedX, roundedY, transform.position.z);

        // BATASAN VALIDASI AKHIR: Memastikan posisi *final* tidak keluar dari batas 
        float minX = 0f;
        float minY = 0f;
        float maxX = MAX_DRAG_COLUMN * gridManager.cellSize;
        float maxY = MAX_DRAG_ROW * gridManager.cellSize;

        if (targetPosition.x < minX || targetPosition.x > maxX ||
            targetPosition.y < minY || targetPosition.y > maxY)
        {
            // Jika posisi target di luar batas, kembalikan ke posisi terakhir yang valid
            transform.position = lastDraggedPosition;
            return;
        }

        // Cek apakah posisi sudah ditempati
        if (IsPositionOccupied(targetPosition))
        {
            transform.position = lastDraggedPosition;
        }
        else
        {
            lastDraggedPosition = targetPosition;
            transform.position = lastDraggedPosition;
        }
    }

    public void ResetPositionToLastDrag()
    {
        transform.position = lastDraggedPosition;
    }

    private bool IsPositionOccupied(Vector3 position)
    {
        Collider2D[] hitColliders = Physics2D.OverlapPointAll(position);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject != this.gameObject && !hitCollider.gameObject.CompareTag("GridCell"))
            {
                // ✨ LOGIKA BARU: Hancurkan Finish, lalu anggap posisi tidak terhambat olehnya
                if (hitCollider.gameObject.CompareTag("Finish"))
                {
                    Destroy(hitCollider.gameObject);
                    continue; // Lanjutkan loop untuk memeriksa tabrakan lain
                }
                // ✨ AKHIR LOGIKA BARU

                return true;
            }
        }
        return false;
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = -Camera.main.transform.position.z;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
}