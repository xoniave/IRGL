using UnityEngine;
using UnityEngine.SceneManagement; // Wajib ditambahkan untuk urusan Scene

public class GameOver : MonoBehaviour
{
    // Fungsi ini HARUS public agar bisa dilihat oleh tombol UI
    public void UlangiPermainan()
    {
        // Ganti "Level1" dengan nama persis scene utama game Anda
        SceneManager.LoadScene("SampleScene");
    }
}