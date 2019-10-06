using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AniroidInput : MonoBehaviour
{
    [SerializeField]
    private bool isRight;
    [SerializeField]
    private KeyStrokeInputController keyCtrl;

    public void OnClick()
    {
        if (isRight)
        {
            keyCtrl.SRightInput.updateAnroid(this);
        }
        else
        {
            keyCtrl.SLeftInput.updateAnroid(this);
        }
    }
}
