
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(PathReceiver), typeof(KeyStrokeInputController))]
public class DoovooController : MonoBehaviour
{
    public enum ImpactDirection
    {
        Left,
        Right
    }

    [SerializeField]
    private float _RotationSpeed;
    [SerializeField]
    private float _BaseVectorSpeed;
    [SerializeField]
    private float _ImpactVectorSpeed;
    [SerializeField]
    private float _MaximumSpeed;

    private float nAngularSpeed;
    private Rigidbody sRigidbody;
    private PathReceiver sPathReceiver;
    private KeyStrokeInputController sKeyStrokeInputController;
    private SpeedController sSpeedController;

    private void Awake()
    {
        this.sRigidbody = this.GetComponent<Rigidbody>();
        this.sPathReceiver = this.GetComponent<PathReceiver>();
        this.sKeyStrokeInputController = this.GetComponent<KeyStrokeInputController>();
        this.sSpeedController = new SpeedController(this._MaximumSpeed);
    }

    private void FixedUpdate()
    {
        this.sSpeedController.update();

        var nAdditionalSpeed = this._BaseVectorSpeed * .5f + (this.sKeyStrokeInputController.LeftSpeed + this.sKeyStrokeInputController.RightSpeed);

        this.nAngularSpeed = this._RotationSpeed * (this.sKeyStrokeInputController.RightSpeed - this.sKeyStrokeInputController.LeftSpeed);

        this.transform.Rotate(Vector3.up, this.nAngularSpeed * Time.deltaTime, Space.Self);

        //Add base velocity.
        this.sRigidbody.AddForce(this.sPathReceiver.PathDirection * this._BaseVectorSpeed, ForceMode.Acceleration);

        //Add directional velocity.
        this.sRigidbody.AddForce(this.transform.forward * nAdditionalSpeed, ForceMode.Acceleration);

        //Add some drag.
        this.sRigidbody.AddForce(Mathf.Min(0f, this.sSpeedController.Speed - Vector3.Dot(this.sRigidbody.velocity, this.sPathReceiver.PathDirection)) * this.sPathReceiver.PathDirection * this._BaseVectorSpeed, ForceMode.Acceleration);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.CompareTag("Wall"))
            return;

        var sContact = collision.GetContact(0);
        var sNormal = sContact.normal;

        this.sRigidbody.AddForce(this._ImpactVectorSpeed * sNormal - this.sRigidbody.velocity, ForceMode.VelocityChange);
        this.sSpeedController.restartAnimation();

        ImpactDirection sImpactDirection;

        if (Vector3.Dot(sContact.point - this.transform.position, this.transform.right) < 0f)
            sImpactDirection = ImpactDirection.Left;
        else
            sImpactDirection = ImpactDirection.Right;

        Debug.Log(sImpactDirection);
    }
}