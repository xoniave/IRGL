using UnityEngine;

public class BlockInteractive : MonoBehaviour
{
    //ini PBO BIASA ENUM CLASS NAMANYUA BLOCKTYPE
    public enum BlockType { LuckyBox, Brick }
    public BlockType tipeBlok; //INI BIKIN CLASS BLIOCKTYPE DENGAN NAMA TIPEBLOK

    //INI UNTUK LUCKY BOX
    public Sprite kotakKosongSprite; // Gambar kotak setelah dipukul
    public GameObject koinPrefab;    // Gammbar Koin
    private bool sudahDipukul = false;

    // Fungsi otomatis Unity saat ada objek lain menabrak blok ini
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Pastikan yang menabrak adalah Player
        if (collision.gameObject.CompareTag("Player"))
        {
            
            // Cek arah tabrakan: normal.y mendekati -1 berarti ditabrak dari BAWAH
            Vector2 arahTabrakan = collision.contacts[0].normal;
            // jadi contacts[0] ini adalah titik pertama kali player sentuh colider box
            // kalau .normal ini setiap box ada nilai kana/kiri itu x atas bawah itu y pakai nilai 1 dan -1
            Debug.Log(arahTabrakan.y);
            if (arahTabrakan.y > 0.5f)
            {
                EksekusiSundulan();
            }
        }
    }

    void EksekusiSundulan()
    {
        if (tipeBlok == BlockType.LuckyBox)
        {
            if (!sudahDipukul)
            {
                sudahDipukul = true;
                Debug.Log("Dapat Koin!");

                // 1. Ubah gambar kotak jadi kotak kosong jika ada sprite-nya
                GetComponent<SpriteRenderer>().sprite = kotakKosongSprite;

                // 2. Munculkan koin di atas kotak jika ada objek koinnya
                if (koinPrefab != null)
                {
                    Vector3 posisiKoin = transform.position + new Vector3(0, 1, 0);
                    GameObject koinBaru = Instantiate(koinPrefab, posisiKoin, Quaternion.identity);
                    Destroy(koinBaru, 0.5f); // Hancurkan koin dalam 0.5 detik (efek visual saja)
                }
            }
        }
        else if (tipeBlok == BlockType.Brick)
        {
            Debug.Log("Batu Hancur!");
            // Efek batu hancur: langsung hilangkan objek batu dari game
            Destroy(gameObject);
        }
    }
}