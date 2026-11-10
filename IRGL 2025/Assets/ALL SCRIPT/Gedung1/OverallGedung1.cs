using System.Collections;
using TMPro;
using UnityEngine;

public class OverallGedung1 : MonoBehaviour
{
    public GameObject uiBrankas;
    public GameObject BrankasE;
    public string Jawaban;

    public TextMeshProUGUI inputText1;
    public TextMeshProUGUI inputText2;
    public TextMeshProUGUI inputText3;
    public TextMeshProUGUI inputText4;
    public TextMeshProUGUI inputText5;

    public GameObject minWaktuUi;

    public GameObject TandaSeru;
    public void CloseUiBrankas()
    {
        if (uiBrankas != null)
            uiBrankas.SetActive(false);
        else
            Debug.LogWarning("UI Brankas belum di-assign!");
    }

    public void EnterBrankas()
    {
        if (Overall.instance == null)
        {
            Debug.LogWarning("Overall.instance belum di-assign!");
            return;
        }

        if (inputText1 == null || inputText2 == null || inputText3 == null || inputText4 == null || inputText5 == null)
        {
            Debug.LogWarning("Input text brankas belum di-assign!");
            return;
        }

        string gabung = inputText1.text + inputText2.text + inputText3.text + inputText4.text + inputText5.text;

        if (!int.TryParse(gabung, out int angka))
        {
            Debug.LogWarning("Input brankas tidak valid: " + gabung);
            return;
        }

        if (!int.TryParse(Jawaban, out int answer))
        {
            Debug.LogWarning("Jawaban brankas tidak valid!");
            return;
        }

        if (angka == answer)
        {

            if (TandaSeru != null)
            {
                TandaSeru.SetActive(false);
            }

            Debug.Log("True");
            CloseUiBrankas();
            Overall.instance.IncreaseScore(1500);
            Overall.instance.Puzzle1 = true;
            if (BrankasE != null)
            {
                BrankasE.SetActive(false);
            }

            
        }
        else
        {
            Overall.instance.MinSecond();

            if (minWaktuUi != null)
                minWaktuUi.SetActive(true);

            inputText1.text = "0";
            inputText2.text = "0";
            inputText3.text = "0";
            inputText4.text = "0";
            inputText5.text = "0";

            StartCoroutine(DelayWaktu());
        }
    }

    public IEnumerator DelayWaktu()
    {
        yield return new WaitForSeconds(1f);
        if (minWaktuUi != null)
            minWaktuUi.SetActive(false);
    }

    public void IncreaseBrankas(TextMeshProUGUI inputText)
    {
        if (inputText == null)
        {
            Debug.LogWarning("Input text untuk increase belum di-assign!");
            return;
        }

        if (!int.TryParse(inputText.text, out int angka))
        {
            Debug.LogWarning("Nilai input tidak valid!");
            return;
        }

        inputText.text = (angka == 0 ? "1" : "0").ToString();
    }
}
