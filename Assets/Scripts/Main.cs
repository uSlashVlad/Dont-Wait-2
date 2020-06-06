using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class Main : MonoBehaviour{
    public string mode;

    public bool nPause = true;

    public float maxTimeStart = 10;

    public float maxTime = 10;
    public float nowTime = 10;
    public float timeScale = 1;
    //
    public float resTime = 1;
    //
    [SerializeField] TextMeshProUGUI timeInd;
    [SerializeField] Slider timeBar;
    [SerializeField] TextMeshProUGUI scoreInd;

    public int needClicks = 100;
    public int prevClicks = 0;
    //
    public int up1Lvl = 0;
    //
    [SerializeField] Slider up1Bar;
    [SerializeField] TextMeshProUGUI up1Proc;

    public float needTime = 15;
    public float prevTime = 0;
    //
    public int up2Lvl = 0;
    //
    [SerializeField] Slider up2Bar;
    [SerializeField] TextMeshProUGUI up2Proc;

    public int clicks;
    public float timer;

    int addTimeScaleMod = 1;
    float addTimeMod = 1;
    float addTimeScale = 0;
    [SerializeField] TextMeshProUGUI modeInd;

    void Start(){
        int modeNum = PlayerPrefs.GetInt("StartMode");
        if (modeNum != 0)
        {
            switch (modeNum)
            {
                case 1:
                    maxTimeStart = 10;
                    addTimeScaleMod = 1;
                    addTimeMod = 1;
                    mode = "Easy";
                    break;
                case 2:
                    maxTimeStart = 10;
                    addTimeScaleMod = 2;
                    addTimeMod = 2;
                    mode = "Normal";
                    break;
                case 3:
                    maxTimeStart = 8;
                    addTimeScaleMod = 3;
                    addTimeMod = 3;
                    mode = "Hard";
                    break;
                case 4:
                    maxTimeStart = 3;
                    addTimeScaleMod = 5;
                    addTimeMod = 3;
                    mode = "Insane";
                    break;
            }
        }
        else{
            SceneManager.LoadScene("Menu");
        }

        if (PlayerPrefs.GetInt("Up1Lvl") != 0){
            UpgradeLoad();
        }
        else{
            UpgradeSave();
        }

        maxTime = maxTimeStart + (float)(1.5 * up2Lvl);
        nowTime = maxTime;

        clicks = PlayerPrefs.GetInt("Clicks");
        timer = PlayerPrefs.GetFloat("Timer");

        scoreInd.text = clicks + " clicks";
        modeInd.text = mode + " - " + ((timeScale + Math.Round(addTimeScale, 2)) * addTimeMod) + "pps";
    }

    void Update(){
        if (nPause){
            nowTime -= Time.deltaTime * (timeScale + addTimeScale) * addTimeMod;

            if (nowTime <= 0)
                GetComponent<Death>().DeathTrigger();

            timer += Time.deltaTime;

            timeInd.text = Math.Round(nowTime, 1) + " / " + Math.Round(maxTime, 1);
            timeBar.maxValue = maxTime;
            timeBar.value = nowTime;

            CheckUpgradeTime();
            UpdateUp2Stat();
        }
    }

    public void PressButton(){
        if (nPause){
            nowTime += resTime + 1 * up1Lvl;
            clicks++;

            float finAdd = (clicks * timer * addTimeScaleMod) / 100000;

            addTimeScale = finAdd;
            modeInd.text = mode + " - " + ((timeScale + Math.Round(addTimeScale, 2)) * addTimeMod) + "pps";

            MiniSave();

            if (nowTime >= maxTime)
                nowTime = maxTime;

            scoreInd.text = clicks + " clicks";

            CheckUpgradeClick();
            UpdateUp1Stat();
        }
    }

    // ----- Saves&Load ----- //

    void MiniSave()
    {
        PlayerPrefs.SetInt("Clicks", clicks);
        PlayerPrefs.SetFloat("Timer", timer);
    }

    void UpgradeSave()
    {
        PlayerPrefs.SetInt("Up1MaxVal", needClicks);
        PlayerPrefs.SetInt("Up1PrevVal", prevClicks);
        PlayerPrefs.SetInt("Up1Lvl", up1Lvl);
        //
        PlayerPrefs.SetFloat("Up2MaxVal", needTime);
        PlayerPrefs.SetFloat("Up2PrevVal", prevTime);
        PlayerPrefs.SetInt("Up2Lvl", up2Lvl);
    }
    //
    void UpgradeLoad()
    {
        needClicks = PlayerPrefs.GetInt("Up1MaxVal");
        prevClicks = PlayerPrefs.GetInt("Up1PrevVal");
        up1Lvl = PlayerPrefs.GetInt("Up1Lvl");
        //
        needTime = PlayerPrefs.GetFloat("Up2MaxVal");
        prevTime = PlayerPrefs.GetFloat("Up2PrevVal");
        up2Lvl = PlayerPrefs.GetInt("Up2Lvl");

        UpdateUp1Stat();
        UpdateUp2Stat();
    }

    // ----- Upgrades ----- //

    public void CheckUpgradeClick()
    {
        if (clicks >= needClicks)
        {
            prevClicks = needClicks;
            needClicks = (int)Math.Round(needClicks * 2.5);

            up1Lvl++;

            UpgradeSave();
        }
    }
    void UpdateUp1Stat()
    {
        int a = needClicks - prevClicks;
        int b = clicks - prevClicks;
        //
        up1Bar.maxValue = a;
        up1Bar.value = b;
        up1Proc.text = (int)((float)b / (float)a * 100) + "%";
    }

    public void CheckUpgradeTime()
    {
        if (timer >= needTime)
        {
            prevTime = needTime;
            needTime = (int)Math.Round(needTime * 2.25);

            up2Lvl++;

            maxTime = maxTimeStart + (float)(1.5 * up2Lvl);

            UpgradeSave();
        }
    }
    void UpdateUp2Stat()
    {
        float a = needTime - prevTime;
        float b = timer - prevTime;
        //
        up2Bar.maxValue = a;
        up2Bar.value = b;
        up2Proc.text = (int)((float)b / (float)a * 100) + "%";
    }

    // ----- Buttons ----- //

    [SerializeField] Button button;
    [SerializeField] GameObject buttonText;

    public void Pause()
    {
        if (nPause)
            nPause = false;
        else
            nPause = true;

        if (!nPause)
        {
            button.interactable = false;
            buttonText.SetActive(true);
        }
        else
        {
            button.interactable = true;
            buttonText.SetActive(false);
        }
    }

    public void Death()
    {
        GetComponent<Death>().DeathTrigger();
    }
}
