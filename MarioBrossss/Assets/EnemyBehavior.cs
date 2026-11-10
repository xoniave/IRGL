using UnityEngine;
using UnityEngine.SceneManagement; // Wajib ditambahkan untuk fungsi mengulang/Restart level

public class EnemyBehavior : MonoBehaviour
{
    [Header("Pengaturan Patroli")]
    public float speed = 3f;
    public float jarakPatroli = 5f;

    private float batasKiri;
    private float batasKanan;
    private int arahGerak = 1; // 1 untuk Kanan, -1 untuk Kiri

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Mengingat posisi awal musuh saat game dimulai
        float posisiAwalX = transform.position.x;

        // Menentukan titik mentok kiri dan kanan (Posisi awal +/- 5)
        batasKiri = posisiAwalX - jarakPatroli;
        batasKanan = posisiAwalX + jarakPatroli;
    }

    void Update()
    {
        // 1. Menggerakkan musuh
        rb.linearVelocityX = speed * arahGerak;

        // 2. Cek apakah musuh sudah mentok di batas kanan atau kiri
        if (transform.position.x >= batasKanan)
        {
            arahGerak = -1; // Putar balik ke kiri
        }
        else if (transform.position.x <= batasKiri)
        {
            arahGerak = 1; // Putar balik ke kanan
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Cek apakah yang bersentuhan adalah Player
        if (collision.gameObject.CompareTag("Player"))
        {
            // ini sama kayak sebelumnya! Ini arah dorongan tabrakan
            Vector2 arahTabrakan = collision.contacts[0].normal;

            // Jika Player berada di ATAS dan Musuh di BAWAH, 
            // dorongan energi bergerak dari atas ke BAWAH (Y menjadi Minus)
            if (arahTabrakan.y < -0.5f)
            {
                Debug.Log("Musuh Mati Diinjak!");
                // Hancurkan musuh
                Destroy(gameObject);
            }
            else
            {
                // Jika ditabrak dari samping (Y = 0) atau musuh jatuh menimpa Player (Y = 1)
                Debug.Log("Player Mati! Mengulang Level...");
                SceneManager.LoadScene("GameOver");
            }
        }
    }
}