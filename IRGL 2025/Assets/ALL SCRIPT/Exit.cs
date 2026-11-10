using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    //DEKLARASI VARIABLE DALAH SATUNYA UI KELUAR
    public GameObject Keluar;
    public bool isTrigger = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Keluar == null)
        {
            Debug.LogError("Exit: GameObject Keluar belum di-assign di Inspector!");
        }
    }

    // Update is called once per frame
    void Update()
    {

        //CHECK APAKAH SUDAH KETRIGGER (DI LOKASI YG PAS) DAN MENCET TOMBOL E?
        if (Input.GetKeyDown(KeyCode.E) && isTrigger || Input.GetKeyDown(KeyCode.Escape) && isTrigger)
        {
            PlayerPrefs.SetInt("Kembali", 0);
            SceneManager.LoadScene("SampleScene");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Keluar != null)
        {
            Keluar.SetActive(true);
        }
        isTrigger = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (Keluar != null)
        {
            Keluar.SetActive(false);
            isTrigger = false;
        }
    }
}
