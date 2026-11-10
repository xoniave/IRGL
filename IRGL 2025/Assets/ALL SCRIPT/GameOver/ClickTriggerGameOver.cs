using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickTriggerGameOver : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseDown()
    {
        Debug.Log("CLICKED");
        if (Overall.instance != null)
        {
            Overall.instance.ResetGame();
        }
        SceneManager.LoadScene("MainMenu");
    }
}
