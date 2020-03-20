using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameStarter : MonoBehaviour
{
    public int score;
    [SerializeField] TextMeshProUGUI scoreInd;
    [SerializeField] Animator anim;

    void Start()
    {
        score = PlayerPrefs.GetInt("TotalScore") + PlayerPrefs.GetInt("Result");
        PlayerPrefs.SetInt("TotalScore", score);

        scoreInd.text = "Total score\n" + score;

        if (PlayerPrefs.GetInt("StartMode") != 0)
            SceneManager.LoadScene("Main");
    }

    int modeFin;

    public void StartGame(int mode)
    {
        modeFin = mode;

        anim.SetTrigger("Fade");
    }

    public void FinStart()
    {
        PlayerPrefs.SetInt("StartMode", modeFin);

        SceneManager.LoadScene("Main");
    }
}
