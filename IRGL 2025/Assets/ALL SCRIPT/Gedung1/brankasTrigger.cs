using UnityEngine;

public class brankasTrigger : MonoBehaviour
{

    public GameObject BrankasE;
    public GameObject ui;
    public bool isTrigger = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Overall.instance == null)
        {
            Debug.LogWarning("Overall.instance belum di-assign!");
            return;
        }

        if (isTrigger && Input.GetKeyDown(KeyCode.E) && !Overall.instance.Puzzle1)
        {

            if(ui != null)
            {
                ui.SetActive(true);
            }
            else
            {
                Debug.LogWarning("UI Brankas belum di-assign di Inspector!");
            }
                
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (Overall.instance == null)
        {
            Debug.LogWarning("Overall.instance belum di-assign!");
            return;
        }

        if (!Overall.instance.Puzzle1)
        {
            isTrigger = true;
            if(BrankasE != null)
            {
                BrankasE.SetActive(true);
            }
            else
            {
                Debug.LogWarning("BrankasE belum di-assign di Inspector!");
            }
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(BrankasE != null)
        {
            isTrigger = false;
            BrankasE.SetActive(false);
        }
        else
        {
            Debug.LogWarning("BrankasE belum di-assign di Inspector!");
        }
    }
}
