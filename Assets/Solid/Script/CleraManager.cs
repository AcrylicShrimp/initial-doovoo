using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CleraManager : MonoBehaviour
{
    public static CleraManager instance;

    public int hpCount = 6;

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
    private void Awake()
    {
        instance = this;
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
}
