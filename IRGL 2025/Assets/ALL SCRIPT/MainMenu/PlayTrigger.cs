using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayTrigger : MonoBehaviour
{
    // Pastikan PanelButton ini terhubung di Inspector
    public GameObject PanelButton;

    void Start()
    {
        // Pengecekan untuk memastikan PanelButton sudah terhubung
        if (PanelButton == null)
        {
            Debug.LogError("PlayTrigger: PanelButton belum di-assign di Inspector! Fungsi OnMouseOver/Exit tidak akan bekerja.");
            // Script tetap berjalan, tapi tanpa efek hover UI.
        }
    }

    private void OnMouseDown()
    {
        // Pastikan nama scene benar
        SceneManager.LoadScene("SampleScene");
    }

    private void OnMouseOver()
    {
        // Pengecekan Null
        if (PanelButton != null)
        {
            PanelButton.SetActive(true);
        }
    }

    private void OnMouseExit()
    {
        // Pengecekan Null
        if (PanelButton != null)
        {
            PanelButton.SetActive(false);
        }
    }
}