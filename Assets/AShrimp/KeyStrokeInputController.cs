
using UnityEngine;

public class KeyStrokeInputController : MonoBehaviour
{
    public class KeyStrokeInput
    {
        public float Coefficient { get; private set; }

        private KeyCode nFirst;
        private KeyCode nSecond;
        private KeyCode nLastKey;
        private float nLastTime;
        private float nMaxTimeDelta;

        public KeyStrokeInput(float nMaxTimeDelta, KeyCode nFirst, KeyCode nSecond)
        {
            this.Coefficient = 0f;
            this.nFirst = nFirst;
            this.nSecond = nSecond;
            this.nLastTime = 0f;
            this.nMaxTimeDelta = nMaxTimeDelta;
        }

        public void update()
        {
            if (Time.timeSinceLevelLoad - this.nLastTime < this.nMaxTimeDelta)
            {
                this.Coefficient = 1f;
                return;
            }

            this.Coefficient = Mathf.Min(this.nMaxTimeDelta / (Time.timeSinceLevelLoad - this.nLastTime), 1f);

            if (Input.GetKeyDown(this.nFirst) && this.nLastKey != this.nFirst)
            {
                this.nLastKey = this.nFirst;
                this.nLastTime = Time.timeSinceLevelLoad;
            }
            else if (Input.GetKeyDown(this.nSecond) && this.nLastKey != this.nSecond)
            {
                this.nLastKey = this.nSecond;
                this.nLastTime = Time.timeSinceLevelLoad;
            }
        }
    }

    public float LeftSpeed { get; private set; }
    public float RightSpeed { get; private set; }

    [SerializeField]
    private float _MaxTimeDelta;
    private KeyStrokeInput sLeftInput;
    private KeyStrokeInput sRightInput;

    private void Awake()
    {
        this.sLeftInput = new KeyStrokeInput(this._MaxTimeDelta, KeyCode.Z, KeyCode.X);
        this.sRightInput = new KeyStrokeInput(this._MaxTimeDelta, KeyCode.Comma, KeyCode.Period);
    }

    private void Update()
    {
        this.sLeftInput.update();
        this.sRightInput.update();

        this.LeftSpeed = this.sLeftInput.Coefficient;
        this.RightSpeed = this.sRightInput.Coefficient;
    }
}