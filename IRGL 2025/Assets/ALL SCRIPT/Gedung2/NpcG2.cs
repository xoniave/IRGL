using TMPro;
using UnityEngine;
using System.Collections;

public class NpcG2 : MonoBehaviour
{
    public GameObject Talk;
    public GameObject DialogueUi;
    public TextMeshProUGUI TextMeshProUGUI;
    public bool isTrigger = false;
    public string Text = "";
    public bool isFinish = true;
    public Coroutine Coroutine;

    void Update()
    {
        if (Overall.instance == null)
        {
            Debug.LogWarning("Overall.instance belum di-assign!");
            return;
        }

        if (Input.GetKeyDown(KeyCode.E) && isTrigger)
        {
            if (DialogueUi == null || TextMeshProUGUI == null)
            {
                Debug.LogWarning("DialogueUi atau TextMeshProUGUI belum di-assign!");
                return;
            }

            if (!Overall.instance.Puzzle2)
            {
                if (isFinish)
                {
                    DialogueUi.SetActive(true);
                    Coroutine = StartCoroutine(ShowText());
                    isFinish = false;
                }
            }
            else
            {
                Text = "Puzzle Done, GO search another mission";
                if (isFinish)
                {
                    DialogueUi.SetActive(true);
                    Coroutine = StartCoroutine(ShowText());
                    isFinish = false;
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (DialogueUi == null || TextMeshProUGUI == null)
                return;

            if (!isFinish)
            {
                if (Coroutine != null)
                    StopCoroutine(Coroutine);

                TextMeshProUGUI.text = Text;
                isFinish = true;
            }
            else
            {
                DialogueUi.SetActive(false);
                isFinish = true;
            }
        }

        if (!isTrigger)
        {
            if (DialogueUi != null)
                DialogueUi.SetActive(false);

            if (Coroutine != null)
                StopCoroutine(Coroutine);

            if (TextMeshProUGUI != null)
                TextMeshProUGUI.text = "";

            isFinish = true;
        }
    }

    IEnumerator ShowText()
    {
        if (TextMeshProUGUI == null)
            yield break;

        TextMeshProUGUI.text = "";
        for (int i = 0; i < Text.Length; i++)
        {
            TextMeshProUGUI.text += Text[i];
            yield return new WaitForSeconds(0.03f);
        }
        isFinish = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Talk != null)
        {
            isTrigger = true;
            Talk.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Talk object belum di-assign di NpcG2!");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        isTrigger = false;
        if (Talk != null)
            Talk.SetActive(false);
    }
}
