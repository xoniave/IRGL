using UnityEngine;

public class SkillManager : MonoBehaviour
{
    // Metode skill "Push"
    public void Push(GameObject targetObject, Vector3 direction)
    {
        // Cek apakah objek target memiliki komponen PushableObject
        PushableObject pushable = targetObject.GetComponent<PushableObject>();

        if (pushable != null)
        {
            // Jika ada, panggil metode BePushed pada komponen tersebut
            pushable.BePushed(direction);
        }
    }
}