using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 7f;

    // Variabel ground check jadi sesimpel ini saja
    public bool isGrounded;

    private float moveInput;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() //update tiap fram kalau 60fps --> 60x dalam 1 detik
    {
        // 1. Bergerak Kanan & Kiri dari inputan dia detect a/d/panah kiri kanan (-1,0,1)
        moveInput = Input.GetAxisRaw("Horizontal");
        rb.linearVelocityX = moveInput * speed;

        // 2. Logika Melompat
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocityY = jumpForce;
        }
    }

    // --- CARA SIMPEL CEK TANAH ---

    // Fungsi ini otomatis terpanggil saat karakter MENABRAK sesuatu
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Mengecek apakah yang ditabrak punya Tag "Ground"
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    // Fungsi ini otomatis terpanggil saat karakter LEPAS dari tabrakan
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}