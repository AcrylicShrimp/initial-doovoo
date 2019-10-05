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
    private void LateUpdate()
    {
        rightSlider.value = keyCtrl.RightSpeed;
        leftSlider.value = keyCtrl.LeftSpeed;
        for (int i = 0; i < rightAnis.Length; i++)
        {
            rightAnis[i].SetFloat("AddSpeed", aniSpeed * keyCtrl.RightSpeed);
        }
        for (int i = 0; i < leftAnis.Length; i++)
        {
            leftAnis[i].SetFloat("AddSpeed", aniSpeed * keyCtrl.LeftSpeed);
        }
    }
}
