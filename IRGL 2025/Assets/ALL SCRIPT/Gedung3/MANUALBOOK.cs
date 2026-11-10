using UnityEngine;

public class MANUALBOOK : MonoBehaviour
{
    // Tambahkan variabel public ini untuk menampung referensi ke ManualBook Canvas
    public GameObject manualBookCanvas; // <--- Variabel REFERENSI

    void Start()
    {
        // Gunakan variabel referensi untuk set aktif/nonaktif
        if (manualBookCanvas != null)
        {
            manualBookCanvas.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleManualBook();
        }
    }

    public void ToggleManualBook()
    {
        if (manualBookCanvas == null)
        {
            Debug.LogError("ManualBook Canvas belum di-assign di Inspector!");
            return;
        }

        bool isCurrentlyActive = manualBookCanvas.activeSelf;
        bool isBecomingActive = !isCurrentlyActive;

        // Atur status aktif pada objek yang di-assign
        manualBookCanvas.SetActive(isBecomingActive); // <--- Gunakan variabel referensi
    }
}