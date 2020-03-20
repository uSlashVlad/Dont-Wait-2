using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;

public class Death : MonoBehaviour
{
    int mode;
    int modifier = 1;

    Main main;

    void Start()
    {
        main = GetComponent<Main>();

        mode = PlayerPrefs.GetInt("StartMode");
        if (mode != 0)
        {
            switch (mode)
            {
                case 1:
                    modifier = 1;
                    break;
                case 2:
                    modifier = 2;
                    break;
                case 3:
                    modifier = 4;
                    break;
                case 4:
                    modifier = 8;
                    break;
            }
        }
        else
        {
            SceneManager.LoadScene("Menu");
        }
    }
    
    public void DeathTrigger()
    {
        int coef = ((int)(main.clicks / 300) + (int)(main.timer / 60)) * modifier;

        PlayerPrefs.DeleteKey("StartMode");
        //
        PlayerPrefs.DeleteKey("Clicks");
        PlayerPrefs.DeleteKey("Timer");
        PlayerPrefs.DeleteKey("Up1MaxVal");
        PlayerPrefs.DeleteKey("Up1PrevVal");
        PlayerPrefs.DeleteKey("Up1Lvl");
        PlayerPrefs.DeleteKey("Up2MaxVal");
        PlayerPrefs.DeleteKey("Up2PrevVal");
        PlayerPrefs.DeleteKey("Up2Lvl");
        //
        PlayerPrefs.SetInt("Result", coef);

        if (Advertisement.IsReady("rewardedVideo"))
        {
            Advertisement.Show("video");
        }

        SceneManager.LoadScene("Menu");
    }
}
