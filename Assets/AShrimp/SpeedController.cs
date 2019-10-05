
using UnityEngine;

public class SpeedController
{
    public float Speed { get; private set; }

    private float nReferenceSpeed;
	private float nStartTime;
	private float nDuration = 0.1f;

    public SpeedController(float nReferenceSpeed)
    {
        this.nReferenceSpeed = nReferenceSpeed;
        this.nStartTime = Time.timeSinceLevelLoad;
    }

    public void restartAnimation()
    {
        this.nStartTime = Time.timeSinceLevelLoad;
        this.nDuration = 3f;
    }

    public void update()
	{
        this.Speed = SpeedController.easeInQuint(Mathf.Min(Time.timeSinceLevelLoad - this.nStartTime, this.nDuration), this.nReferenceSpeed * .25f, this.nReferenceSpeed * .75f, this.nDuration);
	}

    private static float easeInQuint(float nTime, float nBegin, float nDifference, float nDuration)
    {
        nTime /= nDuration;
        --nTime;
        return nDifference * (nTime * nTime * nTime * nTime * nTime + 1f) + nBegin;
    }
}