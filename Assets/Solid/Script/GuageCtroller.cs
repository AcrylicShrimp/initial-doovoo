using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuageCtroller : MonoBehaviour
{
    [SerializeField]
    private Color[] guageColors;
    //private Vector3[] guageColor
    [SerializeField]
    private Image[] guageImages;
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
    private void colorSet()
    {
        //guageColors[0].r;
        //guageColors[0].g;
        //guageColors[0].b;
        //guageColors[1].r;
        //guageColors[1].g;
        //guageColors[1].b;
    }
    private void LateUpdate()
    {
        rightSlider.value = Mathf.Lerp(rightSlider.value, keyCtrl.RightSpeed, 0.1f);
        leftSlider.value = Mathf.Lerp(leftSlider.value, keyCtrl.LeftSpeed, 0.1f);
        guageImages[0].color = Vector4.Lerp(guageColors[0], guageColors[1], keyCtrl.RightSpeed);
        guageImages[1].color = Vector4.Lerp(guageColors[0], guageColors[1], keyCtrl.LeftSpeed);
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
