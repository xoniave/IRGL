using TMPro;
using UnityEngine;

public class Character : MonoBehaviour
{
    public Rigidbody2D rigidBody;
    public int walkkStr;
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    //Timer
    public TextMeshProUGUI timerTextPerScene;
    public GameObject tanganPerScene;

    //Score
    public TextMeshProUGUI ScoreText;
    public GameObject ScoreUi;

    public GameObject ComputerUI;
    public GameObject BrankasUi;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (rigidBody == null || animator == null || spriteRenderer == null)
        {
            Debug.LogError("FATAL ERROR: Komponen Rigidbody2D, Animator, atau SpriteRenderer belum di-assign! Script Character dinonaktifkan.");
            this.enabled = false;
            return;
        }

        int JumlahScore = PlayerPrefs.GetInt("Score",0);
        if (ScoreText != null)
        {
            ScoreText.text = "SCORE : " + JumlahScore;
        }


        // INI BUAT PENYIMPANAN DATA POSISI TERAKHIR DIA DMANA JADI KALO LOAD SCENE GK NGULANG DRI AWAL POSISI NYA
        if (PlayerPrefs.GetInt("Kembali") == 0 && PlayerPrefs.HasKey("PlayerX"))
        {
            //memperbarui POSISI BERDASRKAN DATA
            float x = PlayerPrefs.GetFloat("PlayerX");
            float y = PlayerPrefs.GetFloat("PlayerY");
            transform.position = new Vector3(x, y);


            //delete data nya
            PlayerPrefs.DeleteKey("PlayerX");
            PlayerPrefs.DeleteKey("PlayerY");
            PlayerPrefs.DeleteKey("PlayerZ");

        }

        if (Overall.instance != null)
        {
            if (timerTextPerScene != null) Overall.instance.AssignTimerText(timerTextPerScene);
            if (tanganPerScene != null) Overall.instance.AssignTangan(tanganPerScene);
            if (ScoreText != null) Overall.instance.AssignScoreText(ScoreText);
            if (ScoreUi != null) Overall.instance.AssignScoreUi(ScoreUi);
        }
        else
        {
            Debug.LogWarning("Overall.instance is missing. UI will not be managed globally.");
        }


    }

    // Update is called once per frame
    void Update()
    {
        //FUNGSI GERAK CHARACTER
        movement();
    }


    public void movement()// KODINGAN UNTUK MENGGERAKAN
    {
        if (Input.GetKey(KeyCode.D))
        {
            animator.SetFloat("Speed", 1);
            spriteRenderer.flipX = false;
            rigidBody.linearVelocity = Vector2.right * walkkStr;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            animator.SetFloat("Speed", 1);
            spriteRenderer.flipX = true;
            rigidBody.linearVelocity = Vector2.left * walkkStr;
        }
        else
        {
            animator.SetFloat("Speed", 0);
            rigidBody.linearVelocity = Vector2.zero;
        }
    }
}
