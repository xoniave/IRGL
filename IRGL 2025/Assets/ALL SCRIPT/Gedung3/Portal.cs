using UnityEngine;

public class Portal : MonoBehaviour
{
    // Variabel untuk menyimpan referensi ke portal tujuan
    public Transform destinationPortal;

    // Variabel baru untuk memilih arah keluar portal
    public int exitDirectionChoice;

    // Fungsi yang menerjemahkan angka ke Vector3
    public Vector3 GetExitDirection()
    {
        switch (exitDirectionChoice)
        {
            case 1:
                return Vector3.left;
            case 2:
                return Vector3.right;
            case 3:
                return Vector3.up;
            case 4:
                return Vector3.down;
            default:
                Debug.LogWarning("Pilihan arah keluar portal tidak valid. Menggunakan arah default (kanan).");
                return Vector3.right;
        }
    }
}