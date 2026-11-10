using UnityEngine;

public class OverallGedung2 : MonoBehaviour
{
    public GameObject interactUI;
    public GameObject UI;
    public int jenis = 0;

    public bool isTrigger = false;
    public bool isComputerUIActive = false;

    void Update()
    {
        if (Overall.instance == null)
        {
            Debug.LogWarning("Overall.instance belum di-assign!");
            return;
        }

        if (Input.GetKeyDown(KeyCode.E) && isTrigger && !Overall.instance.Puzzle2)
        {
            if (UI != null)
                UI.SetActive(true);
            else
                Debug.LogWarning("UI belum di-assign di OverallGedung2!");
        }

        if (Input.GetMouseButtonDown(0) && isTrigger && jenis == 0 && !Overall.instance.Puzzle2)
        {
            if (UI != null)
                UI.SetActive(false);
        }

        if (!isTrigger && jenis == 0)
        {
            if (UI != null)
                UI.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Overall.instance == null)
        {
            Debug.LogWarning("Overall.instance belum di-assign!");
            return;
        }

        if (interactUI != null && !Overall.instance.Puzzle2)
        {
            isTrigger = true;
            interactUI.SetActive(true);
        }
        else if (interactUI == null)
        {
            Debug.LogWarning("interactUI belum di-assign di OverallGedung2!");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (interactUI != null)
        {
            isTrigger = false;
            interactUI.SetActive(false);
        }
        else
        {
            Debug.LogWarning("interactUI belum di-assign di OverallGedung2!");
        }
    }
}
