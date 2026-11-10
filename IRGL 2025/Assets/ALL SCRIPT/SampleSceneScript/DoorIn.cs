using UnityEngine;
using UnityEngine.SceneManagement;

public class Door2 : MonoBehaviour
{
    // Pastikan ini terhubung di Inspector
    public GameObject E_Door2;
    public bool isTrigger;
    public int pintu; // 1 = Gedung1, 2 = Gedung2, 3 = Gedung3

    // Metode bantuan untuk memeriksa apakah puzzle terkait sudah selesai
    private bool IsPuzzleDone()
    {
        // Pengecekan awal untuk mencegah NullReferenceException
        if (Overall.instance == null)
        {
            // Peringatan di Editor jika Overall.instance hilang
            Debug.LogError("Overall.instance is NULL! Cannot check puzzle status.");
            return false; // Anggap belum selesai atau tidak bisa diakses
        }

        switch (pintu)
        {
            case 1:
                return Overall.instance.Puzzle1;
            case 2:
                return Overall.instance.Puzzle2;
            case 3:
                return Overall.instance.Puzzle3;
            default:
                // Jika pintu tidak ada dalam daftar puzzle (pintu lain), anggap SELALU bisa masuk
                return false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Cek input, trigger, dan status puzzle
        if (Input.GetKeyDown(KeyCode.E) && isTrigger)
        {
            if (IsPuzzleDone())
            {
                // Kasus A: Puzzle sudah selesai. Tampilkan peringatan atau kunci aksi.
                Debug.LogWarning($"Pintu {pintu}: Puzzle sudah selesai! Tidak bisa masuk lagi.");

                // Tambahan: Matikan E_Door2 dan isTrigger jika ternyata masih aktif
                if (E_Door2 != null && E_Door2.activeSelf)
                {
                    E_Door2.SetActive(false);
                }
                isTrigger = false;
                return; // Hentikan pemrosesan
            }

            // Kasus B: Puzzle BELUM selesai. Lakukan aksi masuk.

            // SIMPAN DATA TERAKHIR KALI
            PlayerPrefs.SetFloat("PlayerX", transform.position.x);
            PlayerPrefs.SetFloat("PlayerY", transform.position.y);
            PlayerPrefs.SetInt("Kembali", 1);
            PlayerPrefs.Save();

            // LOAD KE DALEM RUANGAN
            string sceneToLoad = "";
            if (pintu == 1)
            {
                sceneToLoad = "Gedung1";
            }
            else if (pintu == 2)
            {
                sceneToLoad = "Gedung2";
            }
            else if (pintu == 3)
            {
                sceneToLoad = "Gedung3";
            }

            if (!string.IsNullOrEmpty(sceneToLoad))
            {
                SceneManager.LoadScene(sceneToLoad);
            }
            else
            {
                Debug.LogError($"Pintu {pintu} tidak memiliki scene tujuan yang valid!");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Hanya tampilkan prompt 'E' jika: 
        // 1. Objek E_Door2 ada.
        // 2. Puzzle terkait BELUM selesai.
        if (E_Door2 != null && !IsPuzzleDone())
        {
            E_Door2.SetActive(true);
            isTrigger = true;
        }
        else if (E_Door2 != null && IsPuzzleDone())
        {
            // Sembunyikan prompt 'E' jika puzzle sudah selesai
            E_Door2.SetActive(false);
            isTrigger = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Pengecekan Null yang sudah ada, ini sudah benar
        if (E_Door2 != null)
        {
            E_Door2.SetActive(false);
            isTrigger = false;
        }
    }
}