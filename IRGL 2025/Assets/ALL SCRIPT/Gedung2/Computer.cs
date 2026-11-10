using TMPro;
using UnityEngine;
using UnityEngine.UI;
// using static System.Net.Mime.MediaTypeNames; // Tidak perlu jika tidak digunakan

public class Computer : MonoBehaviour
{
    // Deklarasi Public Variables (pastikan semua di-assign di Inspector!)
    public GameObject ComputerUI;
    public GameObject interactUI;
    public TMP_InputField inputField;
    public GameObject TandaSeru;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // === PENCEGAHAN ERROR: Pengecekan Referensi Null pada Start ===
        // Memastikan semua komponen penting terhubung. Jika ada yang null, script berhenti/menonaktifkan diri.

        if (inputField == null)
        {
            Debug.LogError("FATAL ERROR: TMP_InputField (inputField) belum terhubung di Inspector! Script dinonaktifkan.");
            this.enabled = false;
            return;
        }
        if (ComputerUI == null || interactUI == null || TandaSeru == null)
        {
            Debug.LogError("ERROR: Salah satu Game Object (ComputerUI, interactUI, atau TandaSeru) belum terhubung di Inspector!");
            // Tidak menghentikan script, tapi akan ada NullRef jika dipanggil.
        }

        // Jika semua lolos, atur character limit
        inputField.characterLimit = 6;
    }

    // Update is called once per frame
    void Update()
    {
        // Kosong, tidak ada perubahan
    }

    public void enterButton(TMP_InputField text)
    {
        // Pencegahan Null Reference jika lupa menghubungkan inputField/text
        if (text == null)
        {
            Debug.LogError("Error: TMP_InputField yang dikirim ke enterButton adalah NULL.");
            return;
        }

        string hasil = text.text;

        // === PERINGATAN: Peringatan jika input kosong ===
        if (string.IsNullOrWhiteSpace(hasil))
        {
            // Memberikan peringatan ke konsol (tidak crash)
            Debug.LogWarning("Peringatan: Input kosong! Masukkan kode.");
            text.text = "******";
            return; // Keluar dari fungsi
        }

        // Logika Pengecekan Kode
        if (hasil.Trim().ToUpper() == "IRGL25")
        {
            // Sukses
            if (ComputerUI != null) ComputerUI.SetActive(false);

            // === PENCEGAHAN ERROR: Pengecekan Overall.instance ===
            if (Overall.instance != null)
            {
                Overall.instance.IncreaseScore(1000);
                Overall.instance.Puzzle2 = true;
                Debug.Log("Kode Benar! Skor diperbarui dan Puzzle 2 selesai.");
            }
            else
            {
                Debug.LogError("ERROR: Overall.instance tidak ditemukan! Skor dan status puzzle TIDAK diperbarui.");
            }

            if (interactUI != null) interactUI.SetActive(false);
            if (TandaSeru != null) TandaSeru.SetActive(false);
        }
        else
        {
            // === PERINGATAN: Peringatan jika kode salah ===
            Debug.LogWarning($"Kode yang dimasukkan '{hasil.Trim()}' salah. Coba lagi.");
            text.text = "******";
        }
    }

    public void exitButton()
    {
        if (ComputerUI != null)
        {
            ComputerUI.SetActive(false);
        }
        if (inputField != null)
        {
            inputField.text = "******";
        }
    }

    public void onSelect()
    {
        if (inputField != null)
        {
            inputField.text = "";
        }
    }
}