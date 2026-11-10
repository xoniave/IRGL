using TMPro;
using UnityEngine;

public class LastScore : MonoBehaviour
{

    public TextMeshProUGUI TextMeshProUGUI;
    int lastScore;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Overall.instance != null)
        {
            lastScore = Overall.instance.getScore();
        }
        else
        {
            lastScore = PlayerPrefs.GetInt("Score", 0);
        }
            

        if (TextMeshProUGUI != null)
        {
            TextMeshProUGUI.text = "Last Score : " + lastScore;
        }
        else
        {
            Debug.LogWarning("TextMeshProUGUI belum di-assign di Inspector!");
        }
            
    }

    // Update is called once per frame
    void Update()
    {

    }
}
