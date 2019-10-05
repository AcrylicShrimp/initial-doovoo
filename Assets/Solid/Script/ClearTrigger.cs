using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject timeLineOn;
    [SerializeField]
    private KeyStrokeInputController keyCtrl;
    [SerializeField]
    private GameObject[] disableObjs;
    public void gameOver()
    {
        keyCtrl.enabled = false;
        for (int i = 0; i < disableObjs.Length; i++)
        {
            disableObjs[i].SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("엔딩에 들어오는것" + other.name);
        if (other.CompareTag("Player"))
        {
            keyCtrl.enabled = false;
            timeLineOn.SetActive(true);
            for (int i = 0; i < disableObjs.Length; i++)
            {
                disableObjs[i].SetActive(false);
            }
        }
    }
}
