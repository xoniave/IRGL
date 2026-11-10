// GridManager.cs (VERSI LENGKAP DENGAN FITUR RESET FINISH)
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GridManager : MonoBehaviour
{
    public GameObject gridCellPrefab;
    public GameObject pushKananPrefab;
    public GameObject pushAtasPrefab;
    public GameObject blockPrefab;

    // VARIABEL PORTAL
    public GameObject portalPrefab;
    public GameObject portalExitUpPrefab;
    public GameObject portalExitDownPrefab;
    public GameObject portalExitLeftPrefab;
    public GameObject portalExitRightPrefab;

    public GameObject rightRotationPrefab;
    public GameObject obstaclePrefab;
    public GameObject finishPrefab;

    public int gridWidth = 14;
    public int gridHeight = 12;
    public float cellSize = 1f;

    public int puzzleID = 3;
    private bool puzzleCompleted = false;

    // ✨ DITAMBAHKAN: Struktur untuk menyimpan data finish
    private struct FinishData
    {
        public Vector3 position;
        public string name;
    }

    void Update()
    {
        // Hanya jalankan jika puzzle belum ditandai selesai untuk efisiensi
        if (!puzzleCompleted)
        {
            // Cari semua GameObject dengan tag "Finish" dan "Enemy"
            GameObject[] finishObjects = GameObject.FindGameObjectsWithTag("Finish");
            GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");

            // Jika tidak ada lagi objek "Finish" DAN "Enemy" di scene...
            if (finishObjects.Length == 0 && enemyObjects.Length == 0)
            {
                // 1. Tandai puzzle selesai agar kode ini tidak berjalan lagi
                puzzleCompleted = true;
                Debug.Log("Puzzle " + puzzleID + " selesai!");

                // 2. Cek apakah skrip Overall.cs ada (dari pembahasan kita sebelumnya)
                if (Overall.instance != null)
                {
                    // Update status puzzle di skrip Overall berdasarkan ID
                    switch (puzzleID)
                    {
                        case 1:
                            Overall.instance.Puzzle1 = true;
                            break;
                        case 2:
                            Overall.instance.Puzzle2 = true;
                            break;
                        case 3:
                            Overall.instance.Puzzle3 = true;
                            break;
                    }

                    Overall.instance.IncreaseScore(2500);

                    if(Overall.instance.Puzzle1 == true && Overall.instance.Puzzle2 == true && Overall.instance.Puzzle3 == true)
                    {
                        SceneManager.LoadScene("Finish");
                    }
                    else
                    {
                        SceneManager.LoadScene("SampleScene");
                    }

                        
                }
            }
        }
    }

    // ✨ DITAMBAHKAN: List untuk menyimpan semua data Finish yang dibuat saat Start
    private List<FinishData> initialFinishPositions = new List<FinishData>();

    // KONSTANTA BATAS DRAG AND DROP
    private const int MAX_DRAG_COLUMN = 4;
    private const int MAX_DRAG_ROW = 11;

    private GameObject[,] gridCells;

    void Start()
    {
        CreateGrid();
        SetCameraPosition();
        CreateLevelElements();
    }

    void CreateGrid()
    {
        gridCells = new GameObject[gridWidth, gridHeight];
        Color greyColor = new Color(0.5f, 0.5f, 0.5f, 1f);

        for (int x = 0; x < gridWidth; x++)
            for (int y = 0; y < gridHeight; y++)
            {
                Vector3 position = new Vector3(x * cellSize, y * cellSize, 0);
                GameObject newCell = Instantiate(gridCellPrefab, position, Quaternion.identity);
                newCell.transform.parent = this.transform;
                gridCells[x, y] = newCell;

                // LOGIKA PEWARNAAN BATAS DRAG & DROP
                if (x > MAX_DRAG_COLUMN || y > MAX_DRAG_ROW)
                {
                    SpriteRenderer renderer = newCell.GetComponent<SpriteRenderer>();
                    if (renderer != null)
                    {
                        renderer.color = greyColor;
                    }
                }
            }
    }

    void CreateLevelElements()
    {
        // 1. Pemain & Blocks
        CreatePrefab(pushAtasPrefab, new Vector3(1, 1, -0.1f), "Push Atas 1");
        CreatePrefab(pushAtasPrefab, new Vector3(3, 2, -0.1f), "Push Atas ");
        CreatePrefab(pushAtasPrefab, new Vector3(3, 10, -0.1f), "Push Atas 3");
        CreatePrefab(pushAtasPrefab, new Vector3(2, 5, -0.1f), "Push Atas 4");
        CreatePrefab(pushAtasPrefab, new Vector3(1, 6, -0.1f), "Push Atas 5");

        CreatePrefab(blockPrefab, new Vector3(2, 1, -0.1f), "BlockA");


        // 2. Obstacle Target
        CreatePrefab(obstaclePrefab, new Vector3(1, 8, -0.1f), "Obstacle_Wall1");
        CreatePrefab(obstaclePrefab, new Vector3(3, 4, -0.1f), "Obstacle_Wall2");


        // 3. Portal Utama
        GameObject portalX = CreatePrefab(portalExitUpPrefab, new Vector3(4, 1, -0.1f), "PortalX_In");
        GameObject portalY = CreatePrefab(portalExitUpPrefab, new Vector3(1, 10, -0.1f), "PortalY_Out");
        LinkPortals(portalX, portalY);

        // 4. Portal Kedua
        GameObject portalZ = CreatePrefab(portalExitDownPrefab, new Vector3(13, 10, -0.1f), "PortalZ_In");
        GameObject portalW = CreatePrefab(portalExitDownPrefab, new Vector3(9, 3, -0.1f), "PortalW_Out");
        LinkPortals(portalZ, portalW);


        // 5. Rotasi
        CreatePrefab(rightRotationPrefab, new Vector3(13, 11, -0.1f), "Rotation2_Left");
        CreatePrefab(rightRotationPrefab, new Vector3(0, 11, -0.1f), "Rotation3_Down");

        // Finish/Goal
        CreatePrefab(finishPrefab, new Vector3(12, 2, -0.1f), "Goal_1");
        CreatePrefab(finishPrefab, new Vector3(13, 7, -0.1f), "Goal_2");
        CreatePrefab(finishPrefab, new Vector3(13, 6, -0.1f), "Goal_3");
        CreatePrefab(finishPrefab, new Vector3(13, 5, -0.1f), "Goal_4");
    }

    void SetCameraPosition()
    {
        float centerX = (gridWidth - 1) * 0.5f;
        float centerY = (gridHeight - 1) * 0.5f;

        Camera mainCamera = Camera.main;

        if (mainCamera != null)
        {
            mainCamera.transform.position = new Vector3(centerX * cellSize, centerY * cellSize, -10f);

            if (mainCamera.orthographic)
            {
                float camSizeX = gridWidth * cellSize * 0.5f;
                float camSizeY = gridHeight * cellSize * 0.5f;
                mainCamera.orthographicSize = Mathf.Max(camSizeX * Screen.height / Screen.width, camSizeY) + 1f;
            }
        }
    }

    private GameObject CreatePrefab(GameObject prefab, Vector3 position, string name)
    {
        if (prefab != null)
        {
            GameObject newItem = Instantiate(prefab, position, Quaternion.identity);
            newItem.name = name;

            // LOGIKA TAGGING & PENYIMPANAN FINISH
            if (name.Contains("Goal"))
            {
                newItem.tag = "Finish";

                // ✨ DITAMBAHKAN: Simpan data awal objek Finish
                FinishData data = new FinishData
                {
                    position = position,
                    name = name
                };
                initialFinishPositions.Add(data);
            }

            return newItem;
        }
        else
        {
            Debug.LogError($"{name} Prefab tidak terhubung di Inspector GridManager!");
            return null;
        }
    }

    private void LinkPortals(GameObject p1, GameObject p2)
    {
        if (p1 != null && p2 != null)
        {
            Portal p1_script = p1.GetComponent<Portal>();
            Portal p2_script = p2.GetComponent<Portal>();

            if (p1_script != null && p2_script != null)
            {
                p1_script.destinationPortal = p2.transform;
                p2_script.destinationPortal = p1.transform;
                Debug.Log($"Portals {p1.name} and {p2.name} successfully linked!");
            }
            else
            {
                Debug.LogError("One or both portals are missing the Portal script.");
            }
        }
        else
        {
            Debug.LogError("Could not find one or both portals to link them.");
        }
    }

    // ✨ DITAMBAHKAN: Metode publik untuk membuat ulang semua objek Finish
    public void ResetAllFinishObjects()
    {
        // 1. Hapus semua objek Finish yang masih ada di scene
        GameObject[] currentFinishes = GameObject.FindGameObjectsWithTag("Finish");
        foreach (GameObject finish in currentFinishes)
        {
            Destroy(finish);
        }

        // 2. Buat ulang objek Finish dari data yang tersimpan
        foreach (var data in initialFinishPositions)
        {
            GameObject newFinish = Instantiate(finishPrefab, data.position, Quaternion.identity);
            newFinish.name = data.name;
            newFinish.tag = "Finish";
        }
        Debug.Log("Semua Block Finish telah dikembalikan.");
    }
}