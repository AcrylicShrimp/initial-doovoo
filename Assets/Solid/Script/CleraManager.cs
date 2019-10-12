using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Timers;
using System.Text;

public class CleraManager : MonoBehaviour
{
    public static CleraManager instance;
    [SerializeField]
    private int realHP = 6;
    public int hpCount
    {
        get { return realHP; }
        set
        {
            realHP = value;
            if (realHP <= 0)
            {

                Invoke("gameOverCall", 1f);
            }
        }
    }
    public void gameOverCall()
    {
        isStart = false;
        trigger.gameOver();
        gameOverObj.SetActive(true);
    }
    [SerializeField]
    private ClearTrigger trigger;
    [SerializeField]
    private GameObject gameOverObj;
    [SerializeField]
    private Image Title;
    [SerializeField]
    private GameObject titleBG;
    [SerializeField]
    private float speedTime;
    [SerializeField]
    private Animator ani;
    [SerializeField]
    private PlayableDirector timeline;

    [SerializeField]
    private Text timerText;
    private float timer;
    private bool isStart;
    private float[] previousTime = new float[10];
    [SerializeField]
    private Text TimeListText_L;
    [SerializeField]
    private Text TimeListText_R;
    public void timePause(bool isPause)
    {
        Time.timeScale = isPause ? 0f : 1f;
    }

    private void Update()
    {
        if (isStart)
        {
            timer += Time.deltaTime;
            timerText.text = string.Format("{0:00}:{1:00}:{2:00}", (int)(timer / 60f), (int)(timer % 60f), (int)((timer % 60f % 1f) * 100f));
        }
    }

    private void Awake()
    {
        isStart = true;
        instance = this;
        if (!ES2.Exists("ClearTime"))
        {
            for (int i = 0; i < 10; i++)
            {
                ES2.Save<float>(600f, "ClearTime?Tag=ClearTime_" + i);
                ES2.Save<int>(0, "ClearTime?Tag=ClearLife_" + i);
            }
        }
        for (int i = 0; i < 10; i++)
        {
            previousTime[i] = ES2.Load<float>("ClearTime?Tag=ClearTime_" + i);
        }
    }
    public void clearTimeSave()
    {
        isStart = false;
        for (int i = 0; i < 10; i++)
        {
            if (timer < previousTime[i])
            {
                for (int j = i; j < 9; j++)
                {
                    int life = ES2.Load<int>("ClearTime?Tag=ClearLife_" + i);
                    ES2.Save<float>(previousTime[j], "ClearTime?Tag=ClearTime_" + j + 1);
                    ES2.Save<int>(life, "ClearTime?Tag=ClearLife_" + j + 1);
                }

                ES2.Save<float>(timer, "ClearTime?Tag=ClearTime_" + i);
                ES2.Save<int>(realHP, "ClearTime?Tag=ClearLife_" + i);
                break;
            }

        }

    }
    public void timeList()
    {
        StringBuilder strBuilder_R = new StringBuilder();
        for (int i = 5; i < 10; i++)
        {
            float timerTemp = ES2.Load<float>("ClearTime?Tag=ClearTime_" + i);
            int lifeTemp = ES2.Load<int>("ClearTime?Tag=ClearLife_" + i);
            strBuilder_R.AppendFormat("{4}등 {0:00}:{1:00}:{2:00} - {3:00}%\n", (int)(timerTemp / 60f), (int)(timerTemp % 60f), (int)((timerTemp % 60f % 1f) * 100f), (int)((lifeTemp / 6f) * 100f), i + 1);
        }
        TimeListText_R.text = strBuilder_R.ToString();


        StringBuilder strBuilder_L = new StringBuilder();
        for (int i = 0; i < 5; i++)
        {
            float timerTemp = ES2.Load<float>("ClearTime?Tag=ClearTime_" + i);
            int lifeTemp = ES2.Load<int>("ClearTime?Tag=ClearLife_" + i);
            strBuilder_L.AppendFormat("{4}등 {0:00}:{1:00}:{2:00} - {3:00}%\n", (int)(timerTemp / 60f), (int)(timerTemp % 60f), (int)((timerTemp % 60f % 1f) * 100f), (int)((lifeTemp / 6f) * 100f), i + 1);
        }
        TimeListText_L.text = strBuilder_L.ToString();

    }
    public void clearGuage()
    {
        if (Application.isPlaying)
        {

            timeline.enabled = false;
            StartCoroutine(onClearGuage());
        }
    }
    public void resetScene()
    {
        timePause(false);
        SceneManager.LoadScene(1);
    }
    IEnumerator onClearGuage()
    {
        titleBG.SetActive(true);
        float maxGuage = 0f;
        if (hpCount > 0) { maxGuage = hpCount / 6f; }
        while (Title.fillAmount < maxGuage)
        {
            Title.fillAmount += Time.deltaTime / speedTime;
            yield return new WaitForEndOfFrame();
        }
        Title.fillAmount = maxGuage;
        if (maxGuage < 1f)
        {
            ani.SetTrigger("ClearF");
        }
        else
        {
            ani.SetTrigger("ClearS");
        }
    }
    public void exitGame()
    {
        Application.Quit();
    }
}
