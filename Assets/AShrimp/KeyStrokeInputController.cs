
using System.Collections;

using UnityEngine;

public class KeyStrokeInputController : MonoBehaviour
{
    public class KeyStrokeInput
    {
        public float Coefficient { get; private set; }

        private KeyCode nFirst;
        private KeyCode nSecond;
        private KeyCode nLastKey;
        private int nKeyCount;
        private float nMaximumKeyCount;

        public KeyStrokeInput(float nMaximumKeyCount, KeyCode nFirst, KeyCode nSecond)
        {
            this.Coefficient = 0f;
            this.nFirst = nFirst;
            this.nSecond = nSecond;
            this.nMaximumKeyCount = nMaximumKeyCount;
        }

        public void update(MonoBehaviour sOwner)
        {
            if (Input.GetKeyDown(this.nFirst) && this.nLastKey != this.nFirst)
            {
                this.nLastKey = this.nFirst;
                ++this.nKeyCount;

                sOwner.StartCoroutine(this.reduceKeyCount());
            }
            else if (Input.GetKeyDown(this.nSecond) && this.nLastKey != this.nSecond)
            {
                this.nLastKey = this.nSecond;
                ++this.nKeyCount;

                sOwner.StartCoroutine(this.reduceKeyCount());
            }

            this.Coefficient = Mathf.Min(this.nKeyCount / this.nMaximumKeyCount, 1f);
        }
        public void updateAnroid(MonoBehaviour sOwner)
        {
            this.nLastKey = this.nFirst;
            ++this.nKeyCount;

            sOwner.StartCoroutine(this.reduceKeyCount());

            this.Coefficient = Mathf.Min(this.nKeyCount / this.nMaximumKeyCount, 1f);
        }

        private IEnumerator reduceKeyCount()
        {
            yield return new WaitForSeconds(1f);

            --this.nKeyCount;
        }
    }

    public float LeftSpeed { get; private set; }
    public float RightSpeed { get; private set; }

    [SerializeField]
    private float _MaximumKeyCount;
    private KeyStrokeInput sLeftInput;
    public KeyStrokeInput SLeftInput { get { return sLeftInput; } }
    private KeyStrokeInput sRightInput;
    public KeyStrokeInput SRightInput { get { return sRightInput; } }

    private void Awake()
    {
        this.sLeftInput = new KeyStrokeInput(this._MaximumKeyCount, KeyCode.Z, KeyCode.X);
        this.sRightInput = new KeyStrokeInput(this._MaximumKeyCount, KeyCode.Comma, KeyCode.Period);
#if UNITY_ANDROID
        _MaximumKeyCount = 3;
#endif
    }

    private void Update()
    {
        this.sLeftInput.update(this);
        this.sRightInput.update(this);

        this.LeftSpeed = this.sLeftInput.Coefficient;
        this.RightSpeed = this.sRightInput.Coefficient;
    }
}