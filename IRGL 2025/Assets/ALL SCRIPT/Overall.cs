using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Overall : MonoBehaviour
{
    //INI AGAR SCRIPT OVERALL BISA DI AKSES DARI LUAR 
    public static Overall instance;

    public GameObject Tangan;
    public GameObject ScoreUi;
    public TextMeshProUGUI timerText;

    // Flag baru untuk mengontrol pause
    public bool isPaused = false; // <-- TAMBAHKAN INI

    private bool isHoldingJam = false;
    private float timer = 600f;

    private bool isGameOver = false;

    //DATA PUZZLE YANG SUDAH DI SELESAIKAN
    public bool Puzzle1 = false;
    public bool Puzzle2 = false;
    public bool Puzzle3 = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Hancurkan kalau duplikat
        }

        // Ambil score terakhir dari PlayerPrefs atau inisialisasi ke 0
        PlayerPrefs.SetInt("Score", 0);
    }

    // Update is called once per frame
    void Update()
    {
        // GAMEOVER & TIMER
        if (timer > 0f && !isGameOver ) 
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                timer = 0f;
                gameOver();
            }
        }

        //JAM
        if (Input.GetKeyDown(KeyCode.Q))
        {
            isHoldingJam = true;
            if (Tangan != null)
            {
                Tangan.SetActive(true);
            }
            else
            {
                isHoldingJam = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            isHoldingJam = false;
            if (Tangan != null)
            {
                Tangan.SetActive(false);
            }
        }

        if (isHoldingJam && !isGameOver && timerText != null)
        {
            int minutes = Mathf.FloorToInt(timer / 60f);
            int seconds = Mathf.FloorToInt(timer % 60f);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        //SCORE
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (ScoreUi != null)
            {
                ScoreUi.SetActive(true);
            }
        }

        if (Input.GetKeyUp(KeyCode.Tab))
        {
            if (ScoreUi != null)
            {
                ScoreUi.SetActive(false);
            }
        }
        
        //WIN
        if (Puzzle1 == true && Puzzle2 == true && Puzzle3 == true)
        {
            WebBridge.SendScore(PlayerPrefs.GetInt("Score"));
            SceneManager.LoadScene("Finish");
        }
    }

    // END FUNGSI PAUSE/RESUME TIMER

    public void AssignTimerText(TextMeshProUGUI newText)
    {
        timerText = newText;
    }

    public void AssignTangan(GameObject newTangan)
    {
        Tangan = newTangan;
    }

    public void MinSecond()
    {
        if (!isGameOver) {

            timer -= 10f;
            if (timer <= 0f && !isGameOver)
            {
                timer = 0f;
                gameOver();
            }

        }

    }

    //SCORE ui
    public TextMeshProUGUI ScoreText;
    public int Score = 0;
    public void AssignScoreText(TextMeshProUGUI newText)
    {
        ScoreText = newText;
    }

    public void AssignScoreUi(GameObject newObject)
    {
        ScoreUi = newObject;
    }

    public void IncreaseScore(int jumlah)
    {
        Score += jumlah;
        PlayerPrefs.SetInt("Score", Score);
        if (ScoreText != null)
        {
            ScoreText.text = "SCORE : " + Score;
        }
    }

    public int getScore()
    {
        return PlayerPrefs.GetInt("Score");
    }

    public void gameOver()
    {
        WebBridge.SendScore(PlayerPrefs.GetInt("Score"));
        isGameOver = true;
        Debug.Log("GAMEOVER");
        SceneManager.LoadScene("GameOver");
    }


    public void ResetGame()
    {
        timer = 600f;
        isGameOver = false;
        Score = 0;
        Puzzle1 = false;
        Puzzle2 = false;
        Puzzle3 = false;
        PlayerPrefs.SetInt("Score", 0);

        if (ScoreText != null)
        {
            ScoreText.text = "SCORE : 0";
        }

        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(timer / 60f);
            int seconds = Mathf.FloorToInt(timer % 60f);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}