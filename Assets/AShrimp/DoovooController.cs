
using UnityEngine;

[RequireComponent(typeof(PathReceiver), typeof(Rigidbody))]
public class DoovooController : MonoBehaviour
{
    public enum ImpactDirection
    {
        Left,
        Right
    }

    [SerializeField]
    private float _BaseVectorSpeed;
    [SerializeField]
    private float _MaximumSpeed;

    private PathReceiver sPathReceiver;
    private Rigidbody sRigidbody;

    private void Awake()
    {
        this.sPathReceiver = this.GetComponent<PathReceiver>();
        this.sRigidbody = this.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        //Add base velocity.
        this.sRigidbody.AddForce(this.sPathReceiver.PathDirection * this._BaseVectorSpeed, ForceMode.Acceleration);

        //Add some drag.
        this.sRigidbody.AddForce(Mathf.Min(0f, this._MaximumSpeed - Vector3.Dot(this.sRigidbody.velocity, this.sPathReceiver.PathDirection)) * this.sPathReceiver.PathDirection * this._BaseVectorSpeed, ForceMode.Acceleration);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.CompareTag("Wall"))
            return;

        var sNormal = collision.GetContact(0).normal;

        this.sRigidbody.AddForce(this.sRigidbody.velocity + 2f * sNormal, ForceMode.VelocityChange);

        ImpactDirection sImpactDirection;

        if (Vector3.Dot(sNormal, Vector3.right) < 0f)
            sImpactDirection = ImpactDirection.Right;
        else
            sImpactDirection = ImpactDirection.Left;

        Debug.Log(sImpactDirection);
    }
}
