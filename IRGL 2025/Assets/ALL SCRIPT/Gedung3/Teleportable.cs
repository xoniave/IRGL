using System.Collections;
using UnityEngine;

public class Teleportable : MonoBehaviour
{
    public bool canTeleport = true;

    // Panggil ini setelah objek berteleportasi
    public void DidTeleport()
    {
        canTeleport = false;
        StartCoroutine(ResetTeleportStatus());
    }

    // Coroutine untuk mengaktifkan kembali status teleport setelah jeda
    private IEnumerator ResetTeleportStatus()
    {
        yield return new WaitForSeconds(0.5f); // Jeda setengah detik
        canTeleport = true;
    }
}