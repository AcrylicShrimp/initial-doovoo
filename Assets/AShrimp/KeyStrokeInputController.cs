
using UnityEngine;

public class KeyStrokeInputController : MonoBehaviour
{
    public class KeyStrokeInput
    {
        public float Coefficient { get; private set; }

        private KeyCode nFirst;
        private KeyCode nSecond;
        private KeyCode nLastKey;
        private float nLastTime = 0f;

        public KeyStrokeInput(KeyCode nFirst, KeyCode nSecond)
        {
            this.Coefficient = 0f;
            this.nFirst = nFirst;
            this.nSecond = nSecond;
        }

        public void update()
        {
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

            this.Coefficient = (Time.timeSinceLevelLoad - this.nLastTime) / 100f;
        }
    }

    public float LeftSpeed { get; private set; }
    public float RightSpeed { get; private set; }

    private KeyStrokeInput sLeftInput;
    private KeyStrokeInput sRightInput;

    private void Awake()
    {
        this.sLeftInput = new KeyStrokeInput(KeyCode.Z, KeyCode.X);
        this.sRightInput = new KeyStrokeInput(KeyCode.Comma, KeyCode.Greater);
    }

    private void Update()
    {
        this.sLeftInput.update();
        this.sRightInput.update();

        this.LeftSpeed = this.sLeftInput.Coefficient;
        this.RightSpeed = this.sRightInput.Coefficient;
    }
}