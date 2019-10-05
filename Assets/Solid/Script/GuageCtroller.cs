using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuageCtroller : MonoBehaviour
{
    [SerializeField]
    private KeyStrokeInputController keyCtrl;
    [SerializeField]
    private float aniSpeed;
    [SerializeField]
    private Slider rightSlider;
    [SerializeField]
    private Slider leftSlider;
    [SerializeField]
    private Animator[] rightAnis;
    [SerializeField]
    private Animator[] leftAnis;

    public static GuageCtroller instance;
    private void Awake()
    {
        instance = this;
    }
    private void LateUpdate()
    {
        rightSlider.value = keyCtrl.RightSpeed;
        leftSlider.value = keyCtrl.LeftSpeed;
        for (int i = 0; i < rightAnis.Length; i++)
        {
            if (rightAnis[i] != null) rightAnis[i].SetFloat("AddSpeed", aniSpeed * keyCtrl.RightSpeed);
        }
        for (int i = 0; i < leftAnis.Length; i++)
        {
            if (leftAnis[i] != null) leftAnis[i].SetFloat("AddSpeed", aniSpeed * keyCtrl.LeftSpeed);
        }
    }
    public void removeHuman(Animator ani)
    {
        for (int i = 0; i < rightAnis.Length; i++)
        {
            if (rightAnis[i] == ani)
            {
                rightAnis[i] = null;
                return;
            }
        }
        for (int i = 0; i < leftAnis.Length; i++)
        {
            if (leftAnis[i] == ani)
            {
                leftAnis[i] = null;
                return;
            }
        }
    }
}
