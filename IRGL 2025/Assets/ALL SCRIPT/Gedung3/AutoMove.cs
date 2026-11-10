// AutoMove.cs (VERSI AKHIR YANG DIKOREKSI DENGAN LOGIKA HANCURKAN FINISH)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMove : MonoBehaviour
{
    private bool isMoving = false;
    private GridManager gridManager;
    private SkillManager skillManager;
    private Vector3 moveDirection;

    void Start()
    {
        gridManager = FindAnyObjectByType<GridManager>();
        skillManager = GetComponent<SkillManager>();

        if (gridManager == null)
        {
            Debug.LogError("GridManager tidak ditemukan!");
        }
        if (skillManager == null)
        {
            Debug.LogError("SkillManager tidak ditemukan pada objek ini!");
        }
    }

    public void StartAutoMove(Vector3 direction)
    {
        if (isMoving || gridManager == null) return;
        moveDirection = direction;
        StartCoroutine(MoveStepByStep());
    }

    public void StopAutoMove()
    {
        if (isMoving)
        {
            StopAllCoroutines();
            isMoving = false;
        }
    }

    private IEnumerator MoveStepByStep()
    {
        isMoving = true;
        float moveSpeed = 4f;
        float stepSize = gridManager.cellSize;
        Teleportable myTeleportable = GetComponent<Teleportable>();

        while (isMoving)
        {
            bool justRotated = false;

            // Pengecekan 4-arah di awal setiap langkah untuk RightRotation
            if (CheckForAdjacentRotation())
            {
                // Jika ada RightRotation, ubah arah pergerakan menjadi arah clockwise baru
                moveDirection = GetNextClockwiseDirection(moveDirection);
                transform.Rotate(0, 0, -90); // Rotasi visual
                justRotated = true;
            }


            // Pergerakan normal jika tidak ada RightRotation atau setelah rotasi
            Vector3 startPos = transform.position;
            Vector3 targetPosition = startPos + moveDirection * stepSize;
            GameObject objectInNextCell = GetObjectAtPosition(targetPosition);

            // ✨ LOGIKA BARU: Hancurkan Finish jika ada di posisi target
            if (objectInNextCell != null && objectInNextCell.CompareTag("Finish"))
            {
                Destroy(objectInNextCell);
                // Dapatkan objek lagi, sekarang harusnya null atau objek lain (non-Finish)
                objectInNextCell = GetObjectAtPosition(targetPosition);
            }
            // ✨ AKHIR LOGIKA BARU

            if (!IsWithinGrid(targetPosition))
            {
                if (justRotated)
                {
                    yield return new WaitForSeconds(0.4f);
                    continue;
                }

                StopAutoMove();
                yield break;
            }

            if (objectInNextCell != null)
            {
                Portal portal = objectInNextCell.GetComponent<Portal>();
                PushableObject pushable = objectInNextCell.GetComponent<PushableObject>();

                // Cek apakah objek di depan juga memiliki AutoMove
                AutoMove otherAutoMove = objectInNextCell.GetComponent<AutoMove>();


                // LOGIKA 1: Objek di depan adalah Portal
                if (portal != null)
                {
                    if (myTeleportable != null && !myTeleportable.canTeleport)
                    {
                        float elapsedTime = 0;
                        while (elapsedTime < 1.0f)
                        {
                            transform.position = Vector3.Lerp(startPos, targetPosition, elapsedTime / 1.0f);
                            elapsedTime += Time.deltaTime * moveSpeed;
                            yield return null;
                        }
                        transform.position = targetPosition;
                        yield return new WaitForSeconds(0.4f);
                        continue;
                    }

                    if (portal.destinationPortal != null && myTeleportable.canTeleport)
                    {
                        Vector3 exitPosition = portal.destinationPortal.position + portal.GetExitDirection() * stepSize;
                        GameObject objectAtExit = GetObjectAtPosition(exitPosition);

                        if (IsWithinGrid(exitPosition) && objectAtExit == null)
                        {
                            myTeleportable.DidTeleport();
                            transform.position = exitPosition;
                            yield return new WaitForSeconds(0.4f);
                            continue;
                        }
                        else if (IsWithinGrid(exitPosition) && objectAtExit != null && objectAtExit.GetComponent<PushableObject>() != null)
                        {
                            List<PushableObject> pushChainAtExit = GetPushableChain(exitPosition, portal.GetExitDirection());
                            Vector3 positionAfterChain = pushChainAtExit[pushChainAtExit.Count - 1].transform.position + portal.GetExitDirection() * stepSize;
                            GameObject objectAfterChain = GetObjectAtPosition(positionAfterChain);

                            if (IsWithinGrid(positionAfterChain) && objectAfterChain == null)
                            {
                                myTeleportable.DidTeleport();
                                transform.position = exitPosition;

                                for (int i = pushChainAtExit.Count - 1; i >= 0; i--)
                                {
                                    skillManager.Push(pushChainAtExit[i].gameObject, portal.GetExitDirection() * stepSize);
                                }
                                yield return new WaitForSeconds(0.4f);
                                continue;
                            }
                        }
                    }
                    StopAutoMove();
                    yield break;
                }
                // LOGIKA 2: Objek di depan adalah Block atau Pushable lain
                else if (pushable != null)
                {
                    // 🔴 PERBAIKAN BUG: Jika objek di depan adalah AutoMove (Push Kanan/Atas), ABAIKAN.
                    if (otherAutoMove != null)
                    {
                        // Jika objek di depan juga AutoMove, ABAIKAN dan Lanjutkan Loop (Continue)
                        yield return new WaitForSeconds(0.4f);
                        continue;
                    }

                    // Hanya lanjutkan ke logika rantai dorongan (push chain) jika objek di depan BUKAN AutoMove.
                    List<PushableObject> pushChain = GetPushableChain(targetPosition, moveDirection);
                    if (pushChain.Count > 0)
                    {
                        Vector3 positionAfterChain = pushChain[pushChain.Count - 1].transform.position + moveDirection * stepSize;
                        GameObject objectAfterChain = GetObjectAtPosition(positionAfterChain);

                        // ✨ FITUR BARU: Hancurkan Block Merah jika ada di posisi akhir rantai dorongan
                        if (objectAfterChain != null && objectAfterChain.CompareTag("Finish"))
                        {
                            Destroy(objectAfterChain); // Hancurkan Block Merah
                            objectAfterChain = null; // Set menjadi null agar dianggap kosong, sehingga blok bisa didorong
                        }
                        // ✨ AKHIR FITUR BARU

                        if (IsWithinGrid(positionAfterChain) && objectAfterChain == null)
                        {
                            for (int i = pushChain.Count - 1; i >= 0; i--)
                            {
                                skillManager.Push(pushChain[i].gameObject, moveDirection * stepSize);
                            }

                            Vector3 startPosPush = transform.position;
                            float elapsedTimePush = 0;
                            while (elapsedTimePush < 1.0f)
                            {
                                transform.position = Vector3.Lerp(startPosPush, targetPosition, elapsedTimePush / 1.0f);
                                elapsedTimePush += Time.deltaTime * moveSpeed;
                                yield return null;
                            }
                            transform.position = targetPosition;

                            yield return new WaitForSeconds(0.4f);
                            continue;
                        }
                        else if (IsWithinGrid(positionAfterChain) && objectAfterChain != null && objectAfterChain.GetComponent<Portal>() != null)
                        {
                            Portal blockPortal = objectAfterChain.GetComponent<Portal>();
                            Vector3 firstExitPos = blockPortal.destinationPortal.position + blockPortal.GetExitDirection() * stepSize;

                            List<PushableObject> combinedChain = new List<PushableObject>();
                            combinedChain.AddRange(pushChain);
                            combinedChain.AddRange(GetPushableChain(firstExitPos, blockPortal.GetExitDirection()));

                            bool isExitPathClear = true;
                            for (int i = 0; i < combinedChain.Count; i++)
                            {
                                Vector3 checkPos = firstExitPos + (blockPortal.GetExitDirection() * stepSize * (i));

                                // Cek apakah posisi tidak di dalam grid, atau ada objek di posisi yang BUKAN bagian dari combinedChain
                                GameObject objectAtCheckPos = GetObjectAtPosition(checkPos);
                                if (!IsWithinGrid(checkPos) || (objectAtCheckPos != null && !combinedChain.Contains(objectAtCheckPos.GetComponent<PushableObject>())))
                                {
                                    // ✨ Pengecekan tambahan untuk menghancurkan Block Merah di jalur keluar portal
                                    if (objectAtCheckPos != null && objectAtCheckPos.CompareTag("Enemy"))
                                    {
                                        Destroy(objectAtCheckPos);
                                        // Setelah dihancurkan, jalur dianggap clear untuk blok berikutnya
                                        continue;
                                    }

                                    isExitPathClear = false;
                                    break;
                                }
                            }

                            if (blockPortal.destinationPortal != null && isExitPathClear && myTeleportable.canTeleport)
                            {
                                for (int i = 0; i < combinedChain.Count; i++)
                                {
                                    PushableObject currentBlock = combinedChain[i];
                                    Teleportable blockTeleportable = currentBlock.GetComponent<Teleportable>();
                                    if (blockTeleportable == null || !blockTeleportable.canTeleport)
                                    {
                                        StopAutoMove();
                                        yield break;
                                    }

                                    Vector3 blockExitPos = firstExitPos + (blockPortal.GetExitDirection() * stepSize * i);
                                    blockTeleportable.DidTeleport();
                                    currentBlock.transform.position = blockExitPos;
                                }

                                myTeleportable.DidTeleport();
                                transform.position = blockPortal.destinationPortal.position;

                                yield return new WaitForSeconds(0.4f);
                                continue;
                            }
                            else
                            {
                                StopAutoMove();
                                yield break;
                            }
                        }
                        else
                        {
                            StopAutoMove();
                            yield break;
                        }
                    }
                }
                // LOGIKA 3: Objek di depan adalah penghalang lain
                else
                {
                    if (justRotated)
                    {
                        yield return new WaitForSeconds(0.4f);
                        continue;
                    }

                    StopAutoMove();
                    yield break;
                }
            }
            // Jika tidak ada objek di depan (termasuk kasus di mana objek di depan adalah AutoMove dan diabaikan), gerakkan PushKanan/PushAtas
            else
            {
                float elapsedTime = 0;
                while (elapsedTime < 1.0f)
                {
                    transform.position = Vector3.Lerp(startPos, targetPosition, elapsedTime / 1.0f);
                    elapsedTime += Time.deltaTime * moveSpeed;
                    yield return null;
                }
                transform.position = targetPosition;
                yield return new WaitForSeconds(0.4f);
            }
        }
    }

    private bool IsWithinGrid(Vector3 position)
    {
        if (gridManager == null) return false;

        float minX = 0f;
        float minY = 0f;
        float maxX = (gridManager.gridWidth - 1) * gridManager.cellSize;
        float maxY = (gridManager.gridHeight - 1) * gridManager.cellSize;

        return position.x >= minX && position.x <= maxX && position.y >= minY && position.y <= maxY;
    }

    private List<PushableObject> GetPushableChain(Vector3 startPosition, Vector3 direction)
    {
        var chain = new List<PushableObject>();
        var currentPosition = startPosition;
        float stepSize = gridManager.cellSize;

        while (true)
        {
            var objectAtPosition = GetObjectAtPosition(currentPosition);
            if (objectAtPosition == null) break;

            var pushable = objectAtPosition.GetComponent<PushableObject>();
            if (pushable == null) break;

            // ⚠️ PERBAIKAN UTAMA: Baris ini dihapus/dikomentari untuk memungkinkan AutoMove yang juga Pushable 
            // dimasukkan ke dalam rantai dorongan, menyelesaikan masalah tabrakan dorongan.
            // if (objectAtPosition.GetComponent<AutoMove>() != null) break; 

            chain.Add(pushable);
            currentPosition += direction * stepSize;
        }
        return chain;
    }

    private GameObject GetObjectAtPosition(Vector3 position)
    {
        Collider2D[] hitColliders = Physics2D.OverlapPointAll(position);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject != this.gameObject && !hitCollider.gameObject.CompareTag("GridCell"))
            {
                return hitCollider.gameObject;
            }
        }
        return null;
    }

    // Metode baru untuk memeriksa RightRotation di sekitar karakter
    private bool CheckForAdjacentRotation()
    {
        Vector3 currentPos = transform.position;
        float stepSize = gridManager.cellSize;

        Vector3[] directions = new Vector3[] { Vector3.up, Vector3.down, Vector3.left, Vector3.right };

        foreach (var dir in directions)
        {
            GameObject obj = GetObjectAtPosition(currentPos + dir * stepSize);
            if (obj != null && obj.GetComponent<RightRotation>() != null)
            {
                return true;
            }
        }
        return false;
    }

    // Metode untuk mendapatkan arah clockwise selanjutnya
    private Vector3 GetNextClockwiseDirection(Vector3 currentDirection)
    {
        if (currentDirection == Vector3.right) return Vector3.down;
        if (currentDirection == Vector3.down) return Vector3.left;
        if (currentDirection == Vector3.left) return Vector3.up;
        if (currentDirection == Vector3.up) return Vector3.right;
        return currentDirection;
    }
}